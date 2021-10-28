using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using eModels;
using System.Text.Json;
using eAdmin.Services;
using System;
using System.Linq;
using eAdmin.Pages.PageProjects.ComProjectDetails;

namespace eAdmin.Pages.PageProjects.ComBussinessBranchDetail
{
    public partial class ComBusinessBranchDetail_CashDrawer
    {
        [Parameter]
        public List<CashDrawerModel> cash_drawers { get; set; }

        [Parameter]
        public BusinessBranchModel business_branch { get; set; }

        [CascadingParameter]
        public AppState gv { get; set; }
        [Inject] IHttpService http { get; set; }
        [Inject] IDialogService Dialog { get; set; }
        
        [Inject] ISnackbar toast  { get; set; }
 
    

        bool is_loading;
        async Task AddNewCashDrawer()
        {
            var parameters = new DialogParameters{["model"] = new CashDrawerModel()
            {
                business_branch_id = business_branch.id,
                project_id = business_branch.project_id
            }
            };
            var dialog = Dialog.Show<ComBusinessBranchDetail_AddCashDrawer>("Add New Cash Drawer", parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                cash_drawers.Add((CashDrawerModel)result.Data);
            }
        }

        async Task EditOutlet_Click(CashDrawerModel cash_drawer)
        {
            CashDrawerModel backup_model = JsonSerializer.Deserialize<CashDrawerModel>(JsonSerializer.Serialize(cash_drawer));
            var parameters = new DialogParameters{["model"] = cash_drawer };
            var dialog = Dialog.Show<ComBusinessBranchDetail_AddOutlet>("Edit Cash Drawer", parameters);
            var result = await dialog.Result;
            if (result.Cancelled)
            {
                cash_drawer = backup_model;
            }
        }

        async Task DeleteOutlet_Click(CashDrawerModel cash_drawer)
        {
            string message = "Are you sure you want to delete this Cash Drawer?";
            if (cash_drawer.is_deleted)
            {
                message = "Are you sure you want to restore this Cash Drawer?";
            }

            bool? result = await Dialog.ShowMessageBox("Warning", message, yesText: cash_drawer.is_deleted ? "Restore" : "Delete", cancelText: "Cancel");
            if (result != null)
            {
                is_loading = true;
                cash_drawer.is_deleted = !cash_drawer.is_deleted;
                var resp = await http.ApiPost("cashdrawer/Save", cash_drawer);
                if (resp.IsSuccess)
                {
                    if (cash_drawer.is_deleted)
                    {
                        toast.Add("Delete cash drawer Successfully", Severity.Success);
                    }
                    else
                    {
                        toast.Add("Restore cash drawer successfully", Severity.Success);
                    }
                }
                else
                {
                    cash_drawer.is_deleted = !cash_drawer.is_deleted;
                    if (cash_drawer.is_deleted)
                    {
                        toast.Add("Delete cash drawer Fail", Severity.Warning);
                    }
                    else
                    {
                        toast.Add("Restore cash drawer Fail", Severity.Warning);
                    }
                }
            }

            is_loading = false;
        }



        async Task AddNewCashDrawer(CashDrawerModel cash_drawer)
        {
            var parameters = new DialogParameters { ["model"] = new CashDrawerModel() { 
                project_id = business_branch.project_id,
                business_branch_id = business_branch.id
            } };
            var dialog = Dialog.Show<ComBusinessBranchDetail_AddCashDrawer>("Add New Cash Drawer", parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                business_branch.cash_drawers.Add((CashDrawerModel)result.Data);
            }
        }

        async Task EditCashDrawer_Click(CashDrawerModel _cash_drawer)
        {
            CashDrawerModel backup_model = JsonSerializer.Deserialize<CashDrawerModel>(JsonSerializer.Serialize(_cash_drawer));
            var parameters = new DialogParameters { ["model"] = _cash_drawer };
            var dialog = Dialog.Show<ComBusinessBranchDetail_AddCashDrawer>("Edit Cash Drawer", parameters);
            var result = await dialog.Result;
            if (result.Cancelled)
            {
                _cash_drawer = backup_model;
            }
        }


        async Task DeleteCashDrawer_Click(CashDrawerModel cash_drawer)
        {
            string message = "Are you sure you want to delete this Cash Drawer?";
            if (cash_drawer.is_deleted)
            {
                message = "Are you sure you want to restore this Cash Drawer?";
            }

            bool? result = await Dialog.ShowMessageBox("Warning", message, yesText: cash_drawer.is_deleted ? "Restore" : "Delete", cancelText: "Cancel");
            if (result != null)
            {
                is_loading = true;
                cash_drawer.is_deleted = !cash_drawer.is_deleted;
                var resp = await http.ApiPost($"CashDrawer/save", cash_drawer);
                if (resp.IsSuccess)
                {
                    if (cash_drawer.is_deleted)
                    {
                        toast.Add("Delete Cash Drawer Successfully", Severity.Success);
                    }
                    else
                    {
                        toast.Add("Restore Cash Drawer successfully", Severity.Success);
                    }
                }
                else
                {
                    cash_drawer.is_deleted = !cash_drawer.is_deleted;
                    if (cash_drawer.is_deleted)
                    {
                        toast.Add("Delete Cash Drawer Failed", Severity.Warning);
                    }
                    else
                    {
                        toast.Add("Restore Cash Drawer Failed", Severity.Warning);
                    }
                }
            }

            is_loading = false;
        }

    }
}