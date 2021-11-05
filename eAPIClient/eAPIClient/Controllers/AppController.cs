using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
    public class AppController : ODataController
    {                
        public IConfiguration config { get; }    
        private readonly ApplicationDbContext db;
        public AppController(ApplicationDbContext _db, IConfiguration _config)
        {
            db = _db;
            config = _config;
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
