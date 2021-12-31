using eAPI.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eAPI.Controllers
{
    [ApiController,Route("api/[controller]")]
    public class TestSignalRController:ControllerBase
    {
        private readonly IHubContext<ConnectionHub> hub;
        public TestSignalRController(IHubContext<ConnectionHub> _hub)
        {
            hub = _hub;
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromQuery] string  title)
        {
            await hub.Clients.All.SendAsync("connectionhub",$"create file content <{title}> from Api Admin");
            return Ok();
        }
    }
}
