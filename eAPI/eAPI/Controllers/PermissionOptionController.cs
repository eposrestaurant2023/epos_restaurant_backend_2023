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
    public class PermissionOptionController : ODataController
    {

        private readonly ApplicationDbContext db;
        public PermissionOptionController(ApplicationDbContext _db)
        {
            db = _db;
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

            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return Ok(u);


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
