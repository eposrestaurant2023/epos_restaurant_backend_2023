using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using System.Text; 
using ReportModel;
using System.Data;
using System.Collections.Generic;
using System.Reflection;
using Reporting.Models;
using System.Text.Json;
using Microsoft.Reporting.NETCore;
using System.Linq;

namespace Reporting.Services
{
    public class PrintRequestAction : IPrintRequestAction
    {
        public Task<string> Invoice(DynamicModel receipt_data,  ReceiptSettingModel setting, string file_path)
        {
            try
            {
                LocalReport report = new LocalReport();
                var _file = $"{file_path}\\{setting.invoice_file_name}.rdlc";
                report.ReportPath = _file; 


                //sale data
                DataTable sale_data = new DataTable();
                List<SaleReportModel> sales = new List<SaleReportModel>();
                sales = JsonSerializer.Deserialize<List<SaleReportModel>>(receipt_data.sale_data);
                sale_data = CreateDataTable(sales);
                //sale_product_data
                DataTable sale_product_data = new DataTable();
                List<SaleProductReportModel> sale_products = new List<SaleProductReportModel>();
                sale_products = JsonSerializer.Deserialize<List<SaleProductReportModel>>(receipt_data.sale_product_data);
                sale_product_data = CreateDataTable(sale_products);


                //Grand total data
                DataTable grand_total_data = new DataTable();
                List<GrandTotalReportModel> grand_totals = new List<GrandTotalReportModel>();
                grand_totals = JsonSerializer.Deserialize<List<GrandTotalReportModel>>(receipt_data.grand_total_data);
                grand_total_data = CreateDataTable(grand_totals);

                //Setting data
                DataTable setting_data = new DataTable();
                List<SettingReportModel> settings = new List<SettingReportModel>();
                settings = JsonSerializer.Deserialize<List<SettingReportModel>>(receipt_data.setting_data);
                setting_data = CreateDataTable(settings);

                //coupon voucher data
                DataTable coupon_voucher_data = new DataTable();
                List<CouponVoucherReportModel> coupon_voucher_list = new List<CouponVoucherReportModel>();
                if (receipt_data.coupon_voucher_data != null)
                {
                    coupon_voucher_list = JsonSerializer.Deserialize<List<CouponVoucherReportModel>>(receipt_data.coupon_voucher_data);
                }
                coupon_voucher_data = CreateDataTable(coupon_voucher_list);


                report.DataSources.Add(new ReportDataSource("Sale", sale_data));
                report.DataSources.Add(new ReportDataSource("SaleProduct", sale_product_data));
                report.DataSources.Add(new ReportDataSource("GrandTotal", grand_total_data));
                report.DataSources.Add(new ReportDataSource("Setting", setting_data));
                report.DataSources.Add(new ReportDataSource("CouponVoucher", coupon_voucher_data));

                //return data base64
                return Task.FromResult(GetImageData(report: report, feed_papper: setting.feed_papper));
            }
            catch (Exception ex)
            {
                return Task.FromResult("");
            }
        }       
        public Task<string> Receipt(DynamicModel receipt_data, ReceiptSettingModel setting, string file_path, bool is_reprint = false)
        {
            try
            {
                LocalReport report = new LocalReport();
                var _file = $"{file_path}\\{setting.receipt_file_name}.rdlc";
                report.ReportPath = _file;

                //sale data
                DataTable sale_data = new DataTable();
                List<SaleReportModel> sales = new List<SaleReportModel>();
                sales = JsonSerializer.Deserialize<List<SaleReportModel>>(receipt_data.sale_data);
                sales.ForEach(r => r.is_reprint_receipt = is_reprint);
                sale_data = CreateDataTable(sales);
                //sale_product_data
                DataTable sale_product_data = new DataTable();
                List<SaleProductReportModel> sale_products = new List<SaleProductReportModel>();
                sale_products = JsonSerializer.Deserialize<List<SaleProductReportModel>>(receipt_data.sale_product_data);
                sale_product_data = CreateDataTable(sale_products);


                //Grand total data
                DataTable grand_total_data = new DataTable();
                List<GrandTotalReportModel> grand_totals = new List<GrandTotalReportModel>();
                grand_totals = JsonSerializer.Deserialize<List<GrandTotalReportModel>>(receipt_data.grand_total_data);
                grand_total_data = CreateDataTable(grand_totals);

                //Setting data
                DataTable setting_data = new DataTable();
                List<SettingReportModel> settings = new List<SettingReportModel>();
                settings = JsonSerializer.Deserialize<List<SettingReportModel>>(receipt_data.setting_data);
                setting_data = CreateDataTable(settings);

                //sale payment data
                DataTable sale_payment_data = new DataTable();
                List<SalePaymentReportModel> sale_payments = new List<SalePaymentReportModel>();
                sale_payments = JsonSerializer.Deserialize<List<SalePaymentReportModel>>(receipt_data.sale_payment_data);
                sale_payment_data = CreateDataTable(sale_payments);

                //sale payment change data
                DataTable sale_payment_change_data = new DataTable();
                List<SalePaymentChangeModel> sale_payment_change_list = new List<SalePaymentChangeModel>();
                if (receipt_data.sale_payment_change_data != null)
                {
                    sale_payment_change_list = JsonSerializer.Deserialize<List<SalePaymentChangeModel>>(receipt_data.sale_payment_change_data);
                }
                sale_payment_change_data = CreateDataTable(sale_payment_change_list);



                //coupon voucher data
                DataTable coupon_voucher_data = new DataTable();
                List<CouponVoucherReportModel> coupon_voucher_list = new List<CouponVoucherReportModel>();
                if (receipt_data.coupon_voucher_data != null)
                {
                    coupon_voucher_list = JsonSerializer.Deserialize<List<CouponVoucherReportModel>>(receipt_data.coupon_voucher_data);
                }
                coupon_voucher_data = CreateDataTable(coupon_voucher_list);

                report.DataSources.Add(new ReportDataSource("Sale", sale_data));
                report.DataSources.Add(new ReportDataSource("SaleProduct", sale_product_data));
                report.DataSources.Add(new ReportDataSource("GrandTotal", grand_total_data));
                report.DataSources.Add(new ReportDataSource("Setting", setting_data));
                report.DataSources.Add(new ReportDataSource("SalePayment", sale_payment_data));
                report.DataSources.Add(new ReportDataSource("SalePaymentChange", sale_payment_change_data));
                report.DataSources.Add(new ReportDataSource("CouponVoucher", coupon_voucher_data));

                //return data base64
                return Task.FromResult(GetImageData(report: report, feed_papper: setting.feed_papper));


            }
            catch (Exception ex)
            {
                var x = ex;
                return Task.FromResult("");
            }
        }
        public Task<string> ParkingReceipt(DynamicModel receipt_data, ReceiptSettingModel setting, string file_path, bool is_reprint = false)
        {
            try
            {
                LocalReport report = new LocalReport();
                var _file = $"{file_path}\\{setting.receipt_file_name??setting.invoice_file_name}.rdlc";
                report.ReportPath = _file;

                //sale data
                DataTable sale_data = new DataTable();
                List<SaleReportModel> sales = new List<SaleReportModel>();
                sales = JsonSerializer.Deserialize<List<SaleReportModel>>(receipt_data.sale_data);
                sales.ForEach(r => r.is_reprint_receipt = is_reprint);
                sale_data = CreateDataTable(sales);
                //sale_product_data
                DataTable sale_product_data = new DataTable();
                List<SaleProductReportModel> sale_products = new List<SaleProductReportModel>();
                sale_products = JsonSerializer.Deserialize<List<SaleProductReportModel>>(receipt_data.sale_product_data);
                sale_product_data = CreateDataTable(sale_products.Where(r => r.is_park == true));

                //Setting data
                DataTable setting_data = new DataTable();
                List<SettingReportModel> settings = new List<SettingReportModel>();
                settings = JsonSerializer.Deserialize<List<SettingReportModel>>(receipt_data.setting_data);
                setting_data = CreateDataTable(settings);

                report.DataSources.Add(new ReportDataSource("Sale", sale_data));
                report.DataSources.Add(new ReportDataSource("SaleProduct", sale_product_data));
                report.DataSources.Add(new ReportDataSource("Setting", setting_data));


                //return data base64
                return Task.FromResult(GetImageData(report: report, feed_papper: setting.feed_papper));


            }
            catch (Exception ex)
            {
                var x = ex;
                return Task.FromResult("");
            }
        }
        public Task<string> DeletedInvoice(DynamicModel receipt_data, ReceiptSettingModel setting, string file_path)
        {
            try
            {
                LocalReport report = new LocalReport();
                var _file = $"{file_path}\\{setting.invoice_file_name}.rdlc";
                report.ReportPath = _file; 

                //sale data
                DataTable sale_data = new DataTable();
                List<SaleReportModel> sales = new List<SaleReportModel>();
                sales = JsonSerializer.Deserialize<List<SaleReportModel>>(receipt_data.sale_data);
                sale_data = CreateDataTable(sales);
                //sale_product_data
                DataTable sale_product_data = new DataTable();
                List<SaleProductReportModel> sale_products = new List<SaleProductReportModel>();
                sale_products = JsonSerializer.Deserialize<List<SaleProductReportModel>>(receipt_data.sale_product_data);
                sale_product_data = CreateDataTable(sale_products);


                //Grand total data
                DataTable grand_total_data = new DataTable();
                List<GrandTotalReportModel> grand_totals = new List<GrandTotalReportModel>();
                grand_totals = JsonSerializer.Deserialize<List<GrandTotalReportModel>>(receipt_data.grand_total_data);
                grand_total_data = CreateDataTable(grand_totals);

                //Setting data
                DataTable setting_data = new DataTable();
                List<SettingReportModel> settings = new List<SettingReportModel>();
                settings = JsonSerializer.Deserialize<List<SettingReportModel>>(receipt_data.setting_data);
                setting_data = CreateDataTable(settings);

                report.DataSources.Add(new ReportDataSource("Sale", sale_data));
                report.DataSources.Add(new ReportDataSource("SaleProduct", sale_product_data));
                report.DataSources.Add(new ReportDataSource("GrandTotal", grand_total_data));
                report.DataSources.Add(new ReportDataSource("Setting", setting_data));

                //return data base64
                return Task.FromResult(GetImageData(report: report, feed_papper: setting.feed_papper));
            }
            catch (Exception ex)
            {
                return Task.FromResult("");
            }
        }
        public Task<string> WaitingOrderSlip(DynamicModel receipt_data, ReceiptSettingModel setting, string file_path)
        {
            try
            {
                LocalReport report = new LocalReport();
                var _file = $"{file_path}\\{setting.invoice_file_name}.rdlc";
                report.ReportPath = _file;


                //sale data
                DataTable sale_data = new DataTable();
                List<SaleReportModel> sales = new List<SaleReportModel>();
                sales = JsonSerializer.Deserialize<List<SaleReportModel>>(receipt_data.sale_data);
                sale_data = CreateDataTable(sales);
                //sale_product_data
                DataTable sale_product_data = new DataTable();
                List<SaleProductReportModel> sale_products = new List<SaleProductReportModel>();
                sale_products = JsonSerializer.Deserialize<List<SaleProductReportModel>>(receipt_data.sale_product_data);
                sale_product_data = CreateDataTable(sale_products);


                //Grand total data
                DataTable grand_total_data = new DataTable();
                List<GrandTotalReportModel> grand_totals = new List<GrandTotalReportModel>();
                grand_totals = JsonSerializer.Deserialize<List<GrandTotalReportModel>>(receipt_data.grand_total_data);
                grand_total_data = CreateDataTable(grand_totals);

                //Setting data
                DataTable setting_data = new DataTable();
                List<SettingReportModel> settings = new List<SettingReportModel>();
                settings = JsonSerializer.Deserialize<List<SettingReportModel>>(receipt_data.setting_data);
                setting_data = CreateDataTable(settings);

                report.DataSources.Add(new ReportDataSource("Sale", sale_data));
                report.DataSources.Add(new ReportDataSource("SaleProduct", sale_product_data));
                report.DataSources.Add(new ReportDataSource("GrandTotal", grand_total_data));
                report.DataSources.Add(new ReportDataSource("Setting", setting_data));

                //return data base64
                return Task.FromResult(GetImageData(report: report, feed_papper: setting.feed_papper));
            }
            catch (Exception ex)
            {
                return Task.FromResult("");
            }
        }
        public Task<string> CouponVoucherReceipt(DynamicModel receipt_data, ReceiptSettingModel setting, string file_path)
        {
            try
            {
                LocalReport report = new LocalReport();
                var _file = $"{file_path}\\{setting.receipt_file_name}.rdlc";
                report.ReportPath = _file;

                //Setting data
                DataTable setting_data = new DataTable();
                List<SettingReportModel> settings = new List<SettingReportModel>();
                settings = JsonSerializer.Deserialize<List<SettingReportModel>>(receipt_data.setting_data);
                setting_data = CreateDataTable(settings);

                //coupon voucher data
                DataTable coupon_voucher_data = new DataTable();
                List<CouponVoucherTransactionModel> coupon_voucher_list = new List<CouponVoucherTransactionModel>();
                if (receipt_data.coupon_voucher_data != null)
                {
                    coupon_voucher_list = JsonSerializer.Deserialize<List<CouponVoucherTransactionModel>>(receipt_data.coupon_voucher_data);
                }
                coupon_voucher_data = CreateDataTable(coupon_voucher_list);

                report.DataSources.Add(new ReportDataSource("Setting", setting_data));
                report.DataSources.Add(new ReportDataSource("CouponVoucher", coupon_voucher_data));

                //return data base64
                return Task.FromResult(GetImageData(report: report, feed_papper: setting.feed_papper));


            }
            catch (Exception ex)
            {
                var x = ex;
                return Task.FromResult("");
            }
        }
        public Task<string> CloseWorkingDaySummary(DynamicModel receipt_data, List<DynamicModel> translate, ReceiptSettingModel setting, string file_path, string printed_by = "")
        {
            try
            {
                var report_data = receipt_data;
                LocalReport report = new LocalReport();
                var _file = $"{file_path}\\{setting.receipt_file_name??setting.invoice_file_name}.rdlc";
                report.ReportPath = _file;

                //Working day info
                DataTable working_day_data = new DataTable();
                List<WorkingDayReportModel> working_day = new List<WorkingDayReportModel>();
                working_day = JsonSerializer.Deserialize<List<WorkingDayReportModel>>(report_data.working_day_info);
                working_day_data = CreateDataTable(working_day);

                //Working day summaryu data
                DataTable working_day_summary_data = new DataTable();
                List<CloseWorkingDaySummaryReportModel> working_day_summary = new List<CloseWorkingDaySummaryReportModel>();
                working_day_summary = JsonSerializer.Deserialize<List<CloseWorkingDaySummaryReportModel>>(report_data.working_day_data);
                working_day_summary_data = CreateDataTable(working_day_summary);

                //Close sale data data
                DataTable close_sale_data = new DataTable();
                List<SaleReportModel> close_sales = new List<SaleReportModel>();
                close_sales = JsonSerializer.Deserialize<List<SaleReportModel>>(report_data.sale_data);
                close_sale_data = CreateDataTable(close_sales);

                //Setting data
                DataTable setting_data = new DataTable();
                List<SettingReportModel> settings = new List<SettingReportModel>();
                settings = JsonSerializer.Deserialize<List<SettingReportModel>>(report_data.setting_data);
                settings.ForEach(r => r.printed_by = printed_by);
                setting_data = CreateDataTable(settings);

                report.DataSources.Add(new ReportDataSource("WorkingDay", working_day_data));
                report.DataSources.Add(new ReportDataSource("Setting", setting_data));
                report.DataSources.Add(new ReportDataSource("CloseWorkingDayData", working_day_summary_data));
                report.DataSources.Add(new ReportDataSource("CloseSaleData", close_sale_data));
                report.DataSources.Add(new ReportDataSource("Translate", CreateDataTable(translate)));


                //return data base64
                return Task.FromResult(GetImageData(report: report, feed_papper: setting.feed_papper));


            }
            catch (Exception ex)
            {
                var x = ex;
                return Task.FromResult("");
            }
        }
        public Task<string> CloseWorkingDaySaleProduct(DynamicModel receipt_data, List<DynamicModel> translate, ReceiptSettingModel setting, string file_path, string printed_by = "")
        {
            try
            {
                var report_data = receipt_data;
                LocalReport report = new LocalReport();
                var _file = $"{file_path}\\{setting.receipt_file_name ?? setting.invoice_file_name}.rdlc";
                report.ReportPath = _file;

                //Working day info
                DataTable working_day_data = new DataTable();
                List<WorkingDayReportModel> working_day = new List<WorkingDayReportModel>();
                working_day = JsonSerializer.Deserialize<List<WorkingDayReportModel>>(report_data.working_day_info);
                working_day_data = CreateDataTable(working_day);

                //Setting data
                DataTable setting_data = new DataTable();
                List<SettingReportModel> settings = new List<SettingReportModel>();
                settings = JsonSerializer.Deserialize<List<SettingReportModel>>(report_data.setting_data);
                settings.ForEach(r => r.printed_by = printed_by);
                setting_data = CreateDataTable(settings);

                //Sale Product Data
                DataTable sale_product_data = new DataTable();
                List<SaleProductReportModel> sale_products = new List<SaleProductReportModel>();
                sale_products = JsonSerializer.Deserialize<List<SaleProductReportModel>>(report_data.sale_product_data);
                sale_product_data = CreateDataTable(sale_products);

                //FOC Sale Product Data
                DataTable foc_sale_product_data = new DataTable();
                List<SaleProductReportModel> foc_sale_products = new List<SaleProductReportModel>();
                foc_sale_products = JsonSerializer.Deserialize<List<SaleProductReportModel>>(report_data.foc_sale_product_data);
                foc_sale_product_data = CreateDataTable(foc_sale_products);

                report.DataSources.Add(new ReportDataSource("WorkingDay", working_day_data));
                report.DataSources.Add(new ReportDataSource("Setting", setting_data));
                report.DataSources.Add(new ReportDataSource("SaleProductData", sale_product_data));
                report.DataSources.Add(new ReportDataSource("FOCSaleProductData", foc_sale_product_data));
                report.DataSources.Add(new ReportDataSource("Translate", CreateDataTable(translate)));

                //return data base64
                return Task.FromResult(GetImageData(report: report, feed_papper: setting.feed_papper));


            }
            catch (Exception ex)
            {
                var x = ex;
                return Task.FromResult("");
            }
        }
        public Task<string> CloseWorkingDaySaleTransaction(DynamicModel receipt_data, List<DynamicModel> translate, ReceiptSettingModel setting, string file_path, string printed_by = "")
        {
            try
            {
                var report_data = receipt_data;
                LocalReport report = new LocalReport();
                var _file = $"{file_path}\\{setting.receipt_file_name ?? setting.invoice_file_name}.rdlc";
                report.ReportPath = _file;

                //Working day info
                DataTable working_day_data = new DataTable();
                List<WorkingDayReportModel> working_day = new List<WorkingDayReportModel>();
                working_day = JsonSerializer.Deserialize<List<WorkingDayReportModel>>(report_data.working_day_info);
                working_day_data = CreateDataTable(working_day);

                //Setting data
                DataTable setting_data = new DataTable();
                List<SettingReportModel> settings = new List<SettingReportModel>();
                settings = JsonSerializer.Deserialize<List<SettingReportModel>>(report_data.setting_data);
                settings.ForEach(r => r.printed_by = printed_by);
                setting_data = CreateDataTable(settings);

                //Close sale data data
                DataTable close_sale_data = new DataTable();
                List<SaleReportModel> close_sales = new List<SaleReportModel>();
                close_sales = JsonSerializer.Deserialize<List<SaleReportModel>>(report_data.sale_data);
                close_sale_data = CreateDataTable(close_sales);

                report.DataSources.Add(new ReportDataSource("WorkingDay", working_day_data));
                report.DataSources.Add(new ReportDataSource("Setting", setting_data));
                report.DataSources.Add(new ReportDataSource("CloseSaleData", close_sale_data));
                report.DataSources.Add(new ReportDataSource("Translate", CreateDataTable(translate)));

                //return data base64
                return Task.FromResult(GetImageData(report: report, feed_papper: setting.feed_papper)); 


            }
            catch (Exception ex)
            {
                var x = ex;
                return Task.FromResult("");
            }
        }
        public Task<string> CloseCashierShiftSummary(DynamicModel receipt_data, List<DynamicModel> translate, ReceiptSettingModel setting, string file_path, string printed_by = "")
        {
            try
            {
                var report_data = receipt_data;
                LocalReport report = new LocalReport();
                var _file = $"{file_path}\\{setting.receipt_file_name ?? setting.invoice_file_name}.rdlc";
                report.ReportPath = _file;

                //cashier shift info
                DataTable cashier_shift_data = new DataTable();
                List<CashierShiftReportModel> cashier_shift = new List<CashierShiftReportModel>();
                cashier_shift = JsonSerializer.Deserialize<List<CashierShiftReportModel>>(report_data.cashier_shift_info);
                cashier_shift_data = CreateDataTable(cashier_shift);

                //cashier shift summaryu data
                DataTable cashier_shift_summary_data = new DataTable();
                List<CloseCashierShiftSummaryDataModel> cashier_shift_summary = new List<CloseCashierShiftSummaryDataModel>();
                cashier_shift_summary = JsonSerializer.Deserialize<List<CloseCashierShiftSummaryDataModel>>(report_data.cashier_shift_data);
                cashier_shift_summary_data = CreateDataTable(cashier_shift_summary);

                //Close sale data data
                DataTable close_sale_data = new DataTable();
                List<SaleReportModel> close_sales = new List<SaleReportModel>();
                close_sales = JsonSerializer.Deserialize<List<SaleReportModel>>(report_data.sale_data);
                close_sale_data = CreateDataTable(close_sales);

                //Setting data
                DataTable setting_data = new DataTable();
                List<SettingReportModel> settings = new List<SettingReportModel>();
                settings = JsonSerializer.Deserialize<List<SettingReportModel>>(report_data.setting_data);
                settings.ForEach(r => r.printed_by = printed_by);
                setting_data = CreateDataTable(settings);

                report.DataSources.Add(new ReportDataSource("CashierShiftData", cashier_shift_data));
                report.DataSources.Add(new ReportDataSource("Setting", setting_data));
                report.DataSources.Add(new ReportDataSource("CloseCashierShiftData", cashier_shift_summary_data));
                report.DataSources.Add(new ReportDataSource("CloseSaleData", close_sale_data));
                report.DataSources.Add(new ReportDataSource("Translate", CreateDataTable(translate))); 

                //return data base64
                return Task.FromResult(GetImageData(report: report, feed_papper: setting.feed_papper));


            }
            catch (Exception ex)
            {
                var x = ex;
                return Task.FromResult("");
            }
        }
        public Task<string> CloseCashierShiftSaleProduct(DynamicModel receipt_data, List<DynamicModel> translate, ReceiptSettingModel setting, string file_path, string printed_by = "")
        {
            try
            {
                var report_data = receipt_data;
                LocalReport report = new LocalReport();
                var _file = $"{file_path}\\{setting.receipt_file_name ?? setting.invoice_file_name}.rdlc";
                report.ReportPath = _file;

                //cashier shift info
                DataTable cashier_shift_data = new DataTable();
                List<CashierShiftReportModel> cashier_shift = new List<CashierShiftReportModel>();
                cashier_shift = JsonSerializer.Deserialize<List<CashierShiftReportModel>>(report_data.cashier_shift_info);
                cashier_shift_data = CreateDataTable(cashier_shift);

                //Setting data
                DataTable setting_data = new DataTable();
                List<SettingReportModel> settings = new List<SettingReportModel>();
                settings = JsonSerializer.Deserialize<List<SettingReportModel>>(report_data.setting_data);
                settings.ForEach(r => r.printed_by = printed_by);
                setting_data = CreateDataTable(settings);

                //Sale Product Data
                DataTable sale_product_data = new DataTable();
                List<SaleProductReportModel> sale_products = new List<SaleProductReportModel>();
                sale_products = JsonSerializer.Deserialize<List<SaleProductReportModel>>(report_data.sale_product_data);
                sale_product_data = CreateDataTable(sale_products);

                //FOC Sale Product Data
                DataTable foc_sale_product_data = new DataTable();
                List<SaleProductReportModel> foc_sale_products = new List<SaleProductReportModel>();
                foc_sale_products = JsonSerializer.Deserialize<List<SaleProductReportModel>>(report_data.foc_sale_product_data);
                foc_sale_product_data = CreateDataTable(foc_sale_products);

                report.DataSources.Add(new ReportDataSource("CashierShiftData", cashier_shift_data));
                report.DataSources.Add(new ReportDataSource("Setting", setting_data));
                report.DataSources.Add(new ReportDataSource("SaleProductData", sale_product_data));
                report.DataSources.Add(new ReportDataSource("FOCSaleProductData", foc_sale_product_data));
                report.DataSources.Add(new ReportDataSource("Translate", CreateDataTable(translate)));

                //return data base64
                return Task.FromResult(GetImageData(report: report, feed_papper: setting.feed_papper));


            }
            catch (Exception ex)
            {
                var x = ex;
                return Task.FromResult("");
            }
        }
        public Task<string> CloseCashierShiftSaleTransaction(DynamicModel receipt_data, List<DynamicModel> translate, ReceiptSettingModel setting, string file_path, string printed_by = "")
        {
            try
            {
                var report_data = receipt_data;
                LocalReport report = new LocalReport();
                var _file = $"{file_path}\\{setting.receipt_file_name ?? setting.invoice_file_name}.rdlc";
                report.ReportPath = _file;


                //cashier shift info
                DataTable cashier_shift_data = new DataTable();
                List<CashierShiftReportModel> cashier_shift = new List<CashierShiftReportModel>();
                cashier_shift = JsonSerializer.Deserialize<List<CashierShiftReportModel>>(report_data.cashier_shift_info);
                cashier_shift_data = CreateDataTable(cashier_shift);

                //Close sale data data
                DataTable close_sale_data = new DataTable();
                List<SaleReportModel> close_sales = new List<SaleReportModel>();
                close_sales = JsonSerializer.Deserialize<List<SaleReportModel>>(report_data.sale_data);
                close_sale_data = CreateDataTable(close_sales);

                //Setting data
                DataTable setting_data = new DataTable();
                List<SettingReportModel> settings = new List<SettingReportModel>();
                settings = JsonSerializer.Deserialize<List<SettingReportModel>>(report_data.setting_data);
                settings.ForEach(r => r.printed_by = printed_by);
                setting_data = CreateDataTable(settings);

                report.DataSources.Add(new ReportDataSource("CashierShiftData", cashier_shift_data));
                report.DataSources.Add(new ReportDataSource("Setting", setting_data));
                report.DataSources.Add(new ReportDataSource("CloseSaleData", close_sale_data));
                report.DataSources.Add(new ReportDataSource("Translate", CreateDataTable(translate)));

                //return data base64
                return Task.FromResult(GetImageData(report: report, feed_papper: setting.feed_papper));


            }
            catch (Exception ex)
            {
                var x = ex;
                return Task.FromResult("");
            }
        }
        public Task<string> WifiPassword(DynamicModel receipt_data, string file_path)
        {
            try
            {

                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                var _file = $"{file_path}\\rpt_wifi_password.rdlc";
                LocalReport report = new LocalReport();
                report.ReportPath = _file;

                //Setting data
                DataTable setting_data = new DataTable();
                List<SettingReportModel> settings = new List<SettingReportModel>();
                settings = JsonSerializer.Deserialize<List<SettingReportModel>>(receipt_data.setting_data);
                setting_data = CreateDataTable(settings);

                report.DataSources.Add(new ReportDataSource("Setting", setting_data));             
                //return data base64
                return Task.FromResult(GetImageData(report: report, feed_papper: 0));
                 
            }
            catch (Exception ex)
            {
                return Task.FromResult("");
            }
        }

       

     /// <summary>
     /// Local Funtion 
     /// </summary>
     /// <typeparam name="T"></typeparam>
     /// <param name="list"></param>
     /// <returns></returns>
        private  DataTable CreateDataTable<T>(IEnumerable<T> list)
        {
            Type type = typeof(T);
            var properties = type.GetProperties();

            DataTable dataTable = new DataTable();
            dataTable.TableName = typeof(T).FullName;
            foreach (PropertyInfo info in properties)
            {
                dataTable.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
            }

            foreach (T entity in list)
            {
                object[] values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(entity);
                }

                dataTable.Rows.Add(values);
            }

            return dataTable;
        }
        private Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }
        private string CropTopBottomImage(Bitmap bmp, int image_width,short feed_papper)
        {
            int h = bmp.Height;
            Func<int, bool> allWhiteRow = row =>
            {
                for (int i = 0; i < image_width; ++i)
                    if (bmp.GetPixel(i, row).R != 255)
                        return false;
                return true;
            };

            Func<int, bool> allWhiteColumn = col =>
            {
                for (int i = 0; i < h; ++i)
                    if (bmp.GetPixel(col, i).R != 255)
                        return false;
                return true;
            };

            int topmost = 0;
            for (int row = 0; row < h; ++row)
            {
                if (allWhiteRow(row))
                    topmost = row;
                else break;
            }

            int bottommost = 0;
            for (int row = h - 1; row >= 0; --row)
            {
                if (allWhiteRow(row))
                    bottommost = row;
                else break;
            }


            if (bottommost == 0) bottommost = h; // As reached top.

            int croppedHeight = bottommost - topmost;



            if (croppedHeight == 0) // No border on top or bottom
            {
                topmost = 0;
                croppedHeight = h;
            } 
            croppedHeight = croppedHeight + (feed_papper * 10);

            try
            {
                var target = new Bitmap(image_width, croppedHeight);
                using (Graphics g = Graphics.FromImage(target))
                {
                    g.DrawImage(bmp,
                      new RectangleF(0, 0, image_width, croppedHeight),
                      new RectangleF(0, topmost, image_width, croppedHeight),
                      GraphicsUnit.Pixel);
                }
                MemoryStream ms = new MemoryStream();
                target.Save(ms, ImageFormat.Jpeg);
                byte[] byteImage = ms.ToArray();
                var result = Convert.ToBase64String(byteImage); // Get Base64
                return result;

            }
            catch (Exception ex)
            {
                throw new Exception(
                  string.Format("Values are topmost={0} btm={1} left={2} right={3} croppedWidth={4} croppedHeight={5}", topmost, bottommost, 0, 0, image_width, croppedHeight),
                  ex);
            }
        }
        private string GetImageData(LocalReport report, short feed_papper)
        {
            try
            {
                byte[] byts = report.Render("IMAGE");
                var img = byteArrayToImage(byts);
                Bitmap bmp = new Bitmap(width: img.Width, height: img.Height, PixelFormat.Format32bppArgb);
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.White);
                    g.DrawImage(img, new Rectangle(new Point(), img.Size), new Rectangle(new Point(), img.Size), GraphicsUnit.Pixel);
                }

                return CropTopBottomImage(bmp, bmp.Width, feed_papper);
            }
            catch (Exception ex)
            {
                
                return "";
            }
        }
 
      
    }
}
