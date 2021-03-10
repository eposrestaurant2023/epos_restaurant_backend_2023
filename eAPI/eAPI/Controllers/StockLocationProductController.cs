using System;
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
    public class StockLocationProductController : ODataController
    {

        private readonly ApplicationDbContext db;
        public StockLocationProductController(ApplicationDbContext _db)
        {
            db = _db;
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]   
        public IQueryable<StockLocationProductModel> Get()
        {            
            return db.StockLocationProducts;
        }
        
        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] StockLocationProductModel u)
        {            
            if (u.id == 0)
            {
                db.StockLocationProducts.Add(u);
            }
            else
            {
                db.StockLocationProducts.Update(u);
            }            
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return Ok(u);
        }
        [HttpPost("Save/Multiple")]
        public async Task<ActionResult<string>> SaveMultiple([FromBody] List<QuantityAdjustmentModel> data)
        {
            InventoryTransactionModel inv = new InventoryTransactionModel();
            var product = db.Products.Where(r => r.id == data.FirstOrDefault().product_id).AsNoTracking();
            ProductModel p = product.FirstOrDefault();
            UserModel user = await db.Users.FindAsync(Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));

            foreach (var s in data)
            {

                inv = new InventoryTransactionModel();
                inv.product_id = s.product_id ;
                inv.transaction_date = DateTime.Now;
                inv.inventory_transaction_type_id = 2;
                inv.stock_location_id = s.stock_location_id;
                inv.quantity = s.quantity;
                inv.reference_number = p.product_code;


                inv.url = (p.is_ingredient_product && !p.is_menu_product) ? "ingredient/" + p.id : "product/" + p.id;
                 
                inv.note = (p.is_ingredient_product && !p.is_menu_product) ? $"Ingredient Initial Quantity Adjustment ({p.product_code})" : $"Product Initial Quantity Adjustment ({p.product_code})";
                 
                inv.created_by = user.full_name;
                db.InventoryTransactions.Add(inv);
                db.SaveChanges();
                 
            }
             
            
            return Ok();
        }


        [HttpGet("find")]
        [EnableQuery(MaxExpansionDepth = 4)]
        public SingleResult<StockLocationProductModel> Get([FromODataUri] int key)
        {
            var s = db.StockLocationProducts.Where(r => r.id == key).AsQueryable();
            return SingleResult.Create(s);
        }
    }
}
