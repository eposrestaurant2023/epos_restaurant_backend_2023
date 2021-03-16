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
    public class BusinessInformationController : ODataController
    {

        private readonly ApplicationDbContext db;
        public BusinessInformationController(ApplicationDbContext _db)
        {
            db = _db;
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [AllowAnonymous]

        public IQueryable<BusinessInformationModel> Get()
        {

            return db.BusinessInformations;

        }


        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] BusinessInformationModel u)
        {

            if (u.id == Guid.Empty)
            {
                db.BusinessInformations.Add(u);
            }
            else
            {
                db.BusinessInformations.Update(u);
            }

            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return Ok(u);


        }

        [HttpGet("find")]
        [EnableQuery(MaxExpansionDepth = 4)]
        public SingleResult<BusinessInformationModel> Get([FromODataUri] Guid key)
        {
            var s = db.BusinessInformations.Where(r => r.id == key).AsQueryable();

            return SingleResult.Create(s);
        }
    }
}
