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
             

        public string GetDocumentFormat(DocumentNumberModel _doc)
        {
            if (string.IsNullOrEmpty(_doc.format))
            {
                return string.Format(@"{0}{1:" + _doc.counter_digit + "}", _doc.prefix, _doc.counter);
            }
            return string.Format(@"{0}{1:" + _doc.format + "}{2:" + _doc.counter_digit + "}", _doc.prefix, DateTime.Now, _doc.counter);
        } 

        public DocumentNumberModel GetDocument(string type, string outlet_id)
        {

            DocumentNumberModel _doc = new DocumentNumberModel();       
            var _find = (from r in db.DocumentNumbers
                         where r.document_name == type &&
                               EF.Functions.Like(
                                   (
                                      (r.outlet_id ?? " ")
                                   ).ToLower().Trim(), $"%{outlet_id}%".ToLower().Trim())
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

    }
}
