
using ReportModel.Interface;
using System.Collections.Generic;

namespace ReportModel
{
  
    public interface TranslateCacheReportModel : ITranslateCacheReportModel
    {
        public string language_code { get; set; }
        public List<DynamicModel> translate_data { get; set; }
    }
}
