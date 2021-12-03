using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using eAPIClient.Controllers;
using eAPIClient.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace eAPIClient.Services
{

    public class AppService
    {
        public IConfiguration config { get; }
        private readonly ApplicationDbContext db;
        public AppService(ApplicationDbContext _db, IConfiguration _config)
        {
            db = _db;
            config = _config;
        }          
             

        public string GetDocumentFormat(DocumentNumberModel _doc)
        {
            if (string.IsNullOrEmpty(_doc.format))
            {
                return string.Format(@"{0}{1:" + _doc.counter_digit + "}", _doc.prefix, _doc.counter);
            }
            return string.Format(@"{0}{1:" + _doc.format + "}{2:" + _doc.counter_digit + "}", _doc.prefix, DateTime.Now, _doc.counter);
        } 

        public DocumentNumberModel GetDocument(string type, string cash_drawer_id)
        {

            DocumentNumberModel _doc = new DocumentNumberModel();       
            var _find = (from r in db.DocumentNumbers
                         where r.document_name == type &&
                               EF.Functions.Like(
                                   (
                                      (r.cash_drawer_id ?? " ")
                                   ).ToLower().Trim(), $"%{cash_drawer_id}%".ToLower().Trim())
                         select r);
            if (_find.Count() > 0)
            {
                _doc = _find.FirstOrDefault();
                _doc.counter += 1;
            }  

            return _doc;
        }

        public async Task UpdateDocument(DocumentNumberModel _doc)
        {
            if (_doc.id > 0)
            {
                db.DocumentNumbers.Update(_doc);
                await db.SaveChangesAsync();
            }
        }
        public async Task<List<UserModel>> AllUsers()
        {

            await Task.Delay(100);
            var config = db.ConfigDatas.Where(r=>r.config_type=="user" ).FirstOrDefault();
            if (config != null)
            {
                List<UserModel> users = JsonSerializer.Deserialize<List<UserModel>>(config.data);
                return users;

            }
            return new List<UserModel>();
        }
        public   bool  IsSystemHasFeature(string feature_code)
        {

            
            var config = db.ConfigDatas.Where(r=>r.config_type=="system_feature" ).FirstOrDefault();
            if (config != null)
            {
                List<SystemFeatureModel> system_features= JsonSerializer.Deserialize<List<SystemFeatureModel>>(config.data);

                if (system_features.Where(r => r.feature_code == feature_code).Any()){
                    return true;
                }

            }

            return false;
        }

        public  List< OutletModel> AllOutlets()
        {


            var config = db.ConfigDatas.Where(r => r.config_type == "outlet_config").FirstOrDefault();
            if (config != null)
            {
                List<OutletModel> outlets = JsonSerializer.Deserialize<List<OutletModel>>(config.data);

                return outlets;

            }

            return new List<OutletModel>();
        }
        public BusinessBranchModel GetBusinessBranch()
        {

            var config = db.ConfigDatas.Where(r => r.config_type == "business_branch").FirstOrDefault();
            if (config != null)
            {
                var data  = JsonSerializer.Deserialize<List<BusinessBranchModel>>(config.data);
                if (data.Any())
                {
                    return data.FirstOrDefault();

                }

            }
            return null;


        }
          public OutletModel GetOutletInfo(Guid id)
        {

            var data = AllOutlets().Where(r => r.id == id);
            if (data.Any())
            {
                return data.FirstOrDefault();
            }
            return null;


        }

        public StationModel GetStationInfo(Guid id)
        {

            var data = AllOutlets().ToList().SelectMany(r => r.stations).ToList();
            if (data.Any())
            {
                return data.Where(r => r.id == id).FirstOrDefault();
            }
            return null;
            
        
        }


        public void sendSyncRequest()
        {

            string path = config.GetValue<string>("sync_request_part"); ;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            // Write the specified text asynchronously to a new file named "WriteTextAsync.txt".
            System.IO.File.Create(Path.Combine(path, $"{Guid.NewGuid()}.txt"));
        }
        public void sendPrintRequest(PrintRequestModel model)
        {

            string path = config.GetValue<string>("print_request_path"); ;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            // Write the specified text asynchronously to a new file named "WriteTextAsync.txt".
            System.IO.File.WriteAllText(Path.Combine(path, $"{Guid.NewGuid()}.json"),JsonSerializer.Serialize(model));
        }

        public async Task<WorkingDayModel> GetWorkingDayInfor(WorkingDayModel model, int UserID)
        {

            DocumentNumberModel doc = new DocumentNumberModel();
            var data = db.WorkingDays.Where(r => r.cash_drawer_id == model.cash_drawer_id && r.is_closed == false);
            if (data.Any())
            {
                 var  d = data.FirstOrDefault();
                if (d.working_date == DateTime.Now.Date)
                {
                    return d;
                }else
                {
                    d.is_closed = true;
                    d.is_synced = false;
                    db.WorkingDays.Update(d);

                }
            }
             


            doc = GetDocument("WorkingDayNum", model.cash_drawer_id.ToString());
            model.working_day_number = GetDocumentFormat(doc);
            model.working_date = DateTime.Now;
            db.WorkingDays.Add(model);
             

            //Update Document Number
            if (doc.id == 0)
            {
                await UpdateDocument(doc);
            }
            await SaveChange.SaveAsync(db, UserID);

            sendSyncRequest();

            return model;
        }

        public  bool  IsWorkingDayExist(Guid? id)
        {
            if (id == null)
            {
                return false;
            }
            var data = db.WorkingDays.Where(r => r.id == id);
            return data.Any();
        }
        public  bool  IsCashierShiftExist(Guid? id)
        {
            if (id == null)
            {
                return false;
            }
            var data = db.CashierShifts.Where(r => r.id == id);
            return data.Any();
        }


        public async Task<CashierShiftModel> GetCashierShiftInfo(CashierShiftModel model, int UserID)
        {

            DocumentNumberModel doc = new DocumentNumberModel();
            var data = db.CashierShifts.Where(r => r.cash_drawer_id == model.cash_drawer_id && r.is_closed == false);
            if (data.Any())
            {
                var d = data.FirstOrDefault();
                if (d.working_date == DateTime.Now.Date)
                {
                    return d;
                }
                else
                {
                    d.is_closed = true;
                    db.CashierShifts.Update(d);

                }
            }
             

            doc = GetDocument("CashierShiftNum", model.cash_drawer_id.ToString());
            model.cashier_shift_number = GetDocumentFormat(doc);
            model.working_date = DateTime.Now;
            db.CashierShifts.Add(model);

            


          
            await SaveChange.SaveAsync(db, UserID);
            //Update Document Number
            if (doc.id == 0)
            {
                await UpdateDocument(doc);
            }

            db.Database.ExecuteSqlRaw($"exec sp_update_cashier_shift_information '{model.id}'");

            sendSyncRequest();

            return model;
        }


    }
}
