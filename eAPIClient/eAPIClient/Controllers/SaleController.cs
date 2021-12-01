using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
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
                              ((r.document_number ?? " ") + (r.customer.customer_name_en ?? " ") + (r.sale_number ?? " ") +
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
                //chekc if working is null and system is dont have shift mananement
                //then auto create working day and shift then app to sale model
                WorkingDayModel working_day = new WorkingDayModel();
                if (model.working_day_id == null && !app.IsSystemHasFeature("SHIFT_MGR"))
                {


                      working_day = await GetWorkingDayInfo(model);
                    if (working_day != null)
                    {
                        if (working_day.id != Guid.Empty)
                        {
                            model.working_day_id = working_day.id;
                        }
                    }
                }
                else
                {
                    return BadRequest(new BadRequestModel() { message = "please_start_working_day" });
                }
                //check cashier shift 

                if (model.cashier_shift_id == null && !app.IsSystemHasFeature("SHIFT_MGR"))
                {
                    var cashier_shift = await GetCashierShiftInfo(model);
                    if (cashier_shift != null)
                    {
                        model.cashier_shift_id = cashier_shift.id;
                    }
                }
                else
                {
                    return BadRequest(new BadRequestModel() { message = "please_start_cashier_shift" });
                }
                //end checking working day and cashier shift

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
                    string message = ex.Message;
                }

                return Ok(model);
            }
            catch (Exception _ex)
            {
                return BadRequest(new BadRequestModel() { message = _ex.Message }) ;
            }
        }


        async Task<WorkingDayModel> GetWorkingDayInfo(SaleModel model)
        {

            WorkingDayModel working_day = new WorkingDayModel();
            working_day.cash_drawer_id = model.cash_drawer_id;
            working_day.cash_drawer_name = model.cash_drawer_name;
            working_day.business_branch_id = model.business_branch_id;
            var b = app.GetBusinessBranch();
            if (b != null)
            {
                working_day.business_branch_name_en = b.business_branch_name_en;
                working_day.business_branch_name_kh = b.business_branch_name_kh;
            }
            working_day.created_by = model.created_by;
            working_day.created_date = DateTime.Now;
            working_day.outlet_id = model.outlet_id;
            var outlet = app.GetOutletInfo(model.outlet_id);
            if (outlet != null) {
                working_day.outlet_name_en = outlet.outlet_name_en;
                working_day.outlet_name_kh = outlet.outlet_name_kh;
            }
            
            working_day.working_date = DateTime.Now;
            working_day.opened_station_id = model.station_id;
            var station = app.GetStationInfo(model.station_id);
            if(station != null)
            {
                working_day.opended_station_name_en = station.station_name_en;
                working_day.opended_station_name_kh = station.station_name_kh;
                working_day.closed_station_name_en= station.station_name_en;
                working_day.closed_station_name_kh= station.station_name_kh;

            }

            working_day.closed_station_id = model.station_id;
           
            working_day = await app.GetWorkingDayInfor(working_day, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return working_day;

        }
        async Task<CashierShiftModel> GetCashierShiftInfo(SaleModel model)
        {

            CashierShiftModel cashier_shift = new CashierShiftModel();
            cashier_shift.cash_drawer_id = model.cash_drawer_id;
            cashier_shift.cash_drawer_name = model.cash_drawer_name;
            cashier_shift.working_day_id = model.working_day_id;
            cashier_shift.created_by = model.created_by;
            cashier_shift.created_date = DateTime.Now;
            cashier_shift.outlet_id = model.outlet_id;
            var b = app.GetBusinessBranch();
            if (b != null)
            {
                cashier_shift.business_branch_name = b.business_branch_name_en;
                
            }
            var outlet = app.GetOutletInfo(model.outlet_id);
            if (outlet != null) {
                cashier_shift.outlet_name_en = outlet.outlet_name_en;
                cashier_shift.outlet_name_kh = outlet.outlet_name_kh;
            }
            
            cashier_shift.working_date = DateTime.Now;
            cashier_shift.opened_station_id = model.station_id;
            var station = app.GetStationInfo(model.station_id);
            if(station != null)
            {
                cashier_shift.opened_station_name_en = station.station_name_en;
                cashier_shift.opened_station_name_kh = station.station_name_kh;
                cashier_shift.closed_station_name_en= station.station_name_en;
                cashier_shift.closed_station_name_kh= station.station_name_kh;

            }
            cashier_shift.shift_name = "Daily Shift";
            cashier_shift.closed_station_id = model.station_id;
           
            cashier_shift = await app.GetCashierShiftInfo(cashier_shift, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return cashier_shift;

        }


        [HttpPost]
        [Route("CancelPrintBill/{id}")]
                public async Task<ActionResult> CancelPrintBill(Guid id)
                {
            var _sale = db.Sales.Where(r => r.id == id);
            if (_sale.Any())
            {
                _sale.FirstOrDefault().status_id = 1;
                _sale.FirstOrDefault().is_print_invoice = false;
                db.Sales.Update(_sale.FirstOrDefault());
                await db.SaveChangesAsync();
            }

            return Ok();
        }

    } 
}
