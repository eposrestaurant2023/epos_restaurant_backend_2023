using eModels;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
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
