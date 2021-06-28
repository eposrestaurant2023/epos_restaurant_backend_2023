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
using System.Text.Json;

namespace eAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class WorkingDayController : ODataController
    {
        private readonly ApplicationDbContext db;
        public WorkingDayController(ApplicationDbContext _db)
        {
            db = _db; 
        }        

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]   
        public IQueryable<WorkingDayModel> Get(string keyword = "")
        {
            if (!string.IsNullOrEmpty(keyword))
            {
              return (from r in db.WorkingDays
                           where 
                                 EF.Functions.Like(
                                     (
                                        (r.closed_by ?? " ") + 
                                        (r.close_note ?? " ") +
                                        (r.open_note ?? " ") +
                                        (r.working_day_number ?? " ")
                                     ).ToLower().Trim(), $"%{keyword}%".ToLower().Trim())
                           select r);

            }
            else
            {
                return db.WorkingDays.AsQueryable();
            }
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [Route("getsingle")]
        public async Task<SingleResult<WorkingDayModel>> Get([FromODataUri] Guid key)
        {
            return await Task.Factory.StartNew(() => SingleResult.Create<WorkingDayModel>(db.WorkingDays.Where(r => r.id == key).AsQueryable()));
        }

    }
}