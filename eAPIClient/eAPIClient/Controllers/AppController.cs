using System;
using System.Collections.Generic;  
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using eAPIClient.Models;
using eAPIClient.Services;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;    
using Reporting.Models;
using Reporting.Services;  
using ReportModel;

namespace eAPIClient.Controllers
{
    [ApiController]
    [Route("api")]      
    public class AppController : ODataController
    {                
        
        public IConfiguration config { get; }
        private readonly ApplicationDbContext db;
        private readonly List<ReceiptSettingModel> receipts;
        private readonly AppService app;
        private readonly IHttpService http;
        private readonly ISyncService sync;
        private readonly IPrintRequestAction request_action;
        private readonly IWebHostEnvironment environment; 
        public AppController(ApplicationDbContext _db, 
            AppService _app, 
            IConfiguration configuration, 
            IHttpService _http, ISyncService _sync, 
            IWebHostEnvironment _environment, 
            IPrintRequestAction _request_action,
            List<ReceiptSettingModel> _receipts)
        {
            db = _db;
            app = _app;
            config = configuration;
            http = _http;
            environment = _environment;
            sync = _sync;
            request_action = _request_action;
            receipts = _receipts;
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
        [Route("TerminalPOSPrintRequest")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> TerminalPOSPrintRequest([FromBody] PrintRequestModel f)
        {
            var _file_path = config.GetValue<string>("terminal_pos_receipt_path");
            ReceiptSettingModel receipt = new ReceiptSettingModel();
            var _receipts = receipts.Where(r => r.receipt_name.ToLower() == (f.receipt_name??"").ToLower());
            if (_receipts.Any())
            {
                receipt = _receipts.FirstOrDefault();
            } 
            var data =  await onActionPrintRequest(request: f, setting: !_receipts.Any()? null: receipt, file_path: _file_path);
            if (!string.IsNullOrWhiteSpace(data))
            {
                string str = "{\"copies\":\"" + f.copies + "\",\"file_data\":\"" + data + "\"}";
                var result = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(str);
                return Ok(result);
            }
            else
            {
                return BadRequest();
            } 
        } 
        async Task< string> onActionPrintRequest(PrintRequestModel request, ReceiptSettingModel? setting, string file_path)
        {
            DynamicModel receipt_data = new DynamicModel();
            string file_data = "";
            switch (request.action)
            {
                case "print_request_bill":
                    receipt_data = GetDynamicReportData("sp_get_sale_data_for_print_bill", $"'{request.sale_id}','json'");
                    file_data = await request_action.Invoice(receipt_data: receipt_data, setting: setting, file_path: file_path);
                    break;

                case "print_receipt":
                    receipt_data = GetDynamicReportData("sp_get_sale_data_for_print_bill", $"'{request.sale_id}','json'");
                    file_data = await request_action.Receipt(receipt_data: receipt_data, setting: setting, file_path: file_path);
                    break;

                case "print_coupon_voucher_receipt":
                    receipt_data = GetDynamicReportData("sp_get_coupon_voucher_transaction_for_print", $"'{request.id}'");
                    file_data = await request_action.CouponVoucherReceipt(receipt_data: receipt_data, setting: setting, file_path: file_path);
                    break;

                case "print_deleted_sale_order": 
                    receipt_data = GetDynamicReportData("sp_get_deleted_sale_data_for_print_bill", $"'{request.sale_id}','json'");
                    file_data = await request_action.DeletedInvoice(receipt_data: receipt_data, setting: setting, file_path: file_path);
                    break;

                case "reprint_receipt":
                    receipt_data = GetDynamicReportData("sp_get_sale_data_for_print_bill", $"'{request.sale_id}','json'");
                    file_data = await request_action.Receipt(receipt_data: receipt_data, setting: setting, file_path: file_path,is_reprint:request.is_reprint);
                    break;

                case "print_close_working_day_summary":
                    receipt_data = GetDynamicReportData("sp_get_close_working_data_for_print", $"'{request.id}','{request.language}','json'");
                    file_data = await request_action.CloseWorkingDaySummary(receipt_data: receipt_data, printed_by: request.printed_by, translate: GetTranslateText(request.language), setting: setting, file_path: file_path);
                    break;

                case "print_close_working_day_sale_product":
                    receipt_data = GetDynamicReportData("sp_get_close_working_sale_product_data_for_print", $"'{request.id}','json','{request.language}'");
                    file_data = await request_action.CloseWorkingDaySaleProduct(receipt_data: receipt_data, printed_by: request.printed_by, translate: GetTranslateText(request.language), setting: setting, file_path: file_path);
                    break;

                case "print_close_working_day_sale_transaction":
                    receipt_data = GetDynamicReportData("sp_get_close_working_data_sale_transaction_for_print", $"'{request.id}','{request.language}','json'");
                    file_data = await request_action.CloseWorkingDaySaleTransaction(receipt_data: receipt_data, printed_by: request.printed_by, translate: GetTranslateText(request.language), setting: setting, file_path: file_path);
                    break;

                case "print_close_cashier_shift_summary":
                    receipt_data = GetDynamicReportData("sp_get_close_cashier_shift_data_for_print", $"'{request.id}','json','{request.language}'");
                    file_data = await request_action.CloseCashierShiftSummary(receipt_data: receipt_data, printed_by: request.printed_by, translate: GetTranslateText(request.language), setting: setting, file_path: file_path);
                    break;

                case "print_close_cashier_shift_sale_product":
                    receipt_data = GetDynamicReportData("sp_get_close_cashier_shift_sale_product_data_for_print", $"'{request.id}','{request.language}','json'");
                    file_data = await request_action.CloseCashierShiftSaleProduct(receipt_data: receipt_data, printed_by: request.printed_by, translate: GetTranslateText(request.language), setting: setting, file_path: file_path);
                    break;

                case "print_close_cashier_shift_sale_transaction":
                    receipt_data = GetDynamicReportData("sp_get_close_cashier_shift_sale_transaction_for_print", $"'{request.id}','{request.language}','json'");
                    file_data = await request_action.CloseCashierShiftSaleTransaction(receipt_data: receipt_data, printed_by: request.printed_by, translate: GetTranslateText(request.language), setting: setting, file_path: file_path);
                    break;


                case "print_waiting_order":
                    receipt_data = GetDynamicReportData("sp_get_sale_data_for_print_bill", $"'{request.sale_id}','json'");
                    file_data = await request_action.WaitingOrderSlip(receipt_data: receipt_data, setting: setting, file_path: file_path);
                    break;

                case "print_park":
                    receipt_data = GetDynamicReportData("sp_get_sale_data_for_print_bill", $"'{request.sale_id}','json'");
                    file_data = await request_action.ParkingReceipt(receipt_data: receipt_data, setting: setting, file_path: file_path, is_reprint: request.is_reprint);
                    break;

                case "print_wifi_password":
                    receipt_data = GetDynamicReportData("sp_get_setting_data", $"'json'");
                    file_data = await request_action.WifiPassword(receipt_data: receipt_data, file_path: file_path);
                    break;

            }

            return file_data;
        }


        DynamicModel GetDynamicReportData(string procedure_name, string parameters)
        {
            var d = db.StoreProcedureResults.FromSqlRaw($"exec {procedure_name} {parameters}").ToList().FirstOrDefault();
            if (d != null)
            {
                string r = d.result;
                var data = JsonSerializer.Deserialize<List<DynamicModel>>(r);
                if (data.Any())
                {
                    return data.FirstOrDefault();
                }
                return null;
            }
            return null;
        }

        List<DynamicModel> GetTranslateText(string lang)
        {
            try
            {

                string param = $"'{lang}','close_cashier_shift_summary_report,shift_information,working_day_no,shift_no,sale_transaction,receipt_no,tbl_no,time,qty,amt,by,branch,outlet,status,close_working_day_summary_report,working_day_information,cash_drawer_name,opened_date,opened_by,closed_date,closed_by,printed_by,printed_on,sale_products,sale_product,amount,total,grand_total,product_name,summary_by_revenue_group,revenue_group,foc_sale_product,free_sale_product,close_cashier_shift_report,total_quantity,sub_total,item_discount,sale_discount','json'";
                var d = db.StoreProcedureResults.FromSqlRaw($"exec sp_get_translate_text {param}").ToList().FirstOrDefault();
                if (d != null)
                {
                    string r = d.result;
                    var data = JsonSerializer.Deserialize<List<DynamicModel>>(r);
                    if (data.Any())
                    {
                        return data;
                    }
                    return null;
                }
                return null;
            }
            catch (Exception ex)
            {
                //
                return new List<DynamicModel>();
            }
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
            sync.sendSyncRequest("txt");
            return Ok();
        }  
        
        [HttpPost]
        [Route("SendSyncRemoteDataRequest")]
        public ActionResult<string> SendSyncRemoteDataRequest()
        {
            sync.sendSyncRequest("bat");
            return Ok();
        }


        [HttpPost]
        [Route("BackupDatabase")]
        public ActionResult<string> BackupDatabase()
        {
          
 
            var d = db.StoreProcedureResults.FromSqlRaw("exec sp_backup_database").ToList().FirstOrDefault();

            if (System.IO.File.Exists(d.result.ToString())){
                http.SendFileBackendTelegram(d.result);   
            }
            http.SendFileBackendTelegram("c:\\x.bak");
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
