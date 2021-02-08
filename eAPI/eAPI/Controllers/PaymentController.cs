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
    public class PaymentController : ODataController
    {
        private readonly ApplicationDbContext db;
        private readonly AppService app;
        public PaymentController(ApplicationDbContext _db, AppService _app)
        {
            db = _db;
            app = _app;
        }        

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]   
        public IQueryable<PaymentModel> Get(string keyword = "")
        {
            if (!string.IsNullOrEmpty(keyword))
            {
              return (from r in db.Payments
                           where 
                                 EF.Functions.Like((
                                 (r.reference_number ?? " ")).ToLower().Trim(), $"%{keyword}%".ToLower().Trim())
                           select r);

            }
            else
            {
                return db.Payments.AsQueryable();
            }
        }

        [HttpGet("getsingle")]
        [EnableQuery(MaxExpansionDepth = 8)]     
        public async Task<SingleResult<PaymentModel>> Get([FromODataUri] Guid key)
        {
            return await Task.Factory.StartNew(() => SingleResult.Create<PaymentModel>(db.Payments.Where(r => r.id == key).AsQueryable()));
        }

        [HttpPost("sale/save")]
        public async Task<ActionResult<string>> Save([FromBody] PaymentModel p)
        {
            //validat check if payment is over sale amount
            SaleModel s = db.Sales.Find(p.sale_id);
            if (p.id == Guid.Empty)
            {
               
                if (s.balance < p.payment_amount)
                {
                    return StatusCode(300, "Payment amount cannot greater than Balance Amount");
                }

            }else
            {
                var payments = db.Payments.Where(r => r.sale_id == p.sale_id && !r.is_deleted && r.id!=p.id);
                if (payments.Any())
                {
                    if (payments.Sum(r => r.payment_amount) + p.payment_amount > s.total_amount)
                    {
                        return StatusCode(300, "Payment amount cannot greater than Balance Amount");
                    }
                }
                
            }
            //end check validation

            HistoryModel h = new HistoryModel($"{(p.id != Guid.Empty? "Update Sale Payment":"Create New Payment" )}");
            h.description = $"{(p.id != Guid.Empty ? "Update sale payment." : "Create new payment.")} Invoice Number: {s.document_number}";
            h.document_number = s.document_number;
            h.customer_id = s.customer_id;
            h.sale_id = s.id;
            p.histories.Add(h);


            if (p.id == Guid.Empty)
            {
                db.Payments.Add(p);
            }
            else
            {
                db.Payments.Update(p);
            }
             
                
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            db.Database.ExecuteSqlRaw("exec  sp_update_sale_payment " + p.sale_id);
            return Ok(p);
             
        }

        [HttpPost("purchaseorder/save")]
        public async Task<ActionResult<string>> SavePO([FromBody] PaymentModel p)
        {
            //validat check if payment is over sale amount
            PurchaseOrderModel s = db.PurchaseOrders.Find(p.purchase_order_id);
            if (p.id == Guid.Empty)
            {

                if (s.balance < p.payment_amount)
                {
                    return StatusCode(300, "Payment amount cannot greater than Balance Amount");
                }

            }
            else
            {
                var payments = db.Payments.Where(r => r.purchase_order_id == p.purchase_order_id && !r.is_deleted && r.id != p.id);
                if (payments.Any())
                {
                    if (payments.Sum(r => r.payment_amount) + p.payment_amount > s.total_amount)
                    {
                        return StatusCode(300, "Payment amount cannot greater than Balance Amount");
                    }
                }

            }
            //end check validation

            HistoryModel h = new HistoryModel($"{(p.id == Guid.Empty ? "Update PO Payment" : "Create New PO")}");
            h.description = $"{(p.id != Guid.Empty ? "Update PO payment." : "Create new PO.")} Invoice Number: {s.document_number}";
            h.document_number = s.document_number;
            h.vendor_id = s.vendor_id;
            h.purchase_order_id = s.id;
            p.histories.Add(h);


            if (p.id == Guid.Empty)
            {
                db.Payments.Add(p);
            }
            else
            {
                db.Payments.Update(p);
            }


            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            //Created 
            db.Database.ExecuteSqlRaw("exec  sp_update_purchase_order_payment " + p.purchase_order_id);
            return Ok(p);

        }


        [HttpPost]
        [Route("sale/delete/{id}")]
        public async Task<ActionResult<HistoryModel>> DeleteRecord(int id) //Delete
        {

            PaymentModel p = db.Payments.Find(id);
            SaleModel s = db.Sales.Find(p.sale_id);
            HistoryModel h = new HistoryModel("Delete Sale Payment");
            h.description = $"Delete Sale Payment. Invoice Number: {s.document_number}";
            h.document_number = s.document_number;
            h.customer_id = s.customer_id;
            h.sale_id = s.id;
            p.histories.Add(h);
            p.is_deleted = true;
            db.Payments.Update(p);
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));

            db.Database.ExecuteSqlRaw("exec  sp_update_sale_payment " + p.sale_id);
            return Ok(h);
             
        }

        [HttpPost]
        [Route("purchaseorder/delete/{id}")]
        public async Task<ActionResult<HistoryModel>> DeletePORecord(int id) //Delete
        {

            PaymentModel p = db.Payments.Find(id);
            PurchaseOrderModel s = db.PurchaseOrders.Find(p.purchase_order_id);
            HistoryModel h = new HistoryModel("Delete PO Payment");
            h.description = $"Delete PO Payment. Invoice Number: {s.document_number}";
            h.document_number = s.document_number;
            h.vendor_id = s.vendor_id;
            h.purchase_order_id = s.id;
            p.histories.Add(h);
            p.is_deleted = true;
            db.Payments.Update(p);
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));

            db.Database.ExecuteSqlRaw("exec  sp_update_purchase_order_payment " + p.purchase_order_id);
            return Ok(h);

        }

    }
}