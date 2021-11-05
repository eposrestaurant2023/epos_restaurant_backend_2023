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
    public class CurrencyController : ODataController
    {

        private readonly ApplicationDbContext db;
        public CurrencyController(ApplicationDbContext _db)
        {
            db = _db;
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [AllowAnonymous]      
        public IQueryable<CurrencyModel> Get()
        {
            return db.Currencies;
        }

 
        [HttpPost("save/multiple")]
        public async Task<ActionResult<string>> SaveMultiple([FromBody] List<CurrencyModel> c)
        {
            try
            {
                db.Currencies.UpdateRange(c);
                await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
                db.Database.ExecuteSqlRaw("exec sp_update_business_branch_currency");
                return Ok(c);
            }
            catch (Exception ex){
                string error = ex.Message;
            }
            return Ok();
        
        }


        [HttpPost("save")]
        public async Task<ActionResult> Save([FromBody] CurrencyModel u)
        {

            if (u.id == 0)
            {

                db.Currencies.Add(u);
            }
            else
            {

                db.Currencies.Update(u);
            }

            try
            {
                await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
                return Ok(u);
            }
            catch (Exception e)
            {
                return Problem(e.InnerException.Message);
                throw;
            }
        }
    }

}
