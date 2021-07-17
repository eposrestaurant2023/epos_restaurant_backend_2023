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


namespace eAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class SaleTypeController : ODataController
    {
        private readonly ApplicationDbContext db;
        public SaleTypeController(ApplicationDbContext _db)
        {
            db = _db;
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
