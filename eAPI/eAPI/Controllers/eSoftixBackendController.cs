﻿using DeviceId;
using eAPI.Hubs;
using eAPI.Services;
using eModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;     
namespace eAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class eSoftixBackendController : ControllerBase
    {

        public IConfiguration Configuration { get; }
        private readonly ApplicationDbContext db;
        private readonly IHttpService http;
        private readonly BackendSyncService backend;
        private readonly IHubContext<ConnectionHub> hub;


        eSoftixBackend.ProjectModel project = new eSoftixBackend.ProjectModel();




        public eSoftixBackendController(ApplicationDbContext _db, IConfiguration configuration, IHttpService _http, BackendSyncService _backend, IHubContext<ConnectionHub> _hub)
        {
            db = _db;hub = _hub;
            Configuration = configuration;
            http = _http;
            backend = _backend;
        }

        

        [HttpGet("CheckSystemFeatures")]
        [AllowAnonymous]
        public async Task<ActionResult<bool>> CheckSystemFeatures()
        {
            //get project 

            var result = await backend.CheckSystemFeatures();
            return result;


        }


    
    }
}
