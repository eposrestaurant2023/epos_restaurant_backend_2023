using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Authentication;
using eAPI.Helpers;
using eAPI.Services;
using Microsoft.OData.Edm;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using eModels;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNet.OData.Routing.Conventions;
using System.Web.Http;
using System.Net.Http;
using eAPI.ScheduleTasks;
using eAPI.Hubs;

namespace eAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
           
            services.AddControllers();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAnyOrigin",
                    builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
           
            services.AddDbContext<ApplicationDbContext>(options => options.EnableSensitiveDataLogging().UseSqlServer(Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Transient);

            services.AddControllersWithViews().AddNewtonsoftJson(options => {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                options.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Local;
            });

            services.AddSignalR();
            services.AddMvc();
            services.AddOData();
        

            // configure basic authentication 
            services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

            services.AddScoped<AppService>();
            services.AddScoped<BackendSyncService>();
            // configure DI for application services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped(x => {

                return new HttpClient();
            });
            services.AddScoped<IHttpService, HttpService>();


            services.AddSingleton<IHostedService, EposRestaurantTask>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(x => x
              .AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());

            app.UseRouting();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(env.ContentRootPath, "upload")),
                RequestPath = "/upload"
            });

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.Expand().Select().Filter().OrderBy().Count().MaxTop(10000);

                var routingConventions = ODataRoutingConventions.CreateDefault();
                var defaultConventions = ODataRoutingConventions.CreateDefault();
                var conventions = defaultConventions.Except( defaultConventions.OfType<MetadataRoutingConvention>());
                var route = endpoints.MapODataRoute( "api", "api", GetEdmModel(), pathHandler: new DefaultODataPathHandler(), routingConventions: conventions);
                endpoints.EnableDependencyInjection();
                endpoints.MapHub<ConnectionHub>("/connectionhub");
            });

        }

     

        private static IEdmModel GetEdmModel()
        {
            var odataBuilder = new ODataConventionModelBuilder();
            odataBuilder.EntitySet<RoleModel>("Role");
            odataBuilder.EntitySet<PermissionOptionModel>("PermissionOption");

            var m = odataBuilder.EntitySet<ModifierGroupItemModel>("ModifierGroupItem");
            m.EntityType.HasKey(r => new { r.modifier_group_id, r.modifier_id });

            var mp = odataBuilder.EntitySet<ModifierGroupProductCategoryModel>("ModifierGroupProductCategory");
            mp.EntityType.HasKey(r => new { r.modifer_group_id, r.product_category_id });

            var p = odataBuilder.EntitySet<PermissionOptionRoleModel>("PermissionOptionRole");
            p.EntityType.HasKey(r => new { r.role_id, r.permission_option_id });

            var r = odataBuilder.EntitySet<BusinessBranchRoleModel>("BusinessBranchRole");
            r.EntityType.HasKey(r => new { r.role_id, r.business_branch_id });   
            var bs = odataBuilder.EntitySet<BusinessBranchSettingModel>("BusinessBranchSetting");
            bs.EntityType.HasKey(r => new { r.setting_id, r.business_branch_id, r.setting_value });

            var b = odataBuilder.EntitySet<BusinessBranchPaymentTypeModel>("BusinessBranchPaymentType");
            b.EntityType.HasKey(r => new { r.payment_type_id, r.business_branch_id });

            var cb = odataBuilder.EntitySet<CustomerBusinessBranchModel>("CustomerBusinessBranch");
            cb.EntityType.HasKey(r => new { r.customer_id, r.business_branch_id });

            var bp = odataBuilder.EntitySet<BusinessBranchPriceRule>("BusinessBranchPriceRule");
            bp.EntityType.HasKey(r => new { r.price_rule_id, r.business_branch_id });

            var mi = odataBuilder.EntitySet<ModifierIngredientModel>("ModifierIngredient");
            mi.EntityType.HasKey(r => new { r.modifier_id, r.ingredient_id });

            var bc = odataBuilder.EntitySet<BusinessBranchCurrencyModel>("BusinessBranchCurrency");
            bc.EntityType.HasKey(r => new { r.business_branch_id, r.currency_id });

            odataBuilder.EntitySet<UserModel>("User");     
            odataBuilder.EntitySet<GlobalVariableModel>("GlobalVariable");
            odataBuilder.EntitySet<CustomerGroupModel>("CustomerGroup");
            odataBuilder.EntitySet<CustomerModel>("Customer");
            odataBuilder.EntitySet<PaymentTypeModel>("PaymentType");
            odataBuilder.EntitySet<AttachFilesModel>("AttachFiles");
            odataBuilder.EntitySet<SettingModel>("Setting");
        
            odataBuilder.EntitySet<CountryModel>("Country");
            odataBuilder.EntitySet<BusinessBranchModel>("BusinessBranch");
            odataBuilder.EntitySet<BusinessInformationModel>("BusinessInformation");
            odataBuilder.EntitySet<OutletModel>("Outlet");
            odataBuilder.EntitySet<StationModel>("Station");
            odataBuilder.EntitySet<TableGroupModel>("TableGroup");
            odataBuilder.EntitySet<TableModel>("Table");
            odataBuilder.EntitySet<DiscountCodeModel>("DiscountCode");
            odataBuilder.EntitySet<ProductCategoryModel>("ProductCategory");
            odataBuilder.EntitySet<ProductGroupModel>("ProductGroup");
            odataBuilder.EntitySet<ProductModel>("Product");
            odataBuilder.EntitySet<NoteModel>("Note");
            odataBuilder.EntitySet<CategoryNoteModel>("CategoryNote");
            odataBuilder.EntitySet<PriceRuleModel>("PriceRule");
            odataBuilder.EntitySet<SaleModel>("Sale");
            odataBuilder.EntitySet<ProductPrinterModel>("ProductPrinter");
            odataBuilder.EntitySet<ProductPortionModel>("ProductPortion");
            odataBuilder.EntitySet<ProductMenuModel>("ProductMenu");
            odataBuilder.EntitySet<MenuModel>("Menu");
            odataBuilder.EntitySet<ModifierModel>("Modifier");
            odataBuilder.EntitySet<ModifierGroupModel>("ModifierGroup");
            odataBuilder.EntitySet<ProductMenuModel>("ProductMenu");
           odataBuilder.EntitySet<HistoryModel>("History");                          
            odataBuilder.EntitySet<ConfigDataModel>("ConfigData");
            odataBuilder.EntitySet<ProductMenuModel>("ProductMenu");
            odataBuilder.EntitySet<BusinessBranchProductPriceModel>("BusinessBranchProductPrice");
            odataBuilder.EntitySet<VendorModel>("Vendor");
            odataBuilder.EntitySet<PurchaseOrderModel>("PurchaseOrder");
            odataBuilder.EntitySet<PurchaseOrderPaymentModel>("PurchaseOrderPayment");
            odataBuilder.EntitySet<PurchaseOrderProductModel>("PurchaseOrderProduct");
            odataBuilder.EntitySet<StockLocationModel>("StockLocation");
            odataBuilder.EntitySet<SaleProductModel>("SaleProduct");
            odataBuilder.EntitySet<SaleProductModifierModel>("SaleProductModifier");
            odataBuilder.EntitySet<VendorGroupModel>("VendorGroup");
            odataBuilder.EntitySet<StockTakeModel>("StockTake");
            odataBuilder.EntitySet<StockTakeProductModel>("StockTakeProduct");
            odataBuilder.EntitySet<StockTransferModel>("StockTransfer");
            odataBuilder.EntitySet<StockTransferProductModel>("StockTransferProduct");
            odataBuilder.EntitySet<ProductIngredientModel>("ProductIngredient");
            odataBuilder.EntitySet<UnitModel>("Unit");
            odataBuilder.EntitySet<SalePaymentModel>("SalePayment");
            odataBuilder.EntitySet<UnitCategoryModel>("UnitCategory");
            odataBuilder.EntitySet<ProductIngredientRelatedModel>("ProductIngredientRelated");
            odataBuilder.EntitySet<InventoryTransactionModel>("InventoryTransaction");
            odataBuilder.EntitySet<ProductModifierModel>("ProductModifier");
            odataBuilder.EntitySet<StockLocationProductModel>("StockLocationProduct");
            odataBuilder.EntitySet<KitchenGroupModel>("KitchenGroup");
            odataBuilder.EntitySet<CurrencyModel>("Currency");
            odataBuilder.EntitySet<WorkingDayModel>("WorkingDay");
            odataBuilder.EntitySet<CashierShiftModel>("CashierShift");
            odataBuilder.EntitySet<SystemFeatureModel>("SystemFeature");
            odataBuilder.EntitySet<SaleTypeModel>("SaleType");
            odataBuilder.EntitySet<RevenueGroupModel>("RevenueGroup");
            odataBuilder.EntitySet<ProductTaxModel>("ProductTax");
            odataBuilder.EntitySet<ProductionModel>("Production");
            odataBuilder.EntitySet<ProductionProductModel>("ProductionProduct");
            odataBuilder.EntitySet<CashDrawerAmountModel>("CashDrawerAmount");
            odataBuilder.EntitySet<InventoryCheckModel>("InventoryCheck");
            odataBuilder.EntitySet<eShareModel.ExpenseCategoryModel>("ExpenseCategory");
            odataBuilder.EntitySet< eShareModel.ExpenseItemModel> ("ExpenseItem");
            odataBuilder.EntitySet<ExpenseModel>("Expense");

            odataBuilder.EntitySet<BusinessBranchSystemFeatureModel>("BusinessBranchSystemFeature");
            var business_branch_system_features = odataBuilder.EntitySet<BusinessBranchSystemFeatureModel>("BusinessBranchSystemFeature");
            business_branch_system_features.EntityType.HasKey(r => new { r.business_branch_id, r.system_feature_id });


            return odataBuilder.GetEdmModel();
        }
    }

  
}
