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
           
                return db.BusinessBranchs;
           
        }

        
        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] BusinessBranchModel u)
        {
            
            if (u.id == Guid.Empty)
            {
                db.BusinessBranchs.Add(u);
            }
            else
            {
                db.BusinessBranchs.Update(u);
            }
            
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return Ok(u);


        }

        [HttpPost("save/multiple")]
        public async Task<ActionResult<string>> SaveMultiple([FromBody] List<BusinessBranchModel> branches)
        {

        
            db.BusinessBranchs.UpdateRange(branches);


            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));

            db.Database.ExecuteSqlRaw("exec sp_clear_deleted_record");
            return Ok(branches);
        }

        [HttpGet("find")]
        [EnableQuery(MaxExpansionDepth = 4)]
        public SingleResult<BusinessBranchModel> Get([FromODataUri] Guid key)
        {
            var s = db.BusinessBranchs.Where(r => r.id == key).AsQueryable();

            return SingleResult.Create(s);
        }

        [HttpPost]
        [Route("status/{id}")]
        public async Task<ActionResult<BusinessBranchModel>> UpdateStatus(int id)
        {
            var d = await db.BusinessBranchs.FindAsync(id);
            d.status = !d.status;
            db.BusinessBranchs.Update(d);
            await db.SaveChangesAsync();
            return Ok(d);
        }

        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult<BusinessBranchModel>> DeleteRecord(int id) //Delete
        {
            var u = await db.BusinessBranchs.FindAsync(id);
            u.is_deleted = !u.is_deleted;
            db.BusinessBranchs.Update(u);
            await db.SaveChangesAsync();
            return Ok(u);
        }
    }

}
