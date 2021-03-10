using System.Collections.Generic;
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
    public class UserController : ODataController
    {

        private readonly ApplicationDbContext db;
        public UserController(ApplicationDbContext _db)
        {
            db = _db;
        }   
        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        
        public List<UserModel> Get()
        {

            var data = db.ConfigDatas.Find(1);

                return System.Text.Json.JsonSerializer.Deserialize<List<UserModel>>(data.data).ToList();
           
        }       
    }

}
