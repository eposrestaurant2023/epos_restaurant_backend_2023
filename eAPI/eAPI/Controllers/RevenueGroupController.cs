using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using eModels;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NETCore.Encrypt;


namespace eAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class RevenueGroupController : ODataController
    {
        private readonly ApplicationDbContext db;
        public RevenueGroupController(ApplicationDbContext _db)
        {
            db = _db;
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [AllowAnonymous]
        public IQueryable<RevenueGroupModel> Get()
        {
            return db.RevenueGroups;
        }

        [HttpPost("Save")]
        public async Task<ActionResult<string>> SaveTest([FromBody] List<RevenueGroupModel> u)
        {
 


            AddOrUpdate(u);
            return Ok(u);

        }

        public virtual void AddOrUpdate(List<RevenueGroupModel> entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            db.UpdateRange(entity);
            db.SaveChanges();

            db.Database.ExecuteSqlRaw("exec sp_update_revenue_group_to_product");
        }

    }

  


}
