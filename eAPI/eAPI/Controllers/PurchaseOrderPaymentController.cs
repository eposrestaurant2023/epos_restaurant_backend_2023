using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using eAPI.Services;
using eModels;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using NETCore.Encrypt;
using System.Text.Json;
using eAPI.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace eAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PurchaseOrderPaymentController : ODataController
    {
        private readonly ApplicationDbContext db;
        private readonly AppService app;
        private readonly IHubContext<ConnectionHub> hub;
        public PurchaseOrderPaymentController(ApplicationDbContext _db, AppService _app, IHubContext<ConnectionHub> _hub)
        {
            db = _db;hub = _hub;
            app = _app;
        }        

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]   
        public IQueryable<PurchaseOrderPaymentModel> Get(string keyword = "")
        {
            if (!string.IsNullOrEmpty(keyword))
            {
              return (from r in db.PurchaseOrderPayments
                      where 
                                 EF.Functions.Like((
                                 (r.reference_number ?? " ")).ToLower().Trim(), $"%{keyword}%".ToLower().Trim())
                           select r);

            }
            else
            {
                return db.PurchaseOrderPayments.AsQueryable();
            }
        }

        [HttpGet("getsingle")]
        [EnableQuery(MaxExpansionDepth = 8)]     
        public async Task<SingleResult<PurchaseOrderPaymentModel>> Get([FromODataUri] int key)
        {
            return await Task.Factory.StartNew(() => SingleResult.Create<PurchaseOrderPaymentModel>(db.PurchaseOrderPayments.Where(r => r.id == key).AsQueryable()));
        }

        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] PurchaseOrderPaymentModel p)
        {
            //validat check if payment is over PO amount
            PurchaseOrderModel po = db.PurchaseOrders.Find(p.purchase_order_id);
            if (p.id == 0)
            {

                if (po.balance< (p.payment_amount / Convert.ToDecimal(p.exchange_rate)))
                {
                    return StatusCode(300, "Payment amount cannot greater than balance amount");
                }

            }
            else
            {
                var payments = db.PurchaseOrderPayments.Where(r => r.purchase_order_id== p.purchase_order_id&& !r.is_deleted && r.id != p.id);
                if (payments.Any())
                {
                    if (payments.Sum(r => r.payment_amount / Convert.ToDecimal(r.exchange_rate)) + p.payment_amount / Convert.ToDecimal(p.exchange_rate) > po.total_amount)
                    {
                        return StatusCode(300, "Payment amount cannot greater than balance amount");
                    }
                }

            }
            //end check validation

            HistoryModel h = new HistoryModel($"{(p.id != 0? "Update PO Payment" : "Create New Payment")}");
            h.description = $"{(p.id != 0? "Update PO payment." : "Create new payment.")} Purchase order #: {po.document_number}. Amount ({p.currency_name_en}): {p.payment_amount.ToString(p.currency_format)}";
            h.document_number = po.document_number;
            h.vendor_id = po.vendor_id;
            h.purchase_order_id = po.id;
            h.amount = p.payment_amount;

            p.histories.Add(h);


            if (p.id == 0)
            {
                db.PurchaseOrderPayments.Add(p);
            }
            else
            {
                db.PurchaseOrderPayments.Update(p);
            }

            try
            {
                await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)),hub);
                string _query_update = $"exec sp_update_purchase_order_payment {p.purchase_order_id}";
                db.Database.ExecuteSqlRaw(_query_update);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
           
            return Ok(p);
             
        }



        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult<HistoryModel>> DeleteRecord(int id) //Delete
        {

            PurchaseOrderPaymentModel p = db.PurchaseOrderPayments.Find(id);
            PurchaseOrderModel s = db.PurchaseOrders.Find(p.purchase_order_id);
            p.is_deleted = true;
            db.PurchaseOrderPayments.Update(p);
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)),hub);

            db.Database.ExecuteSqlRaw("exec  sp_update_purchase_order_payment " + p.purchase_order_id);
            return Ok();

        }

    }
}