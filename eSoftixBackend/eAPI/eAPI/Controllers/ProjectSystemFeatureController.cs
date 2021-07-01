using eModels;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace eAPI.Controllers
{
    [ApiController,Route("api/[Controller]"),]
    public class ProjectSystemFeatureController : ControllerBase
    {
        private readonly ApplicationDbContext db;
        public ProjectSystemFeatureController(ApplicationDbContext db)
        {
            this.db = db;
        }
        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 0)]
        public ActionResult<List<ProjectSystemFeatureModel>> Get()
        {
            var per = db.ProjectSystemFeatures;
            return Ok(per);
        }

        [HttpPost("save/multiple")]
        public async Task<ActionResult<bool>> SaveMultiple([FromBody] List<ProjectSystemFeatureModel> models)
        {

            if (models.Count() > 0)
            {
                db.ProjectSystemFeatures.UpdateRange(models);


                await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));

                db.Database.ExecuteSqlRaw($"exec sp_update_project_information '{models.FirstOrDefault().project_id}'");
            }
          
            return Ok(true);
        }


    }

    [ApiController, Route("api/[Controller]"),]
    public class BusinessBranchSystemFeatureController : ControllerBase
    {
        private readonly ApplicationDbContext db;
        public BusinessBranchSystemFeatureController(ApplicationDbContext db)
        {
            this.db = db;
        }
        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 0)]
        public ActionResult<List<BusinessBranchSystemFeatureModel>> Get()
        {
            var per = db.BusinessBranchSystemFeatures;
            return Ok(per);
        }

    }
}
