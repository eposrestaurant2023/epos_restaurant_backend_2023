using eAPI.Services;
using eModels;
using eShareModel;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace eAPI.Controllers
{
    [ApiController,Authorize]
    [Route("api/[Controller]")]
    public class ImageController : ControllerBase
    {
        private readonly ApplicationDbContext db;
        private readonly AppService app;
        public ImageController(ApplicationDbContext _db,AppService app)
        {
            db = _db;
            this.app = app;
        }

        [HttpGet]
        [EnableQuery]
        public IQueryable<ImageModel> Get(string keyword="")
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                var image = from r in db.Images
                        where EF.Functions.Like((
                              (r.title ?? " ") +
                              (r.description ?? " ")).ToLower().Trim(), $"%{keyword}%".ToLower().Trim())
                        select r;
                return image;
            }
                var c = db.Images.AsQueryable();
                return c;
        }


        [HttpPost("save")]
        public async Task<ActionResult<ImageModel>> Save([FromBody] ImageModel u)
        {
            if (u.id == Guid.Empty)
            {
                db.Images.Add(u);
            }
            else
            {
                db.Images.Update(u);
            }
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return Ok(u);
        }
        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 0)]
        [Route("find")]
        public SingleResult<ImageModel> Get([FromODataUri] Guid key)
        {
            var c = db.Images.Where(r => r.id == key).AsQueryable();
            return SingleResult.Create(c);
        }
    }
}
