using eModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eAPI.Controllers
{
    [ApiController,Route("api/[Controller]"),]
    public class AttachFileController:ControllerBase
    {
        private readonly ApplicationDbContext db;
        public AttachFileController(ApplicationDbContext db)
        {
            this.db = db;
        }
        [HttpGet]
        public IQueryable<AttachFileModel> Get()
        {
            return db.AttachFiles;
        }
    }
}
