using eModels;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
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
    [Authorize]
    public class PermissionOptionController:ControllerBase
    {
        private readonly ApplicationDbContext db;
        public PermissionOptionController(ApplicationDbContext _db)
        {
            db = _db;
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 0)]
        public ActionResult<List<PermissionOptionModel>> Get(bool is_deleted = false)
        {
            var cus = db.PermissionOption;
            return Ok(cus);
        }

        [HttpPost("save")]
        public async Task<ActionResult<PermissionOptionModel>> Save([FromBody] PermissionOptionModel c)
        {

            if (c.id == 0)
            {

                db.PermissionOption.Add(c);
            }
            else
            {
                foreach (var a in c.permission_option_roles)
                {
                    if (a.is_delete)
                    {
                        db.PermissionOptionRole.Remove(a);
                    }
                    await db.SaveChangesAsync();
                }
                db.PermissionOption.Update(c);
            }
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return Ok(c);
        }

    }
}
