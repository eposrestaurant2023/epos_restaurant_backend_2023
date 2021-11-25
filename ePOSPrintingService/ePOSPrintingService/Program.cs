﻿using Microsoft.VisualBasic;
using System;
using System.Web;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using Microsoft.Reporting.WinForms;
using Newtonsoft.Json;
using ePOSPrintingService.Models;
using System.Reflection;

namespace ePOSPrintingService
{

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            if (LoadSetting())
            {
#if DEBUG
                 
                ePOSPrintingService myService = new ePOSPrintingService();
                myService.OnDebug();
                System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
#else
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                    new ePOSMobileOrderServices()
                }; 
                ServiceBase.Run(ServicesToRun);  
#endif      
            }
        }

        public static bool LoadSetting()
        {
            bool isLoaded = false;
            try
            {
                DBConnection = eSoftixSecurity.StringCipher.Decrypt(Properties.Settings.Default.DBConnection, "admin@eSoftix");
                FileWatcherPath = Properties.Settings.Default.FileWatcherPath;
                LabelPrinterName = Properties.Settings.Default.LabelPrinterName;
                CashierPrinter = Properties.Settings.Default.CashierPrinter;
                string str_receipt_list = Properties.Settings.Default.ReceiptSettings.Replace("\r\n", "");

                ReceiptLists = JsonConvert.DeserializeObject<List<ReceiptListModel>>(str_receipt_list);
                LabelPageSetup = JsonConvert.DeserializeObject<LabelPageSetupModel>(Properties.Settings.Default.LabelPageSetup.ToString());

                isLoaded = true;
            }
            catch (Exception ex)
            {
                WriteToFile(ex.Message);
                isLoaded = false;
            }
            return isLoaded;
        }
        public static string GetString(object data)
        {
            if (data == null)
                return "";
            else
                return Convert.ToString(data);
        }
        public static DateTime GetDate(object data)
        {
            if (data == null)
                return DateTime.Now;
            else
                return Convert.ToDateTime(data);
        }
        public static int GetInt(object data)
        {
            if (data == null)
                return 0;
            else
                return Convert.ToInt32(Conversion.Val(data));
        }
        public static decimal GetDecimal(object data)
        {
            if (data == null)
                return 0;
            else
                return Convert.ToDecimal(Conversion.Val(data));
        }
        public static bool GetBoolean(object data)
        {
            if (data == null)
                return false;
            else
                return Convert.ToBoolean(data);
        }
        public static decimal GetNumber(string number)
        {
            decimal n = 0;
            try
            {
                n = Convert.ToDecimal(Conversion.Val(number.Replace("$", "").Replace("B", "").Replace(",", "").Replace("R", "").Replace("%", "").Replace("៛", "")));
            }
            catch
            {
                return n;
            }

            return n;
        }



        public static string DBConnection { get; set; }
        public static string FileWatcherPath { get; set; }
        public static string LabelPrinterName { get; set; }
        public static string CashierPrinter { get; set; }
        public static List<ReceiptListModel> ReceiptLists { get; set; }
        public static LabelPageSetupModel LabelPageSetup { get; set; }
        public static bool IsPrintSuccess { get; set; }


        public static object ExecuteSql(string sql)
        {
            SqlConnection cn = new SqlConnection(DBConnection);
            cn.Open();
            SqlCommand cmd = new SqlCommand(sql, cn);
            object data = cmd.ExecuteScalar();
            cmd.Dispose();
            cn.Close();
            cn.Dispose();
            return data;
        }
        public static DataTable ExecuteToDataTable(string sql)
        {
            SqlConnection cn = new SqlConnection(DBConnection);
            cn.Open();
            SqlCommand cmd = new SqlCommand(sql, cn);
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            cmd.Dispose();
            cn.Close();
            cn.Dispose();
            return dt;
        }

        public static DataTable CreateDataTable<T>(IEnumerable<T> list)
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

        //==================Print Receipt ======================
        //Print Report
        private static int m_currentPageIndex;
        private static IList<Stream> m_streams;
        private static Stream CreateStream(string name, string fileNameExtension, Encoding encoding, string mimeType, bool willSeek)
        {
            Stream stream = new MemoryStream();
            m_streams.Add(stream);
            return stream;
        }
        public static void Export(LocalReport report, decimal page_width, decimal page_height, decimal margin_top, decimal margin_left, decimal margin_right, decimal margin_buttom)
        {
            string deviceInfo = "";
            deviceInfo = string.Format(
            @"<DeviceInfo>
                <OutputFormat>emf</OutputFormat>
                <PageWidth>{0}in</PageWidth>
                <PageHeight>{1}in</PageHeight>
                <MarginTop>{2}in</MarginTop>
                <MarginLeft>{3}in</MarginLeft>
                <MarginRight>{4}in</MarginRight>
                <MarginBottom>{5}in</MarginBottom>
            </DeviceInfo>", page_width, page_height, margin_top, margin_left, margin_right, margin_buttom);

            Warning[] warnings;
            m_streams = new List<Stream>();
            report.Render("Image", deviceInfo, CreateStream,
               out warnings);
            foreach (Stream stream in m_streams)
                stream.Position = 0;
        }
        private static void PrintPage(object sender, PrintPageEventArgs ev)
        {
            Metafile pageImage = new
               Metafile(m_streams[m_currentPageIndex]);

            Rectangle adjustedRect = new Rectangle(
                ev.PageBounds.Left - (int)ev.PageSettings.HardMarginX,
                ev.PageBounds.Top - (int)ev.PageSettings.HardMarginY,
                ev.PageBounds.Width,
                ev.PageBounds.Height);

            ev.Graphics.FillRectangle(Brushes.White, adjustedRect);
            ev.Graphics.DrawImage(pageImage, adjustedRect);
            m_currentPageIndex++;
            ev.HasMorePages = (m_currentPageIndex < m_streams.Count);
        }
        public static void Print(string printerName, short copy = 1)
        {
            if (m_streams == null || m_streams.Count == 0)
                throw new Exception("Error: no stream to print.");
            PrintDocument printDoc = new PrintDocument();
            printDoc.PrinterSettings.Copies = copy;
            PrintController printControl = new StandardPrintController();
            printDoc.PrintController = printControl;
            printDoc.PrinterSettings.PrinterName = printerName;
            if (!printDoc.PrinterSettings.IsValid)
            {
                try { throw new Exception("Error: cannot find the default printer."); }
                catch { /*do nothing*/ }
            }
            else
            {
                try
                {
                    printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
                    m_currentPageIndex = 0;
                    printDoc.Print();
                }
                catch {  /*do nothing*/  }
            }
        }
        private static void Dispose()
        {
            if (m_streams != null)
            {
                foreach (Stream stream in m_streams)
                    stream.Close();
                m_streams = null;
            }
        }
        // ============End Print Receipt ==========================

        public static void PrintKitchenOrder(string sale_id)
        {
            try
            {
                ReceiptListModel receipt = new ReceiptListModel();
                receipt = ReceiptLists.Where(r => r.ReceiptName == "Kitchen Order").FirstOrDefault();
                DataTable print_data = new DataTable();
                string sql = string.Format("exec sp_get_sale_product_for_print_to_kitchen '{0}'", sale_id);
                print_data = ExecuteToDataTable(sql);

                //sale data
                DataTable sale_data = new DataTable();
                List<SaleModel> sales = new List<SaleModel>();
                sales = JsonConvert.DeserializeObject<List<SaleModel>>(print_data.Rows[0]["sale_data"].ToString());

                sale_data = CreateDataTable(sales);

                //sale_product_data

                List<SaleProductPrintQueueModel> sale_products = new List<SaleProductPrintQueueModel>();
                sale_products = JsonConvert.DeserializeObject<List<SaleProductPrintQueueModel>>(print_data.Rows[0]["sale_product_data"].ToString());


                //loop to printer for print to document
                foreach (var p in sale_products.Select(r => new { r.printer_name, r.group_item_type_id }).Distinct().ToList())
                {

                    switch (p.group_item_type_id)
                    {
                        case 1: //print all item in 1 document
                            ProcessPrintKitchenOrder(p.printer_name, receipt, sale_data, CreateDataTable(sale_products.Where(r => r.printer_name == p.printer_name)), 1);
                            break;
                        case 2: //print all item in 1 document
                            foreach (var d in sale_products.Where(r => r.printer_name == p.printer_name))
                            {
                                ProcessPrintKitchenOrder(p.printer_name, receipt, sale_data, CreateDataTable(sale_products.Where(r => r.id == d.id)), 1);
                            }

                            break;
                        case 3:
                            foreach (var d in sale_products.Where(r => r.printer_name == p.printer_name))
                            {
                                ProcessPrintKitchenOrder(p.printer_name, receipt, sale_data, CreateDataTable(sale_products.Where(r => r.id == d.id)), (Int16)d.quantity);
                            }
                            break;
                        default:
                            break;
                    }




                }
                //update print status 
                // ExecuteSql(string.Format("exec sp_update_print_to_kitchen_status '{0}'", sale_id));

                IsPrintSuccess = true;

            }
            catch (Exception ex)
            {
                IsPrintSuccess = false;
                WriteToFile(ex.Message);
            };
        }

        static void ProcessPrintKitchenOrder(string printer_name, ReceiptListModel receipt, DataTable sale_data, DataTable sale_product_data, short copies = 1)
        {

            LocalReport report = new LocalReport();

            report.ReportPath = string.Format(@"{0}\RDLC\{1}.rdlc", AppDomain.CurrentDomain.BaseDirectory, receipt.ReceiptFileName);
            report.DataSources.Add(new ReportDataSource("Sale", sale_data));
            report.DataSources.Add(new ReportDataSource("SaleProduct", sale_product_data));

            Export(report,
                 receipt.PageWidth,
                 receipt.PageHeight,
                 receipt.MarginTop,
                 receipt.MarginLeft,
                 receipt.MarginRight,
                 receipt.MarginBottom
                 );

            Print(printer_name, copies);
        }

        public static void PrintIvoice(string sale_id, ReceiptListModel receipt, string printer_name, int copies)
        {
            try
            {



                DataTable receipt_data = new DataTable();
                string sql = string.Format("exec sp_get_sale_data_for_print_bill '{0}'", sale_id);
                receipt_data = ExecuteToDataTable(sql);


                LocalReport report = new LocalReport();

                report.ReportPath = string.Format(@"{0}\RDLC\{1}.rdlc", AppDomain.CurrentDomain.BaseDirectory, receipt.InvoiceFileName);

                //sale data
                DataTable sale_data = new DataTable();
                List<SaleModel> sales = new List<SaleModel>();
                sales = JsonConvert.DeserializeObject<List<SaleModel>>(receipt_data.Rows[0]["sale_data"].ToString());
                sale_data = CreateDataTable(sales);
                //sale_product_data
                DataTable sale_product_data = new DataTable();
                List<SaleProductModel> sale_products = new List<SaleProductModel>();
                sale_products = JsonConvert.DeserializeObject<List<SaleProductModel>>(receipt_data.Rows[0]["sale_product_data"].ToString());
                sale_product_data = CreateDataTable(sale_products);


                //Grand total data
                DataTable grand_total_data = new DataTable();
                List<GrandTotalModel> grand_totals = new List<GrandTotalModel>();
                grand_totals = JsonConvert.DeserializeObject<List<GrandTotalModel>>(receipt_data.Rows[0]["grand_total_data"].ToString());
                grand_total_data = CreateDataTable(grand_totals);

                //Setting data
                DataTable setting_data = new DataTable();
                List<SettingModel> settings = new List<SettingModel>();
                settings = JsonConvert.DeserializeObject<List<SettingModel>>(receipt_data.Rows[0]["setting_data"].ToString());
                setting_data = CreateDataTable(settings);

                report.DataSources.Add(new ReportDataSource("Sale", sale_data));
                report.DataSources.Add(new ReportDataSource("SaleProduct", sale_product_data));
                report.DataSources.Add(new ReportDataSource("GrandTotal", grand_total_data));
                report.DataSources.Add(new ReportDataSource("Setting", setting_data));

                Export(report,
                     receipt.PageWidth,
                     receipt.PageHeight,
                     receipt.MarginTop,
                     receipt.MarginLeft,
                     receipt.MarginRight,
                     receipt.MarginBottom
                     );
                Print(printer_name, Convert.ToInt16(copies));

                IsPrintSuccess = true;
            }
            catch (Exception ex)
            {
                WriteToFile(ex.Message + ex.ToString());
                IsPrintSuccess = false;
            };
        }

        public static void PrintReceipt(string sale_id, ReceiptListModel receipt, string printer_name, int copies, bool is_reprint = false)
        {
            try
            {



                DataTable receipt_data = new DataTable();
                string sql = string.Format("exec sp_get_sale_data_for_print_bill '{0}'", sale_id);
                receipt_data = ExecuteToDataTable(sql);


                LocalReport report = new LocalReport();

                report.ReportPath = string.Format(@"{0}\RDLC\{1}.rdlc", AppDomain.CurrentDomain.BaseDirectory, receipt.ReceiptFileName);

                //sale data
                DataTable sale_data = new DataTable();
                List<SaleModel> sales = new List<SaleModel>();
                sales = JsonConvert.DeserializeObject<List<SaleModel>>(receipt_data.Rows[0]["sale_data"].ToString());
                sales.ForEach(r => r.is_reprint_receipt = is_reprint);
                sale_data = CreateDataTable(sales);
                //sale_product_data
                DataTable sale_product_data = new DataTable();
                List<SaleProductModel> sale_products = new List<SaleProductModel>();
                sale_products = JsonConvert.DeserializeObject<List<SaleProductModel>>(receipt_data.Rows[0]["sale_product_data"].ToString());
                sale_product_data = CreateDataTable(sale_products);


                //Grand total data
                DataTable grand_total_data = new DataTable();
                List<GrandTotalModel> grand_totals = new List<GrandTotalModel>();
                grand_totals = JsonConvert.DeserializeObject<List<GrandTotalModel>>(receipt_data.Rows[0]["grand_total_data"].ToString());
                grand_total_data = CreateDataTable(grand_totals);


                //Grand total data
                DataTable sale_payment_data = new DataTable();
                List<SalePaymentModel> sale_payments = new List<SalePaymentModel>();
                sale_payments = JsonConvert.DeserializeObject<List<SalePaymentModel>>(receipt_data.Rows[0]["sale_payment_data"].ToString());
                sale_payment_data = CreateDataTable(sale_payments);



                //Setting data
                DataTable setting_data = new DataTable();
                List<SettingModel> settings = new List<SettingModel>();
                settings = JsonConvert.DeserializeObject<List<SettingModel>>(receipt_data.Rows[0]["setting_data"].ToString());
                setting_data = CreateDataTable(settings);

                report.DataSources.Add(new ReportDataSource("Sale", sale_data));
                report.DataSources.Add(new ReportDataSource("SaleProduct", sale_product_data));
                report.DataSources.Add(new ReportDataSource("GrandTotal", grand_total_data));
                report.DataSources.Add(new ReportDataSource("Setting", setting_data));
                report.DataSources.Add(new ReportDataSource("SalePayment", sale_payment_data));

                Export(report,
                     receipt.PageWidth,
                     receipt.PageHeight,
                     receipt.MarginTop,
                     receipt.MarginLeft,
                     receipt.MarginRight,
                     receipt.MarginBottom
                     );
                Print(printer_name, Convert.ToInt16(copies));

                IsPrintSuccess = true;
            }
            catch (Exception ex)
            {
                WriteToFile(ex.Message + ex.ToString());
                IsPrintSuccess = false;
            };
        }


        public static void PrintSaleProductToLabelPrinter(int sale_id, DataTable data, string is_paid)
        {

            LocalReport report = new LocalReport();
            report.ReportPath = string.Format(@"{0}\RDLC\rptLabel.rdlc", AppDomain.CurrentDomain.BaseDirectory);
            int total_qty = 0;
            int per_qty = 1;
            foreach (DataRow d in data.Rows)
            {
                total_qty = total_qty + GetInt(d["quantity"]);
            }

            foreach (DataRow d in data.Rows)
            {
                string order_date = d["order_date"].ToString();
                string WaitingNo = d["waiting_number"].ToString();
                string table_name = d["table_name"].ToString();
                string product_name_en = d["item_line_title"].ToString();
                string product_name_kh = d["item_line_title_kh"].ToString();
                string note = d["modifier"].ToString();
                if (note == "")
                {
                    note = " ";
                }

                for (int i = 0; i < GetDecimal(d["quantity"]); i++)
                {
                    report.SetParameters(new ReportParameter[] { new ReportParameter("OrderDate", order_date) });
                    report.SetParameters(new ReportParameter[] { new ReportParameter("WaitingNo", WaitingNo == "" ? "0" : WaitingNo) });
                    report.SetParameters(new ReportParameter[] { new ReportParameter("TableName", table_name) });
                    report.SetParameters(new ReportParameter[] { new ReportParameter("ProductNameEn", product_name_en) });
                    report.SetParameters(new ReportParameter[] { new ReportParameter("ProductNameKh", product_name_kh) });
                    report.SetParameters(new ReportParameter[] { new ReportParameter("Note", note) });
                    report.SetParameters(new ReportParameter[] { new ReportParameter("IsPaid", is_paid) });
                    report.SetParameters(new ReportParameter[] { new ReportParameter("totalQuantity", total_qty.ToString()) });
                    report.SetParameters(new ReportParameter[] { new ReportParameter("perQuantity", per_qty.ToString()) });
                    ExportPrintLabel(report);
                    PrintLabel();
                    per_qty++;
                }
            }
            //Update Is Label Print
            ExecuteSql(string.Format("update tbl_sale_product_kitchen_printer_queue set is_printed_label = 1 where sale_id ={0} and coalesce(is_printed_label,0) = 0 ;", sale_id));
        }

        public static void ExportPrintLabel(LocalReport report)
        {
            string deviceInfo = "";
            deviceInfo = string.Format(
                @"<DeviceInfo>
                            <OutputFormat>emf</OutputFormat>
                            <PageWidth>{0}in</PageWidth>
                            <PageHeight>{1}in</PageHeight>
                            <MarginTop>{2}in</MarginTop>
                            <MarginLeft>{3}in</MarginLeft>
                            <MarginRight>{4}in</MarginRight>
                            <MarginBottom>{5}in</MarginBottom>
                        </DeviceInfo>",
                LabelPageSetup.PageWidth,
                LabelPageSetup.PageHeight,
                LabelPageSetup.MarginTop,
                LabelPageSetup.MarginLeft,
                LabelPageSetup.MarginRight,
                LabelPageSetup.MarginBottom);

            Warning[] warnings;
            m_streams = new List<Stream>();
            report.Render("Image", deviceInfo, CreateStream,
               out warnings);
            foreach (Stream stream in m_streams)
                stream.Position = 0;
        }

        public static void PrintLabel()
        {
            if (m_streams == null || m_streams.Count == 0)
                throw new Exception("Error: no stream to print.");
            PrintDocument printDoc = new PrintDocument();
            PrintController printControl = new StandardPrintController();

            printDoc.PrintController = printControl;
            printDoc.PrinterSettings.PrinterName = LabelPrinterName;

            if (!printDoc.PrinterSettings.IsValid)
            {
                try
                {
                    throw new Exception("Error: cannot find the default printer.");
                }
                catch
                {
                    //do nothing
                }
            }
            else
            {
                try
                {
                    printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
                    m_currentPageIndex = 0;
                    printDoc.Print();
                }
                catch
                {
                    //do nothing
                }
            }
        }



        //Write Log File
        public static void WriteToFile(string Message)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }



            string filepath = string.Format(@"{0}\Logs\{1:yyyyMMdd}_ServiceLog.txt", AppDomain.CurrentDomain.BaseDirectory, DateTime.Now);
            if (!File.Exists(filepath))
            {
                // Create a file to write to.   
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
        }
    }
}
