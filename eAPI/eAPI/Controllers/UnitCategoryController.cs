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
    public class UnitCategoryController : ODataController
    {
        private readonly ApplicationDbContext db;
        public UnitCategoryController(ApplicationDbContext _db)
        {
            db = _db;
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]   
        public IQueryable<UnitCategoryModel> Get()
        {            
            return db.UnitCategorys;
        }
        
        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] UnitCategoryModel u)
        {      
            if (u.id == 0)
            {
                db.UnitCategorys.Add(u);
            }
            else
            {
                db.UnitCategorys.Update(u);
            }            
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return Ok(u);
        }

        [HttpPost("save/multiple")]
        public async Task<ActionResult<string>> SaveMultiple([FromBody] List<UnitCategoryModel> unitCategories)
        {

            string xx = JsonSerializer.Serialize(unitCategories);
            db.UnitCategorys.UpdateRange(unitCategories);
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            //db.Database.ExecuteSqlRaw("delete from tbl_unit where is_deleted = 1");
            return Ok(unitCategories);
        }

        [HttpGet("find")]
        [EnableQuery(MaxExpansionDepth = 4)]
        public SingleResult<UnitCategoryModel> Get([FromODataUri] int key)
        {
            var s = db.UnitCategorys.Where(r => r.id == key).AsQueryable();
            return SingleResult.Create(s);
        }

        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult<PaymentTypeModel>> DeleteRecord(int id) //Delete
        {
            var u = await db.PaymentTypes.FindAsync(id);
            u.is_deleted = !u.is_deleted;            
            db.PaymentTypes.Update(u);
            await db.SaveChangesAsync();
            return Ok(u);
        }
    }
}
