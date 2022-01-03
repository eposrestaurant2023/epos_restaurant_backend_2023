using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
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
    public class ModifierIngredientController : ODataController
    {

        private readonly ApplicationDbContext db;
        private readonly IHubContext<ConnectionHub> hub;
        public ModifierIngredientController(ApplicationDbContext _db,IHubContext<ConnectionHub> _hub)
        {
            db = _db;hub = _hub;
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        public IQueryable<ModifierIngredientModel> Get(string keyword = "")
        {
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var c = from r in db.ModifierIngredients
                        where EF.Functions.Like((
                            (r.modifier.modifier_name ?? "")
                    ).ToLower().Trim(), $"%{keyword}%".ToLower().Trim())
                        select r;
                return c.AsQueryable();


            }
            else
            {
                return db.ModifierIngredients;
            }
            
        }
    }
}
