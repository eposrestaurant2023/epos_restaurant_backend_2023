using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using eModels;
using eAdmin.Services;
using System.Text.Json;
using System;

namespace eAdmin.Pages.PageProjects.ComBussinessBranchDetail
{
    public partial class ComBusinessBranchDetail_AddStation
    {
        [Parameter] public StationModel model { get; set; } = new StationModel();
        [Parameter] public Guid business_branch_id { get; set; } = Guid.Empty;
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Inject] IHttpService http { get; set; }
        [Inject] ISnackbar toast { get; set; }
        bool is_saving;


        public async Task Save_Click()
        {
            is_saving = true;
            StationModel save_model = JsonSerializer.Deserialize<StationModel>(JsonSerializer.Serialize(model));
            if (save_model.cash_drawer_id == Guid.Empty || save_model.cash_drawer_id == null)
            {
                toast.Add("Please select cash drawers.", Severity.Warning);
                is_saving =false;
                return;
            }
            var resp = await http.ApiPost("Station/Save", save_model);
            if (resp.IsSuccess)
            {

                toast.Add("Save station successfully", Severity.Success);
                MudDialog.Close(DialogResult.Ok(JsonSerializer.Deserialize<StationModel>(resp.Content.ToString())));
            }
            else
            {
                toast.Add("Save station fail", Severity.Warning);
            }

            is_saving = false;
        }
    }
}