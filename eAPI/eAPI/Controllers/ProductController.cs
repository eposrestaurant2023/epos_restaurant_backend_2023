﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;    
using System.Threading.Tasks;    
using eModels;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
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
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var c = from r in db.Products
                        where EF.Functions.Like((
                            (r.product_code ?? "") +
                            (r.product_name_en ?? "") +
                            (r.product_name_kh ?? "")
                    ).ToLower().Trim(), $"%{keyword}%".ToLower().Trim())
                        select r;
                return c.AsQueryable();
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

            bool is_add = false;

            if (u.product_modifiers != null && u.product_modifiers.Any())
            {
                foreach (var pm in u.product_modifiers)
                {
                    pm.children.Where(r => r.modifier_id > 0).ToList().ForEach(r => r.modifier = null);
                }
            }

            u.product_menus.ForEach(r => r.menu = null);


            // update stock transfer
            
            u.stock_location_products.ForEach(r => r.stock_location = null);
             
            u.unit = null;
          
            if (u.id == 0)
            {
                is_add = true;
                db.Products.Add(u);
            }
            else
            {

                db.Products.Update(u);
            } 
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            if (u.is_inventory_product)
            {
                if (is_add )
                {
                    await AddProductToInventoryTransaction(u);
                }
                else
                {

                    if (!db.InventoryTransactions.Where(r => r.product_id == u.id).Take(1).Any() )
                    {
                        await AddProductToInventoryTransaction(u);
                    }
                    else
                    { 
                        if (u.is_product_has_inventory_transaction == false)
                        {
                            await AddProductAdjustmentToInventoryTransaction(u);
                        }
                        else {
                            return StatusCode(401, new ApiResponseModel() { message = $"This product was transacted. Please try again."});
                        }

                    }

                }
            }
          
           
            db.Database.ExecuteSqlRaw("exec sp_clear_deleted_record"); 
            return Ok(u); 
        }




        async Task AddProductToInventoryTransaction(ProductModel p )
        {
            InventoryTransactionModel inv = new InventoryTransactionModel();

            UserModel user = await db.Users.FindAsync(Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));

            foreach (var s in p.stock_location_products)
            {
 
                inv = new InventoryTransactionModel();
                inv.product_id = p.id;
                inv.transaction_date = DateTime.Now;
                inv.inventory_transaction_type_id = 1;
                inv.stock_location_id = s.stock_location_id;
                inv.quantity = s.initial_quantity;
          
                inv.reference_number = p.product_code;


                inv.url = (p.is_ingredient_product && !p.is_menu_product) ? "ingredient/" + p.id : "product/" + p.id;
               
                    inv.note = (p.is_ingredient_product && !p.is_menu_product) ? $"Created New Ingredient ({p.product_code})" : $"Created New Product ({p.product_code})";

                

                inv.created_by = user.full_name;

                db.InventoryTransactions.Add(inv);
                db.SaveChanges();



             
            }

        }


        async Task AddProductAdjustmentToInventoryTransaction(ProductModel p)
        {
            InventoryTransactionModel inv = new InventoryTransactionModel();

            UserModel user = await db.Users.FindAsync(Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));

            foreach (var s in p.stock_location_products.Where(r=>(r.quantity - r.initial_quantity)!=0))
            {

                inv = new InventoryTransactionModel();
                inv.product_id = p.id;
                inv.transaction_date = DateTime.Now;
                inv.inventory_transaction_type_id = 2;
                inv.stock_location_id = s.stock_location_id;
                inv.quantity = s.initial_quantity - s.quantity;

                inv.reference_number = p.product_code;


                inv.url = (p.is_ingredient_product && !p.is_menu_product) ? "ingredient/" + p.id : "product/" + p.id;
                

                    inv.note = (p.is_ingredient_product && !p.is_menu_product) ? $"Ingredient Initial Quantity Adjustment ({p.product_code})" : $"Product Initial Quantity Adjustment ({p.product_code})";

             

                inv.created_by = user.full_name;

                db.InventoryTransactions.Add(inv);
                db.SaveChanges();




            }

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
        public ActionResult<ProductModel> Clone(int id) //Delete
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
