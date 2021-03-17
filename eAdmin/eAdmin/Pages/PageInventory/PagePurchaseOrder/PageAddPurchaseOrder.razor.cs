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

namespace eAdmin.Pages.PageInventory.PagePurchaseOrder
{
    public class PageAddPurchaseOrderBase  : PageCore
    {
        [Parameter] public int id { get; set; }
        [Parameter] public int vendor_id { get; set; } 
        [Parameter] public int clone_id { get; set; }

        public PurchaseOrderModel model = new PurchaseOrderModel(); 
        public bool is_show_back { get; set; } = false; 
        public bool is_show_add_payment { get; set; } = false;
        protected override async Task OnInitializedAsync()
        {
            is_loading = true;
            title = (id > 0 ? "Edit Purchase Order" : "New Purchase Order");

            // when vendor of this PO Deleted/Inactive
            await LoadValidateVendor();

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
                error_text = "This purchase order is already fulfilled";
            }
            is_loading = false;
        }
        public async Task CloneRecord()
        {
            is_loading = true;
            var resp = await http.ApiPost($"PurchaseOrder/Clone/{clone_id}");
            if (resp.IsSuccess)
            {
                model = JsonSerializer.Deserialize<PurchaseOrderModel>(resp.Content.ToString());
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
            var old_sale_product = model.active_purchase_order_products.Where(r => r.product.id == sp.product.id && r.unit.Trim().ToLower() == sp.unit.unit_name.Trim().ToLower());
            if (old_sale_product != null && old_sale_product.Count() > 0)
            {
                old_sale_product.FirstOrDefault().quantity = old_sale_product.FirstOrDefault().quantity + sp.quantity;
                return;
            }

            //add new record
            PurchaseOrderProductModel d = new PurchaseOrderProductModel();

            d.product_id = sp.product.id;
            d.product = sp.product;
            d.unit = sp.unit.unit_name;
            d.multiplier = sp.unit.multiplier;
            d.is_allow_discount = sp.is_allow_discount;
            d.is_inventory_product = sp.product.is_inventory_product;
            d.quantity = sp.quantity;
            d.cost= sp.cost;
            d.regular_cost= sp.cost;
            d.is_allow_discount = sp.product.is_allow_discount;
            model.purchase_order_products.Add(d);
        }

        public void OnPOInformationChange(PurchaseOrderModel _model)
        {

            model = _model;
        }

        public async Task LoadValidateVendor()
        {
            if (vendor_id > 0)
            {
                var resp = await http.ApiGet($"vendor({vendor_id})");
                if (resp.IsSuccess)
                {
                    VendorModel c = JsonSerializer.Deserialize<VendorModel>(resp.Content.ToString());
                    if (c.is_deleted)
                    {
                        is_error = true;
                        error_text = lang["Vendor has been deleted"];
                    }
                    else if (c.status == false)
                    {
                        is_error = true;
                        error_text = lang["Vendor is inactive"];
                    }
                    else
                    {
                        model.vendor = c;
                        model.vendor_id = model.vendor.id;
                    }


                }
                else
                {
                    is_error = true;
                    error_text = lang["Vendor does not exists"];
                }
            }

        }

        async Task LoadData()
        {
            is_loading_data = true;
            if (id > 0)
            {
                string url = $"PurchaseOrder({id})?";
                url += $"$expand=vendor,";
                url += $"purchase_order_payments($expand=payment_type;$filter=is_deleted eq false),";
                url += $"purchase_order_products($expand=product($expand=unit($select=unit_category_id));$filter=is_deleted eq false)";

                var resp = await http.ApiGet(url);
                if (resp.IsSuccess)
                {
                    model = JsonSerializer.Deserialize<PurchaseOrderModel>(resp.Content.ToString());
                }
            }
            is_loading_data = false;
            is_loading = false;

        }

        public async Task OnSaveClick()
        {
  
            if (model.vendor_id == 0)
            {
                toast.Add("Please select vendor.", MatToastType.Warning);
                return;
            }

            if (model.business_branch_id == Guid.Empty)
            {
                toast.Add("Please select business branch.", MatToastType.Warning);
                return;
            }

            if (model.stock_location_id == Guid.Empty)
            {
                toast.Add("Please select stock location.", MatToastType.Warning);
                return;
            }

            if (model.active_purchase_order_products.Count() <= 0)
            {
                toast.Add("PO item cannot be empty.", MatToastType.Warning);
                return;
            }
             
            if (model.balance < 0)
            {
                toast.Add("Balance cannot be lower then zero.", MatToastType.Warning);
                return;
            }
            
            //prepare sale data for send to controller
            PurchaseOrderModel save_model = JsonSerializer.Deserialize<PurchaseOrderModel>(JsonSerializer.Serialize(model));
            save_model.vendor = null;
            save_model.purchase_order_products.ForEach(r => { r.product = null; });
            save_model.stock_location = null;
            save_model.business_branch = null;
            save_model.purchase_order_payments = null;  
            is_saving = true;

          
            var resp = await http.ApiPost("PurchaseOrder/save", save_model);
            if (resp.IsSuccess)
            {
                toast.Add("Save successfully.", MatToastType.Success);
                var _model = JsonSerializer.Deserialize<PurchaseOrderModel>(resp.Content.ToString());
                nav.NavigateTo($"purchaseorder/{_model.id}");
            }
            else
            {
                toast.Add(resp.Content, MatToastType.Warning);
                is_saving = false;
            }
        }
    
    }
}