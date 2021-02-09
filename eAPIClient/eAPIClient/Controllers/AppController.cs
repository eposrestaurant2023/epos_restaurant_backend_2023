using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using eModels;
////using eAPIClient.Models;using eModels;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NETCore.Encrypt;


namespace eAPIClient.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AppController : ODataController
    {

        public IConfiguration Configuration { get; }


        private readonly ApplicationDbContext db;
        public AppController(ApplicationDbContext _db, IConfiguration configuration)
        {
            db = _db;
            Configuration = configuration;
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        
        public IQueryable<UserModel> Get(string keyword = "")
        { 
                return db.Users;
           
        }





        [HttpPost]
        [EnableQuery(MaxExpansionDepth = 8)]
        [Route("auth/login")]
        [AllowAnonymous]
        public virtual async Task<ActionResult<UserModel>> Post([FromBody] AuthenticateModel u)
        {
            var pass_encr = EncryptProvider.Base64Encrypt(u.Password);
            var data = await Task.Factory.StartNew(() => db.Users.Where(r => r.username == u.Username && r.password == pass_encr).AsQueryable());
            if (data == null || data.Count() <= 0)
            {
                return NotFound();
            }
            UserModel user = data.FirstOrDefault();
            return Ok(data.FirstOrDefault());
        }

        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromQuery] string password, [FromBody] UserModel u)
        {
           
            if (u.id.ToString() == "")
            {

                db.Users.Add(u);
            }
            else
            {
               
                db.Users.Update(u);
            }
            
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return Ok(u);
        }
 
        [HttpGet]
        [Route("get_user")]
        public UserModel GetCurrentUser([FromQuery] int user_id)
        {


            var data = db.Users;
            return data.FirstOrDefault();


        }

        

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [Route("find")]
        public SingleResult<UserModel> Get([FromODataUri] int key)
        {
            var c = db.Users.Where(r => r.id == key).AsQueryable();
            return SingleResult.Create(c);
        }
         


    }

}
