using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using eAPIClient.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eAPIClient.Controllers
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
        public IQueryable<eShareModel.ExpenseModel> Get(string keyword)
        {
            if ((keyword??"") == "")
            {
                return db.Expenses;
            }else
            {
                
                var data = from r in db.Expenses
                           where
                                 EF.Functions.Like(
                                     (
                                        (r.reference_number ?? " ") +
                        (r.expense_item_name ?? " ") +
                        (r.expense_by ?? " ") +
                        (r.note ?? " ")
                                     ).ToLower().Trim(), $"%{keyword}%".ToLower().Trim())
                           select r;


                return data;
            }
            

        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [Route("getsingle")]
        public async Task<SingleResult<eShareModel.ExpenseModel>> GetQuery([FromODataUri] Guid key)
        {
            return await Task.Factory.StartNew(() => SingleResult.Create<eShareModel.ExpenseModel>(db.Expenses.Where(r => r.id == key).AsQueryable()));
        }

        

        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] eShareModel.ExpenseModel u)
        {
            u.is_synced = false;
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
