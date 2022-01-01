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
    public class PermissionOptionController : ODataController
    {

        private readonly ApplicationDbContext db;
        private readonly IHubContext<ConnectionHub> hub;
        public PermissionOptionController(ApplicationDbContext _db,IHubContext<ConnectionHub> _hub)
        {
            db = _db;hub = _hub;
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [AllowAnonymous]
        public IQueryable<PermissionOptionModel> Get()
        {
           
                return db.PermissionOption;
           
        }

        
        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] PermissionOptionModel u)
        {
            if (u.id == 0)
            {
                db.PermissionOption.Add(u);
                }
            else
            {
                db.PermissionOption.Update(u);
            }
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)),hub);
            return Ok(u);
        }

        [HttpPost("save/multiple/{role_id}")]
        public async Task<ActionResult<string>> SaveMultiple([FromBody] List<PermissionOptionModel> p,int role_id)
        {
            db.Database.ExecuteSqlRaw($"delete tbl_permission_option_role where role_id = {role_id}");
            foreach (var _p in p)
            {                
                db.PermissionOptionRole.AddRange(_p.permission_option_roles);
            }
            db.PermissionOption.UpdateRange(p);

            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)),hub);

            db.Database.ExecuteSqlRaw("exec sp_update_permission_option_role");

            return Ok();
        }


        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult<PermissionOptionModel>> DeleteRecord(int id) //Delete
        {
            var u = await db.PermissionOption.FindAsync(id);
            
            db.PermissionOption.Update(u);
            await db.SaveChangesAsync();
            return Ok(u);
        }

        [HttpGet("find")]
        [EnableQuery(MaxExpansionDepth = 4)]
        public SingleResult<PermissionOptionModel> Get([FromODataUri] int key)
        {
            var s = db.PermissionOption.Where(r => r.id == key).AsQueryable();

            return SingleResult.Create(s);
        }
    }

}
