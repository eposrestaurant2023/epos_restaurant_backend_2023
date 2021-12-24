using Microsoft.VisualBasic;
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
using ePOSPrintingServiceReportModel;
using System.ComponentModel;
using RestSharp;
using System.Threading;

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
                    new ePOSPrintingService()
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

                FileWatcherPath = Properties.Settings.Default.FileWatcherPath;
                LabelPrinterName = Properties.Settings.Default.LabelPrinterName;
                CashierPrinter = Properties.Settings.Default.CashierPrinter;
                string str_receipt_list = Properties.Settings.Default.ReceiptSettings.Replace("\r\n", "");
                string str_telegram_setting = Properties.Settings.Default.telegram.Replace("\r\n", "");

                ReceiptLists = JsonConvert.DeserializeObject<List<ReceiptListModel>>(str_receipt_list);
                telegram_setting = JsonConvert.DeserializeObject<TelegramSettingModel>(str_telegram_setting);
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
        public static TelegramSettingModel  telegram_setting { get; set; }
        public static LabelPageSetupModel LabelPageSetup { get; set; }
        public static bool IsPrintSuccess { get; set; }


        
       
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
        public static void SendTelegramAlert(LocalReport report, string caption )
        {
            try
            {
                string deviceInfo = "";
                deviceInfo =
                @"<DeviceInfo>
                <OutputFormat>bmp</OutputFormat>
                <PageWidth>5in</PageWidth>
                <PageHeight>20in</PageHeight>
                <MarginTop>0in</MarginTop>
                <MarginLeft>0in</MarginLeft>
                <MarginRight>0in</MarginRight>
                <MarginBottom>0in</MarginBottom>
            </DeviceInfo>";

                Warning[] warnings;
                string[] streamIds;
                string mimeType = string.Empty;
                string encoding = string.Empty;
                string extension = string.Empty;

                m_streams = new List<Stream>();

                Byte[] mybytes = report.Render("Image", deviceInfo, out mimeType, out encoding, out extension, out streamIds, out warnings);
                TypeConverter tc = TypeDescriptor.GetConverter(typeof(Bitmap));


                string image_path = CropImage((Bitmap)tc.ConvertFrom(mybytes));
                if (image_path != "")
                {
                    var client = new RestClient(telegram_setting.telegram_alert_url + "bot" + telegram_setting.telegram_alert_token);
                    var request = new RestRequest("sendPhoto", Method.POST)
                    {
                        RequestFormat = DataFormat.Json,
                        AlwaysMultipartFormData = true
                    };
                    request.AddFile("photo", $"{telegram_setting.image_path}{image_path}");
                    request.AddQueryParameter("chat_id", telegram_setting.telegram_chat_id);
                    request.AddQueryParameter("caption", caption);
                    try
                    {
                        var response = client.Post(request);
                        File.Delete($"{ telegram_setting.image_path}{ image_path}");
                    }
                    catch (Exception ex)
                    {
                        string msg = ex.Message;
                    }
                }
            }
            catch(Exception ex)
            {
                WriteToFile(ex.Message);
            }
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

        public static string CropImage(Bitmap bmp)
        {
            int w = bmp.Width;
            int h = bmp.Height;

            Func<int, bool> allWhiteRow = row =>
            {
                for (int i = 0; i < w; ++i)
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

            int leftmost = 0, rightmost = 0;
            for (int col = 0; col < w; ++col)
            {
                if (allWhiteColumn(col))
                    leftmost = col;
                else
                    break;
            }

            for (int col = w - 1; col >= 0; --col)
            {
                if (allWhiteColumn(col))
                    rightmost = col;
                else
                    break;
            }

            if (rightmost == 0) rightmost = w; // As reached left
            if (bottommost == 0) bottommost = h; // As reached top.

            int croppedWidth = rightmost - leftmost;
            int croppedHeight = bottommost - topmost;

            if (croppedWidth == 0) // No border on left or right
            {
                leftmost = 0;
                croppedWidth = w;
            }

            if (croppedHeight == 0) // No border on top or bottom
            {
                topmost = 0;
                croppedHeight = h;
            }

            try
            {
                var target = new Bitmap(croppedWidth, croppedHeight);
                using (Graphics g = Graphics.FromImage(target))
                {
                    g.DrawImage(bmp,
                      new RectangleF(0, 0, croppedWidth, croppedHeight),
                      new RectangleF(leftmost, topmost, croppedWidth, croppedHeight),
                      GraphicsUnit.Pixel);
                }
                string file_name = Guid.NewGuid() + ".jpg"; 
                target.Save(telegram_setting.image_path + file_name);
                return file_name;
                 
            }
            catch (Exception ex)
            {
                throw new Exception(
                  string.Format("Values are topmost={0} btm={1} left={2} right={3} croppedWidth={4} croppedHeight={5}", topmost, bottommost, leftmost, rightmost, croppedWidth, croppedHeight),
                  ex);

            }
            return "";
        }

        // ============End Print Receipt ==========================

        public static void PrintKitchenOrder(string sale_id)
        {
            try
            {
                
             

                DynamicDataModel print_data = new DynamicDataModel();

                var data = GetApiData($"sp_get_sale_product_for_print_to_kitchen", $"'{sale_id}','json'");
                if (data.Any())
                {
                    print_data = data.FirstOrDefault();
                }
                else
                {
                    return;
                }


                ReceiptListModel receipt = new ReceiptListModel();
                receipt = ReceiptLists.Where(r => r.ReceiptName == "Kitchen Order").FirstOrDefault();
               

                //sale data
                DataTable sale_data = new DataTable();
                List<SaleModel> sales = new List<SaleModel>();
                sales = JsonConvert.DeserializeObject<List<SaleModel>>(print_data.sale_data);

                sale_data = CreateDataTable(sales);

                //sale_product_data

                List<SaleProductPrintQueueModel> sale_products = new List<SaleProductPrintQueueModel>();
                sale_products = JsonConvert.DeserializeObject<List<SaleProductPrintQueueModel>>(print_data.sale_product_data);


                //loop to printer for print to document
                foreach (var p in sale_products.Select(r => new { r.printer_name, r.group_item_type_id }).Distinct().ToList())
                {
                    sale_products.ForEach(r => r.total_quantity = r.quantity);

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
                                int copy = (int)Math.Floor(d.quantity);
                                if (copy > 0)
                                {
                                    ProcessPrintKitchenOrder(p.printer_name, receipt, sale_data, CreateDataTable(sale_products.Where(r => r.id == d.id)), (Int16)copy);
                                }

                                if(d.quantity - copy>0)
                                {
                                    sale_products.ForEach(r => { r.group_item_type_id = 1; r.quantity = d.quantity - copy; });
                                    ProcessPrintKitchenOrder(p.printer_name, receipt, sale_data, CreateDataTable(sale_products.Where(r => r.id == d.id)), 1);
                                }

                                 
                                
                            }
                            break;
                        default:
                            break;
                    }




                }
                // update print status
                ExecuteSQLSatement(string.Format("exec sp_update_print_to_kitchen_status '{0}'", sale_id));

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
                DynamicDataModel receipt_data = new DynamicDataModel();

                var data = GetApiData($"sp_get_sale_data_for_print_bill",$"'{sale_id}','json'");
                if (data.Any())
                {
                    receipt_data = data.FirstOrDefault();
               
                     

                LocalReport report = new LocalReport();

                report.ReportPath = string.Format(@"{0}\RDLC\{1}.rdlc", AppDomain.CurrentDomain.BaseDirectory, receipt.InvoiceFileName);

                //sale data
                DataTable sale_data = new DataTable();
                List<SaleModel> sales = new List<SaleModel>();
                sales = JsonConvert.DeserializeObject<List<SaleModel>>(receipt_data.sale_data);
                sale_data = CreateDataTable(sales);
                //sale_product_data
                DataTable sale_product_data = new DataTable();
                List<SaleProductModel> sale_products = new List<SaleProductModel>();
                sale_products = JsonConvert.DeserializeObject<List<SaleProductModel>>(receipt_data.sale_product_data);
                sale_product_data = CreateDataTable(sale_products);


                //Grand total data
                DataTable grand_total_data = new DataTable();
                List<GrandTotalModel> grand_totals = new List<GrandTotalModel>();
                grand_totals = JsonConvert.DeserializeObject<List<GrandTotalModel>>(receipt_data.grand_total_data);
                grand_total_data = CreateDataTable(grand_totals);

                //Setting data
                DataTable setting_data = new DataTable();
                List<SettingModel> settings = new List<SettingModel>();
                settings = JsonConvert.DeserializeObject<List<SettingModel>>(receipt_data.setting_data);
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
            
                Thread t = ThreadStart(() => SendTelegramAlert(report, "Print Sale Invoice"));


                IsPrintSuccess = true;
                }
                
            }
            catch (Exception ex)
            {
                WriteToFile(ex.Message + ex.ToString());
                IsPrintSuccess = false;
            };
        }
       

       public static  Thread ThreadStart(Action action)
        {
            Thread thread = new Thread(() => { action(); });
            thread.Start();
            return thread;
        }


        public static void PrintWaitingOrder(string sale_id, ReceiptListModel receipt, string printer_name, int copies)
        {
            try
            {


                 

                DynamicDataModel receipt_data = new DynamicDataModel();

                var data = GetApiData($"sp_get_sale_data_for_print_bill", $"'{sale_id}','json'");
                if (data.Any())
                {
                    receipt_data = data.FirstOrDefault();
                }
                else
                {
                    return;
                }




                LocalReport report = new LocalReport();

                report.ReportPath = string.Format(@"{0}\RDLC\{1}.rdlc", AppDomain.CurrentDomain.BaseDirectory, receipt.InvoiceFileName);

                //sale data
                DataTable sale_data = new DataTable();
                List<SaleModel> sales = new List<SaleModel>();
                sales = JsonConvert.DeserializeObject<List<SaleModel>>(receipt_data.sale_data);
                sale_data = CreateDataTable(sales);
                //sale_product_data
                DataTable sale_product_data = new DataTable();
                List<SaleProductModel> sale_products = new List<SaleProductModel>();
                sale_products = JsonConvert.DeserializeObject<List<SaleProductModel>>(receipt_data.sale_product_data);
                sale_product_data = CreateDataTable(sale_products);


                //Grand total data
                DataTable grand_total_data = new DataTable();
                List<GrandTotalModel> grand_totals = new List<GrandTotalModel>();
                grand_totals = JsonConvert.DeserializeObject<List<GrandTotalModel>>(receipt_data.grand_total_data);
                grand_total_data = CreateDataTable(grand_totals);

                //Setting data
                DataTable setting_data = new DataTable();
                List<SettingModel> settings = new List<SettingModel>();
                settings = JsonConvert.DeserializeObject<List<SettingModel>>(receipt_data.setting_data);
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
        public static void PrintWifiPassword()
        {
            try
            {

                ReceiptListModel receipt = ReceiptLists.FirstOrDefault();

               

                DynamicDataModel receipt_data = new DynamicDataModel();

                var data = GetApiData($"sp_get_setting_data", $"'json'");
                if (data.Any())
                {
                    receipt_data = data.FirstOrDefault();
                }
                else
                {
                    return;
                }


                LocalReport report = new LocalReport();

                report.ReportPath = string.Format(@"{0}\RDLC\rpt_wifi_password.rdlc", AppDomain.CurrentDomain.BaseDirectory);



                //Setting data
                DataTable setting_data = new DataTable();
                List<SettingModel> settings = new List<SettingModel>();
                settings = JsonConvert.DeserializeObject<List<SettingModel>>(receipt_data.setting_data);
                setting_data = CreateDataTable(settings);


                report.DataSources.Add(new ReportDataSource("Setting", setting_data));

                Export(report,
                     receipt.PageWidth,
                     receipt.PageHeight,
                     receipt.MarginTop,
                     receipt.MarginLeft,
                     receipt.MarginRight,
                     receipt.MarginBottom
                     );
                Print(CashierPrinter,1);

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




                DynamicDataModel receipt_data = new DynamicDataModel();

                var data = GetApiData($"sp_get_sale_data_for_print_bill", $"'{sale_id}','json'");
                if (data.Any())
                {
                    receipt_data = data.FirstOrDefault();
                }else
                {
                    return;
                }


                    LocalReport report = new LocalReport();

                report.ReportPath = string.Format(@"{0}\RDLC\{1}.rdlc", AppDomain.CurrentDomain.BaseDirectory, receipt.ReceiptFileName);

                //sale data
                DataTable sale_data = new DataTable();
                List<SaleModel> sales = new List<SaleModel>();
                sales = JsonConvert.DeserializeObject<List<SaleModel>>(receipt_data.sale_data);
                sales.ForEach(r => r.is_reprint_receipt = is_reprint);
                sale_data = CreateDataTable(sales);
                //sale_product_data
                DataTable sale_product_data = new DataTable();
                List<SaleProductModel> sale_products = new List<SaleProductModel>();
                sale_products = JsonConvert.DeserializeObject<List<SaleProductModel>>(receipt_data.sale_product_data);
                sale_product_data = CreateDataTable(sale_products);


                //Grand total data
                DataTable grand_total_data = new DataTable();
                List<GrandTotalModel> grand_totals = new List<GrandTotalModel>();
                grand_totals = JsonConvert.DeserializeObject<List<GrandTotalModel>>(receipt_data.grand_total_data);
                grand_total_data = CreateDataTable(grand_totals);


                //Grand total data
                DataTable sale_payment_data = new DataTable();
                List<SalePaymentModel> sale_payments = new List<SalePaymentModel>();
                sale_payments = JsonConvert.DeserializeObject<List<SalePaymentModel>>(receipt_data.sale_payment_data);
                sale_payment_data = CreateDataTable(sale_payments);



                //Setting data
                DataTable setting_data = new DataTable();
                List<SettingModel> settings = new List<SettingModel>();
                settings = JsonConvert.DeserializeObject<List<SettingModel>>(receipt_data.setting_data);
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

                 
                Thread t = ThreadStart(() => SendTelegramAlert(report, is_reprint ? "Re Print Receipt" : "Print Receipt"));

               
                IsPrintSuccess = true;
            }
            catch (Exception ex)
            {
                WriteToFile(ex.Message + ex.ToString());
                IsPrintSuccess = false;
            };
        }

        public static void PrintDeletedOrder(string sale_id, ReceiptListModel receipt, string printer_name)
        {
            try
            {



               
                DynamicDataModel receipt_data = new DynamicDataModel();

                var data = GetApiData($"sp_get_deleted_sale_data_for_print_bill", $"'{sale_id}','json'");
                if (data.Any())
                {
                    receipt_data = data.FirstOrDefault();
                }
                else
                {
                    return;
                }



                LocalReport report = new LocalReport();

                report.ReportPath = string.Format(@"{0}\RDLC\{1}.rdlc", AppDomain.CurrentDomain.BaseDirectory, receipt.InvoiceFileName);

                //sale data
                DataTable sale_data = new DataTable();
                List<SaleModel> sales = new List<SaleModel>();
                sales = JsonConvert.DeserializeObject<List<SaleModel>>(receipt_data.sale_data);
                sale_data = CreateDataTable(sales);
                //sale_product_data
                DataTable sale_product_data = new DataTable();
                List<SaleProductModel> sale_products = new List<SaleProductModel>();
                sale_products = JsonConvert.DeserializeObject<List<SaleProductModel>>(receipt_data.sale_product_data);
                sale_product_data = CreateDataTable(sale_products);


                //Grand total data
                DataTable grand_total_data = new DataTable();
                List<GrandTotalModel> grand_totals = new List<GrandTotalModel>();
                grand_totals = JsonConvert.DeserializeObject<List<GrandTotalModel>>(receipt_data.grand_total_data);
                grand_total_data = CreateDataTable(grand_totals);

                //Setting data
                DataTable setting_data = new DataTable();
                List<SettingModel> settings = new List<SettingModel>();
                settings = JsonConvert.DeserializeObject<List<SettingModel>>(receipt_data.setting_data);
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
                Print(printer_name,1);

                Thread t = new Thread(() => SendTelegramAlert(report, "Print Deleted Order"));

               
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
            ExecuteSQLSatement(string.Format("update tbl_sale_product_kitchen_printer_queue set is_printed_label = 1 where sale_id ={0} and coalesce(is_printed_label,0) = 0 ;", sale_id));
        }

        public static void PrintCloseWorkingDay(string working_day_id, ReceiptListModel receipt, string printer_name, string printed_by="")
        {
            try
            {


              


                DynamicDataModel report_data = new DynamicDataModel();

                var data = GetApiData($"sp_get_close_working_data_for_print", $"'{working_day_id}','json'");
                if (data.Any())
                {
                    report_data = data.FirstOrDefault();
                }
                else
                {
                    return;
                }




                LocalReport report = new LocalReport();

                report.ReportPath = string.Format(@"{0}\RDLC\{1}.rdlc", AppDomain.CurrentDomain.BaseDirectory, receipt.ReceiptFileName);

              

                //Working day info
                DataTable working_day_data = new DataTable();
                List<WorkingDayModel> working_day = new List<WorkingDayModel>();
                working_day = JsonConvert.DeserializeObject<List<WorkingDayModel>>(report_data.working_day_info);
                working_day_data = CreateDataTable(working_day);
                
                //Working day summaryu data
                DataTable working_day_summary_data = new DataTable();
                List<CloseWorkingDaySummaryDataModel> working_day_summary = new List<CloseWorkingDaySummaryDataModel>();
                working_day_summary = JsonConvert.DeserializeObject<List<CloseWorkingDaySummaryDataModel>>(report_data.working_day_data);
                working_day_summary_data = CreateDataTable(working_day_summary);
               
                //Close sale data data
                DataTable close_sale_data = new DataTable();
                List<SaleModel> close_sales = new List<SaleModel>();
                close_sales = JsonConvert.DeserializeObject<List<SaleModel>>(report_data.sale_data);
                close_sale_data = CreateDataTable(close_sales);



                //Setting data
                DataTable setting_data = new DataTable();
                List<SettingModel> settings = new List<SettingModel>();
                settings = JsonConvert.DeserializeObject<List<SettingModel>>(report_data.setting_data);
                settings.ForEach(r => r.printed_by = printed_by);
                setting_data = CreateDataTable(settings);
 
                report.DataSources.Add(new ReportDataSource("WorkingDay", working_day_data));
                report.DataSources.Add(new ReportDataSource("Setting", setting_data));
                report.DataSources.Add(new ReportDataSource("CloseWorkingDayData", working_day_summary_data));
                report.DataSources.Add(new ReportDataSource("CloseSaleData", close_sale_data));

                Export(report,
                     receipt.PageWidth,
                     receipt.PageHeight,
                     receipt.MarginTop,
                     receipt.MarginLeft,
                     receipt.MarginRight,
                     receipt.MarginBottom
                     );
                Print(printer_name,1);

                Thread t = new Thread(() => SendTelegramAlert(report, $"Close working day report {working_day.FirstOrDefault().working_day_number}"));
                

                IsPrintSuccess = true;
            }
            catch (Exception ex)
            {
                WriteToFile(ex.Message + ex.ToString());
                IsPrintSuccess = false;
            };
        }


        public static void PrintCloseWorkingDaySaleProduct(string working_day_id, ReceiptListModel receipt, string printer_name, string printed_by = "")
        {
            try
            {

 

                DynamicDataModel report_data = new DynamicDataModel();

                var data = GetApiData($"sp_get_close_working_sale_product_data_for_print", $"'{working_day_id}','json'");
                if (data.Any())
                {
                    report_data = data.FirstOrDefault();
                }
                else
                {
                    return;
                }



                LocalReport report = new LocalReport();

                report.ReportPath = string.Format(@"{0}\RDLC\{1}.rdlc", AppDomain.CurrentDomain.BaseDirectory, receipt.ReceiptFileName);



                //Working day info
                DataTable working_day_data = new DataTable();
                List<WorkingDayModel> working_day = new List<WorkingDayModel>();
                working_day = JsonConvert.DeserializeObject<List<WorkingDayModel>>(report_data.working_day_info);
                working_day_data = CreateDataTable(working_day);

      

                //Setting data
                DataTable setting_data = new DataTable();
                List<SettingModel> settings = new List<SettingModel>();
                settings = JsonConvert.DeserializeObject<List<SettingModel>>(report_data.setting_data);
                settings.ForEach(r => r.printed_by = printed_by);
                setting_data = CreateDataTable(settings);

                //Sale Product Data
                DataTable sale_product_data= new DataTable();
                List<SaleProductModel> sale_products = new List<SaleProductModel>();
                sale_products = JsonConvert.DeserializeObject<List<SaleProductModel>>(report_data.sale_product_data);
                sale_product_data = CreateDataTable(sale_products);

                //FOC Sale Product Data
                DataTable foc_sale_product_data= new DataTable();
                List<SaleProductModel> foc_sale_products = new List<SaleProductModel>();
                foc_sale_products = JsonConvert.DeserializeObject<List<SaleProductModel>>(report_data.foc_sale_product_data);
                foc_sale_product_data = CreateDataTable(foc_sale_products);



                report.DataSources.Add(new ReportDataSource("WorkingDay", working_day_data));
                report.DataSources.Add(new ReportDataSource("Setting", setting_data));
                report.DataSources.Add(new ReportDataSource("SaleProductData", sale_product_data));
                report.DataSources.Add(new ReportDataSource("FOCSaleProductData", foc_sale_product_data));
                 
                Export(report,
                     receipt.PageWidth,
                     receipt.PageHeight,
                     receipt.MarginTop,
                     receipt.MarginLeft,
                     receipt.MarginRight,
                     receipt.MarginBottom
                     );
                Print(printer_name, 1);
                Thread t = new Thread(() => SendTelegramAlert(report, $"Close working day - Sale Product Report  {working_day.FirstOrDefault().working_day_number}"));

                
                IsPrintSuccess = true;
            }
            catch (Exception ex)
            {
                WriteToFile(ex.Message + ex.ToString());
                IsPrintSuccess = false;
            };
        }

        public static void PrintCloseWorkingDaySaleTransaction(string working_day_id, ReceiptListModel receipt, string printer_name, string printed_by = "")
        {
            try
            {


              

                DynamicDataModel report_data = new DynamicDataModel();

                var data = GetApiData($"sp_get_close_working_data_sale_transaction_for_print", $"'{working_day_id}','json'");
                if (data.Any())
                {
                    report_data = data.FirstOrDefault();
                }
                else
                {
                    return;
                }




                LocalReport report = new LocalReport();

                report.ReportPath = string.Format(@"{0}\RDLC\{1}.rdlc", AppDomain.CurrentDomain.BaseDirectory, receipt.ReceiptFileName);



                //Working day info
                DataTable working_day_data = new DataTable();
                List<WorkingDayModel> working_day = new List<WorkingDayModel>();
                working_day = JsonConvert.DeserializeObject<List<WorkingDayModel>>(report_data.working_day_info);
                working_day_data = CreateDataTable(working_day);



                //Setting data
                DataTable setting_data = new DataTable();
                List<SettingModel> settings = new List<SettingModel>();
                settings = JsonConvert.DeserializeObject<List<SettingModel>>(report_data.setting_data);
                settings.ForEach(r => r.printed_by = printed_by);
                setting_data = CreateDataTable(settings);


                //Close sale data data
                DataTable close_sale_data = new DataTable();
                List<SaleModel> close_sales = new List<SaleModel>();
                close_sales = JsonConvert.DeserializeObject<List<SaleModel>>(report_data.sale_data);
                close_sale_data = CreateDataTable(close_sales);



                report.DataSources.Add(new ReportDataSource("WorkingDay", working_day_data));
                report.DataSources.Add(new ReportDataSource("Setting", setting_data));
                report.DataSources.Add(new ReportDataSource("CloseSaleData", close_sale_data));

                Export(report,
                     receipt.PageWidth,
                     receipt.PageHeight,
                     receipt.MarginTop,
                     receipt.MarginLeft,
                     receipt.MarginRight,
                     receipt.MarginBottom
                     );
                Print(printer_name, 1);

                
                Thread t = new Thread(() => SendTelegramAlert(report, $"Close working day - Sale Transaction Report  {working_day.FirstOrDefault().working_day_number}"));

                IsPrintSuccess = true;
            }
            catch (Exception ex)
            {
                WriteToFile(ex.Message + ex.ToString());
                IsPrintSuccess = false;
            };
        }



        public static void PrintCloseCashierShiftSummary(string cashier_shift_id, ReceiptListModel receipt, string printer_name, string printed_by = "")
        {
            try
            {


                

                DynamicDataModel report_data = new DynamicDataModel();

                var data = GetApiData($"sp_get_close_cashier_shift_data_for_print", $"'{cashier_shift_id}','json'");
                if (data.Any())
                {
                    report_data = data.FirstOrDefault();
                }
                else
                {
                    return;
                }





                LocalReport report = new LocalReport();

                report.ReportPath = string.Format(@"{0}\RDLC\{1}.rdlc", AppDomain.CurrentDomain.BaseDirectory, receipt.ReceiptFileName);



                //cashier shift info
                DataTable cashier_shift_data = new DataTable();
                List<CashierShiftModel> cashier_shift = new List<CashierShiftModel>();
                cashier_shift = JsonConvert.DeserializeObject<List<CashierShiftModel>>(report_data.cashier_shift_info);
                cashier_shift_data = CreateDataTable(cashier_shift);

                //cashier shift summaryu data
                DataTable cashier_shift_summary_data = new DataTable();
                List<CloseCashierShiftSummaryDataModel> cashier_shift_summary = new List<CloseCashierShiftSummaryDataModel>();
                cashier_shift_summary= JsonConvert.DeserializeObject<List<CloseCashierShiftSummaryDataModel>>(report_data.cashier_shift_data);
                cashier_shift_summary_data = CreateDataTable(cashier_shift_summary);

                //Close sale data data
                DataTable close_sale_data = new DataTable();
                List<SaleModel> close_sales = new List<SaleModel>();
                close_sales = JsonConvert.DeserializeObject<List<SaleModel>>(report_data.sale_data);
                close_sale_data = CreateDataTable(close_sales);



                //Setting data
                DataTable setting_data = new DataTable();
                List<SettingModel> settings = new List<SettingModel>();
                settings = JsonConvert.DeserializeObject<List<SettingModel>>(report_data.setting_data);
                settings.ForEach(r => r.printed_by = printed_by);
                setting_data = CreateDataTable(settings);

                report.DataSources.Add(new ReportDataSource("CashierShiftData",cashier_shift_data));
                report.DataSources.Add(new ReportDataSource("Setting", setting_data));
                report.DataSources.Add(new ReportDataSource("CloseCashierShiftData", cashier_shift_summary_data));
                report.DataSources.Add(new ReportDataSource("CloseSaleData", close_sale_data));

                Export(report,
                     receipt.PageWidth,
                     receipt.PageHeight,
                     receipt.MarginTop,
                     receipt.MarginLeft,
                     receipt.MarginRight,
                     receipt.MarginBottom
                     );
                Print(printer_name, 1);
                SendTelegramAlert(report, $"Close cashier shift - Sale summary report  {cashier_shift.FirstOrDefault().cashier_shift_number}");
                IsPrintSuccess = true;
            }
            catch (Exception ex)
            {
                WriteToFile(ex.Message + ex.ToString());
                IsPrintSuccess = false;
            };
        }



        public static void PrintCloseCashierShiftSaleTransaction(string cashier_shift_id, ReceiptListModel receipt, string printer_name, string printed_by = "")
        {
            try
            {


               


                DynamicDataModel report_data = new DynamicDataModel();

                var data = GetApiData($"sp_get_close_cashier_shift_sale_transaction_for_print", $"'{cashier_shift_id}','json'");
                if (data.Any())
                {
                    report_data = data.FirstOrDefault();
                }
                else
                {
                    return;
                }




                LocalReport report = new LocalReport();

                report.ReportPath = string.Format(@"{0}\RDLC\{1}.rdlc", AppDomain.CurrentDomain.BaseDirectory, receipt.ReceiptFileName);



                //cashier shift info
                DataTable cashier_shift_data = new DataTable();
                List<CashierShiftModel> cashier_shift = new List<CashierShiftModel>();
                cashier_shift = JsonConvert.DeserializeObject<List<CashierShiftModel>>(report_data.cashier_shift_info);
                cashier_shift_data = CreateDataTable(cashier_shift);

               
                //Close sale data data
                DataTable close_sale_data = new DataTable();
                List<SaleModel> close_sales = new List<SaleModel>();
                close_sales = JsonConvert.DeserializeObject<List<SaleModel>>(report_data.sale_data);
                close_sale_data = CreateDataTable(close_sales);



                //Setting data
                DataTable setting_data = new DataTable();
                List<SettingModel> settings = new List<SettingModel>();
                settings = JsonConvert.DeserializeObject<List<SettingModel>>(report_data.setting_data);
                settings.ForEach(r => r.printed_by = printed_by);
                setting_data = CreateDataTable(settings);

                report.DataSources.Add(new ReportDataSource("CashierShiftData", cashier_shift_data));
                report.DataSources.Add(new ReportDataSource("Setting", setting_data));
                report.DataSources.Add(new ReportDataSource("CloseSaleData", close_sale_data));

                Export(report,
                     receipt.PageWidth,
                     receipt.PageHeight,
                     receipt.MarginTop,
                     receipt.MarginLeft,
                     receipt.MarginRight,
                     receipt.MarginBottom
                     );
                Print(printer_name, 1);
                SendTelegramAlert(report, $"Close cashier shift - Sale transaction report  {cashier_shift.FirstOrDefault().cashier_shift_number}");
                IsPrintSuccess = true;
            }
            catch (Exception ex)
            {
                WriteToFile(ex.Message + ex.ToString());
                IsPrintSuccess = false;
            };
        }



        public static void PrintCloseCashierShiftSaleProduct(string cashier_shift_id, ReceiptListModel receipt, string printer_name, string printed_by = "")
        {
            try
            {


               

                DynamicDataModel report_data = new DynamicDataModel();

                var data = GetApiData($"sp_get_close_cashier_shift_sale_product_data_for_print", $"'{cashier_shift_id}','json'");
                if (data.Any())
                {
                    report_data = data.FirstOrDefault();
                }
                else
                {
                    return;
                }




                LocalReport report = new LocalReport();

                report.ReportPath = string.Format(@"{0}\RDLC\{1}.rdlc", AppDomain.CurrentDomain.BaseDirectory, receipt.ReceiptFileName);



                //cashier shift info
                DataTable cashier_shift_data = new DataTable();
                List<CashierShiftModel> cashier_shift = new List<CashierShiftModel>();
                cashier_shift = JsonConvert.DeserializeObject<List<CashierShiftModel>>(report_data.cashier_shift_info);
                cashier_shift_data = CreateDataTable(cashier_shift);

              

                //Setting data
                DataTable setting_data = new DataTable();
                List<SettingModel> settings = new List<SettingModel>();
                settings = JsonConvert.DeserializeObject<List<SettingModel>>(report_data.setting_data);
                settings.ForEach(r => r.printed_by = printed_by);
                setting_data = CreateDataTable(settings);

                //Sale Product Data
                DataTable sale_product_data = new DataTable();
                List<SaleProductModel> sale_products = new List<SaleProductModel>();
                sale_products = JsonConvert.DeserializeObject<List<SaleProductModel>>(report_data.sale_product_data);
                sale_product_data = CreateDataTable(sale_products);

                //FOC Sale Product Data
                DataTable foc_sale_product_data = new DataTable();
                List<SaleProductModel> foc_sale_products = new List<SaleProductModel>();
                foc_sale_products = JsonConvert.DeserializeObject<List<SaleProductModel>>(report_data.foc_sale_product_data);
                foc_sale_product_data = CreateDataTable(foc_sale_products);



                report.DataSources.Add(new ReportDataSource("CashierShiftData", cashier_shift_data));
                report.DataSources.Add(new ReportDataSource("Setting", setting_data));
         
                report.DataSources.Add(new ReportDataSource("SaleProductData", sale_product_data));
                report.DataSources.Add(new ReportDataSource("FOCSaleProductData", foc_sale_product_data));

             
                Export(report,
                     receipt.PageWidth,
                     receipt.PageHeight,
                     receipt.MarginTop,
                     receipt.MarginLeft,
                     receipt.MarginRight,
                     receipt.MarginBottom
                     );
                Print(printer_name, 1);
                SendTelegramAlert(report, $"Close cashier shift - Sale product report  {cashier_shift.FirstOrDefault().cashier_shift_number}");
                IsPrintSuccess = true;
            }
            catch (Exception ex)
            {
                WriteToFile(ex.Message + ex.ToString());
                IsPrintSuccess = false;
            };
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


      public  static List<DynamicDataModel> GetApiData(string procedure_name, string parameters)
        {

            try
            {
                var client = new RestClient(Properties.Settings.Default.api_url);

                var request = new RestRequest("Printing/GetPrintData");
                request.AddParameter("procedure_name", procedure_name);
                request.AddParameter("parameters", parameters);
                var response = client.Get(request);
                if (response.Content.ToString() != "")
                {
                    var result =System.Text.Json.JsonSerializer.Deserialize<List<DynamicDataModel>>( response.Content);
                  
                
                  
                    return result;
                }
                    
                return new List<DynamicDataModel>();



            }
            catch (Exception ex)
            {
                return new List<DynamicDataModel>();

            }
        }  public  static void ExecuteSQLSatement(string sql)
        {

            try
            {
                var client = new RestClient(Properties.Settings.Default.api_url);

                var request = new RestRequest("Printing/ExecuteSQLSatement");
                request.AddParameter("sql", sql);
                 
                var response = client.Get(request);
                
            }
            catch (Exception ex)
            {
                WriteToFile(ex.ToString());

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
