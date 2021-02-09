using System.Linq;              
using eAPIClient.Models;                     
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;   


namespace eAPIClient.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductPrinterController : ODataController
    {

        private readonly ApplicationDbContext db;
        public ProductPrinterController(ApplicationDbContext _db)
        {
            db = _db;
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        
        public IQueryable<ProductPrinterModel> Get()
        { 


                return db.ProductPrinters;
           
        }
 
    }

}
