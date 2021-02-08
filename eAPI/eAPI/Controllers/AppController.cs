using eModels;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
            gv.product_types = db.ProductTypes.ToList();
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
        [EnableQuery]
        public ActionResult<List<SettingModel>> Setting(string keyword = "")
        {
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                return db.Settings.Where(r =>
                (
                (r.setting_description ?? "") +
                (r.setting_title ?? "")
                ).ToLower().Trim().Contains(keyword.ToLower().Trim())).ToList();
            }
            else
            {
                return db.Settings.ToList();
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
    }
}
