using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
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
    public class HistoryController : ODataController
    {

        private readonly ApplicationDbContext db;
        public HistoryController(ApplicationDbContext _db)
        {
            db = _db;
        }



        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        public IQueryable<HistoryModel> Get(string keyword = "")
        {

            return db.Histories;

        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [Route("getsingle")]
        public async Task<SingleResult<HistoryModel>> Get([FromODataUri] Guid key)
        {
            return await Task.Factory.StartNew(() => SingleResult.Create<HistoryModel>(db.Histories.Where(r => r.id == key).AsQueryable()));
        }


        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] HistoryModel u)
        {


            if (u.id == Guid.Empty)
            {

                db.Histories.Add(u);
            }
            else
            {

                db.Histories.Update(u);
            }

            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));

            return Ok(u);


        }




        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult<HistoryModel>> DeleteRecord(int id) //Delete
        {
            var u = await db.Histories.FindAsync(id);
            u.is_deleted = !u.is_deleted;

            db.Histories.Update(u);
            await db.SaveChangesAsync();
            return Ok(u);
        }
    }

}
