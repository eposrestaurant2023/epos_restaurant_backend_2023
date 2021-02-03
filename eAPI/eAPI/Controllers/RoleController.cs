using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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
    public class RoleController : ODataController
    {

        private readonly ApplicationDbContext db;
        public RoleController(ApplicationDbContext _db)
        {
            db = _db;
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
            if (u.id == 0)
            {
                db.Roles.Add(u);
            }
            else
            {

                db.PermissionOptionRole.AddRange(u.permission_option_roles);
                db.Roles.Update(u);
            }            
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return Ok(u);
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
