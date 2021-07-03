using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using eModels;
using MudBlazor;
using eAdmin.Services;

namespace eAdmin.Pages.PageProjects.ComBussinessBranchDetail
{
    public partial class ComAddBusinessBranch
    {
        [Parameter] public BusinessBranchModel model { get; set; } = new BusinessBranchModel();
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Inject] IHttpService http { get; set; }
        [Inject] ISnackbar toast { get; set; }
        bool is_saving;

        protected override async Task OnInitializedAsync()
        {
          
        }

        public async Task Save_Click()
        {
            is_saving = true;
            var resp = await http.ApiPost("BusinessBranch/Save", model);
            if (resp.IsSuccess)
            {

                toast.Add("Save business branch successfully", Severity.Success);
                MudDialog.Close(DialogResult.Ok(model));
            }else
            {
                toast.Add("Save business branch fail", Severity.Warning);
            }
            
            is_saving = false;
        }
    }
}