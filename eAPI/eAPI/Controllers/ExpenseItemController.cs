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
    public class ExpenseItemController : ODataController
    {

        private readonly ApplicationDbContext db;
        public ExpenseItemController(ApplicationDbContext _db)
        {
            db = _db;
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [AllowAnonymous]
        public IQueryable<eShareModel.ExpenseItemModel> Get()
        {
            return db.ExpenseItems;

        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [Route("getsingle")]
        public async Task<SingleResult<eShareModel.ExpenseItemModel>> GetQuery([FromODataUri] Guid key)
        {
            return await Task.Factory.StartNew(() => SingleResult.Create<eShareModel.ExpenseItemModel>(db.ExpenseItems.Where(r => r.id == key).AsQueryable()));
        }

        [HttpGet("category")]
        [EnableQuery(MaxExpansionDepth = 8)]
        public IQueryable<eShareModel.ExpenseItemModel> GetCategoryNote()
        {
            return db.ExpenseItems;
        }


        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] eShareModel.ExpenseItemModel u)
        {
            if (u.id == Guid.Empty)
            {
                db.ExpenseItems.Add(u);
            }
            else
            {
                db.ExpenseItems.Update(u);
            }
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return Ok(u);
        }

        [HttpGet("find")]
        [EnableQuery(MaxExpansionDepth = 4)]
        public SingleResult<eShareModel.ExpenseItemModel> Get([FromODataUri] Guid key)
        {
            var s = db.ExpenseItems.Where(r => r.id == key).AsQueryable();
            return SingleResult.Create(s);
        }

        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult<eShareModel.ExpenseItemModel>> DeleteRecord(int id) //Delete
        {
            var u = await db.ExpenseItems.FindAsync(id);
            u.is_deleted = !u.is_deleted;
            db.ExpenseItems.Update(u);
            await db.SaveChangesAsync();
            return Ok(u);
        }


    }
}
