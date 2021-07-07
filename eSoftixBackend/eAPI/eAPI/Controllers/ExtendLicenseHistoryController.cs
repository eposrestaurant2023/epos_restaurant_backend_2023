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
    public class ExtendLicenseHistoryController : ODataController
    {

        private readonly ApplicationDbContext db;
        public ExtendLicenseHistoryController(ApplicationDbContext _db)
        {
            db = _db;
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [AllowAnonymous]
        public IQueryable<ExtendLicenseHistoryModel> Get()
        {
                return db.ExtendLicenseHistories;
        }

        
        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] ExtendLicenseHistoryModel u)
        {
            try
            {
                if (u.id == 0)
                {

                    db.ExtendLicenseHistories.Add(u);
                }
                else
                {

                    db.ExtendLicenseHistories.Update(u);

                }

                await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
                return Ok(u);

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }

            return StatusCode(503);


        }

    }

}
