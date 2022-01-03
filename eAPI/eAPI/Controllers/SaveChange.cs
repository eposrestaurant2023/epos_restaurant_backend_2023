using eModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using eShareModel;
using Microsoft.AspNetCore.SignalR;
using eAPI.Hubs;
using System.Collections.Generic;

namespace eAPI.Controllers
{
    public static class SaveChange
    {
        public static Dictionary<string,string> data { get; set; }

        public static async Task SaveAsync(ApplicationDbContext db, int user_id, IHubContext<ConnectionHub> hub)
        {
            UserModel user = await db.Users.FindAsync(user_id);

            if (user != null)
            {

                foreach (var item in db.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added && e.Entity is Core))
                {
                    var entidad = item.Entity as Core;
                    entidad.created_date = DateTime.Now;
                    entidad.created_by = user.full_name;

                    entidad.last_modified_date = DateTime.Now;
                    entidad.last_modified_by = user.full_name;
                }


                foreach (var item in db.ChangeTracker.Entries()
                    .Where(e => e.State == EntityState.Modified && e.Entity is Core))
                {
                    var entidad = item.Entity as Core;
                    if (entidad.is_deleted == true)
                    {
                        entidad.deleted_date = DateTime.Now;
                        entidad.deleted_by = user.full_name;
                    }

                    entidad.last_modified_date = DateTime.Now;
                    entidad.last_modified_by = user.full_name;
                }



            }

            await db.SaveChangesAsync();
            foreach (var item in db.ChangeTracker.Entries() )
            {
                switch (item.Entity)
                {
                    case UserModel :
                    case UnitModel:
                        await hub.Clients.All.SendAsync("Sync", "setting");
                        break;
                    default:
                        break;
                }
            }
                
            
        }

        public static void Save(ApplicationDbContext db, int user_id, IHubContext<ConnectionHub> hub)
        {
            UserModel user = db.Users.Find(user_id);
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

                    entidad.last_modified_date = DateTime.Now;
                    entidad.last_modified_by = user.full_name;
                }
            }
            db.SaveChanges();
        }
    }
}
