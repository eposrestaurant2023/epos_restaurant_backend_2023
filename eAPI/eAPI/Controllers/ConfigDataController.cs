using eAPI;
using eAPI.Controllers;
using eModels;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace eOpticalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ConfigDataController : ControllerBase
    {
        private readonly ApplicationDbContext db;
        public ConfigDataController(ApplicationDbContext _db)
        {
            db = _db;
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 0)]
        public ActionResult<List<ConfigDataModel>> Get()
        {
             
            return Ok(db.ConfigDatas);
        }
         
    }
}
