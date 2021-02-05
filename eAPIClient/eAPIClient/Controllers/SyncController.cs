using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using eAPIClient.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NETCore.Encrypt;
using eAPIClient.Services;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

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


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]

        public IQueryable<UserModel> Get()
        {
            return db.Users;
        }


        [HttpPost("GetRemoteData")]
        [AllowAnonymous]
        public async Task<ActionResult> GetRemoteData()
        {

            string business_branch_id = config.GetValue<string>("business_branch_id");


            //run script prepare config data 
            var x = await http.ApiPost("GetData", new FilterModel() { procedure_name = "sp_prepare_sync_config_data", procedure_parameter = $"'{business_branch_id}'" });
            

            List<MenuModel> menu_datas = await GetRemoteMenu(business_branch_id);
            
            
            //product have printer and modifier
            List<ProductModel> product_datas = await GetRemoteProduct(business_branch_id);

            List<ProductMenuModel> product_menu_datas = await GetRemoteProductMenu(business_branch_id);
 
            // run this when all data read from server done success
            //run clear all old data
            if (is_get_remote_data_success)
            {
                db.Database.ExecuteSqlRaw("exec sp_delete_menu_and_product");


                db.Menus.AddRange(menu_datas);
                db.Products.AddRange(product_datas);
                db.SaveChanges();
                db.ProductMenus.AddRange(product_menu_datas);

                db.SaveChanges();

            }
            return Ok();

        }

        async Task<List<MenuModel>> GetRemoteMenu(string business_branch_id)
        {
           
            var resp = await http.ApiGetOData($"Menu?$select=id,parent_id,menu_name_en,menu_name_kh,text_color,background_color&$filter=business_branch_id eq {business_branch_id} and is_deleted eq false and status eq true");
            if (resp.IsSuccess)
            {
              return  JsonSerializer.Deserialize<List<MenuModel>>(resp.Content.ToString());
            }else
            {
                is_get_remote_data_success = false;
            }
            return new List<MenuModel>();
        }
           async Task<List<ProductModel>> GetRemoteProduct(string business_branch_id)
        {

            //$expand=product_printers($select=id,printer_name,ip_address_port;$filter=is_deleted eq false)
            string url = $"product?$select=id,product_code,product_name_en,product_name_kh,photo,note,is_allow_discount,is_allow_free,is_open_product,is_inventory_product";
            url = url + $"&$expand=product_printers($select=id,product_id,printer_name,ip_address_port;$filter=is_deleted eq false and printer/business_branch_id eq {business_branch_id}),";
            url = url + $"product_modifiers($select=id,product_id,modifier_name,price;$filter=is_deleted eq false)";
            url = url + "&$filter=is_deleted eq false and status eq true";
            var resp = await http.ApiGetOData(url);
            if (resp.IsSuccess)
            {
              return  JsonSerializer.Deserialize<List<ProductModel>>(resp.Content.ToString());
            }else
            {
                is_get_remote_data_success = false;
            }
            return new List<ProductModel>();
        } 
        
        async Task<List<ProductMenuModel>> GetRemoteProductMenu(string business_branch_id)
        {
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
              return  JsonSerializer.Deserialize<List<ProductMenuModel>>(resp.Content.ToString());
            }else
            {
                is_get_remote_data_success = false;
            }
            return new List<ProductMenuModel>();
        }




    }

    

}
