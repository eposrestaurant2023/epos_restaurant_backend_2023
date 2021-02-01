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
    public class PaymentTypeController : ODataController
    {

        private readonly ApplicationDbContext db;
        public PaymentTypeController(ApplicationDbContext _db)
        {
            db = _db;
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [AllowAnonymous]
      
        public IQueryable<PaymentTypeModel> Get()
        {
           
                return db.PaymentTypes;
           
        }
        
        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] PaymentTypeModel u)
        {            
            if (u.id == 0)
            {
                db.PaymentTypes.Add(u);
            }
            else
            { 
                db.Database.ExecuteSqlRaw($"delete tbl_business_branch_payment_type where payment_type_id = {u.id}");
                db.BusinessBranchPaymentTypes.AddRange(u.business_branch_payment_types);
                db.PaymentTypes.Update(u);
            }            
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));


            return Ok(u);
        }

        [HttpGet("find")]
        [EnableQuery(MaxExpansionDepth = 4)]
        public SingleResult<PaymentTypeModel> Get([FromODataUri] int key)
        {
            var s = db.PaymentTypes.Where(r => r.id == key).AsQueryable();
            return SingleResult.Create(s);
        }

        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult<PaymentTypeModel>> DeleteRecord(int id) //Delete
        {
            var u = await db.PaymentTypes.FindAsync(id);
            u.is_deleted = !u.is_deleted;            
            db.PaymentTypes.Update(u);
            await db.SaveChangesAsync();
            return Ok(u);
        }
    }
}
