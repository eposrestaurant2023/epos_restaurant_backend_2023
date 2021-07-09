using eModels;
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
            if (contact.id > 0)
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
    }
}
