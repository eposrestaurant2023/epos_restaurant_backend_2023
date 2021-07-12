using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using eModels;
using eAdmin.Services;
using System.Text.Json;

namespace eAdmin.Pages.PageProjects.ComBussinessBranchDetail
{
    public partial class ComBusinessBranchDetail_AddExtendLicenseHistory
    {
        [Parameter] public ExtendLicenseHistoryModel model { get; set; } = new ExtendLicenseHistoryModel();
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Inject] IHttpService http { get; set; }
        [Inject] ISnackbar toast { get; set; }
        bool is_saving;


        public async Task Save_Click()
        {
            is_saving = true;
            var resp = await http.ApiPost("ExtendLicenseHistory/Save", model);
            if (resp.IsSuccess)
            {

                toast.Add("Save Extend License History successfully", Severity.Success);
                MudDialog.Close(DialogResult.Ok(JsonSerializer.Deserialize<StationModel>(resp.Content.ToString())));
            }
            else
            {
                toast.Add("Save Extend License History fail", Severity.Warning);
            }

            is_saving = false;
        }
    }
}