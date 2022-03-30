using System;                          
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;        
using eAPIClient.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
                var data = db.Notes.Where(r => r.category_note_id == model.category_note_id && r.note.Trim().ToLower() == model.note.Trim().ToLower());

                if (data.Any())
                {
                    return Ok(data.FirstOrDefault());
                }
                else
                {
                    db.Notes.Update(model);
                    await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
                    return Ok(model);
                }
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        [HttpPost("delete")]
        [Authorize]
        public async Task<ActionResult<string>> Delete([FromBody] string ids) {
            //
            try {

                string[] val = ids.Split(',');
                string _data = "";
                 
                foreach(var a in val)
                {
                    _data += $"'{a}',";
                }
                if (_data != "")
                {
                    _data = _data.Substring(0, _data.Length - 1);
                }       
                string _query = $"delete tbl_note where is_predefine_note = 0 and id in ({_data})";   
                db.Database.ExecuteSqlRaw(_query); 
                return Ok();

            }catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }

}