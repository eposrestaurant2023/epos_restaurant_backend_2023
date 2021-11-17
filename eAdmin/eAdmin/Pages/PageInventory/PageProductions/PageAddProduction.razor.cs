using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eModels;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using MatBlazor;

namespace eAdmin.Pages.PageInventory.PageProductions
{
    public class PageAddProductions:PageCore
    {
        [Parameter] public int id { get; set; }
        [Parameter] public int clone_id { get; set; }

        public ProductionModel model = new ProductionModel();
        public bool is_show_back { get; set; } = false;
        public bool is_selecting_business_branch { get; set; } = false;
        protected override async Task OnInitializedAsync()
        {
            is_loading = true;
            title = (id > 0 ? lang["Edit Production"] : lang["New Production"]);

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
                error_text = lang["This production is already fulfilled"];
            }
            is_loading = false;

        }
        public async Task CloneRecord()
        {
            is_loading = true;
            var resp = await http.ApiPost($"production/Clone/{clone_id}");
            if (resp.IsSuccess)
            {
                model = JsonSerializer.Deserialize<ProductionModel>(resp.Content.ToString());
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
            var old_sale_product = model.active_production_products.Where(r => r.product.id == sp.product.id && r.unit.Trim().ToLower() == sp.unit.unit_name.Trim().ToLower());



            if (old_sale_product != null && old_sale_product.Count() > 0)
            {
                old_sale_product.FirstOrDefault().quantity = old_sale_product.FirstOrDefault().quantity + sp.quantity;
                return;
            }

            //add new record
            ProductionProductModel d = new ProductionProductModel();

            d.product_id = sp.product.id;
            d.product = sp.product;

            d.is_inventory_product = sp.product.is_inventory_product;
            d.quantity = sp.quantity;
            d.cost = sp.cost;
            d.regular_cost = sp.cost;
            if (sp.product_portion != null)
            {
                d.unit = sp.product_portion.unit.unit_name;
            }
            d.multiplier = sp.unit.multiplier;
            d.product_portion = sp.product_portion;
            d.product_portion_id = sp.product_portion_id;


            model.production_products.Add(d);

        }

        public void OnPOInformationChange(ProductionModel _model)
        {

            model = _model;
        }

        async Task LoadData()
        {
            is_loading_data = true;
            if (id > 0)
            {
                string url = $"production({id})?";
                url += $"$expand=production_products($expand=product_portion($expand=unit),product($expand=unit);$filter=is_deleted eq false)";
                var resp = await http.ApiGet(url);
                if (resp.IsSuccess)
                {
                    model = JsonSerializer.Deserialize<ProductionModel>(resp.Content.ToString());
                }
            }
            is_loading_data = false;
            is_loading = false;

        }

        public async Task OnSaveClick()
        {
            if (model.business_branch_id == Guid.Empty)
            {
                toast.Add(lang["Please select business branch."], MatToastType.Warning);
                return;
            }

            if (model.stock_location_id == Guid.Empty)
            {
                toast.Add(lang["Please select stock location."], MatToastType.Warning);
                return;
            }

            if (model.active_production_products.Count() <= 0)
            {
                toast.Add(lang["Production item cannot be empty."], MatToastType.Warning);
                return;
            }

            if (model.production_products.Where(r=>r.product_portion_id == 0).Any())
            {
                toast.Add(lang["Please select portion."], MatToastType.Warning);
                return ;
            }
                
                
            
            ProductionModel save_model = JsonSerializer.Deserialize<ProductionModel>(JsonSerializer.Serialize(model));
            save_model.production_products.ForEach(r => r.product = null);
            save_model.stock_location = null;
            save_model.business_branch = null;

            is_saving = true;
            var resp = await http.ApiPost("production/save", save_model);
            if (resp.IsSuccess)
            {
                toast.Add(lang["Save successfully."], MatToastType.Success);
                var _model = JsonSerializer.Deserialize<PurchaseOrderModel>(resp.Content.ToString());
                nav.NavigateTo($"production/{_model.id}");
            }
            else
            {
                toast.Add(lang[resp.Content], MatToastType.Warning);
                is_saving = false;
            }
        }


        public async Task OnStockLocationSeletedChange(Guid _id)
        {
            model.stock_location_id = _id;
        }
        public async Task OnBusinessBranchSeletedChange(Guid _id)
        {
            is_selecting_business_branch = true;
            model.stock_location_id = Guid.Empty;
            await Task.Delay(500);
            model.business_branch_id = _id;
            is_selecting_business_branch = false;
        }
    }
}
