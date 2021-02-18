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
        public IConfiguration Configuration { get; }    
        private readonly ApplicationDbContext db;
        public AppController(ApplicationDbContext _db, IConfiguration configuration)
        {
            db = _db;
            Configuration = configuration;
        }

        

        [HttpGet("is_working")]
        [EnableQuery(MaxExpansionDepth = 0)]             
        public ActionResult<bool> IsAPIWorking()
        {
            return Ok();
        }  
    }

}
