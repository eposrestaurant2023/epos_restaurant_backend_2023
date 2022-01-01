using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using eAPI.Hubs;
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
    public class ProductIngredientController : ODataController
    {

        private readonly ApplicationDbContext db;
        private readonly IHubContext<ConnectionHub> hub;
        public ProductIngredientController(ApplicationDbContext _db,IHubContext<ConnectionHub> _hub)
        {
            db = _db;hub = _hub;
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [AllowAnonymous]      
        public IQueryable<ProductIngredientModel> Get(string keyword ="" )
        {
            return db.ProductIngredients;
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [Route("getsingle")]
        public async Task<SingleResult<ProductIngredientModel>> GetQuery([FromODataUri] int key)
        {
            return await Task.Factory.StartNew(() => SingleResult.Create<ProductIngredientModel>(db.ProductIngredients.Where(r => r.id == key).AsQueryable()));
        }

        [HttpPost("multiple/save")]
        public async Task<ActionResult<string>> Save([FromBody] List<ProductIngredientModel> pis)
        { 

            db.ProductIngredients.AddRange(pis.Where(r=>r.id == 0).ToList());
            db.ProductIngredients.UpdateRange(pis.Where(r=>r.id > 0).ToList());
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)),hub);
            return Ok(pis);
        }

        [HttpGet("find")]
        [EnableQuery(MaxExpansionDepth = 4)]
        public SingleResult<ProductIngredientModel> Get([FromODataUri] int key)
        {
            var s = db.ProductIngredients.Where(r => r.id == key).AsQueryable();
            return SingleResult.Create(s);
        }

        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult<ProductIngredientModel>> DeleteRecord(int id) //Delete
        {
            var u = await db.ProductIngredients.FindAsync(id);
            u.is_deleted = !u.is_deleted;            
            db.ProductIngredients.Update(u);
            await db.SaveChangesAsync();
            return Ok(u);
        }

        [HttpPost]
        [Route("clone/{id}")]
        public async Task<ActionResult<ProductIngredientModel>> CloneRecord(int id) //Delete
        {
            var u = await db.ProductIngredients.FindAsync(id);
            u.id = 0;
            u.created_date = DateTime.Now;
            db.ProductIngredients.Add(u);
            await db.SaveChangesAsync();
            return Ok(u);
        }
    }
}
