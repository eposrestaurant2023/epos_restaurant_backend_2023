﻿using System;
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
    public class ProjectController : ODataController
    {

        private readonly ApplicationDbContext db;
        public ProjectController(ApplicationDbContext _db)
        {
            db = _db;
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [AllowAnonymous]
        public IQueryable<ProjectModel> Get()
        {

            return db.Project;

        }


        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] ProjectModel u)
        {



            if (u.id == 0)
            {
                db.Project.Add(u);
            }
            else
            {
                db.Project.Update(u);
            }

            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return Ok(u);


        }


        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult<ProjectModel>> DeleteRecord(int id) //Delete
        {
            var u = await db.Project.FindAsync(id);
            u.is_deleted = !u.is_deleted;

            db.Project.Update(u);
            await db.SaveChangesAsync();
            return Ok(u);
        }

        [HttpGet("find")]
        [EnableQuery(MaxExpansionDepth = 4)]
        public SingleResult<ProjectModel> Get([FromODataUri] int key)
        {
            var s = db.Project.Where(r => r.id == key).AsQueryable();

            return SingleResult.Create(s);
        }
    }

}