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
