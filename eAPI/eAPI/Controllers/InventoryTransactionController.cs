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
    public class InventoryTransactionController : ODataController
    {
        private readonly ApplicationDbContext db;
        private readonly AppService app;
        public InventoryTransactionController(ApplicationDbContext _db, AppService _app)
        {
            db = _db;
            app = _app;
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        public IQueryable<InventoryTransactionModel> Get(string keyword = "")
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                return (from r in db.InventoryTransactions
                        where
                              EF.Functions.Like(
                                  (
                                     (r.stock_location.stock_location_name ?? " ") +
                                     (r.reference_number ?? " ")
                                  ).ToLower().Trim(), $"%{keyword}%".ToLower().Trim())
                        select r);

            }
            else
            {
                return db.InventoryTransactions.AsQueryable();
            }
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [Route("getsingle")]
        public async Task<SingleResult<InventoryTransactionModel>> Get([FromODataUri] int key)
        {
            return await Task.Factory.StartNew(() => SingleResult.Create<InventoryTransactionModel>(db.InventoryTransactions.Where(r => r.id == key).AsQueryable()));
        }

    }
}