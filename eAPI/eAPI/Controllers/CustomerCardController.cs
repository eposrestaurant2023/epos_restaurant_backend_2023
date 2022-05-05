using eAPI.Hubs;
using eModels;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace eAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerCardController:ControllerBase
    {
        private readonly ApplicationDbContext db;
        private readonly IHubContext<ConnectionHub> hub;
        public CustomerCardController(ApplicationDbContext _db, IHubContext<ConnectionHub> _hub)
        {
            db = _db;
            hub = _hub;
        }
        [HttpGet]
        [EnableQuery]
        public IQueryable<CustomerCardModel> Get()
        {
            return db.CustomerCards.AsQueryable();
        }

        [HttpPost("Save")]
        
        public async Task<ActionResult> Save([FromBody] CustomerCardModel customer_card)
        {
            

            if (customer_card.id == System.Guid.Empty)
            {
                var cards = db.CustomerCards.Where(r => r.card_code == customer_card.card_code).AsNoTracking().ToList();
                if (cards.Any())
                {
                    return BadRequest("Card code already exist.");
                }
                db.CustomerCards.Add(customer_card);
            }
            else
            {
                var cards = db.CustomerCards.Where(r => r.card_code == customer_card.card_code && r.id != customer_card.id).AsNoTracking().ToList();
                if (cards.Any())
                {
                    return BadRequest("Card code already exist.");
                }
                db.CustomerCards.Update(customer_card);
            }
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)), hub);
            return Ok(customer_card);
        }
    }
}
