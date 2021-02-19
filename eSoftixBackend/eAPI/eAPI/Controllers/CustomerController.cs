using eAPI.Services;
using eModels;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace eAPI.Controllers
{
    [ApiController,Authorize]
    [Route("api/[Controller]")]
    public class CustomerController:ODataController
    {
        private readonly ApplicationDbContext db;
        private readonly AppService app;
        public CustomerController(ApplicationDbContext _db,AppService app)
        {
            db = _db;
            this.app = app;
        }

        [HttpGet]
        public async Task<ActionResult> Get(string keyword="")
        {
            var c = db.Customers.ToList();
            return Ok(c);
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
            
            if (p.id == 0)
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

        public SingleResult<CustomerModel> Get([FromODataUri] int key)
        {
            var c = db.Customers.Where(r => r.id == key).AsQueryable();
            return SingleResult.Create(c);
        }

    }
}
