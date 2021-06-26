using eModels;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace eAPI.Controllers
{
    [ApiController,Route("api/[Controller]"),]
    public class AttachFilesController:ControllerBase
    {
        private readonly ApplicationDbContext db;
        public AttachFilesController(ApplicationDbContext db)
        {
            this.db = db;
        }
        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 0)]
        public ActionResult<List<AttachFilesModel>> GetAttachFile(bool is_deleted = false)
        {
            var per = db.AttachFiles;
            return Ok(per);
        }

        [HttpPost("save")]
        public async Task<ActionResult<AttachFilesModel>> SaveAttachFile([FromBody] AttachFilesModel t)
        {
            if (t.id == 0)
            {

                db.AttachFiles.Add(t);
            }
            else
            {
                db.AttachFiles.Update(t);
            }

            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return Ok(t);
        }
        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult<AttachFilesModel>> DeleteRecord(int id) //Delete
        {
            var d = await db.AttachFiles.FindAsync(id);
            d.is_deleted = !d.is_deleted;
            db.AttachFiles.Update(d);
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return Ok(d);
        }
    }
}
