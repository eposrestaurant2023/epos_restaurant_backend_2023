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
    public class CouponVoucherController : ODataController
    {

        private readonly ApplicationDbContext db;     
        private readonly ISyncService sync;

        public CouponVoucherController(ApplicationDbContext _db, ISyncService sync)
        {
            db = _db;     
            this.sync = sync;
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)] 
        public IQueryable<CouponVoucherModel> Find(string keyword)
        {
            
            if (string.IsNullOrEmpty(keyword))
            {
                return db.CouponVouchers;

            }
            else
            {
                var data = from r in db.CouponVouchers
                           where
                                !r.is_deleted && r.status &&
                                 EF.Functions.Like(
                                     ((r.coupon_number ?? " ")
                                     ).ToLower().Trim(), $"%{keyword}%".ToLower().Trim())
                           select r;

                return data;

            } 
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 4)]
        [Route("[action]/{code}")]
        public SingleResult<CouponVoucherModel> Get(string code)
        {
            var s = db.CouponVouchers.Where(r => r.coupon_number == code && r.status && !r.is_deleted).AsQueryable();
            return SingleResult.Create(s);
        }


        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] CouponVoucherModel model)
        {
            model.is_synced = false;
            if (model.id == Guid.Empty)
            {
                db.CouponVouchers.Add(model);
            }
            else
            {
                db.CouponVouchers.Update(model);   
            }

            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));

            sync.sendSyncRequest();
            return Ok(model);
        }


        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult<CouponVoucherModel>> DeleteRecord(Guid id) //Delete
        {
            var u = await db.CouponVouchers.FindAsync(id);
            u.is_synced = false;
            u.is_deleted = true;
            db.CouponVouchers.Update(u);
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            sync.sendSyncRequest();
            return Ok(u);
        }  
    }
}
