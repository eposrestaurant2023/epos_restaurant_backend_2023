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

                foreach (var item in db.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added && (e.Entity is Core)))
                {
                 
                    var entidad = item.Entity as Core;
                    entidad.created_date = DateTime.Now;
                    entidad.created_by = user.full_name;
                }




                foreach (var item in db.ChangeTracker.Entries()
                    .Where(e => e.State == EntityState.Modified && e.Entity is Core ))
                {
                    var entidad = item.Entity as Core; 
                    if (entidad.is_deleted == true)
                    {
                        entidad.deleted_date = DateTime.Now;
                        entidad.deleted_by = user.full_name;
                    }

                   
                }
                foreach (var item in db.ChangeTracker.Entries()
                    .Where(e => e.State == EntityState.Modified && (  e.Entity is WorkingDayModel)))
                {
                    var entidad = item.Entity as WorkingDayModel;

                    if (entidad.is_closed == true)
                    {
                        entidad.closed_date = DateTime.Now;
                        entidad.closed_by = user.full_name;
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
                foreach (var item in db.ChangeTracker.Entries()
                    .Where(e => e.State == EntityState.Added && e.Entity is CoreModel))
                {
                    var entidad = item.Entity as CoreModel;
                    entidad.created_date = DateTime.Now;
                    entidad.created_by = user.full_name;
                }

                foreach (var item in db.ChangeTracker.Entries()
                    .Where(e => e.State == EntityState.Modified && e.Entity is CoreModel))
                {
                    var entidad = item.Entity as CoreModel;
                    if (entidad.is_deleted == true)
                    {
                        entidad.deleted_date = DateTime.Now;
                        entidad.deleted_by = user.full_name;
                    }
                }
            }
            db.SaveChanges();
        }
    
    }


}
