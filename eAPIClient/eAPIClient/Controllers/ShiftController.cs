using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using eAPIClient;
using eAPIClient.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NETCore.Encrypt;


namespace eAPIClient.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ShiftController : ODataController
    {

        private readonly ApplicationDbContext db;
        public ShiftController(ApplicationDbContext _db)
        {
            db = _db;
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)] 
        public IQueryable<ShiftModel> Get()
        {
            return db.Shifts;  
        }

        [HttpGet("find")]
        [EnableQuery(MaxExpansionDepth = 4)]
        public SingleResult<ShiftModel> Get([FromODataUri] int key)
        {
            var s = db.Shifts.Where(r => r.id == key).AsQueryable();

            return SingleResult.Create(s);
        }
    }

}
