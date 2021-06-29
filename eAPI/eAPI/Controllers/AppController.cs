using DeviceId;
using eModels;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public IConfiguration Configuration { get; }
        private readonly ApplicationDbContext db;
        public AppController(ApplicationDbContext _db, IConfiguration configuration)
        {
            db = _db;
            Configuration = configuration;
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
            gv.permission_options = db.PermissionOption.ToList();
            gv.module_views = db.ModuleViews.ToList();
            
            gv.customer_groups = db.CustomerGroups.ToList();
            gv.product_groups = db.ProductGroups.Where(r=>r.is_deleted==false).ToList();
            gv.product_categories = db.ProductCategories.Where(r => r.is_deleted == false).ToList();
            gv.currencies = db.Currencies.ToList();
            gv.roles = db.Roles.ToList();
 
            gv.countries = db.Countries.ToList();
            gv.stock_locations = db.StockLocations.ToList();
            gv.outlets = db.Outlets.ToList();
            gv.vendors = db.Vendors.ToList();
            gv.vendor_groups = db.VendorGroups.Where(r => r.is_deleted == false && status == true).ToList();
            gv.provinces = db.Provinces.ToList();
            gv.category_notes = db.CategoryNotes.ToList();
            gv.bussiness_branches = db.BusinessBranches.ToList();
            gv.printers = db.Printers.ToList();
            gv.price_rules= db.PriceRules.Where(r=>r.is_deleted==false && r.status).ToList();
            gv.units= db.Units.Where(r=>r.is_deleted==false && r.status).ToList();
            gv.unit_categories= db.UnitCategorys.ToList();
            gv.inventory_transaction_type= db.InventoryTransactionTypes.ToList();
            gv.kitchen_groups= db.KitchenGroups.ToList();

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
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return h;
        }

        [HttpPost("save/setting/multiple")]
        public async Task<ActionResult<string>> SaveMultiple([FromBody] List<SettingModel> Settings)
        {
            db.Settings.UpdateRange(Settings);
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return Ok(Settings);
        }

        [HttpPost("Company/save")]
        public async Task<ActionResult<BusinessInformationModel>> Savecompany([FromBody] BusinessInformationModel company)
        {
            db.BusinessInformations.Update(company);
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
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

        [HttpGet("is_working")]
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

         
            if (deviceId == "NCn_YBwcxPq1k9a7F9poQoVWSBJnuLYz0QlnP7bf3wkzyx" ||
                deviceId == "OHHzxvomyr_YzGuY6ysKkYwyDWk9ueuz5VmPHF3exAk" ||
                deviceId == "8T1jIfz9R-YqK2W1DQyFdVMI2XFEcZzJ3I8pGOc5Kdw" ||
                deviceId == "E4_eXtdxdL3OwNp0XTTy-emrisT5XI8_rg8dXHS8AJo" ||
                deviceId == "0edRjKQ3SahFZoGDiyHT6DXez-x7L2Z2_3kOuZHQF1U"   ||
                deviceId == "Vilbvq65BAldEO27ZPGN-SzS-vIguKfSjjEt3E5v9qg" ||
                deviceId == "ONRxV2e8zoYUdTtIY-zf0Nlt9GROAdYpPjmXT4W4nhY"||
                deviceId == "84mCv3v_sF0X4CPXbUdxEQ9UvdMZuvCkaezApIG6K5Y" ||
                deviceId == "R-WsoGUbh9gjl4HOkj2LCeeSJcyMbLXJOxR-OwjNShM"

                )
            {
                //deviceId = "R-WsoGUbh9gjl4HOkj2LCeeSJcyMbLXJOxR-OwjNShM";
                deviceId = "L7_I24eoNzEmQ1iao39ojrek026LhjocLtig7X_DQHs";
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

    }
}
