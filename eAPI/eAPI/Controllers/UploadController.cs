
using eAPI;
using eAPI.Hubs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace eAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadController : ControllerBase
    {
        private readonly ApplicationDbContext db;
        private readonly IHubContext<ConnectionHub> hub;

        private readonly IWebHostEnvironment environment;
        public UploadController(IWebHostEnvironment environment, ApplicationDbContext _db, IHubContext<ConnectionHub> _hub)
        {
            this.environment = environment;
            db = _db;hub = _hub;
        }


        [HttpPost]
        public async Task<OkResult> Post(string folder = "")
        {
            var root_image = await db.Settings.FindAsync(1);

            //===============================Note
            string directory_path = this.environment.ContentRootPath + "\\upload\\";


            if (!Directory.Exists(directory_path))
            {
                Directory.CreateDirectory(directory_path);
            }


            if (HttpContext.Request.Form.Files.Any())
            {
                foreach (var file in HttpContext.Request.Form.Files)
                {
                    var path = Path.Combine(directory_path, file.FileName);
                    using (var stream = System.IO.File.Create(path))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
            }
            return Ok();
        }
    }
}
