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
using Newtonsoft.Json;

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
        
        public IQueryable<ProjectModel> Get()
        {

            return db.Project;

        }


        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] ProjectModel u)
        {
            if (u.id == Guid.Empty)
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


        [HttpPost("SaveServerID")]
        public async Task<ActionResult<string>> SaveServerID([FromBody] ServerConfigModel u)
        {

            var p = db.Project.Find(Guid.Parse(u.project_id));
            p.server_id = u.server_id;
            db.Project.Update(p);

            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return Ok(u);
        }

        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult<ProjectModel>> DeleteRecord(Guid id) //Delete
        {
            var u = await db.Project.FindAsync(id);
            u.is_deleted = !u.is_deleted;

            db.Project.Update(u);
            await db.SaveChangesAsync();
            return Ok(u);
        }

        [HttpPost]
        [Route("clone/{id}")]

        public IActionResult Clone(Guid id)
        {
            var c = db.Project.Where(r => r.id == id).FirstOrDefault();
            c.status = true;
            c.id = Guid.Empty;
            c.project_code = "";
            c.start_date = null;
            c.customer_id = Guid.Empty;
            return Ok(c);
        }
        [HttpPost]
        [Route("ChangedCustomer")]
        public async Task<ActionResult<CustomerModel>> ChangeCustomer(Guid project_id, Guid customer_id)
        {
            var project = await db.Project.FindAsync(project_id);
            var customer = await db.Customers.FindAsync(customer_id);
            project.customer_id = customer_id;
            db.Project.Update(project);
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return Ok(customer);
        }
        [HttpGet("find")]
        [EnableQuery(MaxExpansionDepth = 0)]
        public SingleResult<ProjectModel> Get([FromODataUri] Guid key)
        {
            var s = db.Project.Where(r => r.id == key).AsQueryable();

            return SingleResult.Create(s);
        }
    }

}
