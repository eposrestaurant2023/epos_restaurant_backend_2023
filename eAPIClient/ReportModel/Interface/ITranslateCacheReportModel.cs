
using System.Collections.Generic;

namespace ReportModel.Interface
{
    public interface ITranslateCacheReportModel
    {
        public string language_code { get; set; }
        public List<DynamicModel> translate_data { get; set; }
    } 
}
