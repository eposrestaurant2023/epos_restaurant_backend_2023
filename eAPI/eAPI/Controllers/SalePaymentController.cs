﻿using System;
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
    public class SalePaymentController : ODataController
    {
        private readonly ApplicationDbContext db;
        private readonly AppService app;
        public SalePaymentController(ApplicationDbContext _db, AppService _app)
        {
            db = _db;
            app = _app;
        }        

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]   
        public IQueryable<SalePaymentModel> Get(string keyword = "")
        {
            if (!string.IsNullOrEmpty(keyword))
            {
              return (from r in db.SalePayments
                           where 
                                 EF.Functions.Like((
                                 (r.reference_number ?? " ")).ToLower().Trim(), $"%{keyword}%".ToLower().Trim())
                           select r);

            }
            else
            {
                return db.SalePayments.AsQueryable();
            }
        }

        [HttpGet("getsingle")]
        [EnableQuery(MaxExpansionDepth = 8)]     
        public async Task<SingleResult<SalePaymentModel>> Get([FromODataUri] Guid key)
        {
            return await Task.Factory.StartNew(() => SingleResult.Create<SalePaymentModel>(db.SalePayments.Where(r => r.id == key).AsQueryable()));
        }

        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] SalePaymentModel p)
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
                var payments = db.SalePayments.Where(r => r.sale_id == p.sale_id && !r.is_deleted && r.id!=p.id);
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
                db.SalePayments.Add(p);
            }
            else
            {
                db.SalePayments.Update(p);
            }
             
                
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            //db.Database.ExecuteSqlRaw("exec sp_update_sale_payment " + p.sale_id);
            return Ok(p);
             
        }

        [HttpPost("save/multiple")]
        public async Task<ActionResult<string>> SaveMultiple([FromBody] List<SalePaymentModel> p)
        {
            try
            { 
                // histroy record
                foreach (var r in p)
                {
                    var sale = await db.Sales.FindAsync(r.sale_id);
                    HistoryModel h = new HistoryModel("Create New Payment");
                    h.description = $"Create new payment. Invoice Number: {sale.document_number}";
                    h.document_number = sale.document_number;
                    h.customer_id = sale.customer_id;
                    h.sale_id = sale.id;
                    p.Where(r => r.sale_id == h.sale_id).ToList().ForEach(r => r.histories.Add(h));
                }

                db.SalePayments.UpdateRange(p);
                await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
                return Ok(p);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }

            return Ok();

        }



        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult<HistoryModel>> DeleteRecord(Guid id) //Delete
        {

            SalePaymentModel p = db.SalePayments.Find(id);
            SaleModel s = db.Sales.Find(p.sale_id);
            HistoryModel h = new HistoryModel("Delete Sale Payment");
            h.description = $"Delete Sale Payment. Invoice Number: {s.document_number}";
            h.document_number = s.document_number;
            h.customer_id = s.customer_id;
            h.sale_id = s.id;
            p.histories.Add(h);
            p.is_deleted = true;
            db.SalePayments.Update(p);
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));

            //db.Database.ExecuteSqlRaw("exec  sp_update_sale_payment " + p.sale_id);
            return Ok(h);
             
        }

         

    }
}