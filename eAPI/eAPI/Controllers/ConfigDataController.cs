using eAPI;
using eAPI.Controllers;
using eModels;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;         
using System.Collections.Generic;   

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
        [AllowAnonymous]
        public ActionResult<List<ConfigDataModel>> Get()
        {
             
            return Ok(db.ConfigDatas);
        }
         
    }
}
