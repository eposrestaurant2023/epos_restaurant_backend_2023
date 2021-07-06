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
    public class ProductCategoryController : ODataController
    {

        private readonly ApplicationDbContext db;
        public ProductCategoryController(ApplicationDbContext _db)
        {
            db = _db;
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [AllowAnonymous]
        public IQueryable<ProductCategoryModel> Get()
        {

            return db.ProductCategories;

        }


        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] ProductCategoryModel u)
        {



            if (u.id == 0)
            {
                db.ProductCategories.Add(u);
            }
            else
            {
                db.ProductCategories.Update(u);
            }

            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            db.Database.ExecuteSqlRaw($"sp_update_product_category_information {u.id}");
            return Ok(u);


        }


        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult<ProductCategoryModel>> DeleteRecord(int id) //Delete
        {
            var u = await db.ProductCategories.FindAsync(id);
            u.is_deleted = !u.is_deleted;

            db.ProductCategories.Update(u);
            await db.SaveChangesAsync();
            return Ok(u);
        }

        [HttpGet("find")]
        [EnableQuery(MaxExpansionDepth = 4)]
        public SingleResult<ProductCategoryModel> Get([FromODataUri] int key)
        {
            var s = db.ProductCategories.Where(r => r.id == key).AsQueryable();

            return SingleResult.Create(s);
        }
    }

}
