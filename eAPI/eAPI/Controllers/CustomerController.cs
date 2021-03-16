using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using eAPI.Services;
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
    public class CustomerController : ODataController
    {

        private readonly ApplicationDbContext db;
        private readonly AppService app;
        public CustomerController(ApplicationDbContext _db, AppService _app)
        {
            db = _db;
            app = _app;
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        public IQueryable<CustomerModel> Get(string keyword = "")
        {
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var c = from r in db.Customers
                        where EF.Functions.Like((
                            (r.customer_code ?? "") +
                            (r.customer_name_en ?? "") +
                            (r.customer_name_kh ?? "") +
                            (r.phone_1 ?? "") +
                            (r.phone_2 ?? "") +
                            (r.gender ?? "") +
                            (r.note ?? "")
                    ).ToLower().Trim(), $"%{keyword}%".ToLower().Trim())
                        select r;
                return c.AsQueryable();
            }
            else
            {
                return db.Customers;
            }
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [Route("getsingle")]
        public async Task<SingleResult<CustomerModel>> Get([FromODataUri] Guid key)
        {
            return await Task.Factory.StartNew(() => SingleResult.Create<CustomerModel>(db.Customers.Where(r => r.id == key).AsQueryable()));
        }

        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] CustomerModel p, bool allow_duplicate_name = false)
        {

            bool is_new = true;
            p.customer_name_kh = string.IsNullOrEmpty(p.customer_name_kh) ? p.customer_name_en : p.customer_name_kh;
            var dup = db.Customers.Where(r => r.customer_code == p.customer_code);
            //check validation customer duplicate 

            //check if customer name is duplicate 
            if (1==2)
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

                //====================
                //check phone number 
                if ((p.phone_1 ?? "") != "")
                {
                    check_data = db.Customers.Where(r =>
                    (
                        (r.phone_1 ?? "").Replace(" ", "").Replace("-", "").Trim().ToLower() == (p.phone_1 ?? "").Replace(" ", "").Replace("-", "").Trim().ToLower() ||
                        (r.phone_2 ?? "").Replace(" ", "").Replace("-", "").Trim().ToLower() == (p.phone_1 ?? "").Replace(" ", "").Replace("-", "").Trim().ToLower()

                    ) &&

                    r.id != p.id
                    ).Include(r => r.customer_group).ToList();
                    if (check_data.Any())
                    {
                        return StatusCode(401, new ApiResponseModel() { message = $"Phone Number is duplicate other customer.", customers = check_data.ToList() });
                    }
                }

                if ((p.phone_1 ?? "") != "")
                {
                    check_data = db.Customers.Where(r =>
                    (
                        (r.phone_1 ?? "").Replace(" ", "").Replace("-", "").Trim().ToLower() == (p.phone_2 ?? "").Replace(" ", "").Replace("-", "").Trim().ToLower() ||
                        (r.phone_2 ?? "").Replace(" ", "").Replace("-", "").Trim().ToLower() == (p.phone_2 ?? "").Replace(" ", "").Replace("-", "").Trim().ToLower()
                    ) &&

                    r.id != p.id
                    ).Include(r => r.customer_group).ToList();
                    if (check_data.Any())
                    {
                        return StatusCode(401, new ApiResponseModel() { message = $"Phone Number is duplicate other customer.", customers = check_data.ToList() });
                    }
                }

            }
            
            if (p.id == new Guid())
            {
                if (dup.Count() > 0)
                {
                    return StatusCode(301, "Customer Code already exist!");
                }
                is_new = true;
                string document_number = await app.GetDocumentNumber(19);
                p.customer_code = document_number;
                db.Customers.Add(p);
            }
            else
            {
                //var sss = JsonSerializer.Serialize(db.Database.ExecuteSqlRaw($"delete tbl_customer_business_branch where customer_id = {p.id}"));
                db.Database.ExecuteSqlRaw($"delete tbl_customer_business_branch where customer_id = '{p.id}'");                
                db.CustomerBusinessBranches.AddRange(p.customer_business_branchs);

                db.Customers.Update(p);
            }


            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            if (is_new)
            {
                await app.SaveDocumentNumber(19);
            }

            return Ok(p);
        }

        [HttpPost]
        [Route("status/{id}")]
        public async Task<ActionResult<CustomerModel>> UpdateStatus(Guid id)
        {
            var d = await db.Customers.FindAsync(id);
            d.status = !d.status;
            db.Customers.Update(d);
            await db.SaveChangesAsync();
            return Ok(d);
        }


        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult<CustomerModel>> DeleteRecord(Guid id) //Delete
        {
            var u = await db.Customers.FindAsync(id);
            u.is_deleted = !u.is_deleted;
            db.Customers.Update(u);
            await db.SaveChangesAsync();
            return Ok(u);
        }
    }
}
