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
    public class BusinessBranchPriceRuleController : ODataController
    {

        private readonly ApplicationDbContext db;
        public BusinessBranchPriceRuleController(ApplicationDbContext _db)
        {
            db = _db;
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [AllowAnonymous]
        public IQueryable<BusinessBranchPriceRule> Get()
        {
            return db.BusinessBranchPriceRules;
        }

        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] BusinessBranchPriceRule u)
        {

            if (u.business_branch_id == Guid.Empty)
            {
                db.BusinessBranchPriceRules.Add(u);
            }
            else
            {
                db.BusinessBranchPriceRules.Update(u);
            }

            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return Ok(u);
        }

        [HttpGet("find")]
        [EnableQuery(MaxExpansionDepth = 4)]
        public SingleResult<BusinessBranchPriceRule> Get([FromODataUri] Guid key)
        {
            var s = db.BusinessBranchPriceRules.Where(r => r.business_branch_id == key).AsQueryable();
            return SingleResult.Create(s);
        }

        [HttpPost]
        [Route("status/{id}/{price_id}")]
        public async Task<ActionResult<BusinessBranchPriceRule>> UpdateStatus(Guid id , int price_id)
        {
            var d = await db.BusinessBranchPriceRules.Where(r=>r.business_branch_id == id && r.price_rule_id == price_id).FirstAsync();
            d.status = !d.status;
            db.BusinessBranchPriceRules.Update(d);
            await db.SaveChangesAsync();
            return Ok(d);
        }

        [HttpPost]
        [Route("delete/{id}/{price_id}")]
        public async Task<ActionResult<BusinessBranchPriceRule>> DeleteRecord(Guid id , int price_id) //Delete
        {
            db.Database.ExecuteSqlRaw($"delete tbl_business_branch_price_rule where price_rule_id = {price_id} and business_branch_id = '{id}'");
            await db.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        [Route("is_default/{id}/{price_id}")]
        public async Task<ActionResult<BusinessBranchPriceRule>> UpdateIsDefault(Guid id, int price_id)
        {
            var s = db.BusinessBranchPriceRules;
            if (s != null)
            {
                foreach (var e in s)
                {
                    e.is_default = false;
                    db.BusinessBranchPriceRules.Update(e);
                }
            }
            var d = await db.BusinessBranchPriceRules.Where(r => r.business_branch_id == id && r.price_rule_id == price_id).FirstAsync();
            d.is_default = true;
            
            db.BusinessBranchPriceRules.Update(d);
            await db.SaveChangesAsync();
            return Ok(d);
        }
    }
}
