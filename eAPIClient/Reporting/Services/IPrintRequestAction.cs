
using Reporting.Models;
using ReportModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reporting.Services
{
    public interface IPrintRequestAction
    {
        Task<string> Invoice(DynamicModel receipt_data, ReceiptSettingModel setting,string file_path);
        Task<string> Receipt(DynamicModel receipt_data, ReceiptSettingModel setting, string file_path, bool is_reprint = false);
        Task<string> ParkingReceipt(DynamicModel receipt_data, ReceiptSettingModel setting, string file_path, bool is_reprint = false);
        Task<string> WaitingOrderSlip(DynamicModel receipt_data, ReceiptSettingModel setting, string file_path);
        Task<string> DeletedInvoice(DynamicModel receipt_data, ReceiptSettingModel setting, string file_path);
        Task<string> CouponVoucherReceipt(DynamicModel receipt_data, ReceiptSettingModel setting, string file_path);  
        Task<string> CloseWorkingDaySummary(DynamicModel receipt_data, List<DynamicModel> translate, ReceiptSettingModel setting, string file_path, string printed_by = "");
        Task<string> CloseWorkingDaySaleProduct(DynamicModel receipt_data, List<DynamicModel> translate, ReceiptSettingModel setting, string file_path, string printed_by = "");
        Task<string> CloseWorkingDaySaleTransaction(DynamicModel receipt_data, List<DynamicModel> translate, ReceiptSettingModel setting, string file_path, string printed_by = "");

        Task<string> CloseCashierShiftSummary(DynamicModel receipt_data, List<DynamicModel> translate, ReceiptSettingModel setting, string file_path, string printed_by = "");
        Task<string> CloseCashierShiftSaleProduct(DynamicModel receipt_data, List<DynamicModel> translate, ReceiptSettingModel setting, string file_path, string printed_by = "");
        Task<string> CloseCashierShiftSaleTransaction(DynamicModel receipt_data, List<DynamicModel> translate, ReceiptSettingModel setting, string file_path, string printed_by = "");

        Task<string> WifiPassword(DynamicModel receipt_data, string file_path);
    }
}
