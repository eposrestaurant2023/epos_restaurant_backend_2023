using System;
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
    public class CashierShiftController : ODataController
    {
        private readonly ApplicationDbContext db;
        public CashierShiftController(ApplicationDbContext _db)
        {
            db = _db; 
        }        

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]   
        public IQueryable<CashierShiftModel> Get(string keyword = "")
        {
            if (!string.IsNullOrEmpty(keyword))
            {
              return (from r in db.CashierShifts
                           where 
                                 EF.Functions.Like(
                                     (
                                        (r.closed_by ?? " ") + 
                                        (r.close_note ?? " ") +
                                        (r.open_note ?? " ") +
                                        (r.cashier_shift_number ?? " ")+
                                        (r.close_note ?? " ")
                                     ).ToLower().Trim(), $"%{keyword}%".ToLower().Trim())
                           select r);

            }
            else
            {
                return db.CashierShifts.AsQueryable();
            }
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [Route("getsingle")]
        public async Task<SingleResult<CashierShiftModel>> Get([FromODataUri] Guid key)
        {
            return await Task.Factory.StartNew(() => SingleResult.Create<CashierShiftModel>(db.CashierShifts.Where(r => r.id == key).AsQueryable()));
        }

    }
}