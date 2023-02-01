using Microsoft.EntityFrameworkCore;    
using System.Linq;             
using eAPIClient.Models;
using Microsoft.Extensions.Configuration;

namespace eAPIClient
{
    public class ApplicationDbContext : DbContext
    {
         IConfiguration config { get; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration config) : base(options)
        {
            this.config = config;

        }
        public ApplicationDbContext(IConfiguration config )
        {
            this.config = config;

        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connection_string = config.GetValue<string>("ConnectionStrings:DefaultConnection");
            optionsBuilder.UseSqlServer(connection_string);
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            foreach (var property in builder.Model.GetEntityTypes().SelectMany(t => t.GetProperties()).Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                if (property.GetColumnType() == "decimal(19,10)")
                {
                    property.SetColumnType("decimal(19,10)");
                }
                else
                {
                    property.SetColumnType("decimal(19,8)");
                }
            }

            foreach (var property in builder.Model.GetEntityTypes().SelectMany(t => t.GetProperties()).Where(p => p.ClrType == typeof(string)))
            {
                property.SetCollation("Khmer_100_BIN");
            }

            builder.Entity<StoreProcedureResultModel>().HasNoKey().ToView("store_procedure_result");

        }

        public DbSet<DocumentNumberModel> DocumentNumbers { get; set; }        
        public DbSet<ConfigDataModel> ConfigDatas { get; set; }
        public DbSet<MenuModel> Menus { get; set; }
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<ProductMenuModel> ProductMenus { get; set; }
        public DbSet<ProductPrinterModel> ProductPrinters { get; set; }
        public DbSet<ProductModifierModel> ProductModifiers { get; set; }
        public DbSet<ProductPortionModel> ProductPortions { get; set; }
        public DbSet<ProductPriceModel> ProductPrices { get; set; }
        public DbSet<WorkingDayModel> WorkingDays { get; set; }
        public DbSet<CashierShiftModel> CashierShifts { get; set; }
     
        public DbSet<CustomerModel> Customers { get; set; }
        public DbSet<SalePaymentModel> SalePayments { get; set; }
        public DbSet<SaleModel> Sales { get; set; }
        public DbSet<SaleOrderModel> SaleOrders { get; set; }
        public DbSet<SaleProductModel> SaleProducts { get; set; }          
        public DbSet<SaleProductPrintQueue> SaleProductPrintQueues { get; set; }          
        public DbSet<ShiftModel> Shifts { get; set; }
        public DbSet<SaleProductModifierModel> SaleProductModifiers { get; set; }
        public DbSet<SaleStatusModel> SaleStatuses { get; set; }
        public DbSet<SaleProductStatusModel> SaleProductStatuses { get; set; } 
        public DbSet<CategoryNoteModel> CategoryNotes { get; set; }
        public DbSet<NoteModel> Notes { get; set; }
        public DbSet<CashDrawerAmountModel> CashDrawerAmounts { get; set; }

        public DbSet<StoreProcedureResultModel> StoreProcedureResults { get; set; }
        public DbSet<HistoryModel> Histories{ get; set; }

        public DbSet<ExpenseModel> Expenses { get; set; }
        public DbSet<CustomerCardModel> CustomerCards { get; set; }
        public DbSet<eShareModel.TranslateTextModel> TranslateTexts{ get; set; }
        public DbSet<CouponVoucherModel> CouponVouchers { get; set; }
        public DbSet<CouponVoucherTransactionModel> CouponVoucherTransactions { get; set; }


    }

}