////using eAPIClient.Models;using eModels;
using eModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace eAPIClient.Controllers
{
    public static class SaveChange
    {
        public static async Task SaveAsync(ApplicationDbContext db, int user_id)
        {
            UserModel user = await db.Users.FindAsync(user_id);

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

            await db.SaveChangesAsync();
        }

        public static void Save(ApplicationDbContext db, int user_id)
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
                }
            }
            db.SaveChanges();
        }
    }
}
