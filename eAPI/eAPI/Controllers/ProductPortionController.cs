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
    public class ProductPortionController : ODataController
    {

        private readonly ApplicationDbContext db;
        public ProductPortionController(ApplicationDbContext _db)
        {
            db = _db;
        }



        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        public IQueryable<ProductPortionModel> Get(string keyword = "")
        {
          
                return db.ProductPortions;
           
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [Route("getsingle")]
        public async Task<SingleResult<ProductPortionModel>> Get([FromODataUri] int key)
        {
            return await Task.Factory.StartNew(() => SingleResult.Create<ProductPortionModel>(db.ProductPortions.Where(r => r.id == key).AsQueryable()));
        }


        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] ProductPortionModel u)
        {
            
            if (u.id == 0)
            {

                db.ProductPortions.Add(u);
            }
            else
            {
                
                db.ProductPortions.Update(u);
            }
            
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));

            db.Database.ExecuteSqlRaw("exec sp_clear_deleted_record");
            db.Database.ExecuteSqlRaw("exec sp_update_product_ingredient_related " + u.product_id.ToString());

             

            return Ok(u);


        }

        [HttpPost("save/multiple")]
        public async Task<ActionResult<string>> SaveMultiple([FromBody] List<ProductPortionModel> data)
        {

          
            db.ProductPortions.UpdateRange(data);


            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));

            db.Database.ExecuteSqlRaw("exec sp_clear_deleted_record");
            return Ok(data);
        }



        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult<ProductPortionModel>> DeleteRecord(Guid id) //Delete
        {
            var u = await db.ProductPortions.FindAsync(id);
            u.is_deleted = !u.is_deleted;
            
            db.ProductPortions.Update(u);
            await db.SaveChangesAsync();
            return Ok(u);
        }
    }

}
