using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using eAPIClient.Models;
using eAPIClient.Services;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NETCore.Encrypt;


namespace eAPIClient.Controllers
{
    [ApiController]
    [Route("api")]      
    public class AppController : ODataController
    {                
        
        public IConfiguration config { get; }
        private readonly ApplicationDbContext db;
        private readonly AppService app;
        private readonly IHttpService http;
        private readonly ISyncService sync;

        private readonly IWebHostEnvironment environment;
       

        public AppController(ApplicationDbContext _db, AppService _app, IConfiguration configuration, IHttpService _http, ISyncService sync, IWebHostEnvironment environment)
        {
            db = _db;
            app = _app;
            config = configuration;
            http = _http;
            this.environment = environment;
            this.sync = sync;
        }




        [HttpGet("isClientAPIWork")]
        [EnableQuery(MaxExpansionDepth = 
            0)]             
        public ActionResult<bool> IsAPIWorking()
        {
            return Ok();
        }

        [HttpGet("GetServerAPIURL")]   
        public ActionResult<string> GetServerAPIURL()
        {
            string _url = config.GetValue<string>("server_api_url");
            return Ok(_url);
        }


        [HttpPost]
        [Route("GetData")]
        public ActionResult<string> GetData([FromBody] FilterModel f)
        {
            string sql = string.Format("exec {0} {1}", f.procedure_name, f.procedure_parameter);
            var d = db.StoreProcedureResults.FromSqlRaw(sql).ToList().FirstOrDefault();
            if (d != null)
            {
                string r = d.result.Replace("\\", "").Replace("\"[", "[").Replace("]\"", "]").ToString();
                return r;
            }
            return BadRequest();

        }

        [HttpGet]
        [Route("GetPublicConfig")]
        [AllowAnonymous]
        public ActionResult<string> GetPublicConfig()
        {
            string sql = string.Format("exec sp_get_public_config");
            var d = db.StoreProcedureResults.FromSqlRaw(sql).ToList().FirstOrDefault();
            if (d != null)
            {
                string r = d.result.Replace("\\", "").Replace("\"[", "[").Replace("]\"", "]").ToString();
                return r;
            }
            return BadRequest();

        }



        [HttpGet("GetSystemMachineLicense")]
        [AllowAnonymous]
        public async Task<ActionResult> GetSystemLicense(string station_id)
        {
            if (!string.IsNullOrEmpty(station_id))
            {
                var _resp = await http.ApiGet($"station({station_id})");      
                if (_resp.IsSuccess)
                {
                    StationLicenseModel _station = System.Text.Json.JsonSerializer.Deserialize<StationLicenseModel>(_resp.Content.ToString());
                    //
                    return Ok(_station);
                }
                return BadRequest();
            }
            return NotFound();
        }

        [HttpPost("UpdateStationIsConfig")]
        [AllowAnonymous]
        public async Task<ActionResult> UpdateStationIsConfig(string station_id,bool is_already_config)
        {
            if (!string.IsNullOrEmpty(station_id))
            {
                var _resp = await http.ApiPost($"Station/Update?id={station_id}&is_already_config={is_already_config}");
                if (_resp.IsSuccess)
                {      
                    return Ok();
                }
                return BadRequest();
            }
            return NotFound();
        }

        [HttpPost("UpdateTable")]
        [AllowAnonymous]
        public async Task<ActionResult> UpdateTable([FromBody] List<TableGroupModel> models)
        {    
            var _resp = await http.ApiPost("TableGroup/SaveAll", models);
            if (_resp.IsSuccess)
            {
                return Ok();
            }      
            return BadRequest();
        }


        [HttpPost]
        [Route("PrintRequest")]
        public ActionResult<string> PrintRequest([FromBody] PrintRequestModel f)
        {
            app.sendPrintRequest(f);
            return Ok();
        }

        [HttpPost]
        [Route("SendSyncRequest")]
        public ActionResult<string> SendSyncRequest()
        {
            sync.sendSyncRequest();
            return Ok();
        }  
        
        [HttpPost]
        [Route("SendSyncRemoteDataRequest")]
        public ActionResult<string> SendSyncRemoteDataRequest()
        {
            sync.sendSyncRemoteDataRequest();
            return Ok();
        }


        [HttpPost]
        [Route("BackupDatabase")]
        public ActionResult<string> BackupDatabase()
        {
            db.Database.ExecuteSqlRaw("exec sp_backup_database");
            http.ApiPost("BackupDatabase");
            return Ok();
        }


        [HttpGet("CurrentBusinessBranch")]
        [EnableQuery(MaxExpansionDepth = 0)]
        public ActionResult<bool> CurrentBusinessBranchId(string businessBranchId)
        {
            string _businessBranchId = config.GetValue<string>("business_branch_id");

            if(_businessBranchId.ToLower() != businessBranchId.ToLower())
            {          
                return Ok(false);
            }

            return Ok(true);
        }

         

        [HttpPost]
        [Route("SaveWifiPassword")]
        public ActionResult<bool> SaveWifiPassword(string value)
        {
            var data = db.ConfigDatas.Where(r => r.config_type == "wifi");
            ConfigDataModel d = new ConfigDataModel();
            if (data.Any())
            {
                d = data.FirstOrDefault();
                d.data = value;
                db.ConfigDatas.Update(d);
            }else
            {
                d.config_type = "wifi";
                d.data = value;
                d.id = Guid.NewGuid();
                d.is_local_setting = true;
                db.ConfigDatas.Add(d);

            }
            db.SaveChanges();


            return Ok(true);
        }


         [HttpPost]
        [Route("CheckWorkingDay")]
        public async Task <ActionResult<CheckWorkingModel>> CheckWorkingDay([FromBody] CheckWorkingModel model)
        {
            model.working_day = await app.GetWorkingDayInfor(model.working_day, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));

            
            model.cashier_shift.working_day_id = model.working_day.id;

            model.cashier_shift = await app.GetCashierShiftInfo(model.cashier_shift, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));


            return Ok(model);
        }


        [Route("image/{image_name}")]
        [AllowAnonymous]
        public ActionResult GetImage(string image_name)
        {
            string img_path = this.environment.ContentRootPath + "\\upload\\" + image_name;
            if (!System.IO.File.Exists(img_path))
            {
                //check server
                if (!SaveImageFromUrl(image_name)){
                    img_path = this.environment.ContentRootPath + "\\upload\\placeholder.png";
                }
                
            }
            var image = System.IO.File.OpenRead(img_path);
            return File(image, "image/jpeg");

        }

        public bool SaveImageFromUrl(string filename)
        {
            
            string server_url = config.GetValue<string>("server_api_url");
            server_url = server_url.Replace("/api/", "/");
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    string url = server_url + "upload/" + filename;
                    byte[] dataArr = webClient.DownloadData(url);

                    string img_path = this.environment.ContentRootPath + "\\upload\\" + filename;
                    System.IO.File.WriteAllBytes(img_path, dataArr);
                }
                return true;
            }
            catch
            {
                return false;
            }

        }



    }

    class StationLicenseModel
    {
       public DateTime expired_date { get; set; }
       public bool is_full_license { get; set; }

    }

}
