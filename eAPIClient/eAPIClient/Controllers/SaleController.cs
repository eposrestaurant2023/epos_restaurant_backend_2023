using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using eAPIClient.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;     


namespace eAPIClient.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SaleController : ODataController
    {
        public IConfiguration config { get; }
        private readonly ApplicationDbContext db;
        public SaleController(ApplicationDbContext _db, IConfiguration configuration)
        {
            db = _db;
            config = configuration;
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
         public IQueryable<SaleModel> Get(string keyword = "",string shift="")
       {
           
                return (from r in db.Sales
                        where EF.Functions.Like(
                                  ((r.document_number ?? " ") + (r.customer.customer_name_en ?? " ") +
                                   (r.customer.customer_name_kh ?? " ") + (r.sale_note ?? " ")
                                  ).ToLower().Trim(), $"%{(keyword??"")}%".ToLower().Trim()) && r.cashier_shift.shift == ((shift ?? "") == "" ? r.cashier_shift.shift : shift)
                        select r);

           
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [Route("findOne")]
        public async Task<SingleResult<SaleModel>> Get([FromODataUri] Guid key)
        {
            return await Task.Factory.StartNew(() => SingleResult.Create<SaleModel>(db.Sales.Where(r => r.id == key).AsQueryable()));
        }

        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] SaleModel model)
        {
            try
            {
                bool is_new = true;
                model.customer = null;   

                if(model.id == Guid.Empty)
                {
                    db.Sales.Add(model);
                }
                else
                {
                    is_new = false;
                    db.Sales.Update(model);
                }
                await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));   
                
                if(!is_new)
                {
                    if (model.is_closed==true && (model.document_number == "New" || model.document_number == ""))
                    {
                        model.document_number = await GenerateDocumentNumber(model.outlet_id.ToString());
                        await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
                    }
                }

                return Ok(model);
            }
            catch 
            {
                return BadRequest();
            }
            
        }

     

       async Task<string> GenerateDocumentNumber(string outlet_id)
        {

            var _doc = (from r in db.DocumentNumbers
                    where
                          EF.Functions.Like(
                              (
                                 (r.outlet_id ?? " ") 
                              ).ToLower().Trim(), $"%{outlet_id}%".ToLower().Trim())
                    select r);
            string _result = "";
            if (_doc.Count() > 0)
            {
               var _d = _doc.FirstOrDefault();
                _d.counter += 1;
                db.DocumentNumbers.Update(_d);
                await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
                _result = string.Format(@"{0}{1:"+_d.format+"}{2:"+_d.counter_digit+"}",_d.prefix,DateTime.Now,_d.counter);  
            }      
            return _result;
        }
    }

}
