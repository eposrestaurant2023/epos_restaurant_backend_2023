using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eAPI.Hubs;
using eModels;
using Microsoft.AspNetCore.SignalR;

namespace eAPI.Services
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
        public  string  GetProductDocumentNumber(DocumentNumberModel dc)
        {

        generate_code:
            string code = "";
            if (string.IsNullOrEmpty(dc.format))
            {
                code = string.Format("{0}{1:" + dc.counter_digit + "}", dc.prefix, dc.counter + 1);
            }
            else
            {
                code = string.Format("{0}{1:" + dc.format + "}{2:" + dc.counter_digit + "}", dc.prefix, DateTime.Now, dc.counter + 1);
            }
            //check with database
            var data = db.Products.Where(r => r.product_code.Trim().ToLower() == code.ToLower().Trim()).ToList();
            if (data.Any())
            {
                dc.counter = dc.counter + 1;
                goto generate_code;
            }
            dc.counter = dc.counter + 1;
            return code;
        }
     
        public async Task SaveDocumentNumber(int id)
        {
            DocumentNumberModel dc = await db.DocumentNumbers.FindAsync(id);
            dc.counter = dc.counter + 1;
            db.DocumentNumbers.Update(dc);
            await db.SaveChangesAsync();
        }


        public async Task AddInventoryTransaction(InventoryTransactionModel item)
        {

            db.InventoryTransactions.Add(item);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
    }
}
