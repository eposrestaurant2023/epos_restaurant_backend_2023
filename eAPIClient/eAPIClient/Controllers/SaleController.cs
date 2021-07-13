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
        public IQueryable<SaleModel> Get(string keyword = "", string shift = "", string open_by = "", string close_by = "")
        { 
            return (from r in db.Sales
                    where EF.Functions.Like(
                              ((r.document_number ?? " ") + (r.customer.customer_name_en ?? " ") +
                               (r.customer.customer_name_kh ?? " ") + (r.sale_note ?? " ")
                              ).ToLower().Trim(), $"%{(keyword ?? "")}%".ToLower().Trim()) && r.cashier_shift.shift == ((shift ?? "") == "" ? r.cashier_shift.shift : shift) &&
                              r.created_by == ((open_by ?? "") == "" ? r.created_by : open_by) && r.closed_by == ((close_by ?? "") == "" ? r.closed_by : close_by)
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
                bool is_new = true;
                model.customer = null;
                if (model.id == Guid.Empty)
                {
                    model.sale_number = await app.GenerateDocumentNumber("SaleNum", model.outlet_id.ToString());
                    model.sale_products.ForEach(sp =>
                    {
                        if (sp.sale_product_print_queues != null)
                        {
                            sp.sale_product_print_queues.ForEach(_spq => { 
                                _spq.sale_number = model.sale_number; 
                            });
                        }
                    });
                    db.Sales.Add(model);
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

                if (!is_new)
                {
                    if (model.is_closed == true && (model.document_number == "New" || model.document_number == ""))
                    {
                        model.document_number = await app.GenerateDocumentNumber("SaleDoc", model.outlet_id.ToString());
                        await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
                    }
                }

                string path = @"c:\\epossync";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                } 
                // Write the specified text asynchronously to a new file named "WriteTextAsync.txt".
                using (StreamWriter outputFile = new StreamWriter(Path.Combine(path, $"{model.id}.txt")))
                {
                    await outputFile.WriteAsync(model.id.ToString());
                }
                 

                
                return Ok(model);

              
            }
            catch
            { 
                return BadRequest();
            }

        } 
    }

}
