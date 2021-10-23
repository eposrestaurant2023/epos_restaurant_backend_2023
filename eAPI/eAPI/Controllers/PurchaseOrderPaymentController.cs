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

namespace eAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PurchaseOrderPaymentController : ODataController
    {
        private readonly ApplicationDbContext db;
        private readonly AppService app;
        public PurchaseOrderPaymentController(ApplicationDbContext _db, AppService _app)
        {
            db = _db;
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
            PurchaseOrderModel s = db.PurchaseOrders.Find(p.purchase_order_id);
            if (p.id ==0)
            {
               
                if (s.balance < p.payment_amount)
                {
                    return StatusCode(300, "Payment amount cannot greater than Balance Amount");
                }

            }else
            {
                var PurchaseOrderPayments = db.PurchaseOrderPayments.Where(r => r.purchase_order_id == p.purchase_order_id && !r.is_deleted && r.id != p.id);
                if (PurchaseOrderPayments.Any())
                {
                    if (PurchaseOrderPayments.Sum(r => r.payment_amount) + p.payment_amount > s.total_amount)
                    {
                        return StatusCode(300, "Payment amount cannot greater than Balance Amount");
                    }
                }
                
            }
            //end check validation

            //HistoryModel h = new HistoryModel($"{(p.id != Guid.Empty? "Update PO Payment":"Create New Payment" )}");
            //h.description = $"{(p.id != Guid.Empty ? "Update PO payment." : "Create new payment.")} Invoice Number: {s.document_number}";
            //h.document_number = s.document_number;
            //h.vendor_id = s.vendor_id;
            //h.purchase_order_id = s.id;

            //p.histories.Add(h);


            if (p.id == 0)
            {
                db.PurchaseOrderPayments.Add(p);
            }
            else
            {
                db.PurchaseOrderPayments.Update(p);
            }
             
                
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            string _query_update = $"exec sp_update_purchase_order_payment_currency_value {p.purchase_order_id},{p.id}; exec sp_update_purchase_order_payment {p.purchase_order_id}";
            db.Database.ExecuteSqlRaw(_query_update);
            return Ok(p);
             
        }



        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult<HistoryModel>> DeleteRecord(Guid id) //Delete
        {

            PurchaseOrderPaymentModel p = db.PurchaseOrderPayments.Find(id);
            PurchaseOrderModel s = db.PurchaseOrders.Find(p.purchase_order_id);
            //historymodel h = new historymodel("deleted po payment");
            //h.description = $"Deleted PO Payment. Invoice Number: {s.document_number}";
            //h.document_number = s.document_number;
            //h.vendor_id = s.vendor_id;
            //h.purchase_order_id = s.id;
            //p.histories.Add(h);
            p.is_deleted = true;
            db.PurchaseOrderPayments.Update(p);
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));

            db.Database.ExecuteSqlRaw("exec  sp_update_purchase_order_payment " + p.purchase_order_id);
            return Ok();

        }

    }
}