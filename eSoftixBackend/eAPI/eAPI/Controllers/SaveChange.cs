using eModels;
using eShareModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace eAPI.Controllers
{
    public static class SaveChange
    {
        public static async Task SaveAsync(ApplicationDbContext db, int user_id)
        {
            UserModel user = await db.Users.FindAsync(user_id);

            if (user != null)
            {
                foreach (var item in db.ChangeTracker.Entries().Where(e => e.State == EntityState.Added && (e.Entity is Core || e.Entity is CoreNoDeleted)))
                {
                    var entidad = item.Entity as Core;
                    var entinodeleted = item.Entity as CoreNoDeleted;
                    
                    if (entidad != null)
                    {
                        entidad.created_date = DateTime.Now;
                        entidad.created_by = user.full_name;
                        entidad.last_modified_date = DateTime.Now;
                        entidad.last_modified_by = user.full_name;
                    }

                    if (entinodeleted != null)
                    {
                        entinodeleted.created_date = DateTime.Now;
                        entinodeleted.created_by = user.full_name;
                    }
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
                    entidad.last_modified_by= user.full_name;

                }

            }

            await db.SaveChangesAsync();
        }

        public static void Save(ApplicationDbContext db, int user_id)
        {
            UserModel user = db.Users.Find(user_id);
            if (user != null)
            {
                foreach (var item in db.ChangeTracker.Entries()
                    .Where(e => e.State == EntityState.Added && (e.Entity is Core || e.Entity is CoreNoDeleted)))
                {
                    var entidad = item.Entity as Core;
                    var entinodeleted = item.Entity as CoreNoDeleted;
                    
                    if (entidad != null)
                    {
                        entidad.created_date = DateTime.Now;
                        entidad.created_by = user.full_name;
                    }
                    if (entinodeleted != null)
                    {
                        entinodeleted.created_date = DateTime.Now;
                        entinodeleted.created_by = user.full_name;
                    }

                   
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
                }
            }
            db.SaveChanges();
        }
    }
}
