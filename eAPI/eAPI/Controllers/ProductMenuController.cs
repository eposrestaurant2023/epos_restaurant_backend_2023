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
    public class ProductMenuController: ODataController
    {

        private readonly ApplicationDbContext db;
        public ProductMenuController(ApplicationDbContext _db)
        {
            db = _db;
        }



        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        public IQueryable<ProductMenuModel> Get(string keyword = "")
        {
          
                return db.ProductMenus;
           
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [Route("getsingle")]
        public async Task<SingleResult<ProductMenuModel>> Get([FromODataUri] int key)
        {
            return await Task.Factory.StartNew(() => SingleResult.Create<ProductMenuModel>(db.ProductMenus.Where(r => r.id == key).AsQueryable()));
        }


        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] ProductMenuModel u)
        {
            
            if (u.id == 0)
            {

                db.ProductMenus.Add(u);
            }
            else
            {
                
                db.ProductMenus.Update(u);
            }
            
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));

            db.Database.ExecuteSqlRaw("exec sp_clear_deleted_record");
            return Ok(u);


        }

      


        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult<ProductMenuModel>> DeleteRecord(Guid id) //Delete
        {
            var u = await db.ProductMenus.FindAsync(id);
            u.is_deleted = !u.is_deleted;
            
            db.ProductMenus.Update(u);
            await db.SaveChangesAsync();
            return Ok(u);
        }
    }

}
