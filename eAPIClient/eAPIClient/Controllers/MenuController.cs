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
    public class MenuController : ODataController
    {

        private readonly ApplicationDbContext db;
        public MenuController(ApplicationDbContext _db)
        {
            db = _db;
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth =0)]
        [AllowAnonymous]
        public IQueryable<MenuModel> Get()
        { 


                return db.Menus;
           
        }
 
    }

}
