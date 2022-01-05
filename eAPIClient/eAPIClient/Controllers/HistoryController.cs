using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using eAPIClient;
using eAPIClient.Models;
using eAPIClient.Services;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NETCore.Encrypt;


namespace eAPIClient.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class HistoryController : ODataController
    {

         
        private readonly ApplicationDbContext db;
        private readonly ISyncService sync;
        public HistoryController(ApplicationDbContext _db, ISyncService sync)
        {
            db = _db;
            this.sync = sync;
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
            sync.sendSyncRequest();

            return Ok(u);


        }

         
    }

}
