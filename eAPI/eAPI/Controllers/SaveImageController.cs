using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace eAPI.Controllers
{

    [ApiController,Route("api/[controller]")]
    public class SaveImageController:ControllerBase
    {
        private readonly IConfiguration config;
        private readonly IWebHostEnvironment environment;
        public SaveImageController(IConfiguration _conf, IWebHostEnvironment env)
        {
            config = _conf;
            environment = env;
        }
        [HttpGet("SaveImageFromUrl")]
        public ActionResult SaveImageFromUrl([FromQuery]string filename)
        {
            string server_url = config.GetValue<string>("apieSoftixUrl");
            server_url = server_url.Replace("/api/", "/");
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    string url = server_url + "upload/" + filename;
                    byte[] dataArr = webClient.DownloadData(url);

                    string img_path = environment.ContentRootPath + "\\Upload\\" + filename;
                    System.IO.File.WriteAllBytes(img_path, dataArr);
                }
                return Ok(true);
            }
            catch (Exception e)
            {
                return Ok(e.InnerException.Message);
            }

        }
    }
}
