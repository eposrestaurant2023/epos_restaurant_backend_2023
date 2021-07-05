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
                Path.Combine(env.ContentRootPath, "Upload")),
                RequestPath = "/upload"
            });

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.Expand().Select().Filter().OrderBy().Count().MaxTop(500);
                endpoints.MapODataRoute("api", "api", GetEdmModel());
                endpoints.EnableDependencyInjection();
            });


        }


        private IEdmModel GetEdmModel()
        {
            var odataBuilder = new ODataConventionModelBuilder();
            odataBuilder.EntitySet<RoleModel>("Role");
            odataBuilder.EntitySet<PermissionOptionModel>("PermissionOption");
            var p = odataBuilder.EntitySet<PermissionOptionRoleModel>("PermissionOptionRole");
            p.EntityType.HasKey(r => new { r.role_id, r.permission_option_id });
            odataBuilder.EntitySet<UserModel>("User");
            odataBuilder.EntitySet<GlobalVariableModel>("GlobalVariable");
            odataBuilder.EntitySet<PermissionOptionModel>("PermissionOption");
            odataBuilder.EntitySet<GlobalVariableModel>("GlobalVariable");
            odataBuilder.EntitySet<CustomerGroupModel>("CustomerGroup");
            odataBuilder.EntitySet<CustomerModel>("Customer");

            odataBuilder.EntitySet<AttachFilesModel>("AttachFiles");
            odataBuilder.EntitySet<SettingModel>("Setting");
            odataBuilder.EntitySet<HistoryModel>("History");
            odataBuilder.EntitySet<AttachFileModel>("AttachFile");
            odataBuilder.EntitySet<ProjectModel>("Project");
            odataBuilder.EntitySet<BusinessBranchModel>("BusinessBranch");
            odataBuilder.EntitySet<OutletModel>("Outlet");
            odataBuilder.EntitySet<StationModel>("Station");
            odataBuilder.EntitySet<StockLocationModel>("StockLocation");

            odataBuilder.EntitySet<ProjectSystemFeatureModel>("ProjectSystemFeature");
            var project_feature = odataBuilder.EntitySet<ProjectSystemFeatureModel>("ProjectSystemFeature");
            project_feature.EntityType.HasKey(r => new { r.project_id, r.system_feature_id});

            odataBuilder.EntitySet<BusinessBranchSystemFeatureModel>("BusinessBranchSystemFeature");
            var business_branch_feature= odataBuilder.EntitySet<BusinessBranchSystemFeatureModel>("BusinessBranchSystemFeature");
            business_branch_feature.EntityType.HasKey(r => new { r.business_branch_id, r.system_feature_id });
            
            
            odataBuilder.EntitySet<eKnowledgeBaseModel>("eKnowledgeBase");
            odataBuilder.EntitySet<StockLocationModel>("StockLocation");

            return odataBuilder.GetEdmModel();
        }
    }
}
