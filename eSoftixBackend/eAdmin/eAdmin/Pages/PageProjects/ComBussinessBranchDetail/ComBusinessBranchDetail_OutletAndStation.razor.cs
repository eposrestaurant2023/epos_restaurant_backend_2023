using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using eModels;
using System.Text.Json;
using eAdmin.Services;

namespace eAdmin.Pages.PageProjects.ComBussinessBranchDetail
{
    public partial class ComBusinessBranchDetail_OutletAndStation
    {
        [Parameter]
        public List<OutletModel> outlets { get; set; }

        [Parameter]
        public BusinessBranchModel business_branch { get; set; }

        [CascadingParameter]
        public AppState gv { get; set; }
        [Inject] IHttpService http { get; set; }
        [Inject] IDialogService Dialog { get; set; }
        
        [Inject] ISnackbar toast  { get; set; }
 
    

        bool is_loading;
        async Task AddNewOutlet()
        {
            var parameters = new DialogParameters{["model"] = new OutletModel()
            {business_branch_id = business_branch.id}};
            var dialog = Dialog.Show<ComBusinessBranchDetail_AddOutlet>("Add New Outlet", parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                outlets.Add((OutletModel)result.Data);
            }
        }

        async Task EditOutlet_Click(OutletModel outlet)
        {
            OutletModel backup_model = JsonSerializer.Deserialize<OutletModel>(JsonSerializer.Serialize(outlet));
            var parameters = new DialogParameters{["model"] = outlet};
            var dialog = Dialog.Show<ComBusinessBranchDetail_AddOutlet>("Edit Outlet", parameters);
            var result = await dialog.Result;
            if (result.Cancelled)
            {
                outlet = backup_model;
            }
        }

        async Task DeleteOutlet_Click(OutletModel outlet)
        {
            string message = "Are you sure you want to delete this Outlet?";
            if (outlet.is_deleted)
            {
                message = "Are you sure you want to restore this Outlet?";
            }

            bool? result = await Dialog.ShowMessageBox("Warning", message, yesText: outlet.is_deleted ? "Restore" : "Delete", cancelText: "Cancel");
            if (result != null)
            {
                is_loading = true;
                outlet.is_deleted = !outlet.is_deleted;
                var resp = await http.ApiPost("Outlet/Save", outlet);
                if (resp.IsSuccess)
                {
                    if (outlet.is_deleted)
                    {
                        toast.Add("Delete Outlet Successfully", Severity.Success);
                    }
                    else
                    {
                        toast.Add("Restore Outlet successfully", Severity.Success);
                    }
                }
                else
                {
                    outlet.is_deleted = !outlet.is_deleted;
                    if (outlet.is_deleted)
                    {
                        toast.Add("Delete Outlet Fail", Severity.Warning);
                    }
                    else
                    {
                        toast.Add("Restore Outlet Fail", Severity.Warning);
                    }
                }
            }

            is_loading = false;
        }

        async Task AddNewExtendLisence(StationModel station)
        {
            var parameters = new DialogParameters { ["model"] = new ExtendLicenseHistoryModel() { station_id = station.id } };
            var dialog = Dialog.Show<ComBusinessBranchDetail_AddExtendLicenseHistory>("Add New Extend License History", parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {

                station.extend_license_histories.Add((ExtendLicenseHistoryModel)result.Data);
            }
        }

        async Task AddNewStation(OutletModel outlet)
        {
            var parameters = new DialogParameters{["model"] = new StationModel() {outlet_id = outlet.id}};
            var dialog = Dialog.Show<ComBusinessBranchDetail_AddStation>("Add New Station", parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                outlet.stations.Add((StationModel)result.Data);
            }
        }

        async Task EditStation_Click(StationModel _station)
        {
            StationModel backup_model = JsonSerializer.Deserialize<StationModel>(JsonSerializer.Serialize(_station));
            var parameters = new DialogParameters { ["model"] = _station };
            var dialog = Dialog.Show<ComBusinessBranchDetail_AddStation>("Edit Station", parameters);
            var result = await dialog.Result;
            if (result.Cancelled)
            {
                _station = backup_model;
            }
        }

        async Task DeleteStation_Click(StationModel _station)
        {
            await Task.Delay(100);
        }
    }
}