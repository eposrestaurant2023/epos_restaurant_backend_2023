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
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly ApplicationDbContext db;
        public ContactController(ApplicationDbContext _db)
        {
            db = _db;
        }
        [HttpGet()]
        public IQueryable<ContactModel> Get(){
            return db.Contacts;
        }
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ContactModel contact)
        {
            if (contact.id == 0)
            {
                db.Contacts.Add(contact);
            }
            else
            {
                db.Contacts.Update(contact);
            }
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return Ok(contact);
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 0)]
        [Route("find")]

        public SingleResult<ContactModel> Get([FromODataUri] int key)
        {
            var c = db.Contacts.Where(r => r.id == key).AsQueryable();
            return SingleResult.Create(c);
        }

        [HttpPost("Delete/{id}")]
        [EnableQuery(MaxExpansionDepth = 0)]
        public async Task<ActionResult<ContactModel>> Delete([FromODataUri] int id)
        {
            var c = db.Contacts.Find(id);
            c.is_deleted = c.is_deleted;
            db.Contacts.Update(c);
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return Ok(c);
        }
    }
}
