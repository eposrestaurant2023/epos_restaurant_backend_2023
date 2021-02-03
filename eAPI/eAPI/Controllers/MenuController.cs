﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using eModels;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NETCore.Encrypt;


namespace eAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class MenuController : ODataController
    {

        private readonly ApplicationDbContext db;
        public MenuController(ApplicationDbContext _db)
        {
            db = _db;
        }



        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        public IQueryable<MenuModel> Get(string keyword = "")
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                return db.Menus.Where(r =>
                (
               
                (r.menu_name_en ?? "") +
                (r.menu_name_kh?? "") 
                ).ToLower().Trim().Contains(keyword.ToLower().Trim()));
            }
            else
            {
                return db.Menus;
            }
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [Route("getsingle")]
        public async Task<SingleResult<MenuModel>> Get([FromODataUri] int key)
        {
            return await Task.Factory.StartNew(() => SingleResult.Create<MenuModel>(db.Menus.Where(r => r.id == key).AsQueryable()));
        }


        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] MenuModel u)
        {

            u.parent = null;
            if (u.id == 0)
            {

                db.Menus.Add(u);
            }
            else
            {
                
                db.Menus.Update(u);
            }
            
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));

            db.Database.ExecuteSqlRaw("exec sp_clear_deleted_record");
            db.Database.ExecuteSqlRaw("exec sp_update_menu_path");
            
            return Ok(db.Menus.Find(u.id));


        }

      


        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult<MenuModel>> DeleteRecord(Guid id) //Delete
        {
            var u = await db.Menus.FindAsync(id);
            u.is_deleted = !u.is_deleted;
            
            db.Menus.Update(u);
            await db.SaveChangesAsync();
            return Ok(u);
        }
    }

}