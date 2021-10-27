using eModels;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace eAPI.Controllers
{
    [ApiController, Authorize]
    [Route("api/[Controller]")]
    public class CashDrawerController:ControllerBase
    {
        private readonly ApplicationDbContext db;
        public CashDrawerController(ApplicationDbContext _db)
        {
            db = _db;
        }
        [HttpGet]
        [EnableQuery]
        public IQueryable<CashDrawerModel> Get()
        {
            return db.CashDrawers.AsQueryable();
           
        }
        [HttpPost, Route("save")]

        public async Task<ActionResult<CashDrawerModel>> Save([FromBody] CashDrawerModel p)
        {
            if (p.id == Guid.Empty)
            {
                db.CashDrawers.Add(p);
            }
            else
            {
                db.CashDrawers.Update(p);
            }
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return Ok(p);
        }
    }
}
