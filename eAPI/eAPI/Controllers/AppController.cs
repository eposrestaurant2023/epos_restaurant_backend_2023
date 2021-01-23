﻿using eModels;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly ApplicationDbContext db;
        public AppController(ApplicationDbContext _db)
        {
            db = _db;
        }

        [HttpGet("GlobalVariable")]
        [EnableQuery(MaxExpansionDepth = 0)]
        public ActionResult<GlobalVariableModel> GetGlobalVariable(bool status = true)
        {
            db.Database.ExecuteSqlRaw("exec sp_startup");
            GlobalVariableModel gv = new GlobalVariableModel();
            gv.business_info = db.BusinessInformations.FirstOrDefault();
            gv.payment_types = db.PaymentTypes.ToList();
            gv.settings = db.Settings.ToList();
            gv.permission_options = db.PermissionOption.ToList();
            gv.module_views = db.ModuleViews.ToList();
            
            gv.customer_groups = db.CustomerGroups.ToList();
            
            gv.currencies = db.Currencies.ToList();
            gv.roles = db.Roles.ToList();
            gv.product_types = db.ProductTypes.ToList();
            gv.countries = db.Countries.ToList();
            gv.stock_locations = db.StockLocations.ToList();
            gv.outlets = db.Outlets.ToList();
            gv.vendors = db.Vendors.ToList();
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

    }
}
