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
    public class SaleProductModifierController : ODataController
    {
        private readonly ApplicationDbContext db;
        private readonly IHubContext<ConnectionHub> hub;
        public SaleProductModifierController(ApplicationDbContext _db,IHubContext<ConnectionHub> _hub)
        {
            db = _db;hub = _hub;
        }        

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]   
        public IQueryable<SaleProductModifierModel> Get(string keyword = "")
        {
            if (!string.IsNullOrEmpty(keyword))
            {
              return (from r in db.SaleProductModifiers
                           where 
                                 EF.Functions.Like(
                                     (
                                        (r.sale_product.sale.document_number ?? " ") + 
                                        (r.sale_product.product.product_display_name ?? " ") +
                                        (r.sale_product.product.product_code ?? " ") +
                                        (r.sale_product.sale.business_branch.business_branch_name_kh ?? " ") + 
                                        (r.sale_product.sale.business_branch.business_branch_name_en ?? " ") + 
                                        (r.sale_product.sale.sale_note ?? " ") 
                                     ).ToLower().Trim(), $"%{keyword}%".ToLower().Trim())
                           select r);
            }
            else
            {
                return db.SaleProductModifiers.AsQueryable();
            }
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [Route("getsingle")]
        public async Task<SingleResult<SaleProductModifierModel>> Get([FromODataUri] Guid key)
        {
            return await Task.Factory.StartNew(() => SingleResult.Create<SaleProductModifierModel>(db.SaleProductModifiers.Where(r => r.id == key).AsQueryable()));
        }
    }
}