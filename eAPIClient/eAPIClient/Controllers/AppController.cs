using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using eAPIClient.Models;
using eAPIClient.Services;
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
    public class AppController : ODataController
    {                
        
        public IConfiguration config { get; }
        private readonly ApplicationDbContext db;
        private readonly AppService app;

        public AppController(ApplicationDbContext _db, AppService _app, IConfiguration configuration)
        {
            db = _db;
            app = _app;
            config = configuration;
        }




        [HttpGet("isClientAPIWork")]
        [EnableQuery(MaxExpansionDepth = 0)]             
        public ActionResult<bool> IsAPIWorking()
        {
            return Ok();
        }


        [HttpPost]
        [Route("GetData")]
        public ActionResult<string> GetData([FromBody] FilterModel f)
        {
            var d = db.StoreProcedureResults.FromSqlRaw(string.Format("exec {0} {1}", f.procedure_name, f.procedure_parameter)).ToList().FirstOrDefault();
            if (d != null)
            {
                string r = d.result.Replace("\\", "").Replace("\"[", "[").Replace("]\"", "]").ToString();
                return r;
            }
            return BadRequest();

        }


        [HttpPost]
        [Route("PrintRequest")]
        public ActionResult<string> PrintRequest([FromBody] PrintRequestModel f)
        {
            app.sendPrintRequest(f);
            return Ok();
        }


        [HttpPost]
        [Route("BackupDatabase")]
        public ActionResult<string> BackupDatabase()
        {
            db.Database.ExecuteSqlRaw("exec sp_backup_database");
            return Ok();
        }


        [HttpGet("CurrentBusinessBranch")]
        [EnableQuery(MaxExpansionDepth = 0)]
        public ActionResult<bool> CurrentBusinessBranchId(string businessBranchId)
        {
            string _businessBranchId = config.GetValue<string>("business_branch_id");

            if(_businessBranchId.ToLower() != businessBranchId.ToLower())
            {          
                return Ok(false);
            }

            return Ok(true);
        }





    }

}
