using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using eAPIClient;
using eAPIClient.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NETCore.Encrypt;


namespace eAPIClient.Controllers
{
    [ApiController] 
    [Route("api/[controller]")]
    public class NoteController : ODataController
    {

        private readonly ApplicationDbContext db;
        public NoteController(ApplicationDbContext _db)
        {
            db = _db;
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)] 
        public IQueryable<NoteModel> Get()
        {
            return db.Notes;  
        }

        [HttpGet("category")]
        [EnableQuery(MaxExpansionDepth = 8)]
        public IQueryable<CategoryNoteModel> GetCategoryNote()
        {
            return db.CategoryNotes;
        }

        [HttpGet("find")]
        [EnableQuery(MaxExpansionDepth = 4)]
        public SingleResult<NoteModel> Get([FromODataUri] Guid key)
        {
            var s = db.Notes.Where(r => r.id == key).AsQueryable();

            return SingleResult.Create(s);
        }

        [HttpPost("save")]
        [Authorize]
        public async Task<ActionResult<string>> Save([FromBody] NoteModel model)
        {
            try
            {
                db.Notes.Update(model);
                await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
                return Ok(model);
            }
            catch
            {
                return BadRequest();
            }
        }
    }

}
