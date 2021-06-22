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
    public class RequestLicenseController : ODataController
    {

        private readonly ApplicationDbContext db;
        public RequestLicenseController(ApplicationDbContext _db)
        {
            db = _db;
        }

     
        [HttpGet]
        [Route("request")]
        public ActionResult<LicenseCodeResponseModel > RequestLicenseCode(Guid business_branch_id , Guid outlet_id, Guid station_id )
        {

            if( business_branch_id == Guid.Empty || outlet_id == Guid.Empty || station_id == Guid.Empty)
            {
                return NotFound();
            }


            var data = db.RequestLicenses.Where(r => r.is_deleted == false && r.business_branch_id == business_branch_id && r.outlet_id == outlet_id && r.station_id == station_id && r.status == true);
            if (data.Any())
            {
                RequestLicenseModel l = data.FirstOrDefault();
                return new LicenseCodeResponseModel() { station_id = station_id, expired_date = l.expired_date.ToString("yyyy-MM-dd"), is_full_license = l.is_full_license };

            }
            else
            {
                RequestLicenseModel l = new RequestLicenseModel();
                l.business_branch_id = business_branch_id;
                l.outlet_id = outlet_id;
                l.station_id = station_id;
                l.expired_date = DateTime.Now.AddMonths(1);
                l.is_full_license = false;
                db.RequestLicenses.Add(l);
                db.SaveChanges();

                return new LicenseCodeResponseModel() { station_id = station_id, expired_date = l.expired_date.ToString("yyyy-MM-dd"), is_full_license = l.is_full_license };
            }

        }

        [HttpPost("save")]
        public async Task<ActionResult<RequestLicenseModel>> Save([FromBody] RequestLicenseModel m)
        { 

            if (m.id == 0)
            {

                db.RequestLicenses.Add(m);
            } 
            else
            {
                 
                db.RequestLicenses.Update(m);
            }

            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return Ok(m);


        }
    }

 public   class LicenseCodeResponseModel
    {
        public Guid station_id { get; set; }
        public string expired_date { get; set; }
        public bool is_full_license { get; set; }
    }
}
