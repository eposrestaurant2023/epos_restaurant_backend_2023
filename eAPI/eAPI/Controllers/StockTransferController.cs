using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using eAPI.Hubs;
using eAPI.Services;
using eModels;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NETCore.Encrypt;

namespace eAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class StockTransferController : ODataController
    {
        private readonly ApplicationDbContext db;
        private readonly AppService app;
        private readonly IHubContext<ConnectionHub> hub;
        public StockTransferController(ApplicationDbContext _db, AppService _app, IHubContext<ConnectionHub> _hub)
        {
            db = _db;hub = _hub;
            app = _app;

        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 10,MaxNodeCount =200)]
        public IQueryable<StockTransferModel> Get(string keyword = "")
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                return db.StockTransfers.Where(r =>
                (
                (r.document_number ?? "") +
                (r.reference_number ?? "")+
                (r.to_business_branch.business_branch_name_en ?? "")+
                (r.to_business_branch.business_branch_name_kh ?? "")+
                (r.from_business_branch.business_branch_name_kh ?? "")+
                (r.from_business_branch.business_branch_name_en ?? "")+
                (r.from_stock_location.stock_location_name ?? "")+
                (r.to_stock_location.stock_location_name ?? "")
                ).ToLower().Trim().Contains(keyword.ToLower().Trim()));
            }
            else
            {
                return db.StockTransfers;
            }
        }

        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] StockTransferModel p)
        {
            if (p.id == 0)
            {
                var doc = db.DocumentNumbers.Where(r => r.document_name == "Stock Transfer Document");
                string document_number = await app.GetDocumentNumber(doc.FirstOrDefault());
                p.document_number = document_number;
            }

            if (p.id == 0)
            {
                db.StockTransfers.Add(p);
            }
            else
            {

                db.StockTransfers.Update(p);

            }

            AddHistory(p);

            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)),hub);
            return Ok(p);
        }
        void AddHistory(StockTransferModel s)
        {
            HistoryModel h = new HistoryModel("New Stock Transfer Created");
            h.module = "stock_transfer";
            h.document_number = s.document_number;

            h.description = $"{(s.id == 0 ? "Stock Transfer Updated." : "New Stock Transfer Created.")} Stock Transfer Document Number#: {s.document_number}.";

            s.histories.Add(h);
        }
        [HttpPost]
        [Route("status/{id}")]
        public async Task<ActionResult<StockTransferModel>> UpdateStatus(int id)
        {
            var d = await db.StockTransfers.FindAsync(id);
            d.status = !d.status;
            db.StockTransfers.Update(d);
            await db.SaveChangesAsync();
            return Ok(d);
        }


        [HttpGet("getsingle")]
        [EnableQuery(MaxExpansionDepth = 8)]
        public async Task<SingleResult<StockTransferModel>> Get([FromODataUri] int key)
        {
            return await Task.Factory.StartNew(() => SingleResult.Create<StockTransferModel>(db.StockTransfers.Where(r => r.id == key).AsQueryable()));
        }

        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult<StockTransferModel>> DeleteRecord(int id) //Delete
        {
            var u = await db.StockTransfers.FindAsync(id);
            u.is_deleted = !u.is_deleted;
            db.StockTransfers.Update(u);
            await db.SaveChangesAsync();
            return Ok(u);
        }
        [HttpPost]
        [Route("clone/{id}")]
        public ActionResult<StockTransferModel> Clone(int id)
        {
            var data = db.StockTransfers.Where(r => r.id == id)
                .Include(r => r.stock_transfer_products.Where(r => r.is_deleted == false)).ThenInclude(r => r.product).ThenInclude(r => r.unit)
                .ToList();

            if (data.Any())
            {
                StockTransferModel u = data.FirstOrDefault();
                u.id = 0;
                u.document_number = "";
                u.reference_number = "";
                u.status = true;
                u.is_deleted = false;
                u.created_date = DateTime.Now;
                u.is_fulfilled = false;
                u.stock_transfer_products.ForEach(r => r.id = 0);

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

            StockTransferModel s = db.StockTransfers.Find(id);

            s.is_fulfilled = true;
            db.StockTransfers.Update(s);

            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)),hub);
            // add to history
            db.Database.ExecuteSqlRaw($"exec sp_update_stock_transfer_out_inventory_transaction {id};exec sp_update_stock_transfer_in_inventory_transaction {id}");
            return Ok();

        }

        [HttpPost]
        [Route("CancelMarkAsFulfilled/{id}")]
        public async Task<ActionResult> CancelMarkAsFulfilled(int id) //mark as fullfileld 
        {

            UserModel user = await db.Users.FindAsync(Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));

            StockTransferModel s = db.StockTransfers.Find(id);

            s.is_fulfilled = false;
            db.StockTransfers.Update(s);
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)),hub);
            // add to history
            db.Database.ExecuteSqlRaw($"exec sp_update_stock_transfer_out_inventory_transaction {id};exec sp_update_stock_transfer_in_inventory_transaction {id}");
            return Ok();

        }


    }
}