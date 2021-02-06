using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
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
    public class BusinessBranchController : ODataController
    {

        private readonly ApplicationDbContext db;
        public BusinessBranchController(ApplicationDbContext _db)
        {
            db = _db;
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [AllowAnonymous]
      
        public IQueryable<BusinessBranchModel> Get()
        {
           
                return db.BusinessBranches;
           
        }

        
        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] BusinessBranchModel u)
        {
            
            if (u.id == Guid.Empty)
            {
                db.BusinessBranches.Add(u);
            }
            else
            {
                db.BusinessBranches.Update(u);
            }
            
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return Ok(u);


        }

         [HttpPost("save/multiple")]
        public async Task<ActionResult<string>> SaveMultiple([FromBody] List<BusinessBranchModel> branches)
        {

            string xx = JsonSerializer.Serialize(branches);
            db.BusinessBranches.UpdateRange(branches);


            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));

            db.Database.ExecuteSqlRaw("exec sp_clear_deleted_record");
            return Ok(branches);
        }

        [HttpGet("find")]
        [EnableQuery(MaxExpansionDepth = 4)]
        public SingleResult<BusinessBranchModel> Get([FromODataUri] Guid key)
        {
            var s = db.BusinessBranches.Where(r => r.id == key).AsQueryable();

            return SingleResult.Create(s);
        }

        [HttpPost]
        [Route("status/{id}")]
        public async Task<ActionResult<BusinessBranchModel>> UpdateStatus(Guid id)
        {
            var d = await db.BusinessBranches.FindAsync(id);
            d.status = !d.status;
            db.BusinessBranches.Update(d);
            await db.SaveChangesAsync();
            return Ok(d);
        }

        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult<BusinessBranchModel>> DeleteRecord(Guid id) //Delete
        {
            var u = await db.BusinessBranches.FindAsync(id);
            u.is_deleted = !u.is_deleted;
            
            db.BusinessBranches.Update(u);
            await db.SaveChangesAsync();
            return Ok(u);
        }
    }

}
