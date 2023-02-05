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
        private readonly AppService app;

        public CouponVoucherController(ApplicationDbContext _db, AppService _app, ISyncService sync)
        {
            db = _db;
            app = _app;
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
        public async Task<SingleResult<CouponVoucherModel>> Code(string code)
        {

            var value=  await Task.Factory.StartNew(async ()  =>
             {
                 var data = db.CouponVouchers.Where(r => r.coupon_number == code && r.status && !r.is_deleted).Take(1).AsNoTracking().AsQueryable();
                 if (data.Any())
                 {
                     var d = data.FirstOrDefault();
                    await  db.Database.ExecuteSqlRawAsync($"exec sp_update_coupon_total_current_balance '{d.id}'");
                     var result = db.CouponVouchers.Where(r => r.id == d.id).Take(1).AsNoTracking().AsQueryable();
                     return SingleResult.Create(result);
                 }
                 return SingleResult.Create(data);
             });
            return  value.Result;

        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 4)]
        [Route("[action]/{key}")]
        public async Task<SingleResult<CouponVoucherModel>> Get([FromODataUri] Guid key)
        {
            var value = await Task.Factory.StartNew(async () =>
            {
                var data = db.CouponVouchers.Where(r => r.id == key).AsNoTracking().AsQueryable();
                if (data.Any())
                {
                    var d = data.FirstOrDefault();
                    await db.Database.ExecuteSqlRawAsync($"exec sp_update_coupon_total_current_balance '{d.id}'");
                    var result = db.CouponVouchers.Where(r => r.id == key).AsNoTracking().AsQueryable();
                    return SingleResult.Create(result);
                }
                return SingleResult.Create(data);
            });
            return value.Result;
        }


        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] CouponVoucherModel model)
        {
            var check = db.CouponVouchers.Where(r => r.coupon_number == model.coupon_number && r.id != model.id && !r.is_deleted).AsNoTracking();
            if (check.Any())
            {
                return BadRequest(new BadRequestModel() { message = "coupon_number_already_used" });        
            }

            model.is_synced = false;
            model.coupon_vouchers.ForEach(cv => cv.is_synced = false);
            if (model.id == Guid.Empty)
            {
                db.CouponVouchers.Add(model);
            }
            else
            {
                db.CouponVouchers.Update(model);   
            }
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));

            //generate doc
            var coupon_vouchers = model.coupon_vouchers.Where(r => !r.is_deleted && (r.document_number == "New" || r.document_number == string.Empty)).ToList();
            foreach(var cv in  coupon_vouchers)
            {
                DocumentNumberModel couponDoc = new DocumentNumberModel();
                couponDoc = app.GetDocument("CouponDoc", cv.cash_drawer_id.ToString());
                string doc = couponDoc.id > 0 ? app.GetDocumentFormat(couponDoc) : "New";
                string query = $"update tbl_coupon_voucher_transaction set document_number = N'{doc}'  where id = '{cv.id}';";
                db.Database.ExecuteSqlRaw(query);
                await app.UpdateDocument(couponDoc);
            } 
            sync.sendSyncRequest();
            return Ok();
        }

        [HttpPost("Refund")]
        public async Task<ActionResult<string>> OnRefund(Guid id,Guid rf_wd_id, Guid rf_cs_id, Guid rf_cd_id, decimal amount )
        {

            var coupons = db.CouponVouchers.Where(r => r.id == id).AsNoTracking();
            if (coupons.Any())
            {
                var coupon = coupons.FirstOrDefault();
                if(coupon.total_balance + coupon.total_refund_amount < amount )
                {
                    sync.sendSyncRequest();
                    return BadRequest(new BadRequestModel { message = "you_cannot_to_refund_amount_over"});
                }
                else
                { 
                    string query = $"exec sp_update_coupon_refund '{id}','{rf_wd_id}','{rf_cs_id}','{rf_cd_id}',{amount}";
                    await   db.Database.ExecuteSqlRawAsync(query);


                    sync.createLog(query);
                    sync.sendSyncRequest();
                    return Ok();
                }
            }
            sync.sendSyncRequest();
            return BadRequest(new BadRequestModel { message = "error" });
        }  


        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult<CouponVoucherModel>> DeleteRecord(Guid id) //Delete
        {
            var u = await db.CouponVouchers.Include(r => r.coupon_vouchers).FirstOrDefaultAsync(r => r.id == id);         
            u.is_synced = false;
            u.is_deleted = true;
            u.coupon_vouchers.ForEach(cv => {
                cv.is_deleted = true;
                cv.is_synced = false;
            });
            db.CouponVouchers.Update(u);
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            sync.sendSyncRequest();
            return Ok();
        }  
    }
}
