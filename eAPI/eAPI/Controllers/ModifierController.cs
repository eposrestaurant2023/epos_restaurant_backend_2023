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
    public class ModifierController : ODataController
    {

        private readonly ApplicationDbContext db;
        public ModifierController(ApplicationDbContext _db)
        {
            db = _db;
        }



        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        public IQueryable<ModifierModel> Get(string keyword = "")
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                return db.Modifiers.Where(r =>
                (
                (r.modifier_name ?? "") 
                ).ToLower().Trim().Contains(keyword.ToLower().Trim()));
            }
            else
            {
                return db.Modifiers;
            }
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [Route("getsingle")]
        public async Task<SingleResult<ModifierModel>> Get([FromODataUri] int key)
        {
            return await Task.Factory.StartNew(() => SingleResult.Create<ModifierModel>(db.Modifiers.Where(r => r.id == key).AsQueryable()));
        }


        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] ModifierModel u)
        {

          
            if (u.id == 0)
            {

                db.Modifiers.Add(u);
            }
            else
            {
                
                db.Modifiers.Update(u);
            }
            
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));

         
         
            return Ok(db.Modifiers.Find(u.id));


        }

      


        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult<ModifierModel>> DeleteRecord(Guid id) //Delete
        {
            var u = await db.Modifiers.FindAsync(id);
            u.is_deleted = !u.is_deleted;
            
            db.Modifiers.Update(u);
            await db.SaveChangesAsync();
            return Ok(u);
        }
    }

}
