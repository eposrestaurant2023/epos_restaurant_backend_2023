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
        public SingleResult<CouponVoucherModel> Code(string code)
        {
            var s = db.CouponVouchers.Where(r => r.coupon_number == code && r.status && !r.is_deleted).AsQueryable();
            return SingleResult.Create(s);
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 4)]
        [Route("[action]/{key}")]
        public async Task<SingleResult<CouponVoucherModel>> Get([FromODataUri] Guid key)
        {
            return await Task.Factory.StartNew(() => SingleResult.Create<CouponVoucherModel>(db.CouponVouchers.Where(r => r.id == key).AsQueryable()));
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
        public async Task<ActionResult<string>> OnRefund(Guid id, decimal amount )
        {

            var coupons = db.CouponVouchers.Where(r => r.id == id).AsNoTracking();
            if (coupons.Any())
            {
                var coupon = coupons.FirstOrDefault();
                if(coupon.total_balance + coupon.total_refund_amount < amount )
                {
                     return BadRequest(new BadRequestModel { message = "you_cannot_to_refund_amount_over"});
                }
                else
                { 
                    string query = $"exec sp_update_coupon_refund '{id}',{amount}";
                    db.Database.ExecuteSqlRaw(query);
                    return Ok();
                }
            }
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
