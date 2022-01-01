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
    public class CustomerBusinessBranchController : ODataController
    {

        private readonly ApplicationDbContext db;
        private readonly IHubContext<ConnectionHub> hub;
        public CustomerBusinessBranchController(ApplicationDbContext _db,IHubContext<ConnectionHub> _hub)
        {
            db = _db;hub = _hub;
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [AllowAnonymous]

        public IQueryable<CustomerBusinessBranchModel> Get()
        {
            return db.CustomerBusinessBranches;
        }

        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] CustomerBusinessBranchModel cb)
        {

            if (cb.business_branch_id == Guid.Empty)
            {
                db.CustomerBusinessBranches.Add(cb);
            }
            else
            {
                db.CustomerBusinessBranches.Update(cb);
            }

            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)),hub);
            return Ok(cb);


        }

        [HttpGet("find")]
        [EnableQuery(MaxExpansionDepth = 4)]
        public SingleResult<CustomerBusinessBranchModel> Get([FromODataUri] Guid key)
        {
            var s = db.CustomerBusinessBranches.Where(r => r.business_branch_id == key).AsQueryable();

            return SingleResult.Create(s);
        }


        [HttpPost]
        [Route("status/{id}")]
        public async Task<ActionResult<CustomerModel>> UpdateStatus(Guid id)
        {
            var d = await db.Customers.FindAsync(id);
            d.status = !d.status;
            db.Customers.Update(d);
            await db.SaveChangesAsync();
            return Ok(d);
        }
    }

}
