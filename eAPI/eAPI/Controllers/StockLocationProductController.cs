using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using eModels;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NETCore.Encrypt;


namespace eAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class StockLocationProductController : ODataController
    {

        private readonly ApplicationDbContext db;
        public StockLocationProductController(ApplicationDbContext _db)
        {
            db = _db;
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]   
        public IQueryable<StockLocationProductModel> Get()
        {            
            return db.StockLocationProducts;
        }
        
        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] StockLocationProductModel u)
        {            
            if (u.id == 0)
            {
                db.StockLocationProducts.Add(u);
            }
            else
            {
                db.StockLocationProducts.Update(u);
            }            
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return Ok(u);
        }

        [HttpGet("find")]
        [EnableQuery(MaxExpansionDepth = 4)]
        public SingleResult<StockLocationProductModel> Get([FromODataUri] int key)
        {
            var s = db.StockLocationProducts.Where(r => r.id == key).AsQueryable();
            return SingleResult.Create(s);
        }
    }
}
