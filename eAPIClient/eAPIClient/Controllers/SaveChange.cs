using eAPIClient.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using eAPIClient.Services;
using System.Collections.Generic;
using System.Text.Json;
using eShareModel;

namespace eAPIClient.Controllers
{
    public static class SaveChange
    {

       
        public static async Task SaveAsync(ApplicationDbContext db, int user_id)
        { 
            var config = db.ConfigDatas.Where(r=>r.config_type=="user").FirstOrDefault();
            UserModel user = new UserModel();
            if (config != null)
            {
                List<UserModel> users = JsonSerializer.Deserialize<List<UserModel>>(config.data);
                user = users.Where(r => r.id == user_id).FirstOrDefault();
            }

            if (user != null)
            {
                //entity add 
                foreach (var item in db.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added && (e.Entity is Core)))
                {
                 
                    var entidad = item.Entity as Core;
                    entidad.created_date = DateTime.Now;

                    entidad.created_by = string.IsNullOrEmpty(entidad.created_by) ? user.username : entidad.created_by;


                    entidad.last_modified_by = entidad.created_by;
                    entidad.last_modified_date = DateTime.Now;
                }

                //entity updated or modified
                foreach (var item in db.ChangeTracker.Entries()
                    .Where(e => e.State == EntityState.Modified && e.Entity is Core ))
                {
                    var entidad = item.Entity as Core; 
                    if (entidad.is_deleted == true)
                    {
                        entidad.deleted_date = DateTime.Now;
                        entidad.deleted_by = string.IsNullOrEmpty(entidad.deleted_by) ? user.username : entidad.deleted_by;
                    }

                    entidad.last_modified_by = user.username;
                    entidad.last_modified_date = DateTime.Now;
                }

                //entity of working day modified     
                foreach (var item in db.ChangeTracker.Entries()
                    .Where(e => e.State == EntityState.Modified && (  e.Entity is WorkingDayModel)))
                {
                    var entidad = item.Entity as WorkingDayModel;     
                    if (entidad.is_closed == true)
                    {
                        entidad.closed_date = DateTime.Now;
                        entidad.closed_by = string.IsNullOrEmpty(entidad.closed_by) ? user.username : entidad.closed_by;
                    }
                }

                //entity of cashier shift modified     
                foreach (var item in db.ChangeTracker.Entries()
                    .Where(e => e.State == EntityState.Modified && (e.Entity is CashierShiftModel)))
                {
                    var entidad = item.Entity as CashierShiftModel;
                    if (entidad.is_closed == true)
                    {
                        entidad.closed_date = DateTime.Now;
                        entidad.closed_by = string.IsNullOrEmpty(entidad.closed_by) ? user.username : entidad.closed_by;
                    }
                }



                //entity of sale modified     
                foreach (var item in db.ChangeTracker.Entries()
               .Where(e => e.State == EntityState.Modified && (e.Entity is SaleModel)))
                {
                    var entidad = item.Entity as SaleModel;
                    if (entidad.is_closed == true)
                    {
                        entidad.closed_date = DateTime.Now;
                        entidad.closed_by = string.IsNullOrEmpty(entidad.closed_by) ? user.username : entidad.closed_by;
                    }
                }  
            }   
            await db.SaveChangesAsync();
        }

        public static void Save(ApplicationDbContext db, int user_id)
        {
            var config = db.ConfigDatas.Where(r => r.config_type == "user").FirstOrDefault();
            UserModel user = new UserModel();
            if (config != null)
            {
                List<UserModel> users = JsonSerializer.Deserialize<List<UserModel>>(config.data);
                user = users.Where(r => r.id == user_id).FirstOrDefault();
            }

            if (user != null)
            {

                //on entity add base on core 
                foreach (var item in db.ChangeTracker.Entries()
                    .Where(e => e.State == EntityState.Added && e.Entity is Core))
                {
                    var entidad = item.Entity as Core;
                    entidad.created_date = DateTime.Now;
                    entidad.created_by = string.IsNullOrEmpty(entidad.created_by) ? user.username : entidad.created_by;

                    entidad.last_modified_by = user.full_name;
                    entidad.last_modified_date = DateTime.Now;
                }
                //on entity modified base on core 
                foreach (var item in db.ChangeTracker.Entries()
                    .Where(e => e.State == EntityState.Modified && e.Entity is Core))
                {
                    var entidad = item.Entity as Core;
                    if (entidad.is_deleted == true)
                    {
                        entidad.deleted_date = DateTime.Now;
                        entidad.deleted_by = string.IsNullOrEmpty(entidad.deleted_by) ? user.username : entidad.deleted_by;
                    }

                    entidad.last_modified_by = user.full_name;
                    entidad.last_modified_date = DateTime.Now;
                }

                //on entity modified base on working day 
                foreach (var item in db.ChangeTracker.Entries()
               .Where(e => e.State == EntityState.Modified && (e.Entity is WorkingDayModel)))
                {
                    var entidad = item.Entity as WorkingDayModel;
                    if (entidad.is_closed == true)
                    {
                        entidad.closed_date = DateTime.Now;
                        entidad.closed_by = string.IsNullOrEmpty(entidad.closed_by) ? user.username : entidad.closed_by;
                    }
                }

                //on entity modified base on cashier shift 
                foreach (var item in db.ChangeTracker.Entries()
               .Where(e => e.State == EntityState.Modified && (e.Entity is CashierShiftModel)))
                {
                    var entidad = item.Entity as CashierShiftModel;
                    if (entidad.is_closed == true)
                    {
                        entidad.closed_date = DateTime.Now;
                        entidad.closed_by = string.IsNullOrEmpty(entidad.closed_by) ? user.username : entidad.closed_by;
                    }
                }


                //entity of sale modified     
                foreach (var item in db.ChangeTracker.Entries()
               .Where(e => e.State == EntityState.Modified && (e.Entity is SaleModel)))
                {
                    var entidad = item.Entity as SaleModel;
                    if (entidad.is_closed == true)
                    {
                        entidad.closed_date = DateTime.Now;
                        entidad.closed_by = string.IsNullOrEmpty(entidad.closed_by) ? user.username : entidad.closed_by;
                    }  
                }
            }
            db.SaveChanges();
        }
    
    }


}
