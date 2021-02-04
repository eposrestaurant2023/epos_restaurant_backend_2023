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
    public class ProductController : ODataController
    {

        private readonly ApplicationDbContext db;
        public ProductController(ApplicationDbContext _db)
        {
            db = _db;
        }



        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        public IQueryable<ProductModel> Get(string keyword = "")
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                return db.Products.Where(r =>
                (
                (r.product_code ?? "") +
                (r.product_name_en ?? "") +
                (r.product_name_kh ?? "") 
                ).ToLower().Trim().Contains(keyword.ToLower().Trim()));
            }
            else
            {
                return db.Products;
            }
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [Route("getsingle")]
        public async Task<SingleResult<ProductModel>> Get([FromODataUri] int key)
        {
            return await Task.Factory.StartNew(() => SingleResult.Create<ProductModel>(db.Products.Where(r => r.id == key).AsQueryable()));
        }


        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] ProductModel u)
        {
             


            u.product_modifiers.Where(r => r.modifier_id > 0).ToList().ForEach(r => r.modifier = null);
            u.product_menus.ForEach(r => r.menu = null);
            string xx = JsonSerializer.Serialize(u);
            

            if (u.id == 0)
            {

                db.Products.Add(u);
            }
            else
            {
                
                db.Products.Update(u);
            }
            
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));

            db.Database.ExecuteSqlRaw("exec sp_clear_deleted_record");
            return Ok(u);


        }

      


        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult> DeleteRecord(int id) //Delete
        {
            var u = await db.Products.FindAsync(id);
            u.is_deleted = !u.is_deleted;
            
            db.Products.Update(u);
            await db.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        [Route("ChangeStatus/{id}")]
        public async Task<ActionResult> ChangeStatus(int id) //Delete
        {
            var u = await db.Products.FindAsync(id);
            u.status= !u.status;
            
            db.Products.Update(u);
            await db.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        [Route("Clone/{id}")]
        public async Task<ActionResult<ProductModel>> Clone(int id) //Delete
        {
            var data =   db.Products.Where(r => r.id == id)
                .Include(r=>r.product_portions.Where(r=>r.is_deleted==false)).ThenInclude(r=>r.product_prices.Where(r=>r.is_deleted==false))
                .Include(r=>r.product_printers.Where(r=>r.is_deleted==false)).
                Include(r=>r.product_menus.Where(r=>r.is_deleted==false)).ThenInclude(r=>r.menu)
                .Include(r=>r.product_modifiers.Where(r=>r.is_deleted ==false)).ThenInclude(r=>r.modifier)
                .ToList();
             
            if (data.Any())
            {
                ProductModel p = data.FirstOrDefault();
                p.id = 0;
                p.product_portions.ForEach(r => r.id = 0);
                p.product_portions.SelectMany(r=>r.product_prices).ToList().ForEach(r => r.id = 0);
                p.product_printers.ForEach(r => r.id = 0);
                p.product_menus.ForEach(r => r.id = 0);
                p.product_menus.ForEach(r => r.product_id = 0);
                p.product_menus.ForEach(r => r.menu.menus = new List<MenuModel>());
                p.product_modifiers.ForEach(r => { r.id = 0; r.product_id = 0; });
                 
                return Ok(p);
            }
            else
            {
                return NotFound();
            }
            
        }


    }

}
