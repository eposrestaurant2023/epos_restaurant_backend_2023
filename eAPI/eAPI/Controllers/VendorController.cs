﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using eAPI.Services;
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
    public class VendorController : ODataController
    {

        private readonly ApplicationDbContext db;
        private readonly AppService app;
        public VendorController(ApplicationDbContext _db, AppService _app)
        {
            db = _db;
            app = _app;
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [AllowAnonymous]
        public IQueryable<VendorModel> Get()
        {
           
                return db.Vendors;
           
        }

        
        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] VendorModel u)
        {
            bool is_new = true;
            if (u.id == 0)
            {
                is_new = true;
                string document_number = await app.GetDocumentNumber(21);
                u.vendor_code = document_number;
                db.Vendors.Add(u);
            }
            else
            {
                db.Vendors.Update(u);
            }

            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            if (is_new)
            {
                await app.SaveDocumentNumber(21);
            }
            return Ok(u);
        }


        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult<VendorModel>> DeleteRecord(int id) //Delete
        {
            var u = await db.Vendors.FindAsync(id);
            u.is_deleted = !u.is_deleted;
            
            db.Vendors.Update(u);
            await db.SaveChangesAsync();
            return Ok(u);
        }

        [HttpGet("find")]
        [EnableQuery(MaxExpansionDepth = 4)]
        public SingleResult<VendorModel> Get([FromODataUri] int key)
        {
            var s = db.Vendors.Where(r => r.id == key).AsQueryable();

            return SingleResult.Create(s);
        }
    }

}
