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
    public class InventoryCheckProductController : ODataController
    {
        private readonly ApplicationDbContext db;
        private readonly AppService app;
        private readonly IHubContext<ConnectionHub> hub;
        public InventoryCheckProductController(ApplicationDbContext _db, AppService _app, IHubContext<ConnectionHub> _hub)
        {
            db = _db;hub = _hub;
            app = _app;

        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]

        public IQueryable<InventoryCheckProductModel> Get(string keyword = "")
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                return db.InventoryCheckProduts.Where(r =>
                (
                (r.product.product_display_name ?? "") +
                (r.product.product_name_en ?? "") +
                (r.product.product_name_kh ?? "") +
                (r.product.product_code ?? "") 
                ).ToLower().Trim().Contains(keyword.ToLower().Trim()));
            }
            else
            {
                return db.InventoryCheckProduts;
            }
        }

        [HttpPost("save")]
        public async Task<ActionResult<InventoryCheckProductModel>> Save([FromBody] InventoryCheckProductModel p)
        {
            var data =  db.InventoryCheckProduts.Where(r=>r.id == p.id).AsNoTracking().FirstOrDefault();
            data.actual_quantity = p.actual_quantity;
            db.InventoryCheckProduts.Update(data);
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)), hub);
            return Ok(p);
        }
       

    }
}