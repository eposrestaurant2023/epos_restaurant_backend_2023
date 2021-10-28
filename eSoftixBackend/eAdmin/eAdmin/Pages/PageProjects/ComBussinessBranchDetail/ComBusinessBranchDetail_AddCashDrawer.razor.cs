using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using eModels;
using eAdmin.Services;
using System.Text.Json;

namespace eAdmin.Pages.PageProjects.ComBussinessBranchDetail
{
    public partial class ComBusinessBranchDetail_AddCashDrawer
    {
        [Parameter] public CashDrawerModel model { get; set; } = new CashDrawerModel();
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Inject] IHttpService http { get; set; }
        [Inject] ISnackbar toast { get; set; }
        bool is_saving;


        public async Task Save_Click()
        {
            if (string.IsNullOrEmpty(model.cash_drawer_name))
            {
                toast.Add("Please enter cash drawer name", Severity.Warning);
                return;
            }
            is_saving = true;
            var resp = await http.ApiPost("CashDrawer/Save", model);
            if (resp.IsSuccess)
            {

                toast.Add("Save cash drawer successfully", Severity.Success);
                MudDialog.Close(DialogResult.Ok(JsonSerializer.Deserialize<CashDrawerModel>(resp.Content.ToString())));
            }
            else
            {
                toast.Add("Save cash drawer fail", Severity.Warning);
            }

            is_saving = false;
        }
    }
}