using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using eAPI.Hubs;
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
    [Authorize]
    [Route("api/[controller]")]
    public class PurchaseOrderProductController : ODataController
    {

        private readonly ApplicationDbContext db;
        private readonly IHubContext<ConnectionHub> hub;
        public PurchaseOrderProductController(ApplicationDbContext _db,IHubContext<ConnectionHub> _hub)
        {
            db = _db;hub = _hub;
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 1000, MaxNodeCount=2000)]
        public IQueryable<PurchaseOrderProductModel> Get(string keyword = "")
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                return (from r in db.PurchaseOrderProducts
                        where
                              EF.Functions.Like(
                                  (
                                     (r.purchase_order.document_number ?? " ") +
                                     (r.purchase_order.vendor.vendor_name ?? " ")
                                  ).ToLower().Trim(), $"%{keyword}%".ToLower().Trim())
                        select r);

            }
            else
            {
                return db.PurchaseOrderProducts.AsQueryable();
            }
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [AllowAnonymous]
        public IQueryable<PurchaseOrderProductModel> Get()
        {

            return db.PurchaseOrderProducts;

        }


        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] PurchaseOrderProductModel u)
        {

            if (u.id == 0)
            {
                db.PurchaseOrderProducts.Add(u);
            }
            else
            {
                db.PurchaseOrderProducts.Update(u);
            }

            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)),hub);
            return Ok(u);


        }

        [HttpPost("save/multiple")]
        public async Task<ActionResult<string>> SaveMultiple([FromBody] List<PurchaseOrderProductModel> branches)
        {
            db.PurchaseOrderProducts.UpdateRange(branches);

            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)),hub);

            db.Database.ExecuteSqlRaw("exec sp_clear_deleted_record");
            return Ok(branches);
        }

        [HttpGet("find")]
        [EnableQuery(MaxExpansionDepth = 4)]
        public SingleResult<PurchaseOrderProductModel> Get([FromODataUri] int key)
        {
            var s = db.PurchaseOrderProducts.Where(r => r.id == key).AsQueryable();

            return SingleResult.Create(s);
        }

        [HttpPost]
        [Route("status/{id}")]
        public async Task<ActionResult<PurchaseOrderProductModel>> UpdateStatus(Guid id)
        {
            var d = await db.PurchaseOrderProducts.FindAsync(id);
            d.status = !d.status;
            db.PurchaseOrderProducts.Update(d);
            await db.SaveChangesAsync();
            return Ok(d);
        }

        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult<PurchaseOrderProductModel>> DeleteRecord(Guid id) //Delete
        {
            var u = await db.PurchaseOrderProducts.FindAsync(id);
            u.is_deleted = !u.is_deleted;

            db.PurchaseOrderProducts.Update(u);
            await db.SaveChangesAsync();
            return Ok(u);
        }
    }

}
