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
    public class TableGroupController : ODataController
    {

        private readonly ApplicationDbContext db;
        public TableGroupController(ApplicationDbContext _db)
        {
            db = _db;
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [AllowAnonymous]
        public IQueryable<TableGroupModel> Get()
        {
           
                return db.TableGroups;
           
        }

        
        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] TableGroupModel u)
        {
           
            
            
            if (u.id == 0)
            {
                db.TableGroups.Add(u);
                }
            else
            {
                db.TableGroups.Update(u);
            }

            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return Ok(u);


        }


        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult<TableGroupModel>> DeleteRecord(int id) //Delete
        {
            var u = await db.TableGroups.FindAsync(id);
            u.is_deleted = !u.is_deleted;
            
            db.TableGroups.Update(u);
            await db.SaveChangesAsync();
            return Ok(u);
        }

        [HttpGet("find")]
        [EnableQuery(MaxExpansionDepth = 4)]
        public SingleResult<TableGroupModel> Get([FromODataUri] int key)
        {
            var s = db.TableGroups.Where(r => r.id == key).AsQueryable();

            return SingleResult.Create(s);
        }
    }

}
