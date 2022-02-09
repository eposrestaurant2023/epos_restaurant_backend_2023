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
    public class RoleController : ODataController
    {

        private readonly ApplicationDbContext db;
        private readonly IHubContext<ConnectionHub> hub;
        public RoleController(ApplicationDbContext _db,IHubContext<ConnectionHub> _hub)
        {
            db = _db;hub = _hub;
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [AllowAnonymous]
      
        public IQueryable<RoleModel> Get(string keyword ="" )
        {
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                return db.Roles.Where(r =>
                (
                (r.role_name ?? "")
                ).ToLower().Trim().Contains(keyword.ToLower().Trim()));
            }
            else
            {
                return db.Roles;
            }
        }
        
        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] RoleModel u)
        {
            db.Database.ExecuteSqlRaw($"delete tbl_business_branch_role where role_id = {u.id}");
            db.businessBranchRoles.AddRange(u.business_branch_roles);
            if (u.id == 0)
            {
                db.Roles.Add(u);
            }
            else
            {
                db.Roles.Update(u);
               
            }            
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)),hub);
             db.Database.ExecuteSqlRaw($"exec sp_update_permission_option_role {u.id},''");
            return Ok(u);
        }

        [HttpPost("SavePermission")]
        public ActionResult<RoleModel> SavePermission([FromBody] OptionModel option)
        {
            if (!string.IsNullOrEmpty(option.options))
            {
                db.Database.ExecuteSqlRaw($"exec sp_update_permission_option_role {option.role_id},'{option.options}'");
            }
            return Ok();
        }


        [HttpGet("find")]
        [EnableQuery(MaxExpansionDepth = 4)]
        public SingleResult<RoleModel> Get([FromODataUri] int key)
        {
            var s = db.Roles.Where(r => r.id == key).AsQueryable();
            return SingleResult.Create(s);
        }

        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult<RoleModel>> DeleteRecord(int id) //Delete
        {
            var u = await db.Roles.FindAsync(id);
            u.is_deleted = !u.is_deleted;            
            db.Roles.Update(u);
            await db.SaveChangesAsync();
            return Ok(u);
        }

        [HttpPost]
        [Route("status/{id}")]
        public async Task<ActionResult<RoleModel>> ChangeStatus(int id) //Delete
        {
            var u = await db.Roles.FindAsync(id);
            u.status = !u.status;
            db.Roles.Update(u);
            await db.SaveChangesAsync();
            db.Database.ExecuteSqlRaw($"exec sp_update_permission_option_role {id},''");
            return Ok(u);
        }

        //[HttpPost]
        //[Route("clone/{id}")]
        //public async Task<ActionResult<PriceRuleModel>> CloneRecord(int id) //Delete
        //{
        //    var u = await db.PriceRules.FindAsync(id);
        //    u.id = 0;
        //    u.created_date = DateTime.Now;
        //    await db.SaveChangesAsync();
        //    return Ok(u);
        //}
    }
}
