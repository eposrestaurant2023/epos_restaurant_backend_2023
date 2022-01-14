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
    public class SaleProductController : ODataController
    {
        private readonly ApplicationDbContext db;
        private readonly AppService app;
        private readonly IHubContext<ConnectionHub> hub;
        public SaleProductController(ApplicationDbContext _db, AppService _app, IHubContext<ConnectionHub> _hub)
        {
            db = _db;hub = _hub;
            app = _app;
        }        

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 10, MaxNodeCount = 200)]
        public IQueryable<SaleProductModel> Get(string keyword = "")
        {
            if (!string.IsNullOrEmpty(keyword))
            {
              return (from r in db.SaleProducts
                           where 
                                 EF.Functions.Like(
                                     (
                                        (r.sale.document_number ?? " ") + 
                                        (r.sale.customer.customer_name_en ?? " ") +
                                        (r.sale.customer.customer_name_kh ?? " ") + 
                                        (r.product_name_en ?? " ") + 
                                        (r.product_name_kh ?? " ") + 
                                        (r.product_code ?? " ")  
                                     ).ToLower().Trim(), $"%{keyword}%".ToLower().Trim())
                           select r);

            }
            else
            {
                return db.SaleProducts.AsQueryable();
            }
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [Route("getsingle")]
        public async Task<SingleResult<SaleProductModel>> Get([FromODataUri] Guid key)
        {
            return await Task.Factory.StartNew(() => SingleResult.Create<SaleProductModel>(db.SaleProducts.Where(r => r.id == key).AsQueryable()));
        }

    }
}