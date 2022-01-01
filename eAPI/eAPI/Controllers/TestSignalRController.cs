using eAPI.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

namespace eAPI.Controllers
{
    [ApiController,Route("api/[controller]")]
    public class TestSignalRController:ControllerBase
    {
        private readonly IHubContext<ConnectionHub> hub;
        private readonly ApplicationDbContext db;
        public TestSignalRController(IHubContext<ConnectionHub> _hub, ApplicationDbContext _db)
        {
            hub = _hub;
            db = _db;
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromQuery] Guid id)
        {
           var a = await db.Customers.FindAsync(id);
            await hub.Clients.All.SendAsync("Sync", a);
            return Ok();
        }
    }
}
