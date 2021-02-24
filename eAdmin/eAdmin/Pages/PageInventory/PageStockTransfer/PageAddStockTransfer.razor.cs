﻿using eAdmin.JSHelpers;
using eModels;
using MatBlazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
 
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace eAdmin.Pages.PageInventory.PageStockTake
{
    public class PageAddStockTransferBase : PageCore
    {
        [Parameter] public int id { get; set; }

        public StockTransferModel model = new StockTransferModel(); 
        public bool is_show_back { get; set; } = false;
        protected override async Task OnInitializedAsync()
        {
            is_loading = true;
            title = (id > 0 ? lang["Edit Stock Transfer"] : lang["New Stock Transfer"]);

            if (!is_error)
            {
                await LoadData();
            }

            if (model.is_fulfilled)
            {
                is_error = true;
                error_text = lang["This Stock Transfer is already fulfilled"];
            }

        }
        public void OnSearchProduct(SelectedProductModel sp)
        {
                AddItemToSaleProduct(sp);
        }
        
        public void OnSearchProducts(List<SelectedProductModel> selected_products)
        {

            foreach (var sp in selected_products)
            {
                AddItemToSaleProduct(sp);
            }
        }

        void AddItemToSaleProduct(SelectedProductModel sp)
        {
            var old_sale_product = model.active_stock_transfer_products.Where(r => r.product.id == sp.product.id && r.unit.Trim().ToLower() == sp.unit.unit_name.Trim().ToLower());
 
            if (old_sale_product != null && old_sale_product.Count() > 0)
            {
                old_sale_product.FirstOrDefault().quantity = old_sale_product.FirstOrDefault().quantity + sp.quantity;
                return;
            }


            //add new record
            StockTransferProductModel d = new StockTransferProductModel();

            d.product_id = sp.product.id;
            d.product = sp.product;
            d.is_inventory_product = sp.product.is_inventory_product;
            d.quantity = sp.quantity;
            d.cost = sp.cost;
            d.regular_cost = sp.cost;
            d.unit = sp.unit.unit_name;
            d.multiplier = sp.unit.multiplier;

            model.stock_transfer_products.Add(d);

        }

        public void OnPOInformationChange(StockTransferModel _model)
        {

            model = _model;
        }
 
        async Task LoadData()
        {
            is_loading_data = true;
            if (id > 0)
            {
                string url = $"StockTransfer({id})?";
                url += $"$expand=stock_transfer_products($expand=product;$filter=is_deleted eq false),";
                url += $"from_business_branch($select=business_branch_name_en),from_stock_location($select=stock_location_name)";
                var resp = await http.ApiGet(url);
                if (resp.IsSuccess)
                {
                    model = JsonSerializer.Deserialize<StockTransferModel>(resp.Content.ToString());
                }
            }
            is_loading_data = false;
            is_loading = false;

        }

        public async Task OnSaveClick()
        {
            if (model.to_business_branch_id == Guid.Empty)
            {
                toast.Add(lang["Please select to business branch."], MatToastType.Warning);
                return;
            }
             
            if (model.to_stock_location_id == 0)
            {
                toast.Add(lang["Please select to stock location."], MatToastType.Warning);
                return;
            }

            if (model.from_business_branch_id == Guid.Empty)
            {
                toast.Add("Please select from business branch.", MatToastType.Warning);
                return;
            }
            if (model.from_stock_location_id == 0)
            {
                toast.Add(lang["Please select from stock location."], MatToastType.Warning);
                return;
            }

            if (model.active_stock_transfer_products.Count() <= 0)
            {
                toast.Add(lang["Stock transfer item cannot be empty."], MatToastType.Warning);
                return;
            }
            
            StockTransferModel save_model = JsonSerializer.Deserialize<StockTransferModel>(JsonSerializer.Serialize(model)); 
            save_model.stock_transfer_products.ForEach(r => r.product = null);
            save_model.to_stock_location = save_model.from_stock_location = null;
            save_model.to_business_branch = save_model.from_business_branch = null;

            is_saving = true;

            Console.WriteLine(JsonSerializer.Serialize(save_model));

            var resp = await http.ApiPost("StockTransfer/save", save_model);
            if (resp.IsSuccess)
            {
                toast.Add(lang["Save successfully."], MatToastType.Success);
                var _model = JsonSerializer.Deserialize<PurchaseOrderModel>(resp.Content.ToString());
                nav.NavigateTo($"stocktransfer/{_model.id}");
            }
            else
            {
                toast.Add(resp.Content, MatToastType.Warning);
                is_saving = false;
            }
        }
    
    }
}