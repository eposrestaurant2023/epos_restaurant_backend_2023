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
    [Route("api/[controller]")]
    public class SaleProductStatusController : ODataController
    {
        public IConfiguration config { get; }
        private readonly ApplicationDbContext db;
        public SaleProductStatusController(ApplicationDbContext _db, IConfiguration configuration)
        {
            db = _db;
            config = configuration;
        }


        [HttpGet]
        [AllowAnonymous]
        public ActionResult<List<SaleProductStatusModel>> Get()
        {
            try
            {
                var data = db.SaleProductStatuses;

                return Ok(data);
            }
            catch
            {
                return BadRequest();
            }
        }
    }

}
