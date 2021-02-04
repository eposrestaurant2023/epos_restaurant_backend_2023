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

namespace eAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SaleController : ODataController
    {
        private readonly ApplicationDbContext db;
        private readonly AppService app;
        public SaleController(ApplicationDbContext _db, AppService _app)
        {
            db = _db;
            app = _app;
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
                                        (r.sale_note ?? " ") +
                                        (r.reference_number ?? " ")
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
        [Route("getsingle")]
        public async Task<SingleResult<SaleModel>> Get([FromODataUri] Guid key)
        {
            return await Task.Factory.StartNew(() => SingleResult.Create<SaleModel>(db.Sales.Where(r => r.id == key).AsQueryable()));
        }

    }
}