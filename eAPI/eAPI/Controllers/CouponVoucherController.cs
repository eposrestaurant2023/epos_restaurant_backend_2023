using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using eAPI.Hubs;
using eAPI.Services;
using eModels;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NETCore.Encrypt;


namespace eAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class CouponVoucherController : ODataController
    {

        private readonly ApplicationDbContext db;
        private readonly AppService app;
        private readonly IHubContext<ConnectionHub> hub;
        public CouponVoucherController(ApplicationDbContext _db, AppService _app, IHubContext<ConnectionHub> _hub)
        {
            db = _db;
            hub = _hub;
            app = _app;
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        public IQueryable<CouponVoucherModel> Get()
        {
            return db.CouponVouchers;
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [Route("GetSingle")]
        public async Task<SingleResult<CouponVoucherModel>> Get([FromODataUri] Guid key)
        {
            return await Task.Factory.StartNew(() => SingleResult.Create<CouponVoucherModel>(db.CouponVouchers.Where(r => r.id == key).AsQueryable()));
        }


        [HttpPost("Save")]
        public async Task<ActionResult<string>> Save([FromBody] CouponVoucherModel model)
        {
            try
            {
                var coupon = db.CouponVouchers.Where(r => r.id == model.id).AsNoTracking();
                foreach(var cp in model.coupon_vouchers)
                {
                    db.Entry(cp).State = EntityState.Added;
                    if (cp.id != Guid.Empty)
                    {
                        var coupon_transactions = db.CouponVoucherTransactions.Where(r => r.id == cp.id).AsNoTracking();
                        if (coupon_transactions.Any())
                        {
                            db.Entry(cp).State = EntityState.Modified;
                        }
                    }
                }
                if (coupon.Count() > 0)
                {
                    db.CouponVouchers.Update(model);
                }
                else
                {
                    db.CouponVouchers.Add(model);
                }

                await db.SaveChangesAsync();
                return Ok();
            }

            catch (Exception ex)
            {
                var x = ex.ToString();
                return BadRequest();
            }  
        }   

        [HttpPost]
        [Route("Delete/{id}")]
        public async Task<ActionResult<CouponVoucherModel>> DeleteRecord(Guid id) //Delete
        {
            var u = await db.CouponVouchers.FindAsync(id);
            u.is_deleted = !u.is_deleted;
            db.CouponVouchers.Update(u);
            await db.SaveChangesAsync();
            return Ok(u);
        }       
        [HttpPost]
        [Route("UpdateStatus/{id}")]
        public async Task<ActionResult<CouponVoucherModel>> UpdateStatus(Guid id) //Update
        {
            var u = await db.CouponVouchers.FindAsync(id);
            u.status = !u.status;
            db.CouponVouchers.Update(u);
            await db.SaveChangesAsync();
            return Ok(u);
        }
    }
}
