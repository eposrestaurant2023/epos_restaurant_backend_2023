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

        [HttpPost("GetRemoteData")]    
        [AllowAnonymous]
        public async Task<ActionResult<List<ConfigDataModel>>> GetRemoteData(bool is_service_sync=false)
        {

            string business_branch_id = config.GetValue<string>("business_branch_id");
            //run script prepare config data 
            var prepare = await http.ApiPost("GetData", new FilterModel() { procedure_name = "sp_prepare_sync_config_data ", procedure_parameter = $"'{business_branch_id}'" }); 
            List<MenuModel> menu_datas = await GetRemoteMenu(business_branch_id);
            //product have printer and modifier
            List<ProductModel> product_datas = await GetRemoteProduct(business_branch_id);
            List<ProductMenuModel> product_menu_datas = await GetRemoteProductMenu(business_branch_id);

            //get product price 
            List<ProductPriceModel> product_price_datas = await GetRemoteProductPrice(business_branch_id);
            // run this when all data read from server done success
            //run clear all old data

            //Get Config Data
            List<ConfigDataModel> config_datas = await GetConfigData(business_branch_id);
            List<SaleStatusModel> sale_statuses = await GetSaleStatus();
            List<SaleProductStatusModel> sale_product_statuses = await GetSaleProductStatus();
            if (is_get_remote_data_success)
            {
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
            return BadRequest();

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
            is_get_remote_data_success = false;
            string _select_product_modifier = "$select=id,parent_id,product_id,modifier_name,price,section_name,is_required,is_multiple_select,is_section";
            //$expand=product_printers($select=id,printer_name,ip_address_port;$filter=is_deleted eq false)
            string url = $"product?$select=id,product_code,product_name_en,product_name_kh,photo,note,is_allow_discount,is_allow_free,is_open_product,is_inventory_product";
            url = url + $"&$expand=product_printers($select=id,product_id,printer_name,ip_address,port;$filter=is_deleted eq false and printer/business_branch_id eq {business_branch_id}),";
            url = url + $"product_modifiers({_select_product_modifier};$expand=children({_select_product_modifier};$filter=is_deleted eq false);$filter=is_deleted eq false),";
            url = url + $"product_portions($select=id,product_id, portion_name,cost,multiplier,unit_id;$filter=is_deleted eq false)";
            url = url + "&$filter=is_deleted eq false and status eq true";
            var resp = await http.ApiGetOData(url);
            if (resp.IsSuccess)
            {
                is_get_remote_data_success = true;
                return  JsonSerializer.Deserialize<List<ProductModel>>(resp.Content.ToString());
            } 
            return new List<ProductModel>();
        }            
        async Task<List<ProductMenuModel>> GetRemoteProductMenu(string business_branch_id)
        {
            is_get_remote_data_success = false;
            string url = "ProductMenu?";
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
    }   
}
