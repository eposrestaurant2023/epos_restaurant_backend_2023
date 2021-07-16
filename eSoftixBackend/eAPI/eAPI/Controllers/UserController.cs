using System;
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
    [Route("api/[controller]")]
    public class UserController : ODataController
    {

        private readonly ApplicationDbContext db;
        public UserController(ApplicationDbContext _db)
        {
            db = _db;
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        
        public IQueryable<UserModel> Get(string keyword = "")
        {
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                return db.Users.Where(r =>
                (
                (r.full_name ?? "") +
                (r.username ?? "") +
             
                (r.email ?? "")
                ).ToLower().Trim().Contains(keyword.ToLower().Trim()));
            }
            else
            {
                return db.Users;
            }
        }

        [HttpPost]
        [EnableQuery(MaxExpansionDepth = 8)]
        [Route("auth/login")]
        public virtual async Task<ActionResult<UserModel>> Post([FromBody] AuthenticateModel u)
        {
            var pass_encr = EncryptProvider.Base64Encrypt(u.Password);
            var data = await Task.Factory.StartNew(() => db.Users.Where(r => r.username == u.Username && r.password == pass_encr && !r.is_deleted && r.status).Include(r => r.role).AsQueryable());
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
            //check if user duplicate
            if (u.id == 0)
            {
                var user = db.Users.Where(r => r.username.ToLower().Trim() == u.username.ToLower().Trim() && r.is_deleted == false);
                if (user.Count() > 0)
                {
                    return StatusCode(301, $"Username {u.username} is already exists.");
                }
            }
            else
            {
                var user = db.Users.Where(r => r.username.ToLower().Trim() == u.username.ToLower().Trim() && r.id != u.id && r.is_deleted == false);
                if (user.Count() > 0)
                {
                    return StatusCode(301, $"Username {u.username} is already exists.");
                }

            }
            if (!string.IsNullOrEmpty(password))
            {
                u.password = EncryptProvider.Base64Encrypt(password);
            }
            if (u.id == 0)
            {

                db.Users.Add(u);
            }
            else
            {
                u.role = null;
                db.Users.Update(u);
            }
            
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return Ok(u);


        }
        [HttpPost]
        [Route("changepassword")]
        public async Task<ActionResult<string>> SaveUserProfile([FromBody] UserModel u)
        {
            var user = db.Users.Find(u.id);
            db.Entry<UserModel>(user).State = EntityState.Detached;
            if (u.new_password.Length < 6)
            {
                return BadRequest("Password Must More Than 6 digits");
            }
            else
            {
                if (EncryptProvider.Base64Encrypt(u.current_password) != user.password)
                {
                    return BadRequest("Invalid Current Password");
                }
                else
                {
                    if (u.new_password != u.retype_new_password)
                    {
                        return BadRequest("Confirm Password Not Match");
                    }
                    else
                    {
                        u.password = EncryptProvider.Base64Encrypt(u.new_password);
                        db.Users.Update(u);
                        await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
                        return Ok(u);
                    }
                }
            }
            
        }

        [HttpGet]
        [Route("get_user")]
        public UserModel GetCurrentUser([FromQuery] int user_id)
        {
            var data = db.Users.Where(r => r.id == user_id && !r.is_deleted && r.status).Include(r => r.role).Take(1);

            return data.FirstOrDefault();
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [Route("alluser")]
        public virtual async Task<ActionResult<List<UserModel>>> AllUser([FromQuery] int user_id)
        {

            await Task.Delay(100);
            var data = from a in db.Users.Where(r => !r.is_deleted && r.status)
                       select new
                       {
                           a.full_name,
                           a.username
                       };
            return Ok(data);
        }

        [HttpPost]
        [Route("status/{id}")]
        public async Task<ActionResult<UserModel>> UpdateStatus(int id)
        {
            var d = await db.Users.FindAsync(id);
            d.status = !d.status;
            db.Users.Update(d);
            await db.SaveChangesAsync();
            return Ok(d);
        }

        [HttpPost]
        [Route("clone/{id}")]
        public async Task<ActionResult<UserModel>> Clone(int id)
        {
            var d = await db.Users.FindAsync(id);
            d.status = true;
            d.id = 0;
            return Ok(d);
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [Route("find")]
        public SingleResult<UserModel> Get([FromODataUri] int key)
        {
            var c = db.Users.Where(r => r.id == key).AsQueryable();
            return SingleResult.Create(c);
        }

        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult<UserModel>> DeleteRecord(int id) //Delete
        {
            var u = await db.Users.FindAsync(id);
            u.is_deleted = !u.is_deleted;
            if (!u.is_deleted)
            {
                var user = db.Users.Where(r => r.is_deleted == false && r.username.Trim().ToLower() == u.username.ToLower().Trim() && r.id != id);
                if (user.Count() > 0)
                {
                    return StatusCode(301, $"This user is already exists.");
                }
            }
            db.Users.Update(u);
            await db.SaveChangesAsync();
            return Ok(u);
        }
    }

}
