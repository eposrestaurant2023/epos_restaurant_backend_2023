﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
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
    public class ProductModifierController : ODataController
    {

        private readonly ApplicationDbContext db;
        public ProductModifierController(ApplicationDbContext _db)
        {
            db = _db;
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]   
        public IQueryable<ProductModifierModel> Get()
        {            
            return db.ProductModifiers;
        }
        
        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] ProductModifierModel u)
        {            
            if (u.id == 0)
            {
                db.ProductModifiers.Add(u);
            }
            else
            {
                db.ProductModifiers.Update(u);
            }            
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return Ok(u);
        }

        [HttpGet("find")]
        [EnableQuery(MaxExpansionDepth = 4)]
        public SingleResult<ProductModifierModel> Get([FromODataUri] int key)
        {
            var s = db.ProductModifiers.Where(r => r.id == key).AsQueryable();
            return SingleResult.Create(s);
        }
 

        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult<ProductModifierModel>> DeleteRecord(int id) //Delete
        {
            var u = await db.ProductModifiers.FindAsync(id);
            u.is_deleted = !u.is_deleted;            
            db.ProductModifiers.Update(u);
            await db.SaveChangesAsync();
            return Ok(u);
        }
    }
}