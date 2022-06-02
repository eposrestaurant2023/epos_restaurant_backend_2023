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
        private readonly ISyncService sync; 

        public SaleController(ApplicationDbContext _db, AppService _app,  IConfiguration configuration, ISyncService sync)
        {
            db = _db;
            app = _app;
            config = configuration;
            this.sync = sync;
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
        public async Task<ActionResult<string>> Save([FromBody] SaleModel model,bool is_edit =false)
        {
            try
            {
                int time_charge_product_id = 0;
                int tip_product_id = 0;

                //check if cashift is opened 
                var check_shift_opened = db.CashierShifts.Where(r => r.cash_drawer_id == model.closed_cash_drawer_id && r.is_closed == false && r.id == model.closed_cashier_shift_id );
                if (!check_shift_opened.Any())
                {
                    return BadRequest(new BadRequestModel { message = "please_start_cashier_shift" });
                }

                List<SaleModel> sales = new List<SaleModel>();
                if (model.id != Guid.Empty)
                {                                               
                    sales = db.Sales.Where(r => r.id == model.id).Include(x=>x.sale_products).AsNoTracking().ToList();
                    ///the_bill_was_modified_by_other_device   
                    ///

                    var setting_product_config = db.ConfigDatas.Where(r => r.config_type == "tip_product_id" || r.config_type == "time_charge_product_id");
                    if (setting_product_config.Any())
                    {
                        var time_charge = setting_product_config.Where(r => r.config_type == "time_charge_product_id");
                        var tip = setting_product_config.Where(r => r.config_type == "tip_product_id");
                        if (time_charge.Any())
                        {
                            var _d = time_charge.FirstOrDefault().data;
                            time_charge_product_id = Convert.ToInt32(_d == "" ? "0" : _d);
                        }
                        if (tip.Any())
                        {
                            var _d = tip.FirstOrDefault().data;
                            tip_product_id = Convert.ToInt32(_d == "" ? "0" : _d);
                        }
                    }

                }


                if (!is_edit)
                {                                                                                  
                    if (model.id != Guid.Empty)
                    {
                        if (sales.Any())
                        {
                            //check if bill have item change 
                            if (sales.FirstOrDefault().last_modified_date != model.last_modified_date && (sales.FirstOrDefault().is_print_invoice) == false)
                            {
                                return BadRequest(new BadRequestModel { message = "this_order_was_modified_on_other_devices" });
                            }

                            //check if bill is already print
                            if ((model.is_closed ?? false) == false && (sales.FirstOrDefault().is_print_invoice) == true && sales.FirstOrDefault().last_modified_date != model.last_modified_date)
                            {
                                return BadRequest(new BadRequestModel { message = "this_order_is_print" });
                            }


                            //check if have tip product
                            bool _is_tip_product = false;
                            var _tip_product_exists = model.sale_products.Where(r => r.product_id == tip_product_id && !r.is_deleted);
                            _is_tip_product = _tip_product_exists.Any();
                            //check bill alredy print
                            //user payment with new item order  
                            if ((model.is_closed ?? false) == true && (sales.FirstOrDefault().is_print_invoice) == true && !_is_tip_product)
                            {
                                string _format = "#,###,##0.00#####";
                                string _db_total_amount = string.Format(@"{0:" + _format + "}", sales.FirstOrDefault().total_amount);
                                string _db_total_qty = string.Format(@"{0:" + _format + "}", sales.FirstOrDefault().total_quantity);
                                string _model_total_amount = string.Format(@"{0:" + _format + "}", model.total_amount);
                                string _model_total_qty = string.Format(@"{0:" + _format + "}", model.total_quantity);


                                if (model.sale_products.Where(r => r.id == Guid.Empty).Any() ||
                                   _db_total_amount != _model_total_amount ||  _db_total_qty != _model_total_qty)
                                {
                                    return BadRequest(new BadRequestModel { message = "this_order_is_print" });
                                }
                            }
                            //check if bill is already close
                            if ((sales.FirstOrDefault().is_closed ?? false) == true)
                            {
                                return BadRequest(new BadRequestModel { message = "this_order_is_closed" });
                            }
                        }
                    }
                }
               

                model.is_synced = false;
                DocumentNumberModel _saleNumber = new DocumentNumberModel();
                DocumentNumberModel _waitingNumber = new DocumentNumberModel();
                
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
                    //waiting numbvewr
                    _waitingNumber = app.GetDocument("WaitingNum", model.cash_drawer_id.ToString());
                  
                    await app.UpdateDocument(_waitingNumber);
                    model.waiting_number = _waitingNumber.id > 0 ? app.GetDocumentFormat(_waitingNumber) : "New";

                    //check sale product and change entry state 

                    model.sale_products.ForEach(_sp =>
                    {     
                        if (_sp.sale_product_print_queues != null)
                        {
                            _sp.sale_product_print_queues.ForEach(_spq =>
                            {
                                _spq.sale_number = model.sale_number;
                            });
                        }  
                    });  

                    if (model.sale_products.Where(r => r.id != Guid.Empty && r.is_deleted == false ).Any())
                    {
                        List<SaleProductModel> temp_sale_products = model.sale_products;
                        model.sale_products = null;
                        db.Sales.Add(model);

                        await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
                        model.sale_products = temp_sale_products;
                    }
                    else
                    {
                        db.Sales.Add(model);
                    }
                    

                   
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
                await app.UpdateDocument(_waitingNumber);
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
                        
                    sync.sendSyncRequest();
               

                return Ok(model);
            }
            catch (Exception _ex)
            {
                
                return BadRequest(new BadRequestModel() { message = "save_data_fail_please_try_again" }) ;
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

        [HttpPost]
        [Route("CancelPrintBillAll/{table_id}")]
        public    ActionResult CancelPrintBillAll(int table_id)
        {
            db.Database.ExecuteSqlRaw("exec sp_cancel_print_bill_by_table " + table_id);

            return Ok();
        } 
        
       

        [HttpPost]
        [Route("PrintRequestBill/{id}")]
        public async Task<ActionResult> PrintRequestBill(Guid id)
        {
            var _sale = db.Sales.Where(r => r.id == id);
            if (_sale.Any())
            {
                SaleModel s = _sale.FirstOrDefault();
                s.status_id = 3;
                s.is_print_invoice = true;
                db.Sales.Update(s);
                await db.SaveChangesAsync();
            }
            return Ok();
        }
        [HttpPost]
        [Route("UpdateGuestCover/{id}/{guest_cover}")]
        public async Task<ActionResult> PrintRequestBill(Guid id, int guest_cover)
        {
            var _sale = db.Sales.Where(r => r.id == id);
            if (_sale.Any())
            {
                SaleModel s = _sale.FirstOrDefault();
                s.is_synced = false;
                s.guest_cover = guest_cover;
                db.Sales.Update(s);
                await db.SaveChangesAsync();
            }
            sync.sendSyncRequest();

            return Ok();
        }



    }
}
