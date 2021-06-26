using DeviceId;
using eAPI.Services;
using eModels;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;     
namespace eAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class eSoftixBackendController : ControllerBase
    {
        public IConfiguration Configuration { get; }
        private readonly ApplicationDbContext db;
        private readonly IHttpService http;

        eSoftixBackend.ProjectModel project = new eSoftixBackend.ProjectModel();
        public eSoftixBackendController(ApplicationDbContext _db, IConfiguration configuration, IHttpService _http)
        {
            db = _db;
            Configuration = configuration;
            http = _http;
        }

        public List<eSoftixBackend.BusinessBranchModel> esoftix_business_branch
        {
            get
            {
                if (project != null)
                {
                    return project.business_branches.ToList();
                }
                return new List<eSoftixBackend.BusinessBranchModel>();

            }
        } 
        public List<eSoftixBackend.OutletModel> esoftix_outlets
        {
            get
            {
                if (project != null)
                {
                    return project.business_branches.SelectMany(r=>r.outlets).ToList();
                }
                return new List<eSoftixBackend.OutletModel>();

            }
        }
              public List<eSoftixBackend.StationModel> esoftix_stations
        {
            get
            {
                if (project != null)
                {
                    return project.business_branches.SelectMany(r=>r.outlets).ToList().SelectMany(r => r.stations).ToList();
                }
                return new List<eSoftixBackend.StationModel>();

            }
        } 
        
        public List<eSoftixBackend.StockLocationModel> esoftix_stock_locations
        {
            get
            {
                if (project != null)
                {
                    return project.business_branches.SelectMany(r => r.stock_locations).ToList();
                }
                return new List<eSoftixBackend.StockLocationModel>();

            }
        }
        public  eSoftixBackend.CustomerModel esoftix_customer
        {
            get
            {
                if (project != null)
                {
                    return project.customer;
                }
                return new  eSoftixBackend.CustomerModel();

            }
        }


        [HttpGet("CheckSystemFeatures")]
        [AllowAnonymous]
        public async Task< ActionResult<bool>> CheckSystemFeatures()
        {
            //get project 
            string project_id = db.Settings.Where(r => r.id == 57).AsNoTracking() .FirstOrDefault().setting_value;
            
            string api_url = $"project({project_id})?$expand=customer,business_branches($expand=stock_locations,outlets($expand=stations($filter=is_deleted eq false);$filter=is_deleted eq false);$filter=is_deleted eq false)";
          

            var resp = await http.eSoftixApiGet($"project({project_id})?$expand=customer,business_branches($expand=stock_locations,outlets($expand=stations($filter=is_deleted eq false);$filter=is_deleted eq false);$filter=is_deleted eq false)");
            if (resp.IsSuccess)
            {
                project = JsonSerializer.Deserialize<eSoftixBackend.ProjectModel>(resp.Content.ToString());

                //check branch
                CheckBranch();

                //Check Outlet 
                CheckOutlet();

                //check sttion
                CheckStation();

                CheckStockLocation();

                //check customer
                CheckCustomer();

                return true;
            } 
             

            return false;
        }


        void CheckBranch()
        {
            //get branch from local db
            var branches = db.BusinessBranches;
            foreach(var remote_branch in esoftix_business_branch)
            { BusinessBranchModel local_branch = new BusinessBranchModel();
                if (branches.Where(r => r.id == remote_branch.id).Any())
                {

                    local_branch = branches.Where(r => r.id == remote_branch.id).FirstOrDefault();
                    MapBusinessBranchField(remote_branch, local_branch);
                    db.BusinessBranches.Update(local_branch);
                }else
                {
                    MapBusinessBranchField(remote_branch, local_branch);
                    branches.Add(local_branch);
                }
            }

            db.UpdateRange(branches);

            db.SaveChanges();

        }

        void MapBusinessBranchField(eSoftixBackend.BusinessBranchModel remote_branch, BusinessBranchModel local_branch)
        {
            //mapping field
            local_branch.address_en = remote_branch.address_en;
            local_branch.address_kh = remote_branch.address_kh;
            local_branch.business_branch_name_en = remote_branch.business_branch_name_en;
            local_branch.business_branch_name_kh = remote_branch.business_branch_name_kh;
            local_branch.color = remote_branch.color;
            local_branch.created_by = remote_branch.created_by;
            local_branch.created_date = remote_branch.created_date;
            local_branch.deleted_by = remote_branch.deleted_by;
            local_branch.deleted_date = remote_branch.deleted_date;
            local_branch.email = remote_branch.email;
            local_branch.id = remote_branch.id;
            local_branch.is_deleted = remote_branch.is_deleted;
            local_branch.logo = remote_branch.logo;
            local_branch.note = remote_branch.note;
            local_branch.phone_1 = remote_branch.phone_1;
            local_branch.phone_2 = remote_branch.phone_2;
            local_branch.status = remote_branch.status;
            local_branch.website = remote_branch.website;

            //end mapping field
        }
        /// <summary>
        /// Check for Outlet Update 
        /// </summary>
        void CheckOutlet()
        {
            //get branch from local db
            var outlets= db.Outlets;
            foreach (var remote_outlet in esoftix_outlets)
            {
                OutletModel local_outlet = new  OutletModel();
                if (outlets.Where(r => r.id == remote_outlet.id).Any())
                {

                    local_outlet = outlets.Where(r => r.id == remote_outlet.id).FirstOrDefault();
                    MapOutletField(remote_outlet, local_outlet);
                    
                }
                else
                {
                    MapOutletField(remote_outlet, local_outlet);
                    outlets.Add(local_outlet);
                }
            }

            db.UpdateRange(outlets);

            db.SaveChanges();

        }

        void MapOutletField(eSoftixBackend.OutletModel remote_outlet, OutletModel local_outlet)
        {
            //mapping field
            local_outlet.id = remote_outlet.id;
            local_outlet.business_branch_id = remote_outlet.business_branch_id;
            local_outlet.outlet_name_en = remote_outlet.outlet_name_en;
            local_outlet.outlet_name_kh = remote_outlet.outlet_name_kh;
            local_outlet.created_by = remote_outlet.created_by;
            local_outlet.created_date = remote_outlet.created_date;
            local_outlet.is_deleted = remote_outlet.is_deleted;
            local_outlet.deleted_by = remote_outlet.deleted_by;
            local_outlet.deleted_date = remote_outlet.deleted_date;
            local_outlet.status = remote_outlet.status;

            //end mapping field
        }


        /// <summary>
        /// Check for station  Update 
        /// </summary>
        void CheckStation()
        {
            //get branch from local db
            var stations = db.Stations;
            foreach (var remote_station in esoftix_stations)
            {
                StationModel local_station = new StationModel();
                if (stations.Where(r => r.id == remote_station.id).Any())
                {

                    local_station = stations.Where(r => r.id == remote_station.id).FirstOrDefault();
                    MapStationField(remote_station, local_station);

                }
                else
                {
                    MapStationField(remote_station, local_station);
                    stations.Add(local_station);
                }
            }

            db.Stations.UpdateRange(stations);

            db.SaveChanges();

        }

        void MapStationField(eSoftixBackend.StationModel remote, StationModel local)
        {
            local.id = remote.id;
            local.station_name_en = remote.station_name_en;
            local.station_name_kh = remote.station_name_kh;
            //local.is_already_config = remote.is_already_config;
            local.created_by = remote.created_by;
            local.created_date = remote.created_date;
            local.is_deleted = remote.is_deleted;
            local.deleted_by = remote.deleted_by;
            local.deleted_date = remote.deleted_date;
            local.status = remote.status;
            local.outlet_id = remote.outlet_id;
            local.is_full_license = remote.is_full_license;
            local.expired_date = remote.expired_date;
        }



        /// <summary>
        /// Stock Locatin
        /// </summary>
        void CheckStockLocation()
        {
            //get branch from local db
            var stock_locations = db.StockLocations;
            foreach (var remote_stock_location in esoftix_stock_locations)
            {
                StockLocationModel local_stock_location = new StockLocationModel();
                if (stock_locations.Where(r => r.id == remote_stock_location.id).Any())
                {

                    local_stock_location = stock_locations.Where(r => r.id == remote_stock_location.id).FirstOrDefault();
                    MapStockLocationField(remote_stock_location, local_stock_location);

                }
                else
                {
                    MapStockLocationField(remote_stock_location, local_stock_location);
                    stock_locations.Add(local_stock_location);
                }
            }

            db.StockLocations.UpdateRange(stock_locations);

            db.SaveChanges();

        }

        void MapStockLocationField(eSoftixBackend.StockLocationModel remote, StockLocationModel local)
        {
            local.id = remote.id;
            local.business_branch_id = remote.business_branch_id;
            local.stock_location_name = remote.stock_location_name;
            local.is_default = remote.is_default;
        }


        //check customer update 
        void CheckCustomer()
        {
            BusinessInformationModel biz = db.BusinessInformations.FirstOrDefault();
            biz.id = esoftix_customer.id;
            biz.company_name = (esoftix_customer.company_name ?? "");
            biz.company_name_kh = (esoftix_customer.company_name ?? "" );
            biz.contact_name = (esoftix_customer.customer_name_en ?? "");
            biz.contact_phone_number = (esoftix_customer.phone_1 ?? "");
            biz.office_phone = (esoftix_customer.phone_2 ?? "");
            biz.address = (esoftix_customer.address ?? "");
            biz.email = (esoftix_customer.email??"");

            db.BusinessInformations.Update(biz);
            db.SaveChanges();
        }

    }
}
