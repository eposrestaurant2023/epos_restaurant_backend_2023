using eAPIClient.Models;
using eAPIClient.Services;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace eAPIClient.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CouponVoucherTransactionController : ODataController
    {

        private readonly ApplicationDbContext db;     
        private readonly ISyncService sync;

        public CouponVoucherTransactionController(ApplicationDbContext _db, ISyncService sync)
        {
            db = _db;     
            this.sync = sync;
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)] 
        public IQueryable<CouponVoucherTransactionModel> Find(string keyword, bool only_code = true)
        {              
            if (string.IsNullOrEmpty(keyword))
            {
                return db.CouponVoucherTransactions;
            }
            else
            {
                var data = from r in db.CouponVoucherTransactions
                           where
                                 EF.Functions.Like(
                                     ((r.coupon_number ?? " ")+(only_code ?"": (r.document_number ?? " "))
                                     ).ToLower().Trim(), $"%{keyword}%".ToLower().Trim())
                           select r;
                return data;
            } 
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 4)]
        [Route("[action]/{code}")]
        public SingleResult<CouponVoucherTransactionModel> Get(string code)
        {
            var s = db.CouponVoucherTransactions.Where(r => r.coupon_number == code && r.status && !r.is_deleted && r.base_current_balance > 0).Take(1).AsQueryable();            
            return SingleResult.Create(s);
        }


        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] CouponVoucherTransactionModel model)
        {
            model.is_synced = false;
            if (model.id == Guid.Empty)
            {
                db.CouponVoucherTransactions.Add(model);
            }
            else
            {
                db.CouponVoucherTransactions.Update(model);   
            }

            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));

            sync.sendSyncRequest();
            return Ok(model);
        }


        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult<CouponVoucherTransactionModel>> DeleteRecord(Guid id) //Delete
        {
            var u = await db.CouponVoucherTransactions.FindAsync(id);
            u.is_synced = false;
            u.is_deleted = true;
            db.CouponVoucherTransactions.Update(u);
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            sync.sendSyncRequest();
            return Ok(u);
        }  
    }
}
