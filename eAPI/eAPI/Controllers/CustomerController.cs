using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using eAPI.Hubs;
using eAPI.Services;
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
    public class CustomerController : ControllerBase
    {

        private readonly ApplicationDbContext db;
        private readonly IHubContext<ConnectionHub> hub;
        private readonly AppService app;
        public CustomerController(ApplicationDbContext _db, AppService _app, IHubContext<ConnectionHub> _hub)
        {
            db = _db;hub = _hub;
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
        public async Task<ActionResult<string>> Save([FromBody] CustomerModel p, bool allow_duplicate_name = false,bool is_synch_from_client=false)
        {

            bool is_new = true;

            p.customer_name_kh = string.IsNullOrEmpty(p.customer_name_kh) ? p.customer_name_en : p.customer_name_kh;
            var modelCheck = db.Customers.Where(r => r.id == p.id).AsNoTracking().Include(r => r.customer_business_branchs).AsNoTracking();
            var old_customer = modelCheck.FirstOrDefault();

            if ((p.customer_code??"").ToLower() == "new" || p.customer_code == "")
            {
                string document_number = await app.GetDocumentNumber(19);
                p.customer_code = document_number;
                is_new = true;
            }

            if (old_customer == null)
            {
                db.Customers.Add(p);


            }
            else
            {

                var d = modelCheck.FirstOrDefault();


                db.Customers.Update(p);
            }

            if (p.customer_business_branchs != null && p.customer_business_branchs.Any())
            {
                db.Database.ExecuteSqlRaw($"delete tbl_customer_business_branch where customer_id = '{p.id}'");
                db.CustomerBusinessBranches.AddRange(p.customer_business_branchs);
            }
            else
            {
                if (p.business_branch_id != null)
                {
                    if (old_customer != null)
                    {
                        if (!old_customer.customer_business_branchs.Where(r => r.business_branch_id == p.business_branch_id).Any())
                        {
                            if (!p.customer_business_branchs.Where(r => r.business_branch_id == p.business_branch_id).Any())
                            {
                                p.customer_business_branchs.Add(new CustomerBusinessBranchModel() { business_branch_id = p.business_branch_id ?? Guid.Empty });
                            }

                        }
                    }
                    else
                    {
                        p.customer_business_branchs.Add(new CustomerBusinessBranchModel() { business_branch_id = p.business_branch_id ?? Guid.Empty });
                    }

                }
            }

            try
            {

                await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)),hub);

                db.Database.ExecuteSqlRaw($"exec sp_update_customer_information '{p.id}'");
                if (is_synch_from_client)
                {
                    db.Database.ExecuteSqlRaw($"exec sp_update_sync_status '{p.id}','{p.last_update_business_branch_id}'");
                }else
                {
                    db.Database.ExecuteSqlRaw($"exec sp_update_sync_status '{p.id}',''");
                }

                

                if (is_new)
                {
                    await app.SaveDocumentNumber(19);
                }

                return Ok(p);
            }
            catch (Exception e)
            {
                var message = new ApiResponseModel(e.Message,new List<CustomerModel>(),new List<VendorModel>());
                return BadRequest(message);
            }
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


            db.Database.ExecuteSqlRaw($"exec sp_update_customer_information_after_delete '{id}'");

            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)),hub);
            return Ok(u);
        }
    }
}
