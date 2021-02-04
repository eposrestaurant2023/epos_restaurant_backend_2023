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
using NETCore.Encrypt;
using eAPIClient.Services;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace eAPIClient.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SyncController : ODataController
    {

        private readonly ApplicationDbContext db;
        private readonly IHttpService http;
        private readonly IConfiguration config;
        public SyncController(ApplicationDbContext _db, IHttpService _http, IConfiguration _config)
        {
            db = _db;
            http = _http;
            config = _config;
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        
        public IQueryable<UserModel> Get()
        { 
                return db.Users;
        }


        [HttpPost("GetRemoteData")]
        [AllowAnonymous]
        public async Task<ActionResult> GetRemoteData()
        {
            var resp = await http.ApiGetOData($"ConfigData?$filter=business_branch_id eq {config.GetValue<string>("business_branch_id")}");
            if (resp.IsSuccess)
            {
                var data = JsonSerializer.Deserialize<List<ConfigDataModel>>(resp.Content.ToString());
                foreach(var d in data)
                {
                     
                    if (db.ConfigDatas.Where(r => r.id == d.id).Any())
                    {
                        db.ConfigDatas.Update(d);
                    }else
                    {
                        db.ConfigDatas.Add(d);
                    }
                }
                  

                db.SaveChanges();
            }

            return Ok();
             
        }


    }

}
