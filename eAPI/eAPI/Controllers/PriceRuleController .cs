using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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
    public class PriceRuleController : ODataController
    {

        private readonly ApplicationDbContext db;
        private readonly IHubContext<ConnectionHub> hub;
        public PriceRuleController(ApplicationDbContext _db,IHubContext<ConnectionHub> _hub)
        {
            db = _db;hub = _hub;
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        public IQueryable<PriceRuleModel> Get(string keyword ="" )
        {
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                return db.PriceRules.Where(r =>
                (
                (r.price_name ?? "")
                ).ToLower().Trim().Contains(keyword.ToLower().Trim()));
            }
            else
            {
                return db.PriceRules;
            }
        }
        
        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] PriceRuleModel u)
        {            
            if (u.id == 0)
            {
                db.PriceRules.Add(u);
            }
            else
            {
                db.Database.ExecuteSqlRaw($"delete tbl_business_branch_price_rule where price_rule_id = {u.id}");
                db.BusinessBranchPriceRules.AddRange(u.business_branch_prices);
                db.PriceRules.Update(u);
            }            
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)),hub);
            //generate product all product to have price rule id in tbl_product price
            db.Database.ExecuteSqlRaw($"exec sp_update_price_rule_information  {u.id},'{u.last_modified_by}'");
            return Ok(u);
        }

        

        [HttpGet("find")]
        [EnableQuery(MaxExpansionDepth = 4)]
        public SingleResult<PriceRuleModel> Get([FromODataUri] int key)
        {
            var s = db.PriceRules.Where(r => r.id == key).AsQueryable();
            return SingleResult.Create(s);
        }

        [HttpPost]
        [Route("status/{id}")]
        public async Task<ActionResult<PriceRuleModel>> UpdateStatus(int id)
        {
            var d = await db.PriceRules.FindAsync(id);
            d.status = !d.status;
            db.PriceRules.Update(d);
            await db.SaveChangesAsync();
            return Ok(d);
        }

        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult<PriceRuleModel>> DeleteRecord(int id) //Delete
        {
            var u = await db.PriceRules.FindAsync(id);
            u.is_deleted = !u.is_deleted;            
            db.PriceRules.Update(u);
            await db.SaveChangesAsync();
            return Ok(u);
        }
    }
}
