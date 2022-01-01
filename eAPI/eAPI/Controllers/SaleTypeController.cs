using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
    public class SaleTypeController : ODataController
    {
        private readonly ApplicationDbContext db;
        private readonly IHubContext<ConnectionHub> hub;
        public SaleTypeController(ApplicationDbContext _db,IHubContext<ConnectionHub> _hub)
        {
            db = _db;hub = _hub;
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [AllowAnonymous]
        public IQueryable<SaleTypeModel> Get()
        {
            return db.SaleTypes;
        }

        [HttpPost("SaveTest")]
        public async Task<ActionResult<string>> SaveTest([FromBody] TestModel u)
        {
 


            AddOrUpdate(u);
            return Ok(u);

        }

        public virtual void AddOrUpdate(TestModel entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            db.Update(entity);
            db.SaveChanges();
        }

    }

  


}
