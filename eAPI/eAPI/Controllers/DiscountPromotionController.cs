using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
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
    public class DiscountPromotionController : ODataController
    {

        private readonly ApplicationDbContext db;
        private readonly IHubContext<ConnectionHub> hub;
        public DiscountPromotionController(ApplicationDbContext _db,IHubContext<ConnectionHub> _hub)
        {
            db = _db;hub = _hub;
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        public IQueryable<DiscountPromotionModel> Get()
        {
            return db.DiscountPromotions;
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [Route("getsingle")]
        public async Task<SingleResult<DiscountPromotionModel>> Get([FromODataUri] Guid key)
        {
            return await Task.Factory.StartNew(() => SingleResult.Create<DiscountPromotionModel>(db.DiscountPromotions.Where(r => r.id == key).AsQueryable()));
        }


        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] DiscountPromotionModel model)
        {
            var data = db.DiscountPromotions.Where(r => r.id == model.id).AsNoTracking();
            if (data.Any())
            {
                db.DiscountPromotions.Update(model);
            }else
            {
                db.DiscountPromotions.Add(model);
            }
            try
            {
                await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)), hub);
                return Ok(model);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.ToString());
            }  
        }   

        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult<DiscountPromotionModel>> DeleteRecord(int id) //Delete
        {
            var u = await db.DiscountPromotions.FindAsync(id);
            u.is_deleted = !u.is_deleted;
            db.DiscountPromotions.Update(u);
            await db.SaveChangesAsync();
            return Ok(u);
        }       
        [HttpPost]
        [Route("UpdateStatus/{id}")]
        public async Task<ActionResult<DiscountPromotionModel>> UpdateStatus(Guid id) //Update
        {
            var u = await db.DiscountPromotions.FindAsync(id);
            u.status = !u.status;
            db.DiscountPromotions.Update(u);
            await db.SaveChangesAsync();
            return Ok(u);
        }
    }
}
