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
    public class ProductionController : ODataController
    {
        private readonly ApplicationDbContext db;
        private readonly AppService app;
        public ProductionController(ApplicationDbContext _db, AppService _app)
        {
            db = _db;
            app = _app;

        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]

        public IQueryable<ProductionModel> Get(string keyword = "")
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                return db.Productions.Where(r =>
                (
                (r.production_code ?? "") +
                (r.reference_number ?? "")
                ).ToLower().Trim().Contains(keyword.ToLower().Trim()));
            }
            else
            {
                return db.Productions;
            }
        }
        [HttpPost]
        [Route("clone/{id}")]
        public ActionResult<ProductionModel> Clone(int id)
        {
            var data = db.Productions.Where(r => r.id == id)
                .Include(r => r.production_products.Where(r => r.is_deleted == false)).ThenInclude(r => r.product).ThenInclude(r => r.unit)
                .ToList();

            if (data.Any())
            {
                ProductionModel u = data.FirstOrDefault();
                u.id = 0;
                u.production_code = "";
                u.reference_number = "";
                u.status = true;
                u.is_deleted = false;
                u.created_date = DateTime.Now;
                u.is_fulfilled = false;
                u.production_products.ForEach(r => r.id = 0);

                return Ok(u);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] ProductionModel p)
        {
            

            if (p.id == 0)
            {
                db.Productions.Add(p);
            }
            else
            {

                db.Productions.Update(p);

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
          
        }
        void AddHistory(ProductionModel s)
        {
            HistoryModel h = new HistoryModel("New Production Created");
            h.module = "production";
            h.document_number = s.production_code;

            h.description = $"{(s.id == 0 ? "Production." : "New Production Created.")} Production Code#: {s.production_code}.";

            s.histories.Add(h);
        }
        [HttpPost]
        [Route("status/{id}")]
        public async Task<ActionResult<ProductionModel>> UpdateStatus(int id)
        {
            var d = await db.Productions.FindAsync(id);
            d.status = !d.status;
            db.Productions.Update(d);
            await db.SaveChangesAsync();
            return Ok(d);
        }


        [HttpGet("getsingle")]
        [EnableQuery(MaxExpansionDepth = 8)]
        public async Task<SingleResult<ProductionModel>> Get([FromODataUri] int key)
        {
            return await Task.Factory.StartNew(() => SingleResult.Create<ProductionModel>(db.Productions.Where(r => r.id == key).AsQueryable()));
        }

        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult<ProductionModel>> DeleteRecord(int id) //Delete
        {
            var u = await db.Productions.FindAsync(id);
            u.is_deleted = !u.is_deleted;
            db.Productions.Update(u);
            await db.SaveChangesAsync();
            return Ok(u);
        }
 
        [HttpPost]
        [Route("MarkAsFulfilled/{id}")]
        public async Task<ActionResult> MarkAsFulfilled(int id) //mark as fullfileld 
        {

            UserModel user = await db.Users.FindAsync(Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));

            ProductionModel s = db.Productions.Find(id);

            s.is_fulfilled = true;
            db.Productions.Update(s);
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            // add to history
            //db.Database.ExecuteSqlRaw($"exec sp_update_stock_take_inventory_transaction {id}");
            return Ok();

        }


    }
}