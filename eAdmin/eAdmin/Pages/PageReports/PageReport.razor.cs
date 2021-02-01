using eModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eAdmin.Pages.PageReports
{
    public class PageReportModel: PageCore
    {
        public bool IsOpenedReport { get; set; } = false;
        public List<PermissionOptionModel> main_report_list = new List<PermissionOptionModel>();

        public PermissionOptionModel report { get; set; } = new PermissionOptionModel();
        protected override async Task OnInitializedAsync()
        {

            await Task.Delay(100);
            state.page_title = "Reports";
            main_report_list = gv.permission_options.Where(r => r.parent_id == 44).ToList();
            var x = main_report_list;
        }
        public void ViewReportClick(PermissionOptionModel _report)
        {
            report = _report;
            IsOpenedReport = true;
        }
    }
}
