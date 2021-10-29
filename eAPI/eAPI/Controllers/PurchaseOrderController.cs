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
using NETCore.Encrypt;

namespace eAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PurchaseOrderController : ODataController
    {
        private readonly ApplicationDbContext db;
        private readonly AppService app;
        public PurchaseOrderController(ApplicationDbContext _db, AppService _app)
        {
            db = _db;
            app = _app;

        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]

        public IQueryable<PurchaseOrderModel> Get(string keyword = "")
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                return db.PurchaseOrders.Where(r =>
                (
                (r.document_number ?? "") +
                (r.reference_number ?? "") +
                (r.vendor.vendor_name ?? "")
                ).ToLower().Trim().Contains(keyword.ToLower().Trim()));
            }
            else
            {
                return db.PurchaseOrders;
            }
        }

        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] PurchaseOrderModel p)
        {
            if (p.id == 0)
            {
                var doc = db.DocumentNumbers.Where(r => r.document_name == "PO Document" );
                string document_number = await app.GetDocumentNumber(doc.FirstOrDefault());
                p.document_number = document_number;
            }

            if (p.id == 0)
            {
                db.PurchaseOrders.Add(p);
            }
            else
            {

                db.PurchaseOrders.Update(p);

            }

            AddHistory(p);

            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return Ok(p);
        }
        void AddHistory(PurchaseOrderModel s)
        {
            HistoryModel h = new HistoryModel("New PO Added");
            h.module = "purchase_order";
            h.document_number = s.document_number;

            h.description = $"{(s.id == 0 ? "PO Updated." : "New PO Added.")} PO Document Number#: {s.document_number}.";

            s.histories.Add(h);
        }
        [HttpPost]
        [Route("status/{id}")]
        public async Task<ActionResult<PurchaseOrderModel>> UpdateStatus(int id)
        {
            var d = await db.PurchaseOrders.FindAsync(id);
            d.status = !d.status;
            db.PurchaseOrders.Update(d);
            await db.SaveChangesAsync();
            return Ok(d);
        }


        [HttpGet("getsingle")]
        [EnableQuery(MaxExpansionDepth = 8)]
        public async Task<SingleResult<PurchaseOrderModel>> Get([FromODataUri] int key)
        {
            return await Task.Factory.StartNew(() => SingleResult.Create<PurchaseOrderModel>(db.PurchaseOrders.Where(r => r.id == key).AsQueryable()));
        }

        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult<PurchaseOrderModel>> DeleteRecord(int id) //Delete
        {
            var u = await db.PurchaseOrders.FindAsync(id);
            u.is_deleted = !u.is_deleted;
            db.PurchaseOrders.Update(u);
            await db.SaveChangesAsync();
            return Ok(u);
        }

        [HttpPost]
        [Route("clone/{id}")]
        public ActionResult<PurchaseOrderModel> Clone(int id)
        {
            var data = db.PurchaseOrders.Where(r => r.id == id)
                .Include(r => r.purchase_order_products.Where(r => r.is_deleted == false)).ThenInclude(r=>r.product).ThenInclude(r=>r.unit)
                .Include(r=>r.vendor)
                .ToList();

            if (data.Any())
            {
                PurchaseOrderModel u = data.FirstOrDefault();
                u.id = 0;
                u.document_number = "";
                u.reference_number = "";
                u.status = true;
                u.is_deleted = false;
                u.created_date = DateTime.Now;
                u.is_fulfilled = false;
                u.is_paid = false;
                u.is_over_due = false;
                u.is_partially_paid = false;
                u.purchase_order_products.ForEach(r => r.id = 0);

                return Ok(u);
            }
            else
            {
                return NotFound();
            }
        }


        [HttpPost]
        [Route("MarkAsFulfilled/{id}")]
        public async Task<ActionResult> MarkAsFulfilled(int id) //mark as fullfileld 
        {

            UserModel user = await db.Users.FindAsync(Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));

            PurchaseOrderModel s = db.PurchaseOrders.Find(id);

            s.is_fulfilled = true;
            db.PurchaseOrders.Update(s);
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            // add to history
            db.Database.ExecuteSqlRaw($"exec sp_update_purchase_order_inventory_transaction {id}");
            return Ok();

        }

        [HttpPost]
        [Route("CancelMarkAsFulfilled/{id}")]
        public async Task<ActionResult> CancelMarkAsFulfilled(int id) //mark as fullfileld 
        {

            UserModel user = await db.Users.FindAsync(Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));

            PurchaseOrderModel s = db.PurchaseOrders.Find(id);

            s.is_fulfilled = false;
            db.PurchaseOrders.Update(s);
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            // add to history
            db.Database.ExecuteSqlRaw($"exec sp_update_purchase_order_inventory_transaction {id}");
            return Ok();

        }

    }
}