using eAPIClient.Models;
using eAPIClient.Services;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eAPIClient.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CustomerCardController : ODataController
    {

        private readonly ApplicationDbContext db;
        private readonly AppService app;
        private readonly ISyncService sync;

        public CustomerCardController(ApplicationDbContext _db, AppService _app, ISyncService sync)
        {
            db = _db;
            app = _app;
            this.sync = sync;
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)] 
        public IQueryable<CustomerCardModel> Find(string keyword)
        {
            
            if (string.IsNullOrEmpty(keyword))
            {
                return db.CustomerCards;

            }
            else
            {
                var data = from r in db.CustomerCards
                           where
                                !r.is_deleted && r.status &&
                                 EF.Functions.Like(
                                     (
                                        (r.card_code ?? " ") +
                                        (r.customer.customer_code ?? " ") +
                                        (r.customer.customer_name_en ?? " ") +
                                        (r.customer.phone_1 ?? " ") +
                                        (r.customer.phone_2 ?? " ") +
                                        (r.customer.customer_name_kh ?? " ")
                                     ).ToLower().Trim(), $"%{keyword}%".ToLower().Trim())
                           select r;

                return data;

            } 
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 4)]
        [Route("[action]/{code}")]
        public SingleResult<CustomerCardModel> Get(string code)
        {
            var s = db.CustomerCards.Where(r => r.card_code == code && r.status && !r.is_deleted).AsQueryable();
            return SingleResult.Create(s);
        }


    }
}
