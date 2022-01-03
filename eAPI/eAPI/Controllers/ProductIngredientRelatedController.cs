using eAPI;
using eAPI.Controllers;
using eAPI.Hubs;
using eModels;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace eAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductIngredientRelatedController : ControllerBase
    {
        private readonly ApplicationDbContext db;
        private readonly IHubContext<ConnectionHub> hub;
        public ProductIngredientRelatedController(ApplicationDbContext _db,IHubContext<ConnectionHub> _hub)
        {
            db = _db;hub = _hub;
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 0)]
        public ActionResult<List<ProductIngredientRelatedModel>> Get()
        {
            var per = db.ProductIngredientRelateds;
            return Ok(per);
        }
         
    }
}
