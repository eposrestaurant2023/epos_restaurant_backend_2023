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
    public class PageAddStockTakeBase : PageCore
    {
        [Parameter] public int id { get; set; }
        [Parameter] public int clone_id { get; set; }

        public StockTakeModel model = new StockTakeModel(); 
        public bool is_show_back { get; set; } = false;
        public bool is_selecting_business_branch { get; set; } = false;
        protected override async Task OnInitializedAsync()
        {
            is_loading = true;
            title = (id > 0 ? "Edit Stock Take" : "New Stock Take");

            if (!is_error)
            { 
                if (id > 0)
                {

                    await LoadData();
                }
                else if (clone_id > 0)
                {
                    await CloneRecord();
                }
            }

            if (model.is_fulfilled)
            {
                is_error = true;
                error_text = "This Stock Take is already fulfilled";
            }
            is_loading = false;

        }
        public async Task CloneRecord()
        {
            is_loading = true;
            var resp = await http.ApiPost($"StockTake/Clone/{clone_id}");
            if (resp.IsSuccess)
            {
                model = JsonSerializer.Deserialize<StockTakeModel>(resp.Content.ToString());
            }
            is_loading = false;

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
            var old_sale_product = model.active_stock_take_products.Where(r => r.product.id == sp.product.id && r.unit.Trim().ToLower() == sp.unit.unit_name.Trim().ToLower());



            if (old_sale_product != null && old_sale_product.Count() > 0)
            {
                old_sale_product.FirstOrDefault().quantity = old_sale_product.FirstOrDefault().quantity + sp.quantity;
                return;
            }

            //add new record
            StockTakeProductModel d = new StockTakeProductModel();

            d.product_id = sp.product.id;
            d.product = sp.product;
           
            d.is_inventory_product = sp.product.is_inventory_product;
            d.quantity = sp.quantity;
            d.cost = sp.cost;
            d.regular_cost = sp.cost;
            d.unit = sp.unit.unit_name;
            d.multiplier = sp.unit.multiplier;



            model.stock_take_products.Add(d);

        }

        public void OnPOInformationChange(StockTakeModel _model)
        {

            model = _model;
        }
 
        async Task LoadData()
        {
            is_loading_data = true;
            if (id > 0)
            {
                string url = $"StockTake({id})?";
                url += $"$expand=stock_take_products($expand=product($expand=unit);$filter=is_deleted eq false)";
                var resp = await http.ApiGet(url);
                if (resp.IsSuccess)
                {
                    model = JsonSerializer.Deserialize<StockTakeModel>(resp.Content.ToString());
                }
            }
            is_loading_data = false;
            is_loading = false;

        }

        public async Task OnSaveClick()
        {
            if (model.business_branch_id == Guid.Empty)
            {
                toast.Add("Please select business branch.", MatToastType.Warning);
                return;
            }

            if (model.stock_location_id == 0)
            {
                toast.Add("Please select stock location.", MatToastType.Warning);
                return;
            }

            if (model.active_stock_take_products.Count() <= 0)
            {
                toast.Add("Stock take item cannot be empty.", MatToastType.Warning);
                return;
            }
            
            StockTakeModel save_model = JsonSerializer.Deserialize<StockTakeModel>(JsonSerializer.Serialize(model)); 
            save_model.stock_take_products.ForEach(r => r.product = null);
            save_model.stock_location = null;
            save_model.business_branch = null;

            is_saving = true;
          Console.WriteLine( JsonSerializer.Serialize(save_model));
            var resp = await http.ApiPost("StockTake/save", save_model);
            if (resp.IsSuccess)
            {
                toast.Add("Save successfully.", MatToastType.Success);
                var _model = JsonSerializer.Deserialize<PurchaseOrderModel>(resp.Content.ToString());
                nav.NavigateTo($"stocktake/{_model.id}");
            }
            else
            {
                toast.Add(resp.Content, MatToastType.Warning);
                is_saving = false;
            }
        }


        public async Task OnStockLocationSeletedChange(int _id)
        {
            model.stock_location_id = _id;
        }
        public async Task OnBusinessBranchSeletedChange(Guid _id)
        {
            is_selecting_business_branch = true;
            model.stock_location_id = 0;
            await Task.Delay(500);
            model.business_branch_id = _id;
            is_selecting_business_branch = false;
        }
    }
}