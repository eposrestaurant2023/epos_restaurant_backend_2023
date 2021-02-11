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
    public class SaleProductController : ODataController
    {

        private readonly ApplicationDbContext db;
        public SaleProductController(ApplicationDbContext _db)
        {
            db = _db;
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        public IQueryable<SaleProductModel> Get(string keyword = "")
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                return db.SaleProducts.Where(r =>
                (
                (r.sale_product_note ?? "") 
                ).ToLower().Trim().Contains(keyword.ToLower().Trim()));
            }
            else
            {
                return db.SaleProducts;
            }
        }



        [AllowAnonymous]
        public IQueryable<SaleProductModel> Get()
        {

            return db.SaleProducts;

        }


        //[HttpPost("save")]
        //public async Task<ActionResult<string>> Save([FromBody] SaleProductModel u)
        //{



        //    if (u.id == 0)
        //    {
        //        db.SaleProducts.Add(u);
        //    }
        //    else
        //    {
        //        db.SaleProducts.Update(u);
        //    }

        //    await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
        //    return Ok(u);


        //}


        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult<SaleProductModel>> DeleteRecord(int id) //Delete
        {
            var u = await db.SaleProducts.FindAsync(id);
            u.is_deleted = !u.is_deleted;

            db.SaleProducts.Update(u);
            await db.SaveChangesAsync();
            return Ok(u);
        }

        //[HttpGet("find")]
        //[EnableQuery(MaxExpansionDepth = 4)]
        //public SingleResult<SaleProductModel> Get([FromODataUri] int key)
        //{
        //    var s = db.SaleProducts.Where(r => r.id == key).AsQueryable();

        //    return SingleResult.Create(s);
        //}
    }

}
