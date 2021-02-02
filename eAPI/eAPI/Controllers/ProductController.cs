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
    public class ProductController : ODataController
    {

        private readonly ApplicationDbContext db;
        public ProductController(ApplicationDbContext _db)
        {
            db = _db;
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        
        public IQueryable<ProductModel> Get()
        {
              return db.Products;
        }

        
        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] ProductModel u)
        {
            
            if (u.id == 0)
            {

                db.Products.Add(u);
            }
            else
            {
                
                db.Products.Update(u);
            }
            
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return Ok(u);


        }

      


        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult<ProductModel>> DeleteRecord(Guid id) //Delete
        {
            var u = await db.Products.FindAsync(id);
            u.is_deleted = !u.is_deleted;
            
            db.Products.Update(u);
            await db.SaveChangesAsync();
            return Ok(u);
        }
    }

}
