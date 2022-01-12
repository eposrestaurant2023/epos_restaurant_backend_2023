using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using eAPIClient.Controllers;
using eAPIClient.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace eAPIClient.Services
{
    public interface ISyncService
    {

        Task<bool> SyncSetting();
   
        void sendSyncRequest();
        void sendSyncRemoteDataRequest();

        void OnCreatedAsync(object sender, FileSystemEventArgs e);
        void OnSyncFromRemoteServerAsync(object sender, FileSystemEventArgs e);

    }
        public class SyncService :ISyncService
    {
        public IConfiguration config { get; }
        private readonly ApplicationDbContext db;
        private readonly IHttpService http;

        string path = "";
        string business_branch_name = "";

        private readonly IWebHostEnvironment environment;
        public SyncService(ApplicationDbContext _db, IConfiguration _config, IHttpService _http, IWebHostEnvironment environment)
        {
            db = _db;
            config = _config;
            http = _http;
          
            this.environment = environment;
           path  = environment.ContentRootPath + "\\logs";
            business_branch_name = _config.GetValue<string>("business_branch_name");
        }


        public void sendSyncRequest()
        {

              path = environment.ContentRootPath + "\\logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            // Write the specified text asynchronously to a new file named "WriteTextAsync.txt".
            using (FileStream fs = File.Create(Path.Combine(path, $"{Guid.NewGuid()}.txt")))
            {
                //fs.Write(item.File, 0, item.File.Length);
            }                                                                          
        }
        public void sendSyncRemoteDataRequest()
        {

             path = environment.ContentRootPath + "\\logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            // Write the specified text asynchronously to a new file named "WriteTextAsync.txt".
            using (FileStream fs = File.Create(Path.Combine(path, $"{Guid.NewGuid()}.bat")))
            {
                //fs.Write(item.File, 0, item.File.Length);
            }                                                                        
        }



        bool is_sync_customer_busy = false;
        public async void OnSyncFromRemoteServerAsync(object sender, FileSystemEventArgs e)
        {
            if (is_sync_customer_busy)
            {
                http.SendBackendTelegram($"{business_branch_name}%0aSync Customer busy");
                return;
            }
            is_sync_customer_busy = true;
            try
            {

                http.SendBackendTelegram($"{business_branch_name}%0aStart Sync Customer");
                string value = e.Name.Replace(".txt", "").Split(',')[0];
                List<CustomerBusinessBranchModel> data = new List<CustomerBusinessBranchModel>();
                string query = $"CustomerBusinessBranch?$expand=customer&$filter=is_synced eq false and business_branch_id eq {config.GetValue<string>("business_branch_id")}";
                var resp = await http.ApiGetOData(query);
                if (resp.IsSuccess)
                {
                    data = JsonSerializer.Deserialize<List<CustomerBusinessBranchModel>>(resp.Content.ToString());
                    if (data.Any())
                    {
                        foreach (var b in data)
                        {
                            if (SaveRemoteCustomerToLocalCustomer(b.customer))
                            {
                                b.customer = null;
                                b.is_synced = true;
                               var save_to_server_resp =  await http.ApiPost("CustomerBusinessBranch/Save", b);
                                if (!save_to_server_resp.IsSuccess)
                                {
                                    http.SendBackendTelegram($"{business_branch_name}%0aSync Customer to admin database fail. Data:{JsonSerializer.Serialize(b)}. Error: {save_to_server_resp.Content}");
                                }
                            }
                        }
                    }
                }
                http.SendBackendTelegram($"{business_branch_name}%0aSync Customer complete");
                DeleteOldLogFile();
               
            }
            catch(Exception ex) {
                http.SendBackendTelegram($"{business_branch_name}%0aSync Customer fail. error message: {ex.Message}");
            }
            is_sync_customer_busy = false;
        }


        List<DataForSyncModel> GetDataforSync()
        {

            try
            {
                var d = db.StoreProcedureResults.FromSqlRaw("exec sp_get_data_for_synchronize 'json'").ToList().FirstOrDefault();

                if (d != null)
                {
                    string r = d.result.Replace("\\", "").Replace("\"[", "[").Replace("]\"", "]").ToString();  
                    List<DataForSyncModel> datas = JsonSerializer.Deserialize<List<DataForSyncModel>>(r);
                    return datas;   
                }      
                return new List<DataForSyncModel>();   
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return new List<DataForSyncModel>();   
            }
        }


        bool SaveRemoteCustomerToLocalCustomer(CustomerModel model)
        {
            try
            {
                model.business_branch_id = model.business_branch_id == Guid.Empty ? null : model.business_branch_id;
                model.last_update_business_branch_id= model.last_update_business_branch_id == Guid.Empty ? null : model.last_update_business_branch_id;

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
                Log.Error(ex.ToString() + "=====" + JsonSerializer.Serialize(model));
                http.SendBackendTelegram($"{business_branch_name}%0aSave sync customer fail. Data:{JsonSerializer.Serialize(model)}. error message: {ex.Message}");

            }
            return false;
        }


        public async Task<bool> SyncWorkingDayGet(Guid workingDayId)
        {
            try
            {
                var _workingDayData = db.WorkingDays.Where(r => r.id == workingDayId)
                     .AsNoTracking();
                if (_workingDayData.Count() > 0)
                {
                    var _workingDay = _workingDayData.FirstOrDefault();
                    _workingDay.is_synced = true;
                    var _syncResp = await http.ApiPost("WorkingDay/Save", _workingDay);
                    if (!_syncResp.IsSuccess)
                    {
                        http.SendBackendTelegram($"{business_branch_name}%0aSync working fail.%0a Error Detail:{_syncResp.Content.ToString()}.%0aData: { JsonSerializer.Serialize(_workingDay)}");
                        Log.Error($"Sync working fail. Data: {JsonSerializer.Serialize(_workingDay)}");
                        return false;
                    }
                    db.WorkingDays.Update(_workingDay);
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                Log.Error($"Sync working fail.Exception: {ex.ToString()}.");
            }
            return false;
        }

        public async Task<bool> SyncCashDrawerAmountGet(Guid id)
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
                        http.SendBackendTelegram($"{business_branch_name}%0aSync cash drawer fail. Data: { JsonSerializer.Serialize(_model)}");
                        Log.Error($"Sync cash drawer fail. Data: {JsonSerializer.Serialize(_model)}");
                        return false;
                    }
                    db.CashDrawerAmounts.Update(_model);
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    Log.Error($"Cash drawer Data  not exists. {id.ToString()}");
                    return false;
                }

            }
            catch (Exception ex)
            {
                Log.Error($"Sync cash drawer fail. Ex: {ex.ToString()}");
            }
            return false;
        }

        public async Task<bool> SyncCashierShiftGet(Guid id)
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
                        http.SendBackendTelegram($"{business_branch_name}%0aSync cashier shift fail. Data: { JsonSerializer.Serialize(_model)}");
                        Log.Error($"Sync cashier shift fail. Data: {JsonSerializer.Serialize(_model)}");
                        return false;
                    }
                    db.CashierShifts.Update(_model);
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    Log.Error($"Cash shift  Data  not exists. {id.ToString()}");
                    return false;
                }

            }
            catch (Exception ex)
            {
                Log.Error($"Sync cashier shift fail. Ex: {ex.ToString()}");
            }
            return false;
        }

        public async Task<bool> SyncSaleGet(Guid saleId)
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
                        http.SendBackendTelegram($"{business_branch_name}%0aSync sale fail. Data: { JsonSerializer.Serialize(_sale)}");
                        Log.Error($"Sync sale fail. Data: {JsonSerializer.Serialize(_sale)}");
                        return false;
                    }
                    db.Sales.Update(_sale);
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    Log.Error($"Sale data not exists. {saleId.ToString()}");
                    return false;
                }

            }
            catch (Exception ex)
            {
                Log.Error($"Sync sale fail. Ex: {ex.ToString()}");
            }
            return false;
        }

        public async Task<bool> SyncHistory(Guid id)
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
                        string _data = JsonSerializer.Serialize(_model);
                        http.SendBackendTelegram($"{business_branch_name}%0aSync history fail. Data: { _data}");
                        Log.Error($"Sync history fail. Data: {_data}");  
                        return false;
                    }
                    db.Histories.Update(_model);
                    db.SaveChanges();
                    sendHistoryAlertTelegram(_model);
                    return true;
                }
                else
                {
                    Log.Error($"History data not exists. {id.ToString()}");
                    return false;
                }

            }
            catch (Exception ex)
            {
                Log.Error($"Sync history fail. Ex: {ex.ToString()}");
            }
            return false;
        }

        public async Task<bool> SyncCustomer(Guid id)
        {
            try
            {
                var _modelData = db.Customers.Where(r => r.id == id)
                     .AsNoTrackingWithIdentityResolution();
                if (_modelData.Count() > 0)
                {
                    var _model = _modelData.FirstOrDefault();
                    _model.is_synced = true;
                    _model.business_branch_id = _model.business_branch_id == Guid.Empty ? null : _model.business_branch_id;
                    _model.last_update_business_branch_id = _model.last_update_business_branch_id == Guid.Empty ? null : _model.last_update_business_branch_id;
                    var _syncResp = await http.ApiPost("Customer/Save?is_synch_from_client=true", _model);
                    if (!_syncResp.IsSuccess)
                    {

                        http.SendBackendTelegram($"{business_branch_name}%0aSync customer fail. Data: { JsonSerializer.Serialize(_model)}");
                        Log.Error($"Sync customer fail. Data: {JsonSerializer.Serialize(_model)}");

                        return false;

                    }

                    CustomerModel resp_customer = JsonSerializer.Deserialize<CustomerModel>(_syncResp.Content);
                    _model.customer_code = resp_customer.customer_code;

                    db.Customers.Update(_model);
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    Log.Error($"Customer data not exists. {id.ToString()}");
                    return false;
                }

            }
            catch (Exception ex)
            {
                Log.Error($"Sync customer fail. Ex: {ex.ToString()}");
            }
            return false;
        }


        public async Task<bool> SyncExpense(Guid id)
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

                        http.SendBackendTelegram($"{business_branch_name}%0aSync expense fail. Data: { JsonSerializer.Serialize(_model)}");
                        Log.Error($"Sync expense  fail. Data: {JsonSerializer.Serialize(_model)}");

                        return false;
                    }
                    db.Expenses.Update(_model);
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    Log.Error($"Expense data not exists. {id.ToString()}");
                    return false;
                }

            }
            catch (Exception ex)
            {
                Log.Error($"Sync expense fail. Ex: {ex.ToString()}");
            }
            return false;
        }

        public async Task<bool> SyncSetting()
        {


            string business_branch_id = config.GetValue<string>("business_branch_id");
            var prepare_sync_response = await http.ApiPost("GetData", new FilterModel() { procedure_name = "sp_prepare_sync_config_data", procedure_parameter = $"'{business_branch_id}'" });
            if (prepare_sync_response.IsSuccess)
            {
                List<ConfigDataModel> config_datas = new List<ConfigDataModel>();
                string url = "Configdata?$select=id,data,config_type,note,is_local_setting";
                url = url + $"&$filter=business_branch_id eq {business_branch_id}";
                var resp = await http.ApiGetOData(url);
                if (resp.IsSuccess)
                {

                    config_datas = JsonSerializer.Deserialize<List<ConfigDataModel>>(resp.Content.ToString());
                    if (config_datas.Any())
                    {
                        //get branch info
                        var b = JsonSerializer.Deserialize<List<BusinessBranchModel>>(config_datas.Where(r => r.config_type == "business_branch").FirstOrDefault().data).FirstOrDefault();
                        try
                        {
                            await Task.Factory.StartNew(()=>{
                                var old_config_data = db.ConfigDatas.Where(r => r.is_local_setting == false);
                                db.ConfigDatas.RemoveRange(old_config_data);
                                db.SaveChanges(); 
                                db.ConfigDatas.AddRange(config_datas.Where(r => r.is_local_setting == false));
                                db.SaveChanges(); 
                                
                                http.SendBackendTelegram($"{b.business_branch_name_en}%0aAuto update config data successfully");
                            })   ;
                           


                        }
                        catch (Exception ex)
                        {

                            await Task.Factory.StartNew(() =>
                            {
                                string message = ex.ToString();
                                http.SendBackendTelegram($"{b.business_branch_name_en}%0aAuto update config data successfully%0a{ex}");
                            } );
                        }

                    }
                    return true;
                }



            }

            return false;

        }

        public void sendHistoryAlertTelegram(HistoryModel model)
        {

            string messaage = $"{model.title}%0a{model.description}";
            if (!string.IsNullOrEmpty(model.note))
            {
                messaage = messaage + $"%0aNote: {model.note}";
            }
            messaage = messaage + $"%0a-------------------------";
            messaage = messaage + $"%0aBy: {model.created_by} on {model.created_date.ToString("dd/MM/yyyy hh:mm:ss tt")} ";


            http.SendTelegram(messaage);
            System.Threading.Thread.Sleep(1000);
        }
        private System.Threading.Thread threadStart(Action action)
        {
            System.Threading.Thread thread = new System.Threading.Thread(() => { action(); });
            thread.Start();
            return thread;
        }

        bool is_sync_busy = false;
        async void  ISyncService.OnCreatedAsync(object sender, FileSystemEventArgs e)
        {

            if (is_sync_busy)
            {
                http.SendBackendTelegram($"{business_branch_name}%0aSync process  is busy.");

               
                return;
            }

            is_sync_busy = true;

               
            http.SendBackendTelegram($"{business_branch_name}%0aStart sync data");
            SyncDataToAdminDatabase();

            //run second time for sync some unsync data
            SyncDataToAdminDatabase();

            DeleteOldLogFile();

            is_sync_busy = false;
             
            
        }


        async Task SyncDataToAdminDatabase()
        {
            try
            {
                var data = GetDataforSync();
                if (data.Any())
                {
                    foreach (var r in data)
                    {

                        switch (r.transaction_type.ToString())
                        {
                            case "working_day":
                                await SyncWorkingDayGet(Guid.Parse(r.id));
                                break;
                            case "cash_drawer_amount":
                                await SyncCashDrawerAmountGet(Guid.Parse(r.id.ToString()));
                                break;
                            case "cashier_shift":
                                await SyncCashierShiftGet(Guid.Parse(r.id.ToString()));
                                break;
                            case "sale":
                                await SyncSaleGet(Guid.Parse(r.id.ToString()));
                                break;
                            case "history":
                                await SyncHistory(Guid.Parse(r.id.ToString()));
                                break;
                            case "customer":
                                await SyncCustomer(Guid.Parse(r.id.ToString()));
                                break;
                            case "expense":
                                await SyncExpense(Guid.Parse(r.id.ToString()));
                                break;
                            default:
                                break;
                        }

                        System.Threading.Thread.Sleep(1000);
                    }
                }


                http.SendBackendTelegram($"{ business_branch_name}%0aSync completed");
          



            }
            catch (Exception ex)
            {
                await Task.Factory.StartNew(() => {
                    Log.Error(ex.ToString());
                    http.SendBackendTelegram($"{business_branch_name}%0async data {ex.Message}");
                });
            }

        }


        void DeleteOldLogFile()
        {

            DirectoryInfo dinfo = new DirectoryInfo(path);
            FileInfo[] Files = dinfo.GetFiles("*.*");
            
            foreach(FileInfo f in Files)
            {
                if(f.CreationTime < DateTime.Now.AddMinutes(-1))
                {
                    try
                    {
                        File.Delete(f.FullName);
                    }
                    catch
                    {

                    }
                }
            }

        }
    }
}
