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

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [Route("getsingle")]
        public async Task<SingleResult<CurrencyModel>> Get([FromODataUri] int key)
        {
            return await Task.Factory.StartNew(() => SingleResult.Create<CurrencyModel>(db.Currencies.Where(r => r.id == key && r.is_deleted == false).AsQueryable()));
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

        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult<bool>> DeleteRecord(int id) //Delete
        {
            //check if currency already use by any payment type then now allow to delete 
            var p = db.PaymentTypes.Where(r => r.currency_id == id && r.is_deleted == false).AsNoTracking();
            if (p.Any()) {
                return StatusCode(301, "This currency is being used in payment type");
            }
            var data =   db.Currencies.Where(r => r.id == id).Include(r => r.business_branch_currencies).ToList();

            if (data.Any())
            {
                CurrencyModel c = data.FirstOrDefault();
                c.is_deleted = true;
                c.business_branch_currencies.ForEach(r => r.is_deleted = true);
                db.Currencies.Update(c);
                await db.SaveChangesAsync();
                return Ok();
            }
           
            return NotFound();
        }


        [HttpPost]
        [Route("MarkAsMainCurrency/{id}")]
        public async Task<ActionResult<bool>> MarkAsMainCurrency(int id) //Delete
        {

            var old_main_currency = db.Currencies.Where(r => r.is_main == true);
            if (old_main_currency.Any())
            {
                CurrencyModel old_currency = old_main_currency.FirstOrDefault();
                old_currency.is_main = false;
                old_currency.is_base_exchange_currency = false;
            }
            var data = db.Currencies.Where(r => r.id == id).Include(r => r.business_branch_currencies).ToList(); ;

            if (data.Any())
            {
                CurrencyModel c = data.FirstOrDefault();
                c.is_main=true;
                c.default_change_exchange_rate = 1;
                c.default_exchange_rate = 1;
                c.is_base_exchange_currency = true;

                c.business_branch_currencies.ForEach(r => { r.exchange_rate = 1; r.change_exchange_rate = 1; r.change_exchange_rate_input = 1;r.exchange_rate_input = 1; });
                db.Currencies.Update(c);
                await db.SaveChangesAsync();
                return Ok();
            }

            return NotFound();
        }



        [HttpPost("save")]
        public async Task<ActionResult> Save([FromBody] CurrencyModel u)
        {


            var data = db.Currencies.AsNoTracking();

            if (u.id == 0)
            {
                u.id = data.Max(r => r.id) + 1;
                db.Currencies.Add(u);
            }
            else
            {

                db.Currencies.Update(u);
            }

            try
            {
                await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
 
                    db.Database.ExecuteSqlRaw("exec sp_update_exchange_base_currency_exchange_rate " + u.id);
                

                

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
