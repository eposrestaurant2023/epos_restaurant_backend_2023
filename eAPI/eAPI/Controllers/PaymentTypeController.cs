using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using eAPI.Hubs;
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
    public class PaymentTypeController : ODataController
    {

        private readonly ApplicationDbContext db;
        private readonly IHubContext<ConnectionHub> hub;
        public PaymentTypeController(ApplicationDbContext _db,IHubContext<ConnectionHub> _hub)
        {
            db = _db;hub = _hub;
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]   
        public IQueryable<PaymentTypeModel> Get()
        {            
            return db.PaymentTypes;
        }
        
        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] PaymentTypeModel u)
        {            
            
            u.is_credit = u.payment_type_group == "On Account";
            
            if (u.id == 0)
            {
                db.PaymentTypes.Add(u);
            }
            else
            { 
                db.Database.ExecuteSqlRaw($"delete tbl_business_branch_payment_type where payment_type_id = {u.id}");
                db.BusinessBranchPaymentTypes.AddRange(u.business_branch_payment_types);
                db.PaymentTypes.Update(u);
            }            
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)),hub);
            return Ok(u);
        }

        [HttpGet("find")]
        [EnableQuery(MaxExpansionDepth = 4)]
        public SingleResult<PaymentTypeModel> Get([FromODataUri] int key)
        {
            var s = db.PaymentTypes.Where(r => r.id == key).AsQueryable();
            return SingleResult.Create(s);
        }

        [HttpPost]
        [Route("status/{id}")]
        public async Task<ActionResult<PaymentTypeModel>> UpdateStatus(int id)
        {
            var d = await db.PaymentTypes.FindAsync(id);
            d.status = !d.status;
            db.PaymentTypes.Update(d);
            await db.SaveChangesAsync();
            return Ok(d);
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
