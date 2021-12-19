using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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
    public class ExpenseCategoryController : ODataController
    {

        private readonly ApplicationDbContext db;
        public ExpenseCategoryController(ApplicationDbContext _db)
        {
            db = _db;
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [AllowAnonymous]
        public IQueryable<eShareModel.ExpenseCategoryModel> Get()
        {
              return db.ExpenseCategories;
            
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [Route("getsingle")]
        public async Task<SingleResult<eShareModel.ExpenseCategoryModel>> GetQuery([FromODataUri] Guid key)
        {
            return await Task.Factory.StartNew(() => SingleResult.Create<eShareModel.ExpenseCategoryModel>(db.ExpenseCategories.Where(r => r.id == key).AsQueryable()));
        }

        [HttpGet("category")]
        [EnableQuery(MaxExpansionDepth = 8)]
        public IQueryable<eShareModel.ExpenseCategoryModel> GetCategoryNote()
        {
            return db.ExpenseCategories;
        }


        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] eShareModel.ExpenseCategoryModel u)
        {
            if (u.id == Guid.Empty)
            {
                db.ExpenseCategories.Add(u);
            }
            else
            {
                db.ExpenseCategories.Update(u);
            }
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return Ok(u);
        }

        [HttpGet("find")]
        [EnableQuery(MaxExpansionDepth = 4)]
        public SingleResult<eShareModel.ExpenseCategoryModel> Get([FromODataUri] Guid key)
        {
            var s = db.ExpenseCategories.Where(r => r.id == key).AsQueryable();
            return SingleResult.Create(s);
        }

        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult<eShareModel.ExpenseCategoryModel>> DeleteRecord(int id) //Delete
        {
            var u = await db.ExpenseCategories.FindAsync(id);
            u.is_deleted = !u.is_deleted;
            db.ExpenseCategories.Update(u);
            await db.SaveChangesAsync();
            return Ok(u);
        }


    }
}
