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
    public class SystemFeatureController : ODataController
    {

        private readonly ApplicationDbContext db;
        public SystemFeatureController(ApplicationDbContext _db)
        {
            db = _db;
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [AllowAnonymous]

        public IQueryable<SystemFeatureModel> Get()
        {

            return db.system_features;

        }

        [HttpGet("find")]
        [EnableQuery(MaxExpansionDepth = 4)]
        public SingleResult<SystemFeatureModel> Get([FromODataUri] Guid key)
        {
            var s = db.system_features.Where(r => r.id == key).AsQueryable();

            return SingleResult.Create(s);
        }
    }
}