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
    public class ProductMenuController : ODataController
    {

        private readonly ApplicationDbContext db;
        public ProductMenuController(ApplicationDbContext _db)
        {
            db = _db;
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [AllowAnonymous]
        public IQueryable<ProductMenuModel> Get()
        { 


                return db.ProductMenus;
           
        }
 
    }

}
