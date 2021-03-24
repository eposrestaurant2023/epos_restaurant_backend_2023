
using System.Linq;
using eAPIClient.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eAPIClient.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class PrefixPriceController : ODataController
    {

        private readonly ApplicationDbContext db;
        public PrefixPriceController(ApplicationDbContext _db)
        {
            db = _db;
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)] 
        public IQueryable<PrefixPriceModel> Get()
        {
            return db.PrefixPrices;  
        }

        [HttpGet("find")]
        [EnableQuery(MaxExpansionDepth = 4)]
        public SingleResult<PrefixPriceModel> Get([FromODataUri] int key)
        {
            var s = db.PrefixPrices.Where(r => r.id == key).AsQueryable();

            return SingleResult.Create(s);
        }
    }

}
