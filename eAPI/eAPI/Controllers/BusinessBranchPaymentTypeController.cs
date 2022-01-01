using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
    public class BusinessBranchPaymentTypeController : ODataController
    {

        private readonly ApplicationDbContext db;
        private readonly IHubContext<ConnectionHub> hub;
        public BusinessBranchPaymentTypeController(ApplicationDbContext _db,IHubContext<ConnectionHub> _hub)
        {
            db = _db;hub = _hub;
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [AllowAnonymous]

        public IQueryable<BusinessBranchPaymentTypeModel> Get()
        {
            return db.BusinessBranchPaymentTypes;
        }

        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] BusinessBranchPaymentTypeModel u)
        {
            if (u.business_branch_id == Guid.Empty)
            {
                db.BusinessBranchPaymentTypes.Add(u);
            }
            else
            {
                db.BusinessBranchPaymentTypes.Update(u);
            }
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)),hub);
            return Ok(u);
        }

        [HttpGet("find")]
        [EnableQuery(MaxExpansionDepth = 4)]
        public SingleResult<BusinessBranchPaymentTypeModel> Get([FromODataUri] Guid key)
        {
            var s = db.BusinessBranchPaymentTypes.Where(r => r.business_branch_id == key).AsQueryable();
            return SingleResult.Create(s);
        }

        [HttpPost]
        [Route("status/{id}/{payment_type_id}")]
        public async Task<ActionResult<BusinessBranchPaymentTypeModel>> UpdateStatus(Guid id, int payment_type_id)
        {
            var d = await db.BusinessBranchPaymentTypes.Where(r => r.business_branch_id == id && r.payment_type_id == payment_type_id).FirstAsync();
            d.status = !d.status;
            db.BusinessBranchPaymentTypes.Update(d);
            await db.SaveChangesAsync();
            return Ok(d);
        }

        [HttpPost]
        [Route("delete/{id}/{payment_type_id}")]
        public async Task<ActionResult<BusinessBranchPaymentTypeModel>> DeleteRecord(Guid id, int payment_type_id) //Delete
        {
            db.Database.ExecuteSqlRaw($"delete tbl_business_branch_payment_type where payment_type_id = {payment_type_id} and business_branch_id = '{id}'");
            await db.SaveChangesAsync();
            return Ok();
        }
    }

}
