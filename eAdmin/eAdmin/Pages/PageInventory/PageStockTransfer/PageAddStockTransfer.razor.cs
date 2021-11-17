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
        [Parameter] public int clone_id { get; set; }

        public StockTransferModel model = new StockTransferModel(); 
        public bool is_show_back { get; set; } = false;
        public bool is_selecting_from_business_branch { get; set; } = false;
        public bool is_selecting_to_business_branch { get; set; } = false;
        protected override async Task OnInitializedAsync()
        {
            is_loading = true;
            title = (id > 0 ? lang["Edit Stock Transfer"] : lang["New Stock Transfer"]);

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
                error_text = lang["This Stock Transfer is already fulfilled"];
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

        public async Task CloneRecord()
        {
            is_loading = true;
            var resp = await http.ApiPost($"StockTransfer/Clone/{clone_id}");
            if (resp.IsSuccess)
            {
                model = JsonSerializer.Deserialize<StockTransferModel>(resp.Content.ToString());
            }
            is_loading = false;

        }
        async Task LoadData()
        {
            is_loading_data = true;
            if (id > 0)
            {
                string url = $"StockTransfer({id})?";
                url += $"$expand=stock_transfer_products($expand=product($expand=unit);$filter=is_deleted eq false),";
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
             
            if (model.to_stock_location_id == Guid.Empty)
            {
                toast.Add(lang["Please select to stock location."], MatToastType.Warning);
                return;
            }

            if (model.from_business_branch_id == Guid.Empty)
            {
                toast.Add(lang["Please select from business branch."], MatToastType.Warning);
                return;
            }
            if (model.from_stock_location_id == Guid.Empty)
            {
                toast.Add(lang["Please select from stock location."], MatToastType.Warning);
                return;
            }
            if (model.from_stock_location_id == model.to_stock_location_id)
            {
                toast.Add(lang["Cannot transfer to same stock location."], MatToastType.Warning);
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

            var resp = await http.ApiPost("StockTransfer/save", save_model);
            if (resp.IsSuccess)
            {
                toast.Add(lang["Save successfully."], MatToastType.Success);
                var _model = JsonSerializer.Deserialize<PurchaseOrderModel>(resp.Content.ToString());
                nav.NavigateTo($"stocktransfer/{_model.id}");
            }
            else
            {
                toast.Add(lang[resp.Content], MatToastType.Warning);
                is_saving = false;
            }
        }

        public void OnFormStockLocationSeletedChange(Guid _id)
        {
            model.from_stock_location_id = _id;
        }
        public void OnToStockLocationSeletedChange(Guid _id)
        { 
            model.to_stock_location_id = _id;
        }
        public async Task OnFormBusinessBranchSeletedChange(Guid _id)
        { 
            is_selecting_from_business_branch = true;
            model.from_stock_location_id = Guid.Empty;
            await Task.Delay(500);
            model.from_business_branch_id = _id;
            is_selecting_from_business_branch = false;
 
        }
        
        public async Task OnToBusinessBranchSeletedChange(Guid _id)
        {
            is_selecting_to_business_branch = true;
            model.to_stock_location_id = Guid.Empty;
            await Task.Delay(500);
            model.to_business_branch_id = _id;
            is_selecting_to_business_branch = false;
        }

    }
}