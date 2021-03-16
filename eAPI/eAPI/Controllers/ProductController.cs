
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
                            (r.product_category.product_category_en ?? "") +
                            (r.product_category.product_category_kh ?? "") +
                            (r.vendor.vendor_code ?? "") +
                            (r.vendor.vendor_name ?? "") +
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
            //check product if it is already has inv transaction then retail fail
            if (u.id > 0) {
                var db_product = db.Products.Where(r => r.id == u.id).AsNoTracking().Include(r=>r.stock_location_products).AsNoTracking();
                var p = db_product.FirstOrDefault();
                if (p.is_inventory_product)
                {
                    //if user untick is_inventor6y product but product in db already mark as has inv transaction
                    if (u.is_inventory_product == false && p.is_product_has_inventory_transaction) {
                        return StatusCode(500, "Is Inventory Status cannot Change. This product is already has inventory transaction.");
                    }else  {
                        //use change initialize quantity after product has inv transaction
                        if (u.stock_location_products.Where(r => (r.initial_quantity - r.initial_adjustment_quantity) != 0).Any())
                        {
                            return StatusCode(500, "Quantity cannot change. This product is already has inventory transaction.");
                        }
                        
                    }
                }

                //when update to prevent quantity change we load quantity from database
                //and assign to quantitty that save to database again
                if (u.stock_location_products.Any())
                {
                   if(u.stock_location_products.Where(r => r.id > 0).Any() && p.stock_location_products.Any())
                    {
                        foreach (var sp in u.stock_location_products) {
                            var data = p.stock_location_products.Where(r => r.id == sp.id);
                            if (data.Any()) {
                                sp.quantity = data.FirstOrDefault().quantity;
                            }
                          
                        }
                    }

                }
                //end get quantity from db save to quantity that will submit to db


            }

            if (u.product_modifiers != null && u.product_modifiers.Any())
            {
                foreach (var pm in u.product_modifiers)
                {
                    pm.children.Where(r => r.modifier_id > 0).ToList().ForEach(r => r.modifier = null);
                }
            }

            u.product_menus.ForEach(r => r.menu = null);
            u.stock_location_products.ForEach(r => r.stock_location = null);

      
            
            //if product is inv product then save init qty to init adjusment qty
            if (u.is_inventory_product)
            {
                if (u.stock_location_products.Any())
                {
                    u.stock_location_products.ForEach(r => { r.initial_adjustment_quantity = r.initial_quantity; r.unit = u.unit.unit_name; r.multiplier = u.unit.multiplier; });
                }
            }

            if (u.id == 0)
            {
                is_add = true;
                u.unit = null;
                db.Products.Add(u);
                AddHistory(u,"New Product Created");
            }
            else
            {
                //update unit & modifier when product was updated 
                if (u.stock_location_products.Any())
                {
                    u.stock_location_products.ForEach(r => { r.unit = u.unit.unit_name; r.multiplier = u.unit.multiplier; });
                    db.StockLocationProducts.UpdateRange(u.stock_location_products);
                }
                u.unit = null;
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
                         
                            await AddProductAdjustmentToInventoryTransaction(u);
                        

                    }

                }
            }
            else
            {
                if (!is_add)
                {
                    ////>>????????? where deleted
                }
            }
          
            db.Database.ExecuteSqlRaw("exec sp_clear_deleted_record " + u.id); 
            db.Database.ExecuteSqlRaw("exec sp_update_product_information " + u.id); 
           
            return Ok(u); 
        }

        //[HttpPost("stockadjustmentsave")]

        //public async Task<ActionResult<string>> AdjustmentSave ([FromBody] ProductModel p) 
        //{
        //    InventoryTransactionModel inv = new InventoryTransactionModel();

        //    UserModel user = await db.Users.FindAsync(Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));

        //    foreach (var s in p.stock_location_products)
        //    {
        //        inv = new InventoryTransactionModel();
        //        inv.product_id = p.id;
        //        inv.transaction_date = DateTime.Now;
        //        inv.inventory_transaction_type_id = 2;
        //        inv.stock_location_id = s.stock_location_id;
        //        inv.quantity = s.quantity;
        //        inv.reference_number = p.product_code;
        //        inv.url = (p.is_ingredient_product && !p.is_menu_product) ? "ingredient/" + p.id : "product/" + p.id;
        //        inv.note = (p.is_ingredient_product && !p.is_menu_product) ? $"Stock Adjustment Ingredient ({p.product_code})" : $"Stock Adjustment Product ({p.product_code})";
        //        inv.created_by = user.full_name;

        //        db.InventoryTransactions.Add(inv);
        //        db.SaveChanges();
                
        //    }
        //    return Ok(inv);
        //}
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

            foreach (var s in p.stock_location_products.Where(r=>(r.initial_adjustment_quantity- r.initial_quantity)!=0))
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
            if (u.is_deleted)
            {
                AddHistory(u, "Product Deleted");
            }
            else
            {
                AddHistory(u, "Product Restored");
            }
            db.Products.Update(u);
            await db.SaveChangesAsync();
            db.Database.ExecuteSqlRaw("exec sp_update_product_information " + u.id);
            
            
            return Ok();
        }

        [HttpPost]
        [Route("ChangeStatus/{id}")]
        public async Task<ActionResult> ChangeStatus(int id) //Delete
        {
            var u = await db.Products.FindAsync(id);
            u.status = !u.status;

            // Add into history
            string title = u.status == true ? "Changed to Active Product" : "Changed to Inactive Product";
            AddHistory(u, title);

            db.Products.Update(u);
            await db.SaveChangesAsync();
            
            return Ok();
        }

        [HttpPost]
        [Route("Clone/{id}")]
        public ActionResult<ProductModel> Clone(int id)
        {
            var data = db.Products.Where(r => r.id == id)
                .Include(r=>r.product_portions.Where(r=>r.is_deleted==false)).ThenInclude(r=>r.product_prices.Where(r=>r.is_deleted==false))
                .Include(r=>r.product_printers.Where(r=>r.is_deleted==false)).
                Include(r=>r.product_menus.Where(r=>r.is_deleted==false)).ThenInclude(r=>r.menu)
                .Include(r=>r.product_modifiers.Where(r=>r.is_deleted ==false)).ThenInclude(r=>r.modifier)
                .Include(r=>r.unit)
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
                p.is_product_has_inventory_transaction = false;
                
                return Ok(p);
            }
            else
            {
                return NotFound();
            }
            
        }

        void AddHistory(ProductModel s, string title)
        {
            HistoryModel h = new HistoryModel(title);
            h.module = "product";
            h.document_number = s.product_code;
            h.product_id = s.id;
            h.description = $"{title} Product Code #: {s.product_code}.";

            s.histories.Add(h);
        }

    }

}
