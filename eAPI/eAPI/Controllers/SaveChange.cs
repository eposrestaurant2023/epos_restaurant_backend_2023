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
    
    

        static void SendUpdateToClient(ApplicationDbContext db, IHubContext<ConnectionHub> hub)
        {
            foreach (var item in db.ChangeTracker.Entries())
            {
                switch (item.Entity)
                {
                    case UserModel:
                    case PermissionOptionModel:
                    case CurrencyModel:
                    case BusinessBranchModel:
                    case BusinessBranchCurrencyModel:
                    case UnitCategoryModel:
                    case ExpenseCategoryModel:
                    case SettingModel:
                    case OutletModel:
                    case StationModel:
                    case TableGroupModel:
                    case TableModel:
                    case UnitModel:
                    case ProductGroupModel:
                    case PriceRuleModel:
                    case DiscountCodeModel:
                    case SystemFeatureModel:
                    case SaleTypeModel:
                    case CustomerGroupModel:
                    case PaymentTypeModel:
                        hub.Clients.All.SendAsync("Sync", "setting");
                        break;
                    default:
                        break;
                }
            }
        }
    
    }
}
