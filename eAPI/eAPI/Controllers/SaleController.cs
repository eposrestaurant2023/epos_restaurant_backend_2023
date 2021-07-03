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
                var _chk = db.Sales.Where(r => r.id == model.id);
                if (_chk.Count() > 0)
                {
                    model.sale_products.ForEach(_sp =>
                    {
                        if (db.SaleProducts.Where(r => r.id == _sp.id).Count() <= 0)
                        {
                            db.SaleProducts.Add(_sp);
                             db.SaveChanges();
                        }
                        else if(_sp.sale_product_modifiers.Count() > 0)
                        {
                            _sp.sale_product_modifiers.ForEach(_spm =>
                            {
                                if (db.SaleProductModifiers.Where(r => r.id == _spm.id).Count() <= 0)
                                {
                                    db.SaleProductModifiers.Add(_spm);
                                    db.SaveChanges();
                                }
                            }); 
                        }
                    });

                    model.sale_payments.ForEach(_spay =>
                    {
                        if (db.SalePayments.Where(r => r.id == _spay.id).Count() <= 0)
                        {
                            db.SalePayments.Add(_spay);
                            db.SaveChanges();
                        }
                    });

                    db.Sales.Update(model);
                }
                else
                {
                    db.Sales.Add(model);
                }
                
                await db.SaveChangesAsync();
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