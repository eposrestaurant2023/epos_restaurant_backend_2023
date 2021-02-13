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
    public class StockTakeController : ODataController
    {
        private readonly ApplicationDbContext db;
        private readonly AppService app;
        public StockTakeController(ApplicationDbContext _db, AppService _app)
        {
            db = _db;
            app = _app;

        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]

        public IQueryable<StockTakeModel> Get(string keyword = "")
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                return db.StockTakes.Where(r =>
                (
                (r.document_number ?? "") +
                (r.reference_number ?? "")
                ).ToLower().Trim().Contains(keyword.ToLower().Trim()));
            }
            else
            {
                return db.StockTakes;
            }
        }

        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] StockTakeModel p)
        {
            if (p.id == 0)
            {
                var doc = db.DocumentNumbers.Where(r => r.document_name == "Stock Take Document");
                string document_number = await app.GetDocumentNumber(doc.FirstOrDefault());
                p.document_number = document_number;
            }

            if (p.id == 0)
            {
                db.StockTakes.Add(p);
            }
            else
            {

                db.StockTakes.Update(p);

            }

            AddHistory(p);

            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return Ok(p);
        }
        void AddHistory(StockTakeModel s)
        {
            HistoryModel h = new HistoryModel("New Stock Take Created");
            h.module = "stock_take";
            h.document_number = s.document_number;

            h.description = $"{(s.id == 0 ? "Stock Take Updated." : "New Stock Take Created.")} Stock Take Document Number#: {s.document_number}.";

            s.histories.Add(h);
        }
        [HttpPost]
        [Route("status/{id}")]
        public async Task<ActionResult<StockTakeModel>> UpdateStatus(int id)
        {
            var d = await db.StockTakes.FindAsync(id);
            d.status = !d.status;
            db.StockTakes.Update(d);
            await db.SaveChangesAsync();
            return Ok(d);
        }


        [HttpGet("getsingle")]
        [EnableQuery(MaxExpansionDepth = 8)]
        public async Task<SingleResult<StockTakeModel>> Get([FromODataUri] int key)
        {
            return await Task.Factory.StartNew(() => SingleResult.Create<StockTakeModel>(db.StockTakes.Where(r => r.id == key).AsQueryable()));
        }

        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult<StockTakeModel>> DeleteRecord(int id) //Delete
        {
            var u = await db.StockTakes.FindAsync(id);
            u.is_deleted = !u.is_deleted;
            db.StockTakes.Update(u);
            await db.SaveChangesAsync();
            return Ok(u);
        }

        [HttpPost]
        [Route("clone/{id}")]
        public async Task<ActionResult<StockTakeModel>> CloneRecord(int id) // copy data to create new
        {
            var u = await db.StockTakes.FindAsync(id);
            u.id = 0;
            u.document_number = "";
            u.status = true;
            u.is_deleted = false;
            u.created_date = DateTime.Now;
            return Ok(u);
        }


        [HttpPost]
        [Route("MarkAsFulfilled/{id}")]
        public async Task<ActionResult> MarkAsFulfilled(int id) //mark as fullfileld 
        {

            UserModel user = await db.Users.FindAsync(Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));

            StockTakeModel s = db.StockTakes.Find(id);

            s.is_fulfilled = true;
            db.StockTakes.Update(s);
            await db.SaveChangesAsync();
            //add to history 
            //var data = db.StockTakesProducts.Where(r => r.purchase_order_id == id && r.is_deleted == false);
            //List<InventoryTransactionModel> inv_list = new List<InventoryTransactionModel>();
            //foreach (PurchaseOrderProductModel d in data.Where(r => r.is_inventory_product))
            //{

            //    d.is_fulfilled = true;
            //    InventoryTransactionModel inv = new InventoryTransactionModel();
            //    inv.inventory_transaction_type_id = 2;
            //    inv.stock_location_id = s.stock_location_id;
            //    inv.stock_in_id = s.id;
            //    inv.product_id = d.product_id;
            //    inv.stock_in_product_id = d.id;
            //    inv.quantity = d.quantity;
            //    inv.unit = d.unit;
            //    inv.multiplier = d.multiplier;
            //    inv.created_by = user.created_by;
            //    inv.note = $"Stock In Fulfilled ({s.document_number})";

            //    inv_list.Add(inv);
            //}

            //if (inv_list.Count() > 0)
            //{
            //    db.InventoryTransactions.AddRange(inv_list);
            //}

            try
            {
                await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            }
            catch (Exception ex)
            {

            }
            //db.Database.ExecuteSqlRaw("exec [sp_stock_in_update_to_inventory] " + s.id);
            return Ok();

        }



        [HttpPost]
        [Route("CancelFulfilled/{id}")]
        public async Task<ActionResult> CancelFulfilled(int id) //mark as fullfileld 
        {
            UserModel user = await db.Users.FindAsync(Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));

            StockTakeModel s = db.StockTakes.Find(id);

            s.is_fulfilled = false;
            db.StockTakes.Update(s);
            await db.SaveChangesAsync();
            //add to history 
            //var data = db.StockTakesProducts.Where(r => r.purchase_order_id == id && r.is_deleted == false);
            //List<InventoryTransactionModel> inv_list = new List<InventoryTransactionModel>();
            //foreach (PurchaseOrderProductModel d in data.Where(r => r.is_inventory_product))
            //{
            //    d.is_fulfilled = false;

            //    InventoryTransactionModel inv = new InventoryTransactionModel();

            //    inv.inventory_transaction_type_id = 2;
            //    inv.stock_location_id = s.stock_location_id;
            //    inv.sale_id = s.id;
            //    inv.product_id = d.product_id;
            //    inv.sale_product_id = d.id;
            //    inv.product_variant_id = d.product_variant_id;
            //    inv.quantity = d.quantity * -1;
            //    inv.unit = d.unit;
            //    inv.multiplier = d.multiplier;
            //    inv.created_by = user.created_by;
            //    inv.note = $"PO Canceled Fulfilled ({s.document_number})";

            //    inv_list.Add(inv);
            //}
            //if (inv_list.Count() > 0)
            //{
            //    db.InventoryTransactions.AddRange(inv_list);
            //}


            try
            {
                await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            }
            catch (Exception ex)
            {

            }
            //db.Database.ExecuteSqlRaw("exec [sp_po_update_to_inventory] " + s.id);
            return Ok();

        }
    }
}