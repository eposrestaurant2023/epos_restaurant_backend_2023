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
        [HttpPost]
        [Route("clone/{id}")]
        public ActionResult<StockTakeModel> Clone(int id)
        {
            var data = db.StockTakes.Where(r => r.id == id)
                .Include(r => r.stock_take_products.Where(r => r.is_deleted == false)).ThenInclude(r => r.product).ThenInclude(r => r.unit)
                .ToList();

            if (data.Any())
            {
                StockTakeModel u = data.FirstOrDefault();
                u.id = 0;
                u.document_number = "";
                u.reference_number = "";
                u.status = true;
                u.is_deleted = false;
                u.created_date = DateTime.Now;
                u.is_fulfilled = false;
                u.stock_take_products.ForEach(r => r.id = 0);

                return Ok(u);
            }
            else
            {
                return NotFound();
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

            try
            {
                await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));


                return Ok(p);
            }
            catch(Exception ex) {
                return StatusCode(415,"Save data fail. Please try again.");
            }
            return NotFound();
          
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
        [Route("MarkAsFulfilled/{id}")]
        public async Task<ActionResult> MarkAsFulfilled(int id) //mark as fullfileld 
        {

            UserModel user = await db.Users.FindAsync(Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));

            StockTakeModel s = db.StockTakes.Find(id);

            s.is_fulfilled = true;
            db.StockTakes.Update(s);
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            // add to history
            db.Database.ExecuteSqlRaw($"exec sp_update_stock_take_inventory_transaction {id}");
            return Ok();

        }

        [HttpPost]
        [Route("CancelMarkAsFulfilled/{id}")]
        public async Task<ActionResult> CancelMarkAsFulfilled(int id) //mark as fullfileld 
        {

            UserModel user = await db.Users.FindAsync(Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));

            StockTakeModel s = db.StockTakes.Find(id);

            s.is_fulfilled = false;
            db.StockTakes.Update(s);
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            // add to history
            db.Database.ExecuteSqlRaw($"exec sp_update_stock_take_inventory_transaction {id}");
            return Ok();

        }

    }
}