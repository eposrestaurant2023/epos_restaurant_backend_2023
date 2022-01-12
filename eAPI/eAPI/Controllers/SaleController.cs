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
using Microsoft.AspNetCore.SignalR;
using eAPI.Hubs;

namespace eAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SaleController : ODataController
    {
        private readonly ApplicationDbContext db;
        private readonly AppService app;
        private readonly IHubContext<ConnectionHub> hub;
        public SaleController(ApplicationDbContext _db, AppService _app, IHubContext<ConnectionHub> _hub)
        {
            db = _db;
            hub = _hub;
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
                                     (r.sale_number ?? " ") +
                                     (r.customer.customer_name_en ?? " ") +
                                     (r.customer.customer_code ?? " ") +
                                     (r.customer.phone_1 ?? " ") +
                                     (r.customer.customer_name_kh ?? " ") +
                                     (r.business_branch.business_branch_name_kh ?? " ") +
                                     (r.business_branch.business_branch_name_en ?? " ") +
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
        [Route("getsingle")]
        public async Task<SingleResult<SaleModel>> Get([FromODataUri] Guid key)
        {
            return await Task.Factory.StartNew(() => SingleResult.Create<SaleModel>(db.Sales.Where(r => r.id == key).AsQueryable()));
        }

        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] SaleModel model)
        {
            try
            {
                var saleCheck = db.Sales.Where(r => r.id == model.id).AsNoTracking()
                                  
                                    .Include(r=>r.sale_payments).AsNoTracking();
                if (saleCheck.Count() > 0)
                {
                    var _sale = saleCheck.FirstOrDefault();
                    foreach (var _sp in model.sale_products)
                    {
                        db.Entry(_sp).State = EntityState.Added;
                        if (_sp.id != Guid.Empty)
                        {
                            var _old_sale_product = db.SaleProducts.Where(r => r.id == _sp.id).AsNoTracking();
                            if (_old_sale_product.Any())
                            {
                                db.Entry(_sp).State = EntityState.Modified;
                            } 
                        } 

                        if (_sp.sale_product_modifiers.Count() > 0)
                        {
                            
                            foreach(var _spm in _sp.sale_product_modifiers)
                            {
                                
                                    db.Entry(_spm).State = EntityState.Added;
                                if (_spm.id != Guid.Empty)
                                {
                                    var _old_spm= db.SaleProductModifiers.Where(r => r.id == _spm.id).AsNoTracking();
                                    if (_old_spm.Any())
                                    {
                                        db.Entry(_spm).State = EntityState.Modified;
                                    }
                                }

                            }
                        }
                    }



                    model.sale_payments.ForEach(_spay =>
                    { 
                        if (_sale.sale_payments.Where(r => r.id == _spay.id).Count() <= 0)
                        {
                            _spay.sale = null;
                            db.Entry(_spay).State = EntityState.Added;
                        }
                    });

                    db.Sales.Update(model);
                }
                else
                {

                  foreach(var sp in   model.sale_products.Where(r=>r.id != Guid.Empty))
                    {
                        db.Entry(sp).State = EntityState.Added;
                        var _old_sale_product = db.SaleProducts.Where(r => r.id == sp.id).AsNoTracking();
                        if (_old_sale_product.Any())
                        {
                            db.Entry(sp).State = EntityState.Modified;
                        }

                       
                    }

                    db.Sales.Add(model);
                }
                
                await db.SaveChangesAsync();
                db.Database.ExecuteSqlRaw($"exec sp_update_sale_information '{model.id}'");
                db.Database.ExecuteSqlRaw($"exec sp_update_sale_inventory_transaction '{model.id}'");
                return Ok();
            }
            catch(Exception ex) 
            {
                var _ex = ex;
                return BadRequest();
            }
        }
    
    
    }
}