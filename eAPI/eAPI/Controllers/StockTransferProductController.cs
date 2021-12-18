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
    public class StockTransferProductController : ODataController
    {
        private readonly ApplicationDbContext db;
        private readonly AppService app;
        public StockTransferProductController(ApplicationDbContext _db, AppService _app)
        {
            db = _db;
            app = _app;
        }        

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8, MaxNodeCount = 200)]   
        public IQueryable<StockTransferProductModel> Get(string keyword = "")
        {
            if (!string.IsNullOrEmpty(keyword))
            {
              return (from r in db.StockTransferProducts
                           where 
                                 EF.Functions.Like(
                                     (
                                        (r.stock_transfer.document_number ?? " ") + 
                                        (r.stock_transfer.reference_number ?? " ") 
                                     ).ToLower().Trim(), $"%{keyword}%".ToLower().Trim())
                           select r);

            }
            else
            {
                return db.StockTransferProducts.AsQueryable();
            }
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [Route("getsingle")]
        public async Task<SingleResult<StockTransferProductModel>> Get([FromODataUri] int key)
        {
            return await Task.Factory.StartNew(() => SingleResult.Create<StockTransferProductModel>(db.StockTransferProducts.Where(r => r.id == key).AsQueryable()));
        }

    }
}