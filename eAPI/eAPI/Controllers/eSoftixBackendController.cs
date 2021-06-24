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

                //check station
                CheckBranch();

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



    }
}
