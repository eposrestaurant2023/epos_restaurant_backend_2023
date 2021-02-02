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


            services.AddMvc();
            services.AddOData();


            // configure basic authentication 
            services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

            services.AddScoped<AppService>();
            // configure DI for application services
            services.AddScoped<IUserService, UserService>();
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
                endpoints.Expand().Select().Filter().OrderBy().Count().MaxTop(500);

                var routingConventions = ODataRoutingConventions.CreateDefault();
                var defaultConventions = ODataRoutingConventions.CreateDefault();
                var conventions = defaultConventions.Except( defaultConventions.OfType<MetadataRoutingConvention>());
                var route = endpoints.MapODataRoute( "api", "api", GetEdmModel(), pathHandler: new DefaultODataPathHandler(), routingConventions: conventions);
                endpoints.EnableDependencyInjection();

            });

        }

     

        private static IEdmModel GetEdmModel()
        {
            var odataBuilder = new ODataConventionModelBuilder();
            odataBuilder.EntitySet<RoleModel>("Role");
            odataBuilder.EntitySet<PermissionOptionModel>("PermissionOption");
            var p = odataBuilder.EntitySet<PermissionOptionRoleModel>("PermissionOptionRole");
            p.EntityType.HasKey(r => new { r.role_id, r.permission_option_id });

            odataBuilder.EntitySet<UserModel>("User");
            var u = odataBuilder.EntitySet<UserBusinessBranchModel>("UserBusinessBranch");
            u.EntityType.HasKey(r => new { r.user_id, r.business_branch_id });

            odataBuilder.EntitySet<PaymentTypeModel>("PaymentType");
            odataBuilder.EntitySet<BusinessBranchModel>("BusinessBranch");
            var b = odataBuilder.EntitySet<BusinessBranchPaymentTypeModel>("BusinessBranchPaymentType");
            b.EntityType.HasKey(r => new { r.payment_type_id, r.business_branch_id });

            odataBuilder.EntitySet<PermissionOptionModel>("PermissionOption");
            odataBuilder.EntitySet<GlobalVariableModel>("GlobalVariable");
            odataBuilder.EntitySet<CustomerGroupModel>("CustomerGroup");
            odataBuilder.EntitySet<CustomerModel>("Customer");
            odataBuilder.EntitySet<PaymentTypeModel>("PaymentType");
            odataBuilder.EntitySet<AttachFilesModel>("AttachFiles");
            odataBuilder.EntitySet<SettingModel>("Setting");
            odataBuilder.EntitySet<HistoryModel>("History");
            odataBuilder.EntitySet<CountryModel>("Country");
            odataBuilder.EntitySet<BusinessBranchModel>("BusinessBranch");
            odataBuilder.EntitySet<BusinessInformationModel>("BusinessInformation");
            odataBuilder.EntitySet<OutletModel>("Outlet");
            odataBuilder.EntitySet<StationModel>("Station");
            odataBuilder.EntitySet<TableGroupModel>("TableGroup");
            odataBuilder.EntitySet<TableModel>("Table");
            odataBuilder.EntitySet<DiscountCodeModel>("DiscountCode");

            return odataBuilder.GetEdmModel();
        }
    }

    class Student
    {
        public int id { get; set; }
    }
}
