using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using eModels;

namespace eAdmin.Pages
{
    public class Dashboard:PageCore
    {
        public DashboardModel model { get; set; }
        public List<DashboardChart> models { get; set; } = new List<DashboardChart>();
        public List<ProjectByProjectType> projects { get; set; } = new List<ProjectByProjectType>();
        public List<CustomerByCustomerGroup> customers { get; set; } = new List<CustomerByCustomerGroup>();

        protected override async Task OnInitializedAsync()
        {
            
            is_loading_data = true;

            models.Add(new DashboardChart() { new_customer=12, open_value = 40, close_value = 12, month="Jan"});
            models.Add(new DashboardChart() { new_customer = 35, open_value = 30, close_value = 25, month="Feb"});
            models.Add(new DashboardChart() { new_customer = 56, open_value = 45, close_value = 5, month="Mar"});
            models.Add(new DashboardChart() { new_customer = 12, open_value = 60, close_value = 45, month="Apr"});
            models.Add(new DashboardChart() { new_customer = 12, open_value = 60, close_value = 45, month="May"});
            models.Add(new DashboardChart() { new_customer = 35, open_value = 12, close_value = 9, month="Jun"});
            models.Add(new DashboardChart() { new_customer = 12, open_value = 40, close_value = 12, month = "July" });
            models.Add(new DashboardChart() { new_customer = 35, open_value = 30, close_value = 25, month = "Aug" });
            models.Add(new DashboardChart() { new_customer = 56, open_value = 45, close_value = 5, month = "Sep" });
            models.Add(new DashboardChart() { new_customer = 12, open_value = 60, close_value = 45, month = "Oct" });
            models.Add(new DashboardChart() { new_customer = 35, open_value = 12, close_value = 9, month = "Nov" });
            models.Add(new DashboardChart() { new_customer = 12, open_value = 60, close_value = 45, month = "Dec" });
            projects.Add(new ProjectByProjectType() { value = 20, label = "Type 1" });
            projects.Add(new ProjectByProjectType() { value = 25, label = "Type 2" });
            customers.Add(new CustomerByCustomerGroup() { value = 55, label = "Group 1" });
            customers.Add(new CustomerByCustomerGroup() { value = 15, label = "Group 2" });
            var res = await http.ApiPost("GetData",new FilterModel() { 
                procedure_name= "sp_get_dashboard_data"
            });
            if (res.IsSuccess)
            {
                model = JsonSerializer.Deserialize<DashboardModel>(res.Content.ToString());
            }
            

            is_loading_data = false;
        }
    }
  
    public class DashboardChart
    {
        public string label { get; set; }
        public decimal close_value { get; set; }
        public decimal open_value { get; set; }
        public decimal new_customer { get; set; }
        public string month { get; set; }
    }

    public class ProjectByProjectType
    {
        public string label { get; set; }
        public decimal value { get; set; }
    }
    public class CustomerByCustomerGroup
    {
        public string label { get; set; }
        public decimal value { get; set; }
    }
}
