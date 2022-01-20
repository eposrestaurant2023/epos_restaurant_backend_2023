using System;
using System.Collections.Generic;
using System.Linq;              
using System.Threading.Tasks;   
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;    
using eAPIClient.Services;
using Microsoft.Extensions.Configuration;
using System.Text.Json;    
using eAPIClient.Models;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Security.Claims;

namespace eAPIClient.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PrintingController : ODataController
    {    
       
        private readonly ApplicationDbContext db;
        private readonly IHttpService http;
        private readonly IConfiguration config;
        private readonly AppService app;
        public PrintingController(ApplicationDbContext _db, IHttpService _http, IConfiguration _config, AppService _app)
        {
            db = _db;
            http = _http;
            config = _config;
            app = _app;
        }

    
     
       

        [HttpGet("GetPrintData")] 
        [AllowAnonymous]
        public ActionResult<List<DynamicModel>> GetPrintData(string procedure_name, string parameters)
        {
            var d = db.StoreProcedureResults.FromSqlRaw($"exec {procedure_name} {parameters}").ToList().FirstOrDefault();
            if (d != null)
            {
                string r = d.result;
                var data = JsonSerializer.Deserialize<List<DynamicModel>>(r) ;

                return Ok(data);
            }
            return   Ok(new List<DynamicModel>()) ;
        } 
           
        
        
        [HttpGet("ExecuteSQLSatement")] 
        [AllowAnonymous]
        public ActionResult ExecuteSQLSatement(string sql)
        {
            db.Database.ExecuteSqlRaw(sql);
            return Ok();
             
        } 

                 

                
    } 
 

     
}
