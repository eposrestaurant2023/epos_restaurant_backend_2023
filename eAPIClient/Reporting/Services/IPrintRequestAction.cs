
using Reporting.Models;
using ReportModel;
using System.Threading.Tasks;

namespace Reporting.Services
{
    public interface IPrintRequestAction
    {
        Task<string> Invoice(DynamicModel receipt_data, ReceiptSettingModel setting,string file_path);
    }
}
