using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using eAPIClient.Models;
using Microsoft.EntityFrameworkCore;

namespace eAPIClient.Services
{

    public class AppService
    {
        private readonly ApplicationDbContext db;
        public AppService(ApplicationDbContext _db)
        {
            db = _db;

        }

        public async Task<string> GetDocumentNumber(int id)
        {
            DocumentNumberModel dc = await db.DocumentNumbers.FindAsync(id);
            dc.counter = dc.counter + 1;
            if (string.IsNullOrEmpty(dc.format))
            {
                return string.Format("{0}{1:" + dc.counter_digit + "}", dc.prefix, dc.counter);
            }
            else
            {
                return string.Format("{0}{1:" + dc.format + "}{2:" + dc.counter_digit + "}", dc.prefix, DateTime.Now, dc.counter);
            }
        } 
        public async Task<string> GetDocumentNumber(DocumentNumberModel dc)
        {
            await Task.Delay(10);
            dc.counter = dc.counter + 1;
            if (string.IsNullOrEmpty(dc.format))
            {
                return string.Format("{0}{1:" + dc.counter_digit + "}", dc.prefix, dc.counter);
            }
            else
            {
                return string.Format("{0}{1:" + dc.format + "}{2:" + dc.counter_digit + "}", dc.prefix, DateTime.Now, dc.counter);
            }
        } 
    
        public async Task SaveDocumentNumber(int id)
        {
            DocumentNumberModel dc = await db.DocumentNumbers.FindAsync(id);
            dc.counter = dc.counter + 1;
            db.DocumentNumbers.Update(dc);
            await db.SaveChangesAsync();
        }

       public async Task<string> GenerateDocumentNumber(string type, string outlet_id)
        {

            var _doc = (from r in db.DocumentNumbers
                        where r.document_name == type &&
                              EF.Functions.Like(
                                  (
                                     (r.outlet_id ?? " ")
                                  ).ToLower().Trim(), $"%{outlet_id}%".ToLower().Trim())
                        select r);
            string _result = "";
            if (_doc.Count() > 0)
            {
                var _d = _doc.FirstOrDefault();
                _d.counter += 1;
                db.DocumentNumbers.Update(_d);
                await db.SaveChangesAsync();

                if (string.IsNullOrEmpty(_d.format))
                {
                    _result = string.Format(@"{0}{1:" + _d.counter_digit + "}", _d.prefix, _d.counter);
                }
                else
                {
                    _result = string.Format(@"{0}{1:" + _d.format + "}{2:" + _d.counter_digit + "}", _d.prefix, DateTime.Now, _d.counter);
                }

            }
            return _result;
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

    }
}
