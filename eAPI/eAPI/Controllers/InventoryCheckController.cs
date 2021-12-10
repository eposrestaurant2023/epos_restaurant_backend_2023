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
using NETCore.Encrypt;

namespace eAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class InventoryCheckController : ODataController
    {
        private readonly ApplicationDbContext db;
        private readonly AppService app;
        public InventoryCheckController(ApplicationDbContext _db, AppService _app)
        {
            db = _db;
            app = _app;

        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]

        public IQueryable<InventoryCheckModel> Get(string keyword = "")
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                return db.InventoryChecks.Where(r =>
                (
                (r.document_number ?? "") +
                (r.reference_number ?? "") 
                ).ToLower().Trim().Contains(keyword.ToLower().Trim()));
            }
            else
            {
                return db.InventoryChecks;
            }
        }

        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] InventoryCheckModel p)
        {
            if (p.id==Guid.Empty)
            {
                var doc = db.DocumentNumbers.Where(r => r.document_name == "Inventory Check Document" );
                string document_number = await app.GetDocumentNumber(doc.FirstOrDefault());
                p.document_number = document_number;
            }

            if (p.id ==Guid.Empty)
            {
                db.InventoryChecks.Add(p);
            }
            else
            {

                db.InventoryChecks.Update(p);

            }

            AddHistory(p);

            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            db.Database.ExecuteSqlRaw($"exec sp_update_inventory_check '{p.id}'");
            return Ok(p);
        }
        void AddHistory(InventoryCheckModel s)
        {
            HistoryModel h = new HistoryModel("New Inventory Check Added");
            h.module = "Inventory Checked";
            h.document_number = s.document_number;
            h.description = $"{(s.id != Guid.Empty ? "Inventory Check Updated." : "New Inventory Check Added.")} Inventory Check Document Number#: {s.document_number}.";
            s.histories.Add(h);
        }
        [HttpPost]
        [Route("status/{id}")]
        public async Task<ActionResult<InventoryCheckModel>> UpdateStatus(int id)
        {
            var d = await db.InventoryChecks.FindAsync(id);
            d.status = !d.status;
            db.InventoryChecks.Update(d);
            await db.SaveChangesAsync();
            return Ok(d);
        }


        [HttpGet("getsingle")]
        [EnableQuery(MaxExpansionDepth = 8)]
        public async Task<SingleResult<InventoryCheckModel>> Get([FromODataUri] Guid key)
        {
            return await Task.Factory.StartNew(() => SingleResult.Create<InventoryCheckModel>(db.InventoryChecks.Where(r => r.id == key).AsQueryable()));
        }

        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult<InventoryCheckModel>> DeleteRecord(Guid id) //Delete
        {
            var u = await db.InventoryChecks.FindAsync(id);
            u.is_deleted = !u.is_deleted;
            db.InventoryChecks.Update(u);
            await db.SaveChangesAsync();
            return Ok(u);
        }

        


        [HttpPost]
        [Route("MarkAsFulfilled/{id}")]
        public async Task<ActionResult> MarkAsFulfilled(Guid id) //mark as fullfileld 
        {

            UserModel user = await db.Users.FindAsync(Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));

            InventoryCheckModel s = db.InventoryChecks.Find(id);

            s.is_fulfilled = true;
            db.InventoryChecks.Update(s);
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            // add to history
          //  db.Database.ExecuteSqlRaw($"exec sp_update_purchase_order_inventory_transaction {id}");
            return Ok();

        }

        [HttpPost]
        [Route("CancelMarkAsFulfilled/{id}")]
        public async Task<ActionResult> CancelMarkAsFulfilled(Guid id) //mark as fullfileld 
        {

            UserModel user = await db.Users.FindAsync(Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));

            InventoryCheckModel s = db.InventoryChecks.Find(id);

            s.is_fulfilled = false;
            db.InventoryChecks.Update(s);
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            // add to history
            //db.Database.ExecuteSqlRaw($"exec sp_update_purchase_order_inventory_transaction {id}");
            return Ok();

        }

    }
}