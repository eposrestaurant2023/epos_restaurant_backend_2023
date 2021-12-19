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
    public class ExpenseController : ODataController
    {

        private readonly ApplicationDbContext db;
        public ExpenseController(ApplicationDbContext _db)
        {
            db = _db;
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [AllowAnonymous]
        public IQueryable<eShareModel.ExpenseModel> Get()
        {
            return db.Expenses;

        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [Route("getsingle")]
        public async Task<SingleResult<eShareModel.ExpenseModel>> GetQuery([FromODataUri] Guid key)
        {
            return await Task.Factory.StartNew(() => SingleResult.Create<eShareModel.ExpenseModel>(db.Expenses.Where(r => r.id == key).AsQueryable()));
        }

        [HttpGet("category")]
        [EnableQuery(MaxExpansionDepth = 8)]
        public IQueryable<eShareModel.ExpenseModel> GetCategoryNote()
        {
            return db.Expenses;
        }


        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] eShareModel.ExpenseModel u)
        {
            if (u.id == Guid.Empty)
            {
                db.Expenses.Add(u);
            }
            else
            {
                db.Expenses.Update(u);
            }
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return Ok(u);
        }

        [HttpPost("SyncSave")]
        public async Task<ActionResult<string>> SyncSave([FromBody] eShareModel.ExpenseModel model)
        {
            try
            {
                var _modelCheck = db.Expenses.Where(r => r.id == model.id).AsNoTracking();
                if (_modelCheck.Count() > 0)
                {
                    db.Expenses.Update(model);
                }
                else
                {
                    db.Expenses.Add(model);
                }

                await db.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                var _ex = ex;
                return BadRequest();
            }
        }

        [HttpGet("find")]
        [EnableQuery(MaxExpansionDepth = 4)]
        public SingleResult<eShareModel.ExpenseModel> Get([FromODataUri] Guid key)
        {
            var s = db.Expenses.Where(r => r.id == key).AsQueryable();
            return SingleResult.Create(s);
        }

        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult<eShareModel.ExpenseModel>> DeleteRecord(int id) //Delete
        {
            var u = await db.Expenses.FindAsync(id);
            u.is_deleted = !u.is_deleted;
            db.Expenses.Update(u);
            await db.SaveChangesAsync();
            return Ok(u);
        }


    }
}
