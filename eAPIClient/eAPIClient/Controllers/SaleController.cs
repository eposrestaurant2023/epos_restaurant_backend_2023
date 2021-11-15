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
    public class SaleController : ODataController
    {
        public IConfiguration config { get; }
        private readonly ApplicationDbContext db;
        private readonly AppService app; 

        public SaleController(ApplicationDbContext _db, AppService _app,  IConfiguration configuration)
        {
            db = _db;
            app = _app;
            config = configuration; 
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        public IQueryable<SaleModel> Get(string keyword = "")
        { 
            return (from r in db.Sales
                    where EF.Functions.Like(
                              ((r.document_number ?? " ") + (r.customer.customer_name_en ?? " ") +
                               (r.customer.customer_name_kh ?? " ") + (r.sale_note ?? " ")
                              ).ToLower().Trim(), $"%{(keyword ?? "")}%".ToLower().Trim()) 
                             
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
                model.is_synced = false;
                DocumentNumberModel _saleNumber = new DocumentNumberModel();
                bool is_new = true;
                model.customer = null;
                model.is_foc = false;
                if (model.sale_payments.Any())
                {
                    model.is_foc = model.sale_payments.Where(r => r.is_deleted == false && r.payment_type_group == "FOC").Any();
                }
                model.sale_products.ForEach(r => r.sale_order = r.sale_order_id != Guid.Empty ? null : r.sale_order);
                if (model.id == Guid.Empty)
                {
                    _saleNumber = app.GetDocument("SaleNum", model.cash_drawer_id.ToString());
                    model.sale_number = _saleNumber.id > 0 ? app.GetDocumentFormat(_saleNumber) : "New";
                    model.sale_products.ForEach(sp =>
                    {
                        if (sp.sale_product_print_queues != null)
                        {
                            sp.sale_product_print_queues.ForEach(_spq =>
                            {
                                _spq.sale_number = model.sale_number;
                            });
                        }
                    });
                    db.Sales.Add(model);
                    is_new = !(model.is_closed ?? false);
                }
                else
                {
                    is_new = false;
                    model.sale_products.ForEach(sp =>
                    {
                        if (sp.sale_product_print_queues != null)
                        {
                            sp.sale_product_print_queues.ForEach(_spq => { _spq.sale_number = model.sale_number; });
                        }
                    });
                    db.Sales.Update(model);
                }
                await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
                //Update Document
                await app.UpdateDocument(_saleNumber);
                if (!is_new)
                {
                    if (model.is_closed == true && (model.document_number == "New" || model.document_number == ""))
                    {
                        DocumentNumberModel _saleDoc = new DocumentNumberModel();
                        _saleDoc = app.GetDocument("SaleDoc", model.closed_cash_drawer_id.ToString());
                        model.document_number = _saleDoc.id > 0 ? app.GetDocumentFormat(_saleDoc) : "New";
                        await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
                        await app.UpdateDocument(_saleDoc);
                    }
                }


                //for anyupdate that related to sale if have
                db.Database.ExecuteSqlRaw($"exec sp_update_sale_infomation '{model.id}'");   
                try
                {         
                    app.sendSyncRequest();
                }
                catch(Exception ex)
                {
                    //
                }

                return Ok(model);
            }
            catch (Exception _ex)
            {
                return BadRequest(new BadRequestModel() { message = _ex.Message }) ;
            }
        } 
    } 
}
