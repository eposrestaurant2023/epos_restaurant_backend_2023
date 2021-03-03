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
        public IQueryable<SaleModel> Get(string keyword = "")
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                return (from r in db.Sales
                        where
                              EF.Functions.Like(
                                  (
                                     (r.document_number ?? " ") +
                                     (r.customer.customer_name_en ?? " ") +
                                     (r.customer.customer_name_kh ?? " ") +
                                     (r.sale_note ?? " ")
                                  ).ToLower().Trim(), $"%{keyword}%".ToLower().Trim())
                        select r);

            }
            else
            {
                return db.Sales.AsQueryable();
            }
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
                model.customer = null;

                if(model.id == Guid.Empty)
                {
                    db.Sales.Add(model);
                }
                else
                {
                    db.Sales.Update(model);
                }
                await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));

                return Ok(model);
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
            
        }
    }

}
