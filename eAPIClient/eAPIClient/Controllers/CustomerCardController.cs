using eAPIClient.Models;
using eAPIClient.Services;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eAPIClient.Controllers
{
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
        [EnableQuery(MaxExpansionDepth = 0)]
        [AllowAnonymous]
        public IQueryable<CustomerCardModel> Get()
        {


            return db.CustomerCards;

        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 0)]
        [AllowAnonymous]
        public SingleResult<CustomerCardModel> Get([FromODataUri] Guid key)
        { 
            var s = db.CustomerCards.Where(r => r.id == key).AsQueryable();
            return SingleResult.Create(s); 
        } 
    }
}
