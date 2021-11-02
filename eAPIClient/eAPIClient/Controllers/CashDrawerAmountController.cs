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
    public class CashDrawerAmountController : ODataController
    {

        private readonly ApplicationDbContext db;
        private readonly AppService app;
        public CashDrawerAmountController(ApplicationDbContext _db, AppService _app)
        {
            db = _db;
            app = _app;
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)] 
        public IQueryable<CashDrawerAmountModel> Get()
        {  
                return db.CashDrawerAmounts;     
        }

        
        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] CashDrawerAmountModel u)
        {
            try
            {
                DocumentNumberModel _doc = new DocumentNumberModel();
                if (u.id == Guid.Empty)
                {
                  
                    db.CashDrawerAmounts.Add(u);
                }
                else
                {
                    db.CashDrawerAmounts.Update(u);
                }
                await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
                //Update Document
                await app.UpdateDocument(_doc);
                return Ok(u);
            }
            catch (Exception _ex)
            {
                return BadRequest(new BadRequestModel() { message = _ex.Message });        
            }
        }


        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult<CashDrawerAmountModel>> DeleteRecord(Guid id) //Delete
        {
            try
            {
                var u = await db.CashDrawerAmounts.FindAsync(id);
                u.is_deleted = !u.is_deleted;

                db.CashDrawerAmounts.Update(u);
                await db.SaveChangesAsync();
                return Ok(u);
            }catch(Exception _ex)
            {
                return BadRequest(new BadRequestModel() { message = _ex.Message });
            }
        }

        [HttpGet("find")]
        [EnableQuery(MaxExpansionDepth = 4)]
        public SingleResult<CashDrawerAmountModel> Get([FromODataUri] Guid key)
        {
            var s = db.CashDrawerAmounts.Where(r => r.id == key).AsQueryable();     
            return SingleResult.Create(s);
        }
    }

}
