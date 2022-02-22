using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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
    public class DefaultStockLocationProductController : ODataController
    {

        private readonly ApplicationDbContext db;
        private readonly IHubContext<ConnectionHub> hub;
        public DefaultStockLocationProductController(ApplicationDbContext _db,IHubContext<ConnectionHub> _hub)
        {
            db = _db;hub = _hub;
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [AllowAnonymous]
        public IQueryable<DefaultStockLocationProductModel> Get()
        {
            return db.defaultStockLocationProducts;

        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [Route("getsingle")]
        public async Task<SingleResult<DefaultStockLocationProductModel>> GetQuery([FromODataUri] Guid key)
        {
            return await Task.Factory.StartNew(() => SingleResult.Create<DefaultStockLocationProductModel>(db.defaultStockLocationProducts.Where(r => r.id == key).AsQueryable()));
        }

        [HttpGet("category")]
        [EnableQuery(MaxExpansionDepth = 8)]
        public IQueryable<DefaultStockLocationProductModel> GetCategoryNote()
        {
            return db.defaultStockLocationProducts;
        }


        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] DefaultStockLocationProductModel u)
        {
            if (u.id == Guid.Empty)
            {
                db.defaultStockLocationProducts.Add(u);
            }
            else
            {
                db.defaultStockLocationProducts.Update(u);
            }
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)),hub);
            return Ok(u);
        }

        [HttpGet("find")]
        [EnableQuery(MaxExpansionDepth = 4)]
        public SingleResult<DefaultStockLocationProductModel> Get([FromODataUri] Guid key)
        {
            var s = db.defaultStockLocationProducts.Where(r => r.id == key).AsQueryable();
            return SingleResult.Create(s);
        }

        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult<DefaultStockLocationProductModel>> DeleteRecord(int id) //Delete
        {
            var u = await db.defaultStockLocationProducts.FindAsync(id);
            db.defaultStockLocationProducts.Update(u);
            await db.SaveChangesAsync();
            return Ok(u);
        }


    }
}
