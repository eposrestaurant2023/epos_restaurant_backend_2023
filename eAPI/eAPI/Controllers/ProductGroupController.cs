﻿using System;
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
    public class ProductGroupController : ODataController
    {

        private readonly ApplicationDbContext db;
        public ProductGroupController(ApplicationDbContext _db)
        {
            db = _db;
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        
        public IQueryable<ProductGroupModel> Get()
        {
              return db.ProductGroups;
        }

        
        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] ProductGroupModel u)
        {
            
            if (u.id == 0)
            {

                db.ProductGroups.Add(u);
            }
            else
            {
                
                db.ProductGroups.Update(u);
            }
            
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return Ok(u);


        }

      


        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult<ProductGroupModel>> DeleteRecord(Guid id) //Delete
        {
            var u = await db.ProductGroups.FindAsync(id);
            u.is_deleted = !u.is_deleted;
            
            db.ProductGroups.Update(u);
            await db.SaveChangesAsync();
            return Ok(u);
        }
    }

}