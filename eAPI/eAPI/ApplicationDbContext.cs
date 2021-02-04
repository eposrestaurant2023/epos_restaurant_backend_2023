using System;
using Microsoft.EntityFrameworkCore;

using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eModels;

namespace eAPI
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            //xxx

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
            builder.Entity<PermissionOptionRoleModel>().HasKey(t => new { t.role_id, t.permission_option_id });              
            builder.Entity<PermissionOptionRoleModel>()
                .HasOne(pt => pt.permission_option)
                .WithMany(p => p.permission_option_roles)
                .HasForeignKey(pt => pt.permission_option_id);                   
            builder.Entity<CustomerBusinessBranchModel>().HasKey(t => new { t.customer_id, t.business_branch_id});
            builder.Entity<BusinessBranchPaymentTypeModel>().HasKey(t => new { t.payment_type_id, t.business_branch_id });

            builder.Entity<BusinessBranchPaymentTypeModel>().HasKey(t => new { t.payment_type_id, t.business_branch_id });
            builder.Entity<BusinessBranchPriceRule>().HasKey(t => new { t.business_branch_id, t.price_rule_id });
            builder.Entity<OutletStationModel>().HasKey(t => new { t.station_id, t.outlet_id});
            builder.Entity<UserBusinessBranchModel>().HasKey(t => new { t.user_id, t.business_branch_id});              
            builder.Entity<TableGroupScreenModel>().HasKey(t => new { t.table_group_id, t.outlet_id,t.station_id });
            builder.Entity<StoreProcedureResultModel>().HasNoKey();
            builder.Entity<StoreProcedureResultDecimalModel>().HasNoKey();                
            builder.Entity<NumberModel>().HasNoKey();
        
        }


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
        public DbSet<BusinessBranchModel> BusinessBranches { get; set; }
        public DbSet<CategoryNoteModel> CategoryNotes{ get; set; }
        public DbSet<CustomerBusinessBranchModel> CustomerBusinessBranches{ get; set; }
        public DbSet<NoteModel> Notes{ get; set; }
        public DbSet<OutletStationModel> OutletStations{ get; set; }
        public DbSet<PrinterModel> Printers{ get; set; }
        public DbSet<StationModel> Stations{ get; set; }
        public DbSet<TableGroupModel> TableGroups{ get; set; }
        public DbSet<TableGroupScreenModel> TableGroupScreens{ get; set; }
        public DbSet<TableModel> Tables{ get; set; }
        public DbSet<UserBusinessBranchModel> UserBusinessBranches{ get; set; }
        public DbSet<ProductGroupModel> ProductGroups { get; set; }
        public DbSet<ProductCategoryModel> ProductCategories{ get; set; }
        public DbSet<ProductModel> Products{ get; set; }
        public DbSet<ProductTypeModel> ProductTypes{ get; set; }          
        public DbSet<StockLocationModel> StockLocations{ get; set; }
        public DbSet<VendorModel> Vendors{ get; set; }
        public DbSet<BusinessInformationModel> BusinessInformations{ get; set; }
        public DbSet<StoreProcedureResultDecimalModel> StoreProcedureResultsDecimal { get; set; }
        public DbSet<BusinessBranchPaymentTypeModel> BusinessBranchPaymentTypes { get; set; }
        public DbSet<BusinessBranchPriceRule> BusinessBranchPriceRules { get; set; }      
        public DbSet<MenuModel> Menus { get; set; }      
        public DbSet<ModifierModel> Modifiers { get; set; }      
        public DbSet<ProductModifierModel> ProductModifiers { get; set; }      
        public DbSet<PriceRuleModel> PriceRules { get; set; }      
        public DbSet<ProductPriceModel> ProductPrices { get; set; }      
        public DbSet<DiscountCodeModel> DiscountCodes { get; set; }
        public DbSet<SaleModel> Sales { get; set; }
        public DbSet<SaleProductModel> SaleProducts { get; set; }
        public DbSet<PaymentModel> Payments { get; set; }
        public DbSet<ProductPrinterModel> ProductPrinters { get; set; }
        public DbSet<ProductPortionModel> ProductPortions { get; set; }
        public DbSet<ProductMenuModel> ProductMenus{ get; set; }
        public DbSet<ModifierGroupModel> ModifierGroups{ get; set; }
        public DbSet<ModifierGroupProductCategoryModel> ModifierGroupProductCategories{ get; set; }
        public DbSet<ConfigDataModel> ConfigDatas{ get; set; }

    }

}