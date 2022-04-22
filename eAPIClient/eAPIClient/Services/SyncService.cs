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
        Task SyncRemoteCustomer();
        Task<bool> SyncAllData();

        void sendSyncRequest();
        void sendSyncRemoteDataRequest();

        void OnCreatedAsync(object sender, FileSystemEventArgs e);
        void OnSyncFromRemoteServerAsync(object sender, FileSystemEventArgs e);

    }
    public class SyncService : ISyncService
    {
        public IConfiguration config { get; }

        private readonly IHttpService http;

        string path = "";
        string business_branch_name = "";
        string business_branch_id = "";

        private readonly IWebHostEnvironment environment;
        public SyncService(IConfiguration _config, IHttpService _http, IWebHostEnvironment environment)
        {

            config = _config;
            http = _http;

            this.environment = environment;
            path = environment.ContentRootPath + "\\logs";
            business_branch_name = _config.GetValue<string>("business_branch_name");
            business_branch_id = _config.GetValue<string>("business_branch_id");
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

        public async void OnSyncFromRemoteServerAsync(object sender, FileSystemEventArgs e)
        {
            await SyncRemoteCustomer();
        }

        bool is_sync_customer_busy = false;
        public async Task SyncRemoteCustomer()
        {
            if (is_sync_customer_busy)
            {
                http.SendBackendTelegram($"{business_branch_name}\nSync Customer busy");
                return;
            }
            is_sync_customer_busy = true;
            try
            {    
                http.SendBackendTelegram($"{business_branch_name}\nStart Sync Customer");    
                List<CustomerBusinessBranchModel> data = new List<CustomerBusinessBranchModel>();
                string query = $"CustomerBusinessBranch?$expand=customer($expand=customer_cards)&$filter=is_synced eq false and business_branch_id eq {config.GetValue<string>("business_branch_id")}";
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
                                var save_to_server_resp = await http.ApiPost("CustomerBusinessBranch/Save", b);
                                if (!save_to_server_resp.IsSuccess)
                                {
                                    http.SendBackendTelegram($"{business_branch_name}\nSync Customer to admin database fail. Data:{JsonSerializer.Serialize(b)}. Error: {save_to_server_resp.Content}");
                                }
                            }
                        }
                    }
                }
                http.SendBackendTelegram($"{business_branch_name}\nSync Customer complete");
                DeleteOldLogFile();

            }
            catch (Exception ex)
            {
                http.SendBackendTelegram($"{business_branch_name}\nSync Customer fail. error message: {ex.Message} \n {ex.ToString()}");
            }
            is_sync_customer_busy = false;
        }





            List<DataForSyncModel> GetDataforSync()
        {

            try
            {
                using (var db = new ApplicationDbContext(config))
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
                using (var db = new ApplicationDbContext(config))
                {
                    Guid _business_branch_id = Guid.Parse(config.GetValue<string>("business_branch_id"));  
                    model.business_branch_id = _business_branch_id;
                    model.last_update_business_branch_id = _business_branch_id;
                    model.is_synced = true;
                    var _modelCheck = db.Customers.Where(r => r.id == model.id).AsNoTracking();
                    //
                    foreach (var cc in model.customer_cards)
                    {
                        db.Entry(cc).State = EntityState.Added;
                        if (cc.id != Guid.Empty)
                        {
                            var _old_sale_product = db.CustomerCards.Where(x => x.id == cc.id).AsNoTracking();
                            if (_old_sale_product.Any())
                            {
                                db.Entry(cc).State = EntityState.Modified;
                            }
                        }
                    }

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

            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString() + "=====" + JsonSerializer.Serialize(model));
                http.SendBackendTelegram($"{business_branch_name}\nSave sync customer fail. Data:{JsonSerializer.Serialize(model)}. error message: {ex.Message}\n{ex.ToString()}");

            }
            return false;
        }


        public async Task<bool> SyncWorkingDayGet(Guid workingDayId)
        {
            try
            {
                using (var db = new ApplicationDbContext(config))
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
                            http.SendBackendTelegram($"{business_branch_name}\nSync working fail.\n Error Detail:{_syncResp.Content.ToString()}.\nData: { JsonSerializer.Serialize(_workingDay)}");
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
                using (var db = new ApplicationDbContext(config))
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
                            http.SendBackendTelegram($"{business_branch_name}\nSync cash drawer fail. Data: { JsonSerializer.Serialize(_model)}");
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
                using (var db = new ApplicationDbContext(config))
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
                            http.SendBackendTelegram($"{business_branch_name}\nSync cashier shift fail. Data: { JsonSerializer.Serialize(_model)}");
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
                using (var db = new ApplicationDbContext(config))
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
                            http.SendBackendTelegram($"{business_branch_name}\nSync sale fail. Data: { JsonSerializer.Serialize(_sale)}");
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
                using (var db = new ApplicationDbContext(config))
                {
                    var _modelData = db.Histories.Where(r => r.id == id)
                     .AsNoTrackingWithIdentityResolution();
                    if (_modelData.Count() > 0)
                    {
                        var _model = _modelData.FirstOrDefault();

                        _model.is_synced = true;
                        var _syncResp = await http.ApiPost("History/Sync", _model);
                        if (!_syncResp.IsSuccess)
                        {
                            string _data = JsonSerializer.Serialize(_model);
                            http.SendBackendTelegram($"{business_branch_name}\nSync history fail. Data: { _data}");
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
                using (var db = new ApplicationDbContext(config))
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

                            http.SendBackendTelegram($"{business_branch_name}\nSync customer fail. Data: { JsonSerializer.Serialize(_model)}");
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
                using (var db = new ApplicationDbContext(config))
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

                            http.SendBackendTelegram($"{business_branch_name}\nSync expense fail. Data: { JsonSerializer.Serialize(_model)}");
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
            }
            catch (Exception ex)
            {
                Log.Error($"Sync expense fail. Ex: {ex.ToString()}");
            }
            return false;
        }
        bool is_sync_all_busy = false;
        public async Task<bool> SyncAllData()
        {
            bool sync_success = false;
            if (is_sync_all_busy)
            {
                http.SendBackendTelegram($"{business_branch_name}\nSync Auto update data from admin db is busy.");

            }
            else
            {
                is_sync_all_busy = true;
                http.SendBackendTelegram($"{business_branch_name}\nStart Auto update data from admin db.");
                await SyncSetting();

                await SyncNote();

                await SyncMenuAndProduct();

                await SyncTranslateText();
                is_sync_all_busy = false;
                http.SendBackendTelegram($"{business_branch_name}\nAuto update data from admin db complete.");

            }


            return sync_success;

        }
        public async Task<bool> SyncSetting()
        {


            http.SendBackendTelegram($"{business_branch_name}\nStart Auto update config data.");

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
                            using (var db = new ApplicationDbContext(config))
                            {
                                var old_config_data = db.ConfigDatas.Where(r => r.is_local_setting == false);
                                db.ConfigDatas.RemoveRange(old_config_data);
                                db.SaveChanges();
                                db.ConfigDatas.AddRange(config_datas.Where(r => r.is_local_setting == false));
                                db.SaveChanges();

                                http.SendBackendTelegram($"{business_branch_name}\nAuto update config data successfully");

                            }


                        }
                        catch (Exception ex)
                        {

                            await Task.Factory.StartNew(() =>
                            {
                                string message = ex.ToString();
                                http.SendBackendTelegram($"{business_branch_name}\nAuto update config data fail\n{ex}");
                            });
                        }

                    }
                    return true;
                }



            }

            return false;

        }

        async Task<bool> SyncMenuAndProduct()
        {
            try
            {
                using (var db = new ApplicationDbContext(config))
                {


                    http.SendBackendTelegram($"{business_branch_name}\nAuto update Menu and Product Start.");



                    db.Database.ExecuteSqlRaw("exec [sp_delete_menu_and_product]");

                    List<MenuModel> menu_datas = await GetRemoteMenu();
                    db.Menus.AddRange(menu_datas);
                    List<ProductModel> product_datas = await GetRemoteProduct();
                    db.Products.AddRange(product_datas);
                    await db.SaveChangesAsync();


                    List<ProductMenuModel> product_menu_datas = await GetRemoteProductMenu();
                    //get product price 
                    List<ProductPriceModel> product_price_datas = await GetRemoteProductPrice();


                    db.ProductMenus.AddRange(product_menu_datas);
                    db.ProductPrices.AddRange(product_price_datas);
                    await db.SaveChangesAsync();

                    db.Database.ExecuteSqlRaw("exec sp_update_product_portion_price");


                    http.SendBackendTelegram($"{business_branch_name}\nAuto update Menu and Product successfully.");

                    return true;
                }
            }
            catch (Exception ex)
            {

                http.SendBackendTelegram($"{business_branch_name}\nAuto update Menu and Product Fail.{ex.ToString()}");
            }
            return false;
        }

        async Task<List<ProductPriceModel>> GetRemoteProductPrice()
        {

            string url = "BusinessBranchProductPrice?";
            url = url + $"&$filter=business_branch_id eq {business_branch_id}";
            var resp = await http.ApiGetOData(url);
            if (resp.IsSuccess)
            {

                return JsonSerializer.Deserialize<List<ProductPriceModel>>(resp.Content.ToString());
            }
            return new List<ProductPriceModel>();
        }

        async Task<List<ProductMenuModel>> GetRemoteProductMenu()
        {

            string url = "ProductMenu?$select=id,product_id,menu_id";
            url = url + "&$filter=is_deleted eq false and  ";
            url = url + "menu/is_deleted eq false  and ";
            url = url + "menu/status eq true ";
            url = url + "and product/is_deleted eq false and ";
            url = url + "product/status eq true and product/is_menu_product eq true and ";
            url = url + $"menu/business_branch_id eq {business_branch_id}";

            var resp = await http.ApiGetOData(url);
            if (resp.IsSuccess)
            {

                return JsonSerializer.Deserialize<List<ProductMenuModel>>(resp.Content.ToString());
            }
            return new List<ProductMenuModel>();
        }


        async Task<List<MenuModel>> GetRemoteMenu()
        {

            var resp = await http.ApiGetOData($"Menu?$select=id,parent_id,menu_name_en,menu_name_kh,text_color,background_color,root_menu_id,is_shortcut_menu&$filter=business_branch_id eq {business_branch_id} and is_deleted eq false and status eq true");
            if (resp.IsSuccess)
            {

                return JsonSerializer.Deserialize<List<MenuModel>>(resp.Content.ToString());
            }
            return new List<MenuModel>();
        }

        async Task<List<ProductModel>> GetRemoteProduct()
        {
            List<ProductModel> _products = new List<ProductModel>();


            string _select_product_modifier = "$select=id,parent_id,product_id,modifier_name,price,section_name,is_required,is_multiple_select,is_section,sort_order,modifier_id";

            string url = $"product?$select=revenue_group_name,product_group_id,product_tax_value,product_category_id,product_category_en,product_category_kh,id,is_open_product,";
            url += "product_code,product_name_en,product_name_kh,photo,note,is_allow_discount,is_allow_change_price,is_allow_free,is_open_product,is_inventory_product,kitchen_group_name,kitchen_group_sort_order";
            url += $"&$expand=product_printers($select=id,product_id,printer_name,ip_address,port,group_item_type_id;$filter=is_deleted eq false and printer/business_branch_id eq {business_branch_id}),";
            url += $"product_modifiers({_select_product_modifier};$expand=children({_select_product_modifier};$filter=is_deleted eq false);$filter=is_deleted eq false),";
            url += $"product_portions($select=id,product_id, portion_name,cost,multiplier,unit_id;$filter=is_deleted eq false)";
            url += "&$filter=is_deleted eq false and status eq true and is_menu_product eq true";
            var resp = await http.ApiGetOData(url);
            if (resp.IsSuccess)
            {


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
        async Task<bool> SyncNote()
        {
            http.SendBackendTelegram($"{business_branch_name}\nStart Auto update note from admin db");
            try
            {
                using (var db = new ApplicationDbContext(config))
                {

                    string _query = $"CategoryNote?$select=id,category_note_name_en,category_note_name_kh,is_multiple_select&$expand=notes($filter=is_deleted eq false and business_branch_id eq {business_branch_id})";
                    var resp = await http.ApiGetOData(_query);
                    if (resp.IsSuccess)
                    {

                        List<CategoryNoteModel> _tempNewCategoryNotes = new List<CategoryNoteModel>();
                        List<CategoryNoteModel> _tempAppendCategoryNotes = new List<CategoryNoteModel>();
                        List<NoteModel> _tempNewNotes = new List<NoteModel>();

                        List<CategoryNoteModel> _LocalCategoryNotes = new List<CategoryNoteModel>();


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
                                category_note_name_en = c.category_note_name_en,
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


                        db.CategoryNotes.RemoveRange(db.CategoryNotes);
                        await db.SaveChangesAsync();

                        if (_tempNewCategoryNotes.Count() > 0)
                        {
                            db.CategoryNotes.AddRange(_tempNewCategoryNotes);
                        }
                        if (_tempAppendCategoryNotes.Count() > 0)
                        {
                            db.CategoryNotes.UpdateRange(_tempAppendCategoryNotes);
                        }
                        await db.SaveChangesAsync();


                        var remote_note = JsonSerializer.Deserialize<List<NoteModel>>(JsonSerializer.Serialize(data.SelectMany(r => r.notes).ToList()));

                        //clear old note 
                        db.Notes.RemoveRange(db.Notes.Where(r => r.is_predefine_note == true));
                        await db.SaveChangesAsync();
                        remote_note.ForEach(r => r.is_predefine_note = true);
                        db.Notes.AddRange(remote_note);

                        await db.SaveChangesAsync();
                        http.SendBackendTelegram($"{business_branch_name}\nAuto update note from admin db successfully");
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                http.SendBackendTelegram($"{business_branch_name}\nAuto update note from admin db fail. {ex.ToString()}");
            }
            return false;
        }
        async Task<bool> SyncTranslateText()
        {
            try
            {
                using (var db = new ApplicationDbContext(config))
                {

                    List<eShareModel.TranslateTextModel> translate_texts = await GetTranslateText();
                    if (translate_texts.Any())
                    {
                        db.Database.ExecuteSqlRaw("delete tbl_translate_text");
                        db.TranslateTexts.AddRange(translate_texts);
                        db.SaveChanges();
                        http.SendBackendTelegram($"{business_branch_name}\nAuto update translate text complete");
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                http.SendBackendTelegram($"{business_branch_name}\nAuto update translate text. {ex.ToString()}");
            }

            return false;
        }

        async Task<List<eShareModel.TranslateTextModel>> GetTranslateText()
        {

            var resp = await http.ApiGet("GetTranslateText");
            if (resp.IsSuccess)
            {

                return JsonSerializer.Deserialize<List<eShareModel.TranslateTextModel>>(resp.Content.ToString());
            }
            return new List<eShareModel.TranslateTextModel>();
        }

        public void sendHistoryAlertTelegram(HistoryModel model)
        {

            string messaage = $"{model.title}\n{model.description}";
            if (!string.IsNullOrEmpty(model.note))
            {
                messaage = messaage + $"\nNote: {model.note}";
            }
            messaage = messaage + $"\n-------------------------";
            messaage = messaage + $"\nBy: {model.created_by} on {model.created_date.ToString("dd/MM/yyyy hh:mm:ss tt")} ";


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
        async void ISyncService.OnCreatedAsync(object sender, FileSystemEventArgs e)
        {

            if (is_sync_busy)
            {
                http.SendBackendTelegram($"{business_branch_name}\nSync process  is busy.");


                return;
            }

            is_sync_busy = true;


            http.SendBackendTelegram($"{business_branch_name}\nStart sync data");
            await SyncDataToAdminDatabase();

            //run second time for sync some unsync data
            await SyncDataToAdminDatabase();

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


                http.SendBackendTelegram($"{ business_branch_name}\nSync completed");




            }
            catch (Exception ex)
            {
                await Task.Factory.StartNew(() => {
                    Log.Error(ex.ToString());
                    http.SendBackendTelegram($"{business_branch_name}\nsync data {ex.Message}");
                });
            }

        }


        void DeleteOldLogFile()
        {

            DirectoryInfo dinfo = new DirectoryInfo(path);
            FileInfo[] Files = dinfo.GetFiles("*.*");

            foreach (FileInfo f in Files)
            {
                if (f.CreationTime < DateTime.Now.AddMinutes(-1))
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