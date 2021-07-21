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
    public class eKnowledgeBaseController : ODataController
    {

        private readonly ApplicationDbContext db;
        public eKnowledgeBaseController(ApplicationDbContext _db)
        {
            db = _db;
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [AllowAnonymous]
        public IQueryable<eKnowledgeBaseModel> Get()
        {
           
                return db.eKnowledgeBases.AsQueryable();
           
        }


        [HttpGet]
        [Route("GetData")]
        [AllowAnonymous]
        public IQueryable<eKnowledgeBaseModel> GetData()
        {

            return db.eKnowledgeBases;

        }

        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] List<eKnowledgeBaseModel> u)
        {
            var a = u.Where(r => r.id == Guid.Empty).ToList();
            var b= u.Where(r => r.id != Guid.Empty).ToList();
            db.eKnowledgeBases.AddRange(a);
            db.eKnowledgeBases.UpdateRange(b);
            try
            {
                await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            }
            catch (Exception c)
            {
                var x = c.Message;
                throw;
            }
           
            return Ok(u);


        }

        [HttpPost("savesingle")]
        public async Task<ActionResult<string>> savesingle([FromBody] eKnowledgeBaseModel u)
        {



            if (u.id == Guid.Empty)
            {
                db.eKnowledgeBases.Add(u);
            }
            else
            {
                db.eKnowledgeBases.Update(u);
            }

            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return Ok(u);


        }

        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult<eKnowledgeBaseModel>> DeleteRecord(int id) //Delete
        {
            var u = await db.eKnowledgeBases.FindAsync(id);
            
            
            db.eKnowledgeBases.Update(u);
            await db.SaveChangesAsync();
            return Ok(u);
        }

        [HttpGet("find")]
        [EnableQuery(MaxExpansionDepth = 4)]
        public SingleResult<eKnowledgeBaseModel> Get([FromODataUri] Guid key)
        {
            var s = db.eKnowledgeBases.Where(r => r.id == key).AsQueryable();

            return SingleResult.Create(s);
        }

    }

}
