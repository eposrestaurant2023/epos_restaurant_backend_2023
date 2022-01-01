using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
    public class ModifierGroupProductCategoryController : ODataController
    {
        private readonly ApplicationDbContext db;
        private readonly IHubContext<ConnectionHub> hub;
        public ModifierGroupProductCategoryController(ApplicationDbContext _db,IHubContext<ConnectionHub> _hub)
        {
            db = _db;hub = _hub;
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [AllowAnonymous]

        public IQueryable<ModifierGroupProductCategoryModel> Get()
        {
            return db.ModifierGroupProductCategories;
        }

        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] ModifierGroupProductCategoryModel u)
        {
            if (u.id == 0)
            {
                db.ModifierGroupProductCategories.Add(u);
            }
            else
            {
                db.ModifierGroupProductCategories.Update(u);
            }
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)),hub);
            return Ok(u);
        }

        [HttpGet("find")]
        [EnableQuery(MaxExpansionDepth = 4)]
        public SingleResult<ModifierGroupProductCategoryModel> Get([FromODataUri] int key)
        {
            var s = db.ModifierGroupProductCategories.Where(r => r.product_category_id == key).AsQueryable();
            return SingleResult.Create(s);
        }
    }

}
