using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
    public class ProductGroupController : ODataController
    {

        private readonly ApplicationDbContext db;
        public ProductGroupController(ApplicationDbContext _db)
        {
            db = _db;
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        
        public IQueryable<ProductGroupModel> Get()
        {

            return db.ProductGroups;

        }


        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] ProductGroupModel u)
        {



            if (u.id == 0)
            {
                db.ProductGroups.Add(u);
            }
            else
            {
                db.ProductGroups.Update(u);
            }

            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return Ok(u);


        }


        [HttpPost]
        [Route("delete")]
        public async Task<ActionResult<ProductGroupModel>> DeleteRecord(ProductGroupModel u) //Delete
        { 
            u.is_deleted = !u.is_deleted;
            u.product_categories.ForEach(r => r.is_deleted = u.is_deleted);
            db.ProductGroups.Update(u);
            await db.SaveChangesAsync();
            return Ok(u);
        }

        [HttpGet("find")]
        [EnableQuery(MaxExpansionDepth = 4)]
        public SingleResult<ProductGroupModel> Get([FromODataUri] int key)
        {
            var s = db.ProductGroups.Where(r => r.id == key).AsQueryable();

            return SingleResult.Create(s);
        }
    }

}
