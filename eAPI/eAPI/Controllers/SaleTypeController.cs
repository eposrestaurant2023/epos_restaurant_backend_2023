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

        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] SaleTypeModel u)
        {

            if (u.id == Guid.Empty)
            {

                db.SaleTypes.Add(u);
            }
            else
            {

                db.SaleTypes.Update(u);
            }

            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return Ok(u);

        }

    }

    
}
