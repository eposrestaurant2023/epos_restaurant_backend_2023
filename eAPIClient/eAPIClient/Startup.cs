using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;      
using System.Linq;               
using Microsoft.EntityFrameworkCore;          
using Microsoft.AspNetCore.Authentication;
using eAPIClient.Helpers;
using eAPIClient.Services;
using Microsoft.OData.Edm;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using eAPIClient.Models;
using System.Text.Json.Serialization;
using Microsoft.Extensions.FileProviders;
using System.IO;
using eAPIClient.ScheduleTasks;

namespace eAPIClient
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
             
            services.AddControllers().AddJsonOptions(x =>
   x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);


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

    
            services.AddMvc();
            services.AddOData();


            // configure basic authentication 
            services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

            services.Configure<IISServerOptions>(options =>
            {
                options.AutomaticAuthentication = false;
            });



            services.AddScoped<AppService>();
            services.AddScoped<ISyncService,SyncService>();
            // configure DI for application services
            services.AddScoped<IUserService, UserService>();
            services.AddHttpClient<IHttpService, HttpService>();
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
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
            Path.Combine(env.ContentRootPath, "upload")),
                RequestPath = "/upload"
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.Expand().Select().Filter().OrderBy().Count().MaxTop(1000);
                endpoints.MapODataRoute("api", "api", GetEdmModel());
                endpoints.EnableDependencyInjection();

            });

        }



        private static IEdmModel GetEdmModel()
        {
            var odataBuilder = new ODataConventionModelBuilder();
            odataBuilder.EntitySet<UserModel>("User");
            odataBuilder.EntitySet<AppModel>("Sync");
            odataBuilder.EntitySet<AppModel>("Printing");
            odataBuilder.EntitySet<MenuModel>("Menu");
            odataBuilder.EntitySet<ProductModel>("Product");
            odataBuilder.EntitySet<ProductPrinterModel>("ProductPrinter");
            odataBuilder.EntitySet<ProductMenuModel>("ProductMenu");
            //odataBuilder.EntitySet<ProductModifierModel>("ProductModifier");
            odataBuilder.EntitySet<ProductPortionModel>("ProductPortion");
            odataBuilder.EntitySet<WorkingDayModel>("WorkingDay");
            odataBuilder.EntitySet<CashierShiftModel>("CashierShift");
            odataBuilder.EntitySet<ShiftModel>("Shift");
            odataBuilder.EntitySet<CustomerModel>("Customer");
            
            odataBuilder.EntitySet<ConfigDataModel>("ConfigData");                 
            odataBuilder.EntitySet<SaleModel>("Sale"); 
            odataBuilder.EntitySet<NoteModel>("Note");
            odataBuilder.EntitySet<SaleProductModel>("SaleProduct");
            odataBuilder.EntitySet<SaleProductModifierModel>("SaleProductModifier");
            odataBuilder.EntitySet<CashDrawerAmountModel>("CashDrawerAmount");
            odataBuilder.EntitySet<HistoryModel>("History");
            odataBuilder.EntitySet<ExpenseModel>("Expense");
            odataBuilder.EntitySet<CustomerCardModel>("CustomerCard");
            return odataBuilder.GetEdmModel();
        }
    }


}
