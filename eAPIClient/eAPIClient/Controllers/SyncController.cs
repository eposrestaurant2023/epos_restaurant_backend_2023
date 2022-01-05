using System;
using System.Collections.Generic;
using System.Linq;              
using System.Threading.Tasks;   
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;    
using eAPIClient.Services;
using Microsoft.Extensions.Configuration;
using System.Text.Json;    
using eAPIClient.Models;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Security.Claims;
using Serilog;

namespace eAPIClient.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SyncController : ODataController
    {    
        bool is_get_remote_data_success=true;    
        private readonly ApplicationDbContext db;
        private readonly IHttpService http;
        private readonly IConfiguration config;
        private readonly AppService app;
        public SyncController(ApplicationDbContext _db, IHttpService _http, IConfiguration _config, AppService _app)
        {
            db = _db;
            http = _http;
            config = _config;
            app = _app;
        }

    
     
       

        [HttpGet("GetDataForSynchronize")] 
        [AllowAnonymous]
        public ActionResult<List<DynamicModel>> GetDataForSynchronize()
        {
            var d = db.StoreProcedureResults.FromSqlRaw("exec sp_get_data_for_synchronize 'json'").ToList().FirstOrDefault();
            if (d != null)
            {
                string r = d.result.Replace("\\", "").Replace("\"[", "[").Replace("]\"", "]").ToString();
                var data = JsonSerializer.Deserialize<List<DynamicModel>>(r);
                return Ok(data);
            }
            return   Ok(new List<DynamicModel>()) ;
        } 

        
        [HttpPost("Sale")] 
        [AllowAnonymous]
        public async Task<ActionResult> SyncSale(Guid saleId)
        {
            try
            {
                var _saleData = db.Sales.Where(r => r.id == saleId)
                     .Include(r => r.sale_payments) 
                     .Include(r => r.sale_products).ThenInclude(r => r.sale_product_modifiers)
                     .AsNoTrackingWithIdentityResolution();  
                if (_saleData.Count() > 0)
                {
                    var _sale = _saleData.FirstOrDefault();
                    _sale.is_synced = true;
                    var _syncResp = await http.ApiPost("Sale/Save", _sale);
                    if (!_syncResp.IsSuccess)
                    {
                        return BadRequest();
                    }
                    db.Sales.Update(_sale);
                    await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
                    return Ok();
                }
                else
                {
                    return NotFound();
                } 
            }
            catch(Exception ex)
            {
                return BadRequest();
            } 
        }


        [HttpGet("SyncWorkingDay")]
        [AllowAnonymous]
        public async Task<ActionResult> SyncWorkingDayGet(Guid workingDayId)
        {
            try
            {
                var _workingDayData = db.WorkingDays.Where(r => r.id == workingDayId)
                     .AsNoTrackingWithIdentityResolution();
                if (_workingDayData.Count() > 0)
                {
                    var _workingDay = _workingDayData.FirstOrDefault();
                    _workingDay.is_synced = true;
                    var _syncResp = await http.ApiPost("WorkingDay/Save", _workingDay);
                    if (!_syncResp.IsSuccess)
                    {
                        return BadRequest(_workingDay);
                    }
                    db.WorkingDays.Update(_workingDay);
                    await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
                    return Ok();
                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpGet("SyncCashierShift")]
        [AllowAnonymous]
        public async Task<ActionResult> SyncCashierShiftGet(Guid id)
        {
            try
            {
                var _modelData = db.CashierShifts.Where(r => r.id == id)
                     .AsNoTrackingWithIdentityResolution();
                if (_modelData.Count() > 0)
                {
                    var _model = _modelData.FirstOrDefault();
                    _model.is_synced = true;
                    var _syncResp = await http.ApiPost("CashierShift/Save", _model);
                    if (!_syncResp.IsSuccess)
                    {
                        return BadRequest(_model);
                      
                    }
                    db.CashierShifts.Update(_model);
                    await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
                    return Ok();
                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpGet("SyncExpense")]
        [AllowAnonymous]
        public async Task<ActionResult> SyncExpense(Guid id)
        {
            try
            {
                var _modelData = db.Expenses.Where(r => r.id == id)
                     .AsNoTrackingWithIdentityResolution();
                if (_modelData.Count() > 0)
                {
                    var _model = _modelData.FirstOrDefault();
                    _model.is_synced = true;
                    var _syncResp = await http.ApiPost("Expense/SyncSave", _model);
                    if (!_syncResp.IsSuccess)
                    {
                        return BadRequest();

                    }
                    db.Expenses.Update(_model);
                    await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
                    return Ok();
                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return BadRequest();
            }
        }


        [HttpGet("SyncCustomer")]
        [AllowAnonymous]
        public async Task<ActionResult> SyncCustomer(Guid id)
        {
            try
            {
                var _modelData = db.Customers.Where(r => r.id == id)
                     .AsNoTrackingWithIdentityResolution();
                if (_modelData.Count() > 0)
                {
                    var _model = _modelData.FirstOrDefault();
                    _model.is_synced = true;
                    var _syncResp = await http.ApiPost("Customer/Save?is_synch_from_client=true", _model);
                    if (!_syncResp.IsSuccess)
                    {
                        return Ok(_model);
                        return BadRequest();
                   
                    }

                    CustomerModel resp_customer = JsonSerializer.Deserialize<CustomerModel>(_syncResp.Content);
                    _model.customer_code = resp_customer.customer_code;

                    db.Customers.Update(_model);
                    await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
                    return Ok();
                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }


        [HttpGet("SyncCashDrawerAmount")]
        [AllowAnonymous]
        public async Task<ActionResult> SyncCashDrawerAmountGet(Guid id)
        {
            try
            {
                var _modelData = db.CashDrawerAmounts.Where(r => r.id == id)
                     .AsNoTrackingWithIdentityResolution();
                if (_modelData.Count() > 0)
                {
                    var _model = _modelData.FirstOrDefault();
                    _model.is_synced = true;
                    var _syncResp = await http.ApiPost("CashDrawerAmount/Save", _model);
                    if (!_syncResp.IsSuccess)
                    {
                        return BadRequest();
                    }
                    db.CashDrawerAmounts.Update(_model);
                    await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
                    return Ok();
                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }




        [HttpGet("SyncHistory")]
        [AllowAnonymous]
        public async Task<ActionResult> SyncHistory(Guid id)
        {
            try
            {
                var _modelData = db.Histories.Where(r => r.id == id)
                     .AsNoTrackingWithIdentityResolution();
                if (_modelData.Count() > 0)
                {
                    var _model = _modelData.FirstOrDefault();
                  
                    _model.is_synced = true;
                    var _syncResp = await http.ApiPost("History/Save", _model);
                    if (!_syncResp.IsSuccess)
                    {
                        return BadRequest();
                    }
                    db.Histories.Update(_model);
                    db.SaveChanges();
                    app.sendHistoryAlertTelegram(_model);
                    return Ok();
                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }



        [HttpGet("SyncSale")]
        [AllowAnonymous]
        public async Task<ActionResult> SyncSaleGet(Guid saleId)
        {
            try
            {
                var _saleData = db.Sales.Where(r => r.id == saleId)
                     .Include(r => r.sale_payments)
                     .Include(r => r.sale_products).ThenInclude(r => r.sale_product_modifiers)
                     .AsNoTrackingWithIdentityResolution(); 
                if (_saleData.Count() > 0)
                {
                    var _sale = _saleData.FirstOrDefault();
                    _sale.is_synced = true;
                    
                    var _syncResp = await http.ApiPost("Sale/Save",_sale);
                    if (!_syncResp.IsSuccess)
                    {
                        return Ok(_sale);
                       // return BadRequest();
                    }
                    db.Sales.Update(_sale);
                    await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
                    return Ok();
                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception ex)
            {
                return BadRequest();
            } 
        }


        [HttpGet("SyncRemoteData")]
        [AllowAnonymous]
        public async Task<ActionResult> SyncRemoteData(Guid business_branch_id)
        {
            try
            {
                //1. Sync Customer 
                await SyncRemoteCustomer(business_branch_id);

                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(new BadRequestModel() { message = ex.Message });
            }
        }



        async Task SyncRemoteCustomer(Guid business_branch_id)
        {
            List<CustomerBusinessBranchModel> data = new List<CustomerBusinessBranchModel>();
            string query = $"CustomerBusinessBranch?$expand=customer&$filter=is_synced eq false and business_branch_id eq {business_branch_id}";
            var resp = await http.ApiGetOData(query);
            if (resp.IsSuccess)
            {
                data = JsonSerializer.Deserialize<List<CustomerBusinessBranchModel>>(resp.Content.ToString());
                if (data.Any())
                {
                    foreach(var b in data)
                    {
                        if (SaveRemoteCustomerToLocalCustomer(b.customer))
                        {
                            

                            b.customer = null;
                            b.is_synced = true;
                            await http.ApiPost("CustomerBusinessBranch/Save", b);
                        }
                    }
                }
            }


        }

        bool SaveRemoteCustomerToLocalCustomer(CustomerModel model)
        {
            try
            {
                var _modelCheck = db.Customers.Where(r => r.id == model.id).AsNoTracking();
                if (_modelCheck.Count() > 0)
                {
                    db.Customers.Update(model);
                }
                else
                {
                    db.Customers.Add(model);
                }

                db.SaveChanges();

                return true;
                
            }
            catch (Exception ex)
            {
                var _ex = ex;
                
            }
            return false;
        }



        [HttpPost("GetRemoteData")]
        [AllowAnonymous]
        public async Task<ActionResult<List<ConfigDataModel>>> GetRemoteData(bool isFirstSetup=false)
        {

            string business_branch_id = config.GetValue<string>("business_branch_id");
            //run script prepare config data 
            var prepare = await http.ApiPost("GetData", new FilterModel() { procedure_name = "sp_prepare_sync_config_data ", procedure_parameter = $"'{business_branch_id}'" });

            //
            await GetNote(business_branch_id);
            if (!is_get_remote_data_success)
                return BadRequest();

            List<MenuModel> menu_datas = await GetRemoteMenu(business_branch_id);
            if (!is_get_remote_data_success)
                return BadRequest();
            //product have printer and modifier
            List<ProductModel> product_datas = await GetRemoteProduct(business_branch_id);
            if (!is_get_remote_data_success)
                return BadRequest();

            List<ProductMenuModel> product_menu_datas = await GetRemoteProductMenu(business_branch_id);
            if (!is_get_remote_data_success)
                return BadRequest();

            //get product price 
            List<ProductPriceModel> product_price_datas = await GetRemoteProductPrice(business_branch_id);
            if (!is_get_remote_data_success)
                return BadRequest();
            // run this when all data read from server done success
            //run clear all old data

            //Get Config Data
            List<ConfigDataModel> config_datas = await GetConfigData(business_branch_id);
            if (!is_get_remote_data_success)
                return BadRequest();
            List<SaleStatusModel> sale_statuses = await GetSaleStatus();
            if (!is_get_remote_data_success)
                return BadRequest();
            List<SaleProductStatusModel> sale_product_statuses = await GetSaleProductStatus();
            if (!is_get_remote_data_success)
                return BadRequest();

            
            //Success of Data processing
            db.Database.ExecuteSqlRaw("exec sp_delete_menu_and_product");
            db.Menus.AddRange(menu_datas);   
            db.Products.AddRange(product_datas);
            await db.SaveChangesAsync();


            db.ProductMenus.AddRange(product_menu_datas);
            db.ProductPrices.AddRange(product_price_datas);
            await db.SaveChangesAsync();
            db.Database.ExecuteSqlRaw("exec sp_update_product_portion_price");


            //Config Data
            if (isFirstSetup)
            {
                foreach (var a in sale_statuses)
                {
                    var _sps = db.SaleStatuses.Where(r => r.id == a.id).AsNoTracking().ToList();
                    if (_sps.Count() <= 0)
                    {
                        db.SaleStatuses.Add(a);
                    }
                    else
                    {
                        db.SaleStatuses.Update(a);
                    }
                }

                foreach (var a in sale_product_statuses)
                {
                    var _sps = db.SaleProductStatuses.Where(r => r.id == a.id).AsNoTracking().ToList();
                    if (_sps.Count() <= 0)
                    {
                        db.SaleProductStatuses.Add(a);
                    }
                    else
                    {
                        db.SaleProductStatuses.Update(a);
                    }
                }
            }


            string _deleteQuery = string.Format("delete tbl_config_data where is_local_setting=0; ");
            db.Database.ExecuteSqlRaw(_deleteQuery);
            db.ConfigDatas.AddRange(config_datas.Where(r=>r.is_local_setting==false));


            //get remote translate text 

            List<eShareModel.TranslateTextModel> translate_texts = await GetTranslateText();
            if (translate_texts.Any())
            {
                db.Database.ExecuteSqlRaw("delete tbl_translate_text");
                db.TranslateTexts.AddRange(translate_texts);
            }



            await db.SaveChangesAsync();
            //


            return Ok( business_branch_id ); 

        }


      
        async Task<List<MenuModel>> GetRemoteMenu(string business_branch_id)
        {      
            is_get_remote_data_success = false;
            var resp = await http.ApiGetOData($"Menu?$select=id,parent_id,menu_name_en,menu_name_kh,text_color,background_color,root_menu_id,is_shortcut_menu&$filter=business_branch_id eq {business_branch_id} and is_deleted eq false and status eq true");
            if (resp.IsSuccess)
            {
                is_get_remote_data_success = true;
                return  JsonSerializer.Deserialize<List<MenuModel>>(resp.Content.ToString());
            } 
            return new List<MenuModel>();
        }

        async Task<List<ProductModel>> GetRemoteProduct(string business_branch_id)
        {
            List < ProductModel > _products = new List<ProductModel>();

            is_get_remote_data_success = false;
            string _select_product_modifier = "$select=id,parent_id,product_id,modifier_name,price,section_name,is_required,is_multiple_select,is_section,sort_order,modifier_id";

            string url = $"product?$select=revenue_group_name,product_group_id,product_tax_value,product_category_id,product_category_en,product_category_kh,id,is_open_product,";
            url += "product_code,product_name_en,product_name_kh,photo,note,is_allow_discount,is_allow_change_price,is_allow_free,is_open_product,is_inventory_product,kitchen_group_name,kitchen_group_sort_order";
            url += $"&$expand=product_printers($select=id,product_id,printer_name,ip_address,port,group_item_type_id;$filter=is_deleted eq false and printer/business_branch_id eq {business_branch_id}),";
            url += $"product_modifiers({_select_product_modifier};$expand=children({_select_product_modifier};$filter=is_deleted eq false);$filter=is_deleted eq false),";
            url += $"product_portions($select=id,product_id, portion_name,cost,multiplier,unit_id;$filter=is_deleted eq false)";
            url += "&$filter=is_deleted eq false and status eq true and is_menu_product eq true";
            var resp = await http.ApiGetOData(url);
            if (resp.IsSuccess)
            {
                
                is_get_remote_data_success = true;
                _products = JsonSerializer.Deserialize<List<ProductModel>>(resp.Content.ToString());                
                _products.ForEach((p) =>
                {
                    //Get Product Tax Config
                    p.product_tax_value = string.IsNullOrEmpty(p.product_tax_value) ? "[]" : p.product_tax_value;
                    List<BusinessBranchProductTaxConfigModel> _productTaxs = new List<BusinessBranchProductTaxConfigModel>();
                    _productTaxs = JsonSerializer.Deserialize<List<BusinessBranchProductTaxConfigModel>>(p.product_tax_value);
                    var _productTaxData = _productTaxs.Where(r => r.business_branch_id.ToLower() == business_branch_id.ToLower());
                    if (_productTaxData.Count() > 0)
                    {
                        var _t = _productTaxData.FirstOrDefault();
                        ProductTaxConfigModel _tax = new ProductTaxConfigModel()
                        {
                            tax_1_rate = _t.tax_1_rate,
                            tax_2_rate = _t.tax_2_rate,
                            tax_3_rate = _t.tax_3_rate
                        };                        
                        p.product_tax_value = JsonSerializer.Serialize(_tax);
                    }
                    else
                    {
                        p.product_tax_value = JsonSerializer.Serialize(new ProductTaxConfigModel());
                    } 
                });

                return _products;
            } 
            return _products;
        }            
        async Task<List<ProductMenuModel>> GetRemoteProductMenu(string business_branch_id)
        {
            is_get_remote_data_success = false;
            string url = "ProductMenu?$select=id,product_id,menu_id";
            url = url + "&$filter=is_deleted eq false and  ";
            url = url + "menu/is_deleted eq false  and ";
            url = url + "menu/status eq true ";
            url = url + "and product/is_deleted eq false and ";
            url = url + "product/status eq true and product/is_menu_product eq true and ";
            url = url + $"menu/business_branch_id eq {business_branch_id}";

            var resp = await http.ApiGetOData(url);
            if (resp.IsSuccess)
            {
                is_get_remote_data_success = true;
                return  JsonSerializer.Deserialize<List<ProductMenuModel>>(resp.Content.ToString());
            } 
            return new List<ProductMenuModel>();
        }
        async Task<List<ProductPriceModel>> GetRemoteProductPrice(string business_branch_id)
        {
            is_get_remote_data_success = false;
            string url = "BusinessBranchProductPrice?";
            url = url + $"&$filter=business_branch_id eq {business_branch_id}";   
            var resp = await http.ApiGetOData(url);
            if (resp.IsSuccess)
            {
                is_get_remote_data_success = true;
                return JsonSerializer.Deserialize<List<ProductPriceModel>>(resp.Content.ToString());
            }   
            return new List<ProductPriceModel>();
        }
        async Task<List<ConfigDataModel>> GetConfigData(string business_branch_id)
        {
            is_get_remote_data_success = false;
            string url = "Configdata?$select=id,data,config_type,note,is_local_setting";
            url = url + $"&$filter=business_branch_id eq {business_branch_id}";  
            var resp = await http.ApiGetOData(url);
            if (resp.IsSuccess)
            {
                is_get_remote_data_success = true;
                return JsonSerializer.Deserialize<List<ConfigDataModel>>(resp.Content.ToString());
            }  
            return new List<ConfigDataModel>();
        }

        async Task<List<SaleStatusModel>> GetSaleStatus()
        {
            is_get_remote_data_success = false;
            var resp = await http.ApiGet("SaleStatus");
            if (resp.IsSuccess)
            {
                is_get_remote_data_success = true;
                return JsonSerializer.Deserialize<List<SaleStatusModel>>(resp.Content.ToString());
            }
            return new List<SaleStatusModel>();
        }   
        async Task<List<eShareModel.TranslateTextModel>> GetTranslateText()
        {
            is_get_remote_data_success = false;
            var resp = await http.ApiGet("GetTranslateText");
            if (resp.IsSuccess)
            {
                is_get_remote_data_success = true;
                return JsonSerializer.Deserialize<List<eShareModel.TranslateTextModel>>(resp.Content.ToString());
            }
            return new List<eShareModel.TranslateTextModel>();
        }

        async Task<List<SaleProductStatusModel>> GetSaleProductStatus()
        {
            is_get_remote_data_success = false;                                     
            var resp = await http.ApiGet("SaleProductStatus");
            if (resp.IsSuccess)
            {
                is_get_remote_data_success = true;
                return JsonSerializer.Deserialize<List<SaleProductStatusModel>>(resp.Content.ToString());
            }
            return new List<SaleProductStatusModel>();
        }

        async Task GetNote(string business_branch_id) 
        {
            is_get_remote_data_success = false;
            string _query = $"CategoryNote?$select=id,category_note_name_en,category_note_name_kh,is_multiple_select&$expand=notes($filter=is_deleted eq false and business_branch_id eq {business_branch_id})";
            var resp = await http.ApiGetOData(_query);
            if (resp.IsSuccess)
            {
                is_get_remote_data_success = true;

                List<CategoryNoteModel> _tempNewCategoryNotes = new List<CategoryNoteModel>();
                List<CategoryNoteModel> _tempAppendCategoryNotes = new List<CategoryNoteModel>();
                List<NoteModel> _tempNewNotes = new List<NoteModel>();
             
                List<CategoryNoteModel> _LocalCategoryNotes = new List<CategoryNoteModel>();
              

                List<NoteModel> _LocalNotes = new List<NoteModel>();
                _LocalNotes = db.Notes.AsNoTracking().ToList();

                List<NoteCategory> data = new List<NoteCategory>(); 
                data = JsonSerializer.Deserialize<List<NoteCategory>>(resp.Content.ToString());
                //Get Category 
                data.ForEach(c =>
                {
                    CategoryNoteModel _categoryNote = new CategoryNoteModel()
                    { 
                        category_note_id = c.id,
                        category_note_name_en=c.category_note_name_en,
                        category_note_name_kh = c.category_note_name_kh,
                        is_multiple_select = c.is_multiple_select
                    };

                    var _c = _LocalCategoryNotes.Where(r => r.category_note_id == c.id);
                    if (_c.Count() > 0)
                    {
                        _categoryNote.id = _c.FirstOrDefault().id;
                        _tempAppendCategoryNotes.Add(_categoryNote); 
                    }
                    else
                    {
                        _categoryNote.id = Guid.NewGuid();
                        _tempNewCategoryNotes.Add(_categoryNote);
                    } 
                });
                 

                //

                if (_tempNewCategoryNotes.Count() > 0)
                {
                    db.CategoryNotes.AddRange(_tempNewCategoryNotes);
                }
                if (_tempAppendCategoryNotes.Count() > 0)
                {
                    db.CategoryNotes.UpdateRange(_tempAppendCategoryNotes);
                }

                //clear local note
                db.Database.ExecuteSqlRaw("exec sp_clear_data_before_sync_remote_database");
                 
                var remote_note =JsonSerializer.Deserialize<List<NoteModel>>(JsonSerializer.Serialize( data.SelectMany(r => r.notes).ToList()));
                remote_note.ForEach(r => r.is_predefine_note = true); 
                db.Notes.AddRange(remote_note);
                 
                await  db.SaveChangesAsync();
            } 
        }
    } 
    
    class NoteCategory : ShareCategoryNoteModel
    { 
        public List<ShareNoteModel> notes { get; set; }
    }

     
}
