using eModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController:ControllerBase
    {
        private readonly ApplicationDbContext db;
        public SearchController(ApplicationDbContext _db)
        {
            db= _db;
        }
        [HttpGet]
        public ActionResult Get(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                var result = from r in db.Searchs.Take(20)
                             where EF.Functions.Like((
                                     (r.keyword ?? " ")).ToLower().Trim(), $"%{keyword}%".ToLower().Trim())
                               select r;
                return Ok(result);
            }
            return Ok(new SearchModel());
        }
    }
}
