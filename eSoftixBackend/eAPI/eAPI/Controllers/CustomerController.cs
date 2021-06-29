using eAPI.Services;
using eModels;
using eShareModel;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace eAPI.Controllers
{
    [ApiController,Authorize]
    [Route("api/[Controller]")]
    public class CustomerController:ControllerBase
    {
        private readonly ApplicationDbContext db;
        private readonly AppService app;
        public CustomerController(ApplicationDbContext _db,AppService app)
        {
            db = _db;
            this.app = app;
        }

        [HttpGet]
        [EnableQuery]
        public IQueryable<CustomerModel> Get(string keyword="")
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                var customer = from r in db.Customers
                        where EF.Functions.Like((
                              (r.customer_name_en ?? " ") +
                              (r.customer_code ?? " ") +
                              (r.customer_name_kh ?? " ") +
                              (r.note ?? " ") +
                              (r.position ?? " ") +
                              (r.customer_group.customer_group_name_en ?? " ") +
                              (r.customer_group.customer_group_name_kh ?? " ")).ToLower().Trim(), $"%{keyword}%".ToLower().Trim())
                        select r;
                return customer;
            }
                var c = db.Customers.AsQueryable();
                return c;
        }
        [HttpPost,Route("save")]

        public async Task<ActionResult<CustomerModel>> Save([FromBody] CustomerModel p,[FromQuery] bool allow_duplicate_name)
        {
            p.customer_name_kh = string.IsNullOrEmpty(p.customer_name_kh) ? p.customer_name_en : p.customer_name_kh;
            if (allow_duplicate_name)
            {
                List<CustomerModel> check_data = db.Customers.Where(r => r.customer_name_en.Trim().ToLower() == p.customer_name_kh.Trim().ToLower() && r.id != p.id).Include(r => r.customer_group).ToList();
                if (check_data.Any())
                {
                    //return error code 401 for Customer name (En) is already exists.
                    return StatusCode(401, new ApiResponseModel() { message = $"Customer name En ({p.customer_name_en}) is duplicate with other customer.", customers = check_data.ToList() });
                }
                //chekc customer name kh
                check_data = db.Customers.Where(r => r.customer_name_kh.Trim().ToLower() == p.customer_name_en.Trim().ToLower() && r.id != p.id).Include(r => r.customer_group).ToList();
                if (check_data.Any())
                {
                    //return error code 401 for Customer name (En) is already exists.
                    return StatusCode(401, new ApiResponseModel() { message = $"Customer name en ({p.customer_name_en}) is duplicate with other customer.", customers = check_data.ToList() });
                }
                //check customer name kh from model
                check_data = db.Customers.Where(r => r.customer_name_kh.Trim().ToLower() == p.customer_name_kh.Trim().ToLower() && r.id != p.id).Include(r => r.customer_group).ToList();
                if (check_data.Any())
                {
                    //return error code 401 for Customer name (En) is already exists.
                    return StatusCode(401, new ApiResponseModel() { message = $"Customer name kh ({p.customer_name_en}) is duplicate with other customer.", customers = check_data.ToList() });
                }
                //chekc customer name kh
                check_data = db.Customers.Where(r => r.customer_name_kh.Trim().ToLower() == p.customer_name_en.Trim().ToLower() && r.id != p.id).Include(r => r.customer_group).ToList();
                if (check_data.Any())
                {
                    //return error code 401 for Customer name (En) is already exists.
                    return StatusCode(401, new ApiResponseModel() { message = $"Customer name kh ({p.customer_name_en}) is duplicate with other customer.", customers = check_data.ToList() });
                }
            }
            p.customer_group = null;


            if (p.id == Guid.Empty)
            {
                p.customer_code = await app.GetDocumentNumber(1);
                await app.SaveDocumentNumber(1);
                db.Customers.Add(p);
            }
            else
            {
                db.Customers.Update(p);
            }
            
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return Ok(p);
        }
        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 0)]
        [Route("find")]

        public SingleResult<CustomerModel> Get([FromODataUri] Guid key)
        {
            var c = db.Customers.Where(r => r.id == key).AsQueryable();
            return SingleResult.Create(c);
        }
        [HttpPost]
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var c = await db.Customers.FindAsync(id);
            c.is_deleted = !c.is_deleted;
            db.Customers.Update(c);
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return Ok(c);
        }

        [HttpPost]
        [EnableQuery(MaxExpansionDepth = 0)]
        [Route("clone/{id}")]

        public  IActionResult  Clone(Guid id)
        {
            var c = db.Customers.Where(r=>r.id == id).Include(r=>r.contacts).FirstOrDefault();
            c.status = true;
            c.contacts.ForEach(r => { r.id = 0;r.customer_id = Guid.Empty; });
            c.id = Guid.Empty;
            return Ok(c);
        }

        [HttpPost]
        [EnableQuery(MaxExpansionDepth = 0)]
        [Route("status/{id}")]

        public IActionResult Status(Guid id)
        {
            var c = db.Customers.Find(id);
            c.status = !c.status;
            db.Customers.Update(c);
            SaveChange.Save(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return Ok(c);
        }
    }
}
