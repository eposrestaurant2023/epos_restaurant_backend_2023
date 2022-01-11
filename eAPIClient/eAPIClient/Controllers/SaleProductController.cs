using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using eAPIClient.Models;
using eAPIClient.Services;
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
    public class SaleProductController : ODataController
    {
        public IConfiguration config { get; }
        private readonly ApplicationDbContext db;
        private readonly AppService app; 

        public SaleProductController(ApplicationDbContext _db, AppService _app,  IConfiguration configuration)
        {
            db = _db;
            app = _app;
            config = configuration; 
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        public IQueryable<SaleProductModel> Get(string keyword = "", string shift = "", string open_by = "", string close_by = "")
        {
            return (from r in db.SaleProducts
                    where EF.Functions.Like(
                              ((r.sale.document_number ?? " ") + (r.sale.customer.customer_name_en ?? " ") + (r.sale.sale_number ?? " ") +
                               (r.sale.customer.customer_name_kh ?? " ") + (r.sale.sale_note ?? " ") + (r.product_code ?? " ") + (r.product_name_en ?? " ") + (r.product_name_kh ?? " ")
                              ).ToLower().Trim(), $"%{(keyword ?? "")}%".ToLower().Trim())

                    select r);
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [Route("findOne")]
        public async Task<SingleResult<SaleProductModel>> Get([FromODataUri] Guid key)
        {
            return await Task.Factory.StartNew(() => SingleResult.Create<SaleProductModel>(db.SaleProducts.Where(r => r.id == key).AsQueryable()));
        }

       
    }

}
