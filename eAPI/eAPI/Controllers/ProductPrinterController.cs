﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using eAPI.Hubs;
using eModels;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NETCore.Encrypt;


namespace eAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ProductPrinterController : ODataController
    {

        private readonly ApplicationDbContext db;
        private readonly IHubContext<ConnectionHub> hub;
        public ProductPrinterController(ApplicationDbContext _db,IHubContext<ConnectionHub> _hub)
        {
            db = _db;hub = _hub;
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        
        public IQueryable<ProductPrinterModel> Get()
        {
              return db.ProductPrinters;
        }

        
        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] ProductPrinterModel u)
        {
            
            if (u.id == 0)
            {

                db.ProductPrinters.Add(u);
            }
            else
            {
                
                db.ProductPrinters.Update(u);
            }
            
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)),hub);
            return Ok(u);


        }

      


        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult<ProductPrinterModel>> DeleteRecord(Guid id) //Delete
        {
            var u = await db.ProductPrinters.FindAsync(id);
            u.is_deleted = !u.is_deleted;
            
            db.ProductPrinters.Update(u);
            await db.SaveChangesAsync();
            return Ok(u);
        }
    }

}
