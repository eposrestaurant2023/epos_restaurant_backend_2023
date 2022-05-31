using eModels;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eAdmin.Pages.PageReports
{
    public class PageReportModel: PageCore
    {
        public bool IsOpenedReport { get; set; } = false;
        public bool IsMenuReportOpened { get; set; } = false;

        public List<PermissionOptionModel> main_report_list = new List<PermissionOptionModel>();
        public string keyword { get; set; } = "";
        public PermissionOptionModel report { get; set; } = new PermissionOptionModel();

        public string report_title
        {
            get
            {
                if (report != null && report.id > 0)
                {
                    return lang["Report"] + "-" + gv.current_language.language_id == "km-KH" ? report.report_title_kh : report.report_title;
                }
                else
                {
                    return lang["Report"];
                }
            }

        }
        protected override async Task OnInitializedAsync()
        {

            await Task.Delay(100);
            state.page_title = "Reports";
            main_report_list = gv.permission_options.Where(r => r.parent_id == 44 && r.is_public_report == true).ToList();
            var x = main_report_list;
        }

        public async Task ViewReportClick(PermissionOptionModel _report)
        {
            if (report == null) report = new PermissionOptionModel();
            if (report.id != _report.id)
            {
                is_loading_data = true;
                await Task.Delay(100);
                report = _report;
                is_loading_data = false;
            }
            IsMenuReportOpened = false;



        }
        
        public void OnViewFullscreenReport()
        {
            var parameters = new DialogParameters { ["parent_id"] = report.parent_id, ["report_id"] = report.id, ["gv"] = gv };
            Dialog.Show<eAdmin.Shared.Components.ComPreviewReport>(lang["Reports"], parameters, new DialogOptions() { FullScreen = true, CloseButton = true });

        }
    }
}
