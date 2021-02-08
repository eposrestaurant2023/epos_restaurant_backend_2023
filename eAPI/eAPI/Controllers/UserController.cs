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
    [Authorize]
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
        [AllowAnonymous]
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
            if (u.id.ToString() == "")
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
            if (u.id.ToString() == "")
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
        [Route("SaveUserProfile")]
        public async Task<ActionResult<string>> SaveUserProfile([FromQuery] string password, [FromBody] UserModel u)
        {

            if (!string.IsNullOrEmpty(password))
            {
                u.password = EncryptProvider.Base64Encrypt(password);
            }
            u.role = null;
            db.Users.Update(u);

            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            u = db.Users.Where(r => r.id == u.id).Include(r => r.role).FirstOrDefault();
            return Ok(u);
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
                           a.username  ,
                           a.pin_code,
                           a.password                ,
                              a.date_of_birth
                       };
            return Ok(data);
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [Route("GetByBusinessBranch")]
        [AllowAnonymous]
        public virtual ActionResult<List<UserModel>> GetUserByBusinessBranch([FromQuery] Guid business_branch_id)
        {
            try
            {
                var data = from a in db.Users.Where(r => !r.is_deleted && r.status && r.is_allow_front_end_login)
                           join b in db.UserBusinessBranches.Where(r => r.business_branch_id == business_branch_id) on a.id equals b.user_id
                           select new
                           {    a.id,
                               a.full_name,
                               a.username,
                               a.pin_code,
                               a.password,
                               a.date_of_birth
                           };

                return Ok(data);
            }
            catch
            {
                return NotFound();
            }
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
