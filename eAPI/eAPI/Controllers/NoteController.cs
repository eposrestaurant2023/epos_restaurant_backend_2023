﻿using System;
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
    public class NoteController : ODataController
    {

        private readonly ApplicationDbContext db;
        private readonly IHubContext<ConnectionHub> hub;
        public NoteController(ApplicationDbContext _db,IHubContext<ConnectionHub> _hub)
        {
            db = _db;hub = _hub;
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [AllowAnonymous]
        public IQueryable<NoteModel> Get(string keyword = "")
        {
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                return db.Notes.Where(r =>
                (
                (r.note ?? "") +
                (r.business_branch.business_branch_name_en ?? "") +
                (r.category_note.category_note_name_en ?? "")
                ).ToLower().Trim().Contains(keyword.ToLower().Trim()));
            }
            else
            {
                return db.Notes;
            }
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [Route("getsingle")]
        public async Task<SingleResult<NoteModel>> GetQuery([FromODataUri] Guid key)
        {
            return await Task.Factory.StartNew(() => SingleResult.Create<NoteModel>(db.Notes.Where(r => r.id == key).AsQueryable()));
        }

        [HttpGet("category")]
        [EnableQuery(MaxExpansionDepth = 8)]
        public IQueryable<CategoryNoteModel> GetCategoryNote()
        {
            return db.CategoryNotes;
        }


        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] NoteModel u)
        {
            if (u.id == Guid.Empty)
            {
                db.Notes.Add(u);
            }
            else
            {
                db.Notes.Update(u);
            }
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)),hub);
            return Ok(u);
        }

        [HttpGet("find")]
        [EnableQuery(MaxExpansionDepth = 4)]
        public SingleResult<NoteModel> Get([FromODataUri] Guid key)
        {
            var s = db.Notes.Where(r => r.id == key).AsQueryable();
            return SingleResult.Create(s);
        }

        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult<NoteModel>> DeleteRecord(int id) //Delete
        {
            var u = await db.Notes.FindAsync(id);
            u.is_deleted = !u.is_deleted;
            db.Notes.Update(u);
            await db.SaveChangesAsync();
            return Ok(u);
        }


    }
}
