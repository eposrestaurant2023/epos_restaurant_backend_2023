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
        public SyncController(ApplicationDbContext _db, IHttpService _http, IConfiguration _config)
        {
            db = _db;
            http = _http;
            config = _config;
        }

        [HttpPost("Sale")] 
        [AllowAnonymous]
        public async Task<ActionResult> SyncSale(Guid saleId)
        {
            try
            {
               var _saleData = db.Sales.Where(r=>r.id==saleId)
                    .Include(r => r.sale_payments)
                    .Include(r=>r.sale_products).ThenInclude(r=>r.sale_product_modifiers)
                    .AsNoTrackingWithIdentityResolution();


                if (_saleData.Count() > 0)
                { 
                    var _syncResp = await http.ApiPost("Sale/Save", _saleData.FirstOrDefault());
                    if (!_syncResp.IsSuccess)
                    {
                        return BadRequest();
                    }
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
                    var _syncResp = await http.ApiPost("Sale/Save", _saleData.FirstOrDefault());
                    if (!_syncResp.IsSuccess)
                    {
                        return BadRequest();
                    }
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



        [HttpPost("GetRemoteData")]    
        [AllowAnonymous]
        public async Task<ActionResult<List<ConfigDataModel>>> GetRemoteData(bool is_service_sync=false)
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


            string _deleteQuery = string.Format("delete tbl_config_data;");
            db.Database.ExecuteSqlRaw(_deleteQuery);
            db.ConfigDatas.AddRange(config_datas);     
            await db.SaveChangesAsync();
            //
            return Ok();       

        }

        async Task<List<MenuModel>> GetRemoteMenu(string business_branch_id)
        {      
            is_get_remote_data_success = false;
            var resp = await http.ApiGetOData($"Menu?$select=id,parent_id,menu_name_en,menu_name_kh,text_color,background_color,root_menu_id&$filter=business_branch_id eq {business_branch_id} and is_deleted eq false and status eq true");
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
            string _select_product_modifier = "$select=id,parent_id,product_id,modifier_name,price,section_name,is_required,is_multiple_select,is_section";

            string url = $"product?$select=product_group_id,product_tax_value,product_category_id,product_category_en,product_category_kh,id,product_code,product_name_en,product_name_kh,photo,note,is_allow_discount,is_open_product,is_inventory_product,kitchen_group_name,kitchen_group_sort_order";
            url = url + $"&$expand=product_printers($select=id,product_id,printer_name,ip_address,port;$filter=is_deleted eq false and printer/business_branch_id eq {business_branch_id}),";
            url = url + $"product_modifiers({_select_product_modifier};$expand=children({_select_product_modifier};$filter=is_deleted eq false);$filter=is_deleted eq false),";
            url = url + $"product_portions($select=id,product_id, portion_name,cost,multiplier,unit_id;$filter=is_deleted eq false)";
            url = url + "&$filter=is_deleted eq false and status eq true";
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
            url = url + "product/status eq true and ";
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
            string url = "Configdata?$select=id,data,config_type,note";
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
            string _query = $"Note/Category?$select=id,category_note_name_en,category_note_name_kh,is_multiple_select&$expand=notes($filter=is_deleted eq false and status eq true and business_branch_id eq {business_branch_id})";
            var resp = await http.ApiGet(_query);
            if (resp.IsSuccess)
            {
                is_get_remote_data_success = true;

                List<CategoryNoteModel> _tempNewCategoryNotes = new List<CategoryNoteModel>();
                List<CategoryNoteModel> _tempAppendCategoryNotes = new List<CategoryNoteModel>();
                List<NoteModel> _tempNewNotes = new List<NoteModel>();
                List<NoteModel> _tempAppendNotes = new List<NoteModel>();
                List<CategoryNoteModel> _LocalCategoryNotes = new List<CategoryNoteModel>();
                _LocalCategoryNotes = db.CategoryNotes.AsNoTracking().ToList();

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
                ////Get Note 
                data.SelectMany(r => r.notes).ToList().ForEach(n =>
                {
                    NoteModel _note = new NoteModel()
                    {
                        note_id = n.id,
                        category_note_id = n.category_note_id,
                        created_by = n.created_by,
                        created_date = n.created_date,
                        is_deleted = n.is_deleted,
                        deleted_by = n.deleted_by,
                        deleted_date = n.deleted_date,
                        note = n.note,
                        status = n.status
                    };
                    var _n = _LocalNotes.Where(r => r.note_id == n.id);
                    if (_n.Count() > 0)
                    {
                        _note.id = _n.FirstOrDefault().id;
                        _tempAppendNotes.Add(_note);
                    }
                    else
                    {
                        _note.id = Guid.NewGuid();
                        _tempNewNotes.Add(_note);
                    }
                });

                //

                if (_tempNewCategoryNotes.Count()>0)
                {
                    db.CategoryNotes.AddRange(_tempNewCategoryNotes); 
                }
                if (_tempAppendCategoryNotes.Count() > 0)
                { 
                    db.CategoryNotes.UpdateRange(_tempAppendCategoryNotes);
                }
                if (_tempNewNotes.Count() > 0)
                {
                    db.Notes.AddRange(_tempNewNotes);
                }
                if (_tempAppendNotes.Count() > 0)
                {
                    db.Notes.UpdateRange(_tempAppendNotes);
                }
                await  db.SaveChangesAsync();
            } 
        }
    } 
    
    class NoteCategory : ShareCategoryNoteModel
    { 
        public List<ShareNoteModel> notes { get; set; }
    }
}
