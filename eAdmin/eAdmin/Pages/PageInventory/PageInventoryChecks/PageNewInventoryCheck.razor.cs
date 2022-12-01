using eAdmin.JSHelpers;
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

namespace eAdmin.Pages.PageInventory.PageInventoryChecks
{
    public class PageNewInventoryCheckBase : PageCore
    {
        [Parameter] public Guid id { get; set; }
    

        public InventoryCheckModel model = new InventoryCheckModel();
        
        public HashSet<TreeViewModel> SelectedProductCategory { get; set; }
        public HashSet<TreeViewModel> product_category_tree { get; set; } = new HashSet<TreeViewModel>();
        public bool is_selecting_business_branch { get; set; } = false;




        protected override async Task OnInitializedAsync()
        {
            is_loading = true;
            title = (id == Guid.Empty ? "Edit Inventory Check" : "New Inventory Check");
            if (!is_error)
            {
                if (id != Guid.Empty)
                {
                    await LoadData();

                }
                else
                {
                    var busi = gv.business_branch_by_role;
                    if (busi.Any())
                    {
                        await OnBusinessBranchSeletedChange(busi.FirstOrDefault().id);
                    }
                }

            }
            await BuildCategoryTreeAsync();
            if (id == Guid.Empty || model.inventory_check_type == "Full")
            {
                SelectedProductCategory = new HashSet<TreeViewModel>();
                SelectedProductCategory.Clear();
                foreach (var d in product_category_tree)
                {
                    UpdateSelected(d);
                }
            }
            
            

            if (model.is_fulfilled)
            {
                is_error = true;
                error_text = "This transaction is already  fulfilled";
            }
            is_loading = false;

        }
        string GetGuid()
        {
            if (id != Guid.Empty)
            {
                return id.ToString();
            }
            return "";
        }
       
        async Task BuildCategoryTreeAsync()
        {
            var resp = await http.ApiGetDataFromStoreProcedure("sp_get_product_group_category_tree", $"'{GetGuid()}'");
            if (resp.IsSuccess)
            {
                var data = JsonSerializer.Deserialize<List<TreeViewModel>>(resp.Content.ToString());

                foreach (var g in data.Where(r=>r.parent_id==null))
                {
                    TreeViewModel item = new TreeViewModel();
                    item.id = g.id;
                    item.title = g.title;
                    item.is_selected = g.is_selected;
                    foreach (var c in data.Where(r=>r.parent_id == g.id))
                    {
                        item.tree_items.Add(new TreeViewModel()
                        {
                            id = c.id,
                            title = c.title,
                            is_selected = c.is_selected
                        }) ;
                    }
                    product_category_tree.Add(item);
                }
                
            }

        }
       
       
        async Task LoadData()
        {
            is_loading_data = true;
            if (id !=Guid.Empty)
            {
                string url = $"InventoryCheck({id})";
                var resp = await http.ApiGet(url);
                if (resp.IsSuccess)
                {
                    model = JsonSerializer.Deserialize<InventoryCheckModel>(resp.Content.ToString());
                }
            }
            is_loading_data = false;
            is_loading = false;

        }

        public async Task OnSaveClick()
        {
            if (model.business_branch_id == Guid.Empty)
            {
                toast.Add(lang["Please select business branch."], MudBlazor.Severity.Warning);
                return;
            }

            if (model.stock_location_id == Guid.Empty)
            {
                toast.Add(lang["Please select stock location."], MudBlazor.Severity.Warning);
                return;
            }

            model.product_categories = string.Join(",", SelectedProductCategory.Select(r => r.id).Distinct());
            
            InventoryCheckModel save_model = JsonSerializer.Deserialize<InventoryCheckModel>(JsonSerializer.Serialize(model));
             
            save_model.stock_location = null;
            save_model.business_branch = null;

            is_saving = true;
            var resp = await http.ApiPost("InventoryCheck/save", save_model);
            if (resp.IsSuccess)
            {
                toast.Add(lang["Save successfully."], MudBlazor.Severity.Success);
                var _model = JsonSerializer.Deserialize<InventoryCheckModel>(resp.Content.ToString());
                nav.NavigateTo($"inventorycheck/{_model.id}");
            }
            else if (resp.status_code==409)
            {
                toast.Add(lang[resp.Content], MudBlazor.Severity.Warning);
                
            }
            else
            {
                toast.Add(lang["Save data fail."], MudBlazor.Severity.Warning);
              
            }
            is_saving = false;
        }


        public void OnStockLocationSeletedChange(Guid _id)
        {
            model.stock_location_id = _id;
        }
        public async Task OnBusinessBranchSeletedChange(Guid _id)
        {
            is_selecting_business_branch = true;
            model.stock_location_id = Guid.Empty;
            var sl = gv.stock_locations.Where(r => r.business_branch_id == _id);
            if (sl.Any())
            {
                OnStockLocationSeletedChange(sl.FirstOrDefault().id);
            }
            await Task.Delay(500);
            model.business_branch_id = _id;
            is_selecting_business_branch = false;
        }

        public void onSelectedValueChange(HashSet<TreeViewModel> value)
        {
            SelectedProductCategory = value;
            StateHasChanged();
        }

        public void InVentoryCheckTypeChanged(string value)
        {
            model.inventory_check_type = value;
            if (value == "Full")
            {
                SelectedProductCategory.Clear();
                foreach (var d in product_category_tree) {
                    UpdateSelected(d);
                }
                
            }
         
        }

        void UpdateSelected(TreeViewModel data)
        {
            if (model.inventory_check_type=="Full")
            {
                data.is_selected = true;
            }
            SelectedProductCategory.Add(data);
            foreach (var d in data.tree_items)
            {
                
                UpdateSelected(d);
            }
        }

       
    }
}