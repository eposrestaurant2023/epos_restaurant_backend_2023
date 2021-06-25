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
    public class BusinessBranchCurrencyController : ODataController
    {

        private readonly ApplicationDbContext db;
        public BusinessBranchCurrencyController(ApplicationDbContext _db)
        {
            db = _db;
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [AllowAnonymous]      
        public IQueryable<BusinessBranchCurrencyModel> Get()
        {
           
                return db.BusinessBranchCurrencies;
           
        }

 
        [HttpPost("save/multiple")]
        public async Task<ActionResult<string>> SaveMultiple([FromBody] List<BusinessBranchCurrencyModel> bbc)
        {
            db.BusinessBranchCurrencies.UpdateRange(bbc);
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return Ok(bbc);
        }
    }

}