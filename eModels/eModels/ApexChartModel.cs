using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eModels
{
    public class ApexChartModel
    {
        public ApexChartModel()
        {
            data = new List<ApexChartDataModel>();
        }
        public string title { get; set; }
        public string color { get; set; }

        public decimal value { get; set; }

        public List<ApexChartDataModel> data { get; set; } = new List<ApexChartDataModel>();
    }
    public class ApexChartDataModel
    {
        public string label { get; set; }
        public decimal value { get; set; }
    }
}
