using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;         
using System.Text.Json;
using System.Threading.Tasks;
using eAPIClient.Models;
using Microsoft.AspNet.OData;               
using Microsoft.AspNetCore.Mvc;            
using Microsoft.Extensions.Configuration;    


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

          
               
                var alert = data.Where(r => r.config_type == "telegram");
                if (alert.Any())
                {
                    try
                    {
                        string _val = alert.FirstOrDefault().data;
                        Program.Telegrams = JsonSerializer.Deserialize<List<TelegramAlertModel>>(_val).ToList();
                    }
                    catch (Exception ex)
                    {

                    }
                }

                var sync_logs = data.Where(r => r.config_type == "telegram_sync_logs");
                if (sync_logs.Any())
                {
                    try
                    {
                        Program.TelegramSyncLog = JsonSerializer.Deserialize<TelegramModel>(sync_logs.FirstOrDefault().data);
                    }
                    catch (Exception ex)
                    {
                        //
                    }
                }  

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
