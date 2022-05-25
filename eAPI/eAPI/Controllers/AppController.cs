﻿using DeviceId;
using eAPI.Hubs;
using eModels;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;     
namespace eAPI.Controllers
{
    [ApiController]
    [Route("api")]

    public class AppController : ControllerBase
    {
        private readonly IHubContext<ConnectionHub> hub;
        public IConfiguration Configuration { get; }
        private readonly ApplicationDbContext db;
        public AppController(ApplicationDbContext _db, IConfiguration configuration, IHubContext<ConnectionHub> _hub)
        {
            db = _db;hub = _hub;
            Configuration = configuration;
            hub = _hub;
        }
        

        [HttpGet("GlobalVariable")]
        [EnableQuery(MaxExpansionDepth = 0)]
      
        public ActionResult<GlobalVariableModel> GetGlobalVariable(bool status = true)
        {
            db.Database.ExecuteSqlRaw("exec sp_startup");
            GlobalVariableModel gv = new GlobalVariableModel();
            gv.business_info = db.BusinessInformations.FirstOrDefault();
            gv.bussiness_branches = db.BusinessBranches.Where(r=>r.is_deleted==false && r.status).ToList();
            gv.payment_types = db.PaymentTypes.ToList();
            gv.settings = db.Settings.ToList();
            gv.permission_options = db.PermissionOption.Where(r=>r.status==true).ToList();
            gv.module_views = db.ModuleViews.ToList();
            
            gv.customer_groups = db.CustomerGroups.ToList();
            gv.product_groups = db.ProductGroups.Where(r=>r.is_deleted==false).ToList();
            gv.product_categories = db.ProductCategories.Where(r => r.is_deleted == false).ToList();
            gv.currencies = db.Currencies.Where(r=>r.is_deleted==false).ToList();
            gv.roles = db.Roles.ToList();
 
            gv.countries = db.Countries.ToList();
            gv.stock_locations = db.StockLocations.Where(r=>r.is_deleted == false).ToList();
            gv.outlets = db.Outlets.Include(r=>r.stations).ToList();
            gv.vendors = db.Vendors.ToList();
            gv.vendor_groups = db.VendorGroups.Where(r => r.is_deleted == false && status == true).ToList();
            gv.provinces = db.Provinces.ToList();
            gv.category_notes = db.CategoryNotes.ToList();
            gv.printers = db.Printers.ToList();
            gv.price_rules= db.PriceRules.Where(r=>r.is_deleted==false && r.status).ToList();
            gv.units= db.Units.Where(r=>r.is_deleted==false && r.status).ToList();
            gv.unit_categories= db.UnitCategorys.ToList();
            gv.inventory_transaction_type= db.InventoryTransactionTypes.ToList();
            gv.kitchen_groups= db.KitchenGroups.ToList();
            gv.system_features = db.system_features.ToList();
            gv.business_branch_system_features= db.BusinessBranchSystemFeatures.ToList();
            gv.revenue_groups= db.RevenueGroups.ToList();
            gv.sale_types= db.SaleTypes.ToList();
            gv.expeneses_categories= db.ExpenseCategories.ToList();
            gv.expenses_items= db.ExpenseItems.ToList();
            gv.stations= db.Stations.ToList();

            return Ok(gv);
        }

        [HttpGet("PermissionOptionRole")]
        [EnableQuery(MaxExpansionDepth = 0)]
        public ActionResult<List<PermissionOptionRoleModel>> Get(bool is_deleted = false)
        {
            var per = db.PermissionOptionRole;
            return Ok(per);
        }

        [HttpGet("Setting")]
        [EnableQuery(MaxExpansionDepth = 8)]
        public IQueryable<SettingModel> Setting(string keyword = "")
        {
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                return db.Settings.Where(r =>
                (
                (r.setting_description ?? "") +
                (r.setting_title ?? "")
                ).ToLower().Trim().Contains(keyword.ToLower().Trim()));
            }
            else
            {
                return db.Settings;
            }
        }
        [HttpPost("setting/save")]
        public async Task<SettingModel> SaveSettings([FromBody] SettingModel h)
        {
            db.Database.ExecuteSqlRaw($"delete tbl_business_branch_setting where setting_id = {h.id}");
            db.BusinessBrachSettings.AddRange(h.business_branch_settings);
            db.Update(h);
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)),hub);
            return h;
        }

        [HttpPost("save/setting/multiple")]
        public async Task<ActionResult<string>> SaveMultiple([FromBody] List<SettingModel> Settings)
        {
            db.Settings.UpdateRange(Settings);
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)),hub);

            db.Database.ExecuteSqlRaw("exec sp_update_setting");
            return Ok(Settings);
        }

        [HttpPost("setting/taxrule/save")]
        public async Task<ActionResult<string>> SaveTaxRule([FromBody] eShareModel.TaxRuleModel value)
        {
            SettingModel record = await db.Settings.FindAsync(59);

            string data = JsonSerializer.Serialize(value);
            record.setting_value = data;
            db.Settings.Update(record);
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)),hub);
            return Ok(record);
        }

        //[HttpGet("ProductPortion")]
        //[EnableQuery]
        //public async Task<List<ProductPortionModel>> GetProductPortion(string keyword = "")
        //{
            
        //        return db.ProductPortions.Where(r=>r.is_deleted == false).ToList();
            
        //}

        [HttpPost("Company/save")]
        public async Task<ActionResult<BusinessInformationModel>> Savecompany([FromBody] BusinessInformationModel company)
        {
            db.BusinessInformations.Update(company);
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)),hub);
            return Ok(company);

        }

        [HttpGet("country")]
        [EnableQuery]
        public async Task<List<CountryModel>> GetCountry(string keyword = "")
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                return await (db.Countries.Where(r =>
                (
                (r.country_name ?? "") +
                (r.country_note ?? "")
                ).ToLower().Trim().Contains(keyword.ToLower().Trim())).ToListAsync());
            }
            else
            {
                return await (db.Countries.ToListAsync());
            }
        }

        [HttpGet("SaleProductStatus")]
        [EnableQuery]
        public async Task<List<SaleProductStatusModel>> GetSaleProductStatus()
        {
            return await (db.SaleProductStatuses.ToListAsync());

        }
        [HttpGet("SaleStatus")]
        [EnableQuery]
        public async Task<List<SaleStatusModel>> GetSaleStatus()
        {
            return await (db.SaleStatuses.ToListAsync());
        }



        [HttpGet("NoteCategory")]
        [EnableQuery]
        public async Task<List<CategoryNoteModel>> GetNoteCategory()
        {
            return await (db.CategoryNotes.ToListAsync());
            
        }

        [HttpPost]
        [Route("GetData")]
        public ActionResult<string> GetData([FromBody] FilterModel f)
        {
            f.procedure_parameter = f.procedure_parameter.Replace("\\", "");
            var d = db.StoreProcedureResults.FromSqlRaw(string.Format("exec {0} {1}", f.procedure_name, f.procedure_parameter)).ToList().FirstOrDefault();
            if (d != null)
            {
                string r = d.result.Replace("\\", "").Replace("\"[", "[").Replace("]\"", "]").ToString();
                return r;
            }

            return BadRequest();

        }
        [HttpPost]
        [Route("GetDataSql")]

        public ActionResult<string> GetDataSQL([FromBody] FilterModel f)
        {

            var d = db.StoreProcedureResults.FromSqlRaw(f.sql_statement).ToList().FirstOrDefault();
            if (d != null)
            {
                string r = d.result.Replace("\\", "").Replace("\"[", "[").Replace("]\"", "]").ToString();
                return r;
            }
            return BadRequest();
        }
        [HttpPost]
        [Route("GetDataDecimal")]
        public ActionResult<decimal> GetDataDecimal([FromBody] FilterModel f)
        {
            var d = db.StoreProcedureResultsDecimal.FromSqlRaw(string.Format("exec {0} {1}", f.procedure_name, f.procedure_parameter)).ToList().FirstOrDefault();
            if (d != null)
            {
                return Convert.ToDecimal(d.result);
            }
            return BadRequest();
        }

        [HttpGet("isServerAPIWork")]
        [EnableQuery(MaxExpansionDepth = 0)]
        public ActionResult<bool> IsAPIWorking()
        {
            return Ok();
        } 
       
        
        [HttpGet("ServerConfig")]
        [AllowAnonymous]
        public ActionResult<ServerConfigModel> ServerConfig()
        {
            var data = db.Settings.Where(r => r.id == 57 || r.id == 58);

            ServerConfigModel s = new ServerConfigModel();
            s.project_id = data.Where(r => r.id == 57).FirstOrDefault().setting_value;
            s.server_id= data.Where(r => r.id == 58).FirstOrDefault().setting_value;
            s.hardware_server_id = GetServerID();

            return Ok(s);
        }
        
        [HttpGet("ServerID")]
         [AllowAnonymous]
        public ActionResult<string> ServerID()
        {
           

            return Ok(GetServerID());
        }

        string GetServerID()
        {
            string deviceId = new DeviceIdBuilder()
           .AddProcessorId()
           .AddMotherboardSerialNumber()
           .AddSystemDriveSerialNumber()
           .AddMacAddress()
           .ToString();

         
            if (
                deviceId == "NCn_YBwcxPq1k9a7F9poQoVWSBJnuLYz0QlnP7bf3wkzyx" ||
                deviceId == "OHHzxvomyr_YzGuY6ysKkYwyDWk9ueuz5VmPHF3exAk" ||
                deviceId == "8T1jIfz9R-YqK2W1DQyFdVMI2XFEcZzJ3I8pGOc5Kdw" ||
                deviceId == "E4_eXtdxdL3OwNp0XTTy-emrisT5XI8_rg8dXHS8AJo" ||
                deviceId == "0edRjKQ3SahFZoGDiyHT6DXez-x7L2Z2_3kOuZHQF1U" ||
                deviceId == "Vilbvq65BAldEO27ZPGN-SzS-vIguKfSjjEt3E5v9qg" ||
                deviceId == "ONRxV2e8zoYUdTtIY-zf0Nlt9GROAdYpPjmXT4W4nhY" ||
                deviceId == "84mCv3v_sF0X4CPXbUdxEQ9UvdMZuvCkaezApIG6K5Y" ||
                deviceId == "R-WsoGUbh9gjl4HOkj2LCeeSJcyMbLXJOxR-OwjNShM" ||
                deviceId == "fNqPfLuN8aHbB2BVj82vJjh_Fe-nXjUBhw3Rl7KD-Dc" ||
                deviceId == "jZouDyVlwOjh8QAH89f0cl7lsNIUSQDpoWP0WKfVPvY" ||
                deviceId == "L7_I24eoNzEmQ1iao39ojrek026LhjocLtig7X_DQHs" ||
                deviceId == "rczv27Je0KGcoeVClR5mr3LbDOrDjQ4upErIxWXHz2g" ||
                deviceId == "xa3Nl4DGd-08vBN0lJ4_C2qPFxZ9YkW3WYCxpXhqIak" ||
                deviceId == "kKyn93RkgIlEc_qVi9YUyONmU30U0bOfFWfW6yozYu8" ||
                deviceId == "a5EVCN1u2QLiTFH9OuTQ6gneNoz7_9UVdRjSV__C9bk" ||
                deviceId == "TlFv1IKgyj3JJf75n7_Yy9gR0dqktTCgLBxiRfH_jcg" ||
                deviceId == "2jiw9Z5jOWQBKT-pjmInMSrayVo_iRYHD638lW9gIVU" ||
                deviceId == "3c3cxn3pzCyzSWnulIm72uR9NR7g2c3Z63JPyvSUzLk" ||
                deviceId == "WAn0dZQ4kWPFVL-e-F-4NoeO0ae2kfeW150IXjM_KHY" ||
                deviceId == "wBUFZdMYBWdcb9rEsMPoF2ESUEXE5SpsaQhPA4k9LhE" ||
                deviceId == "O0C30pohUFQEEbe3uJSIPOr4FKKne-zdIf4JBbwBA6Q" ||
                deviceId == "Sc0UgR4gWFuepT5mHtZJZ8EQD1eMlNFgWFa4JVlHq60" ||
                deviceId == "SDOoJUSChdeLkqSBYCcDhyCW_nuzlcnjFYacKAwQS9s" ||
                deviceId == "cPTB7pEjQoCgGOpEgGLPxktvUmaICYQq7WVl1IzQPK8" ||
                deviceId == "4efs1BZOzIg_-wWTNBAfzfWWABfjmNJzmNgW4ecQa0M" ||
                deviceId == "CsUhcPGkd_hNOyjA1_7jk0t2mhjhWurAQ1MiCymzIj8" ||
                deviceId == "0v54OjqC4D1ch-996YgPIO_LmwW-D8oGifpMeZVncgw" ||
                deviceId == "aZkTozkl05Dw_A5YMEymXmz6gZBhBUKtPSykg9DpdVE" ||
                deviceId == "BFrmsTXtMY2akk98Cd9wxRomzqRKaJrrL080xAB-c4I" ||
                deviceId == "XJHGOXQ1QzMVlFTYrz2Cv7coEkEUmvHE-6hz9FFf8uk" ||
                deviceId == "xWLXg2-DSH_U4Onq06hQpSoQef0P6mqRIW3hv0-Rib8" ||
                deviceId == "3lhOl-I3LerDCvMbuB024mBbxwduiWiBaLpC6seAvTs"
                    

                )
            {
                
                deviceId = "SDOoJUSChdeLkqSBYCcDhyCW_nuzlcnjFYacKAwQS9s";
            }

            return deviceId;
        }


        [HttpGet("CheckRequiredResetDatabase")]
        [AllowAnonymous]
        public ActionResult<bool> CheckRequiredResetDatabase()
        {
            var d = db.Set<StoreProcedureResultDecimalModel>().FromSqlRaw("exec [sp_check_required_reset_database]");
            return d.AsEnumerable().FirstOrDefault().result==1;
        }


        [HttpGet("GetTranslateText")]
        [AllowAnonymous]
        public ActionResult<List<eShareModel.TranslateTextModel>> GetTranslateText()
        {
            return db.TranslateTexts.ToList();
            
        }


        [HttpPost]
        [Route("ConfirmBackendData")]
        public async Task< ActionResult<string>> ConfirmBackendData([FromBody] eSoftixBackend.ProjectModel p)
        {
            //udpate project id to setting 
            SettingModel project_id = db.Settings.Find(57);
            project_id.setting_value = p.id.ToString();
            db.Settings.Update(project_id);

            //update server id 
            SettingModel server_id = db.Settings.Find(58);
            server_id.setting_value = GetServerID();
            db.Settings.Update(server_id);
            //add record to tbl_business_information mappint this data from tbl_customer in eostix backend project
            var business_informations = db.BusinessInformations.ToList();
            BusinessInformationModel biz = new BusinessInformationModel();
            if (business_informations.Any())
            {
                biz = business_informations.FirstOrDefault();
            }

            biz.id = p.customer.id;
            biz.company_name = (p.customer.company_name ?? "");
            biz.company_name_kh = (p.customer.company_name ?? "" );
            biz.contact_name = (p.customer.customer_name_en ?? "");
            biz.contact_phone_number = (p.customer.phone_1 ?? "");
            biz.office_phone = (p.customer.phone_2 ?? "");
            biz.address = (p.customer.address ?? "");
            biz.email = (p.customer.email??"");
            //need more field to map

            List<BusinessBranchModel> branches = new List<BusinessBranchModel>();

            branches = JsonSerializer.Deserialize<List<BusinessBranchModel>>(JsonSerializer.Serialize( p.business_branches.ToList()));
            db.BusinessBranches.AddRange(branches);

            //project feature
            foreach(var f in p.project_system_features)
            {
                f.system_feature.status = f.status;
                db.system_features.Add(JsonSerializer.Deserialize<SystemFeatureModel>(JsonSerializer.Serialize(f.system_feature)));
            }

            //check businss branch system feature
            var business_branch_system_features = p.business_branches.SelectMany(r => r.business_branch_system_features);
            business_branch_system_features.ToList().ForEach(r => r.system_feature = null);
            db.BusinessBranchSystemFeatures.AddRange(JsonSerializer.Deserialize<List<BusinessBranchSystemFeatureModel>>(JsonSerializer.Serialize(business_branch_system_features)));


            //cash drawer
            db.CashDrawers.AddRange(JsonSerializer.Deserialize<List<eShareModel.CashDrawerModel>>(JsonSerializer.Serialize(p.cash_drawers)));


            if (business_informations.Any())
            {

                db.BusinessInformations.Update(biz);
            }
            else
            {

                db.BusinessInformations.Add(biz);
            }


         
            await     db.SaveChangesAsync();

            db.Database.ExecuteSqlRaw("exec sp_setup_config_data 1");


            return Ok();
        }


        [HttpPost]
        [Route("BackupDatabase")]
        public ActionResult<string> BackupDatabase()
        {
            db.Database.ExecuteSqlRaw("exec sp_backup_database");
            return Ok();
        }

        [HttpPost]
        [Route("UpdateDataToClient")]
        public ActionResult<string> UpdateDataToClient()
        {
            hub.Clients.All.SendAsync("Sync", "all");
            return Ok();
        }


    }
}
