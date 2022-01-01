using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using eAPI.Services;
using eModels;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;   
using NETCore.Encrypt;
using System.Text.Json;
using eAPI.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace eAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CashDrawerAmountController : ODataController
    {
        private readonly ApplicationDbContext db;
        private readonly IHubContext<ConnectionHub> hub;
        public CashDrawerAmountController(ApplicationDbContext _db,IHubContext<ConnectionHub> _hub)
        {
            db = _db;hub = _hub; 
        }        

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]   
        public IQueryable<CashDrawerAmountModel> Get(string keyword = "")
        {
                return db.CashDrawerAmounts.AsQueryable();
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [Route("getsingle")]
        public async Task<SingleResult<CashDrawerAmountModel>> Get([FromODataUri] Guid key)
        {
            return await Task.Factory.StartNew(() => SingleResult.Create<CashDrawerAmountModel>(db.CashDrawerAmounts.Where(r => r.id == key).AsQueryable()));
        }


        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] CashDrawerAmountModel model)
        {
            try
            {
                var _modelCheck = db.CashDrawerAmounts.Where(r => r.id == model.id).AsNoTracking();
                if (_modelCheck.Count() > 0)
                {
                    db.CashDrawerAmounts.Update(model);
                }
                else
                {
                    db.CashDrawerAmounts.Add(model);
                }

                await db.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                var _ex = ex;
                return BadRequest();
            }
        }

    }
}