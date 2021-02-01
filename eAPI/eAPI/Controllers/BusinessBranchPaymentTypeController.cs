using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
    public class BusinessBranchPaymentTypeController : ODataController
    {

        private readonly ApplicationDbContext db;
        public BusinessBranchPaymentTypeController(ApplicationDbContext _db)
        {
            db = _db;
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

            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return Ok(u);


        }

        [HttpGet("find")]
        [EnableQuery(MaxExpansionDepth = 4)]
        public SingleResult<BusinessBranchPaymentTypeModel> Get([FromODataUri] Guid key)
        {
            var s = db.BusinessBranchPaymentTypes.Where(r => r.business_branch_id == key).AsQueryable();

            return SingleResult.Create(s);
        }
    }

}
