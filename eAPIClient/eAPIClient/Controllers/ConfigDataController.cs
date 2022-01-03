using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using eAPIClient.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NETCore.Encrypt;


namespace eAPIClient.Controllers
{
    [ApiController]
    [Route("api")]      
    public class ConfigDataController : ODataController
    {                
        public IConfiguration Configuration { get; }    
        private readonly ApplicationDbContext db;
        public ConfigDataController(ApplicationDbContext _db, IConfiguration configuration)
        {
            db = _db;
            Configuration = configuration;
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 0)]
        public ActionResult<List<ConfigDataModel>> Get()
        {
            try
            {
                var data = db.ConfigDatas;
                return Ok(data);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("find")]
        [EnableQuery(MaxExpansionDepth = 0)]
        public SingleResult<ConfigDataModel> Get(string config_type)
        {   
                var s = db.ConfigDatas.Where(r => r.config_type == config_type).AsQueryable();
                return SingleResult.Create(s);   
           
        }
        [HttpPost("Save")]
        public async Task<ActionResult<ConfigDataModel>> POST([FromBody] ConfigDataModel model)
        {
            try
            {
                ConfigDataModel _d = db.ConfigDatas.Where(r => r.config_type == model.config_type).FirstOrDefault();   
                _d.data = model.data;
                db.ConfigDatas.Update(_d);
                await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
                return Ok(_d);
            }
            catch
            {
                return BadRequest();
            }

        }
    }

}
