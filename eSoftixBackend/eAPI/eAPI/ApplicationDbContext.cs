using System;
using Microsoft.EntityFrameworkCore;

using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eModels;
using eShareModel;

namespace eAPI
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            //
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            foreach (var property in builder.Model.GetEntityTypes().SelectMany(t => t.GetProperties()).Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(16,4)");
            }

            foreach (var property in builder.Model.GetEntityTypes().SelectMany(t => t.GetProperties()).Where(p => p.ClrType == typeof(string)))
            {
                property.SetCollation("Khmer_100_BIN");
            }


            builder.Entity<PermissionOptionRoleModel>().HasKey(t => new { t.role_id, t.permission_option_id });
        

            builder.Entity<PermissionOptionRoleModel>()
                .HasOne(pt => pt.permission_option)
                .WithMany(p => p.permission_option_roles)
                .HasForeignKey(pt => pt.permission_option_id);

            builder.Entity<BusinessBranchSystemFeatureModel>().HasKey(t => new { t.business_branch_id, t.system_feature_id });
          




            builder.Entity<ProjectSystemFeatureModel>().HasKey(t => new { t.project_id, t.system_feature_id });
   


            builder.Entity<StoreProcedureResultModel>().HasNoKey();

            builder.Entity<NumberModel>().HasNoKey();
        }

        public DbSet<StoreProcedureResultModel> StoreProcedureResult { get; set; }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<RoleModel> Roles { get; set; }
        public DbSet<PermissionOptionModel> PermissionOption { get; set; }
        public DbSet<PermissionOptionRoleModel> PermissionOptionRole { get; set; }
        public DbSet<StoreProcedureResultModel> StoreProcedureResults { get; set; }

        public DbSet<DocumentNumberModel> DocumentNumbers { get; set; }
        public DbSet<CurrencyModel> Currencies { get; set; }
        public DbSet<PaymentTypeModel> PaymentTypes { get; set; }
        public DbSet<CustomerModel> Customers { get; set; }
        public DbSet<CustomerGroupModel> CustomerGroups { get; set; }
        public DbSet<HistoryModel> Histories { get; set; }
        public DbSet<CountryModel> Countries { get; set; }
        public DbSet<SettingModel> Settings { get; set; }
        public DbSet<NumberModel> Numbers { get; set; }
        public DbSet<ModuleViewModel> ModuleViews { get; set; }
        public DbSet<AttachFilesModel> AttachFiles { get; set; }
        public DbSet<OutletModel> Outlets { get; set; }
        public DbSet<StockLocationModel> StockLocations { get; set; }
        public DbSet<BusinessBranchModel> BusinessBranches { get; set; }

        public DbSet<NoteModel> Notes { get; set; }
        public DbSet<ContactModel> Contacts { get; set; }

        public DbSet<StationModel> Stations { get; set; }



        public DbSet<RequestLicenseModel> RequestLicenses { get; set; }
        public DbSet<ProjectModel> Project { get; set; }
        public DbSet<ProjectTypeModel> ProjectType { get; set; }
        public DbSet<MonthTextModel> MonthTexts { get; set; }
        public DbSet<BusinessBranchSystemFeatureModel> BusinessBranchSystemFeatures { get; set; }
        public DbSet<SystemFeatureModel> SystemFeatures { get; set; }
        public DbSet<ProjectSystemFeatureModel> ProjectSystemFeatures { get; set; }


    }

}