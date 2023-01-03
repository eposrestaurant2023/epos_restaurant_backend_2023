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
using ePOSPrintingService.Models;
using System.Reflection;
using ePOSPrintingServiceReportModel;
using System.ComponentModel;
using RestSharp;
using System.Threading;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Text.Json;
using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using File = System.IO.File;               

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

        public static  bool LoadSetting()
        {
            bool isLoaded = false;
            try
            {
                FileWatcherPath = Properties.Settings.Default.FileWatcherPath;
                LabelPrinterName = Properties.Settings.Default.LabelPrinterName;
                CashierPrinter = Properties.Settings.Default.CashierPrinter;
                string str_receipt_list = Properties.Settings.Default.ReceiptSettings.Replace("\r\n", "");
                ReceiptLists = JsonSerializer.Deserialize<List<ReceiptListModel>>(str_receipt_list);
                translate_caches = new List<TranslateCacheModel>();

                TelegramFileWatcherPath = $"{FileWatcherPath}\\telegram\\";
                if(!Directory.Exists(TelegramFileWatcherPath))
                {
                    Directory.CreateDirectory(TelegramFileWatcherPath);
                }
                Task.Factory.StartNew(async () =>
                {     
                   await GetTranslateText("en");
                   await GetTranslateText("kh");
                });

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
        
        public static string FileWatcherPath { get; set; }
        public static string TelegramFileWatcherPath { get; set; }
        public static string LabelPrinterName { get; set; }
        public static string CashierPrinter { get; set; }
        public static List<ReceiptListModel> ReceiptLists { get; set; }
      
        public static bool IsPrintSuccess { get; set; }


        public static List<TranslateCacheModel> translate_caches { get; set; }


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
            try
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
            } catch (Exception ex)
            {
                WriteToFile(ex.ToString());
            }
        }
        public static void SendTelegramAlert(LocalReport report, string actionName, TelegramMessageParamModel param)
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
                string file_name = CropImage((Bitmap)tc.ConvertFrom(mybytes));
                if (file_name != "")
                {
                    try
                    {
                       SendTelegramImage($"{TelegramFileWatcherPath}{file_name}", actionName,param);
                    }
                    catch (Exception ex)
                    {
                        string msg = ex.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                WriteToFile(ex.Message);
            }
        }
        public static byte[] getByteImage(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        private static async Task SendTelegramImage(string imagePath, string actionName, TelegramMessageParamModel param)
        {  
            try
            {
                ConfigDataModel configData = new ConfigDataModel();
                var resp = await GetApi("configdata?config_type=telegram");
                if (resp.IsSuccess)
                {
                    configData = JsonSerializer.Deserialize<ConfigDataModel>(resp.Content.ToString());
                }
                if (configData.data != null && configData.data!="")
                {
                    //get business branch 
                    List<BusinessBranchModel> businessBranches = new List<BusinessBranchModel>();
                    string business_branch_name = "";
                    var respBusi = await GetApi("configdata?config_type=business_branch");
                    if (respBusi.IsSuccess)
                    {

                        var   confBus = JsonSerializer.Deserialize<ConfigDataModel>(respBusi.Content.ToString());
                        if(confBus.data != null && confBus.data != "")
                        {
                            businessBranches = JsonSerializer.Deserialize<List<BusinessBranchModel>>(confBus.data);
                            if (businessBranches.Any())
                            {
                                business_branch_name = businessBranches.FirstOrDefault().business_branch_name_en;
                            }
                        }
                      
                    }


                    List<TelegramAlertModel> telegrams = new List<TelegramAlertModel>();
                    telegrams = JsonSerializer.Deserialize<List<TelegramAlertModel>> (configData.data);

                    foreach (var t in telegrams)
                    {
                        var actions = t.actions.Where(r => r.allow_send && r.name == actionName);
                        if (actions.Any())
                        { 
                            var action = actions.FirstOrDefault();

                            string _msg = string.Format(action.msg,
                                                        param.document_number,
                                                        business_branch_name,
                                                        param.outlet_name,
                                                        param.station_name,
                                                        param.shift_name,
                                                        param.printed_by,
                                                        param.printed_date.ToString("dd-MM-yyyy")
                                                        );

                            TelegramBotClient Bot = new TelegramBotClient(t.token);
                            using (var file = new FileStream(imagePath, FileMode.Open))
                            {
                                Stream s = new MemoryStream(getByteImage(file));
                                await Bot.SendChatActionAsync(t.chat_id, ChatAction.UploadPhoto);
                                await Bot.SendPhotoAsync(
                                    chatId: t.chat_id,
                                    photo: new InputOnlineFile(s),
                                    caption: $"{action.title}\n\n{_msg}"
                                ); 
                                s.Close();
                                file.Dispose();
                               
                            }
                        }
                    }

                    File.Delete($"{imagePath}");
                }
            }
            catch (Exception _ex)
            {
                WriteToFile(_ex.Message + "\n" + _ex.ToString());
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
        static void Print58MM(LocalReport report)
        {
            string deviceInfo = "";
            deviceInfo = string.Format(
            @"<DeviceInfo>
                <OutputFormat>emf</OutputFormat>
                <PageWidth>{0}in</PageWidth>
                <PageHeight>{1}in</PageHeight>
                <MarginTop>0in</MarginTop>
                <MarginLeft>0in</MarginLeft>
                <MarginRight>0in</MarginRight>
                <MarginBottom>0in</MarginBottom>
            </DeviceInfo>", 2.85, 50);

            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;

            var byts = report.Render("Image", deviceInfo, out mimeType, out encoding, out extension, out streamIds, out warnings);
            var img = byteArrayToImage(byts);

            Bitmap bmp = new Bitmap(img.Width, img.Height, PixelFormat.Format32bppArgb);      
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);
                g.DrawImage(img, new Rectangle(new Point(), img.Size), new Rectangle(new Point(), img.Size), GraphicsUnit.Pixel);
            }
            CropTopBottomImage(bmp, bmp.Width);

            //bmp.Save(file, ImageFormat.Png);
        }
        public static Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }
        public static string  CropTopBottomImage(Bitmap bmp, int image_width)
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
                string file_name = Guid.NewGuid() + ".jpg";
                target.Save(TelegramFileWatcherPath + file_name);
                return file_name;

            }
            catch (Exception ex)
            {
                throw new Exception(
                  string.Format("Values are topmost={0} btm={1} left={2} right={3} croppedWidth={4} croppedHeight={5}", topmost, bottommost, 0, 0, image_width, croppedHeight),
                  ex);

            }
        }


        public static void Print(LocalReport report, ReceiptListModel receipt, string printerName, short copies=1)
        {

            for (int i = 0; i < copies; i++)
            {
                try
                {
                    Print58MM(report);
                }
                catch(Exception ex)
                {
                    var xx = ex;
                    //
                }
                Export(report, receipt.PageWidth, 0, receipt.MarginTop, receipt.MarginLeft, receipt.MarginRight, receipt.MarginBottom);

                if (m_streams == null || m_streams.Count == 0)
                    throw new Exception("Error: no stream to print.");
                PrintDocument printDoc = new PrintDocument();
                //printDoc.PrinterSettings.Copies = copy;
                PrintController printControl = new StandardPrintController();
                printDoc.PrintController = printControl;
                printDoc.PrinterSettings.PrinterName = printerName;

                var paper_sizes = printDoc.PrinterSettings.PaperSizes ;
                int paper_size_index = -1;
                int x=0;
                for (x = 0; x < paper_sizes.Count; x++)
                {
                    if (paper_sizes[x].ToString().ToLower().Contains("epos receipt"))
                    {
                        paper_size_index = x;
                        break;
                                              
                    }
                }

                if (paper_size_index >= 0)
                {
                    printDoc.DefaultPageSettings.PaperSize = printDoc.PrinterSettings.PaperSizes[paper_size_index];
                }

                
                if (!printDoc.PrinterSettings.IsValid)
                {
                    try { throw new Exception("Error: cannot find the default printer."); }
                    catch (Exception ex)
                    {
                        WriteToFile($"Print {printerName} \n=> {ex.ToString()}");
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
                    catch (Exception ex)
                    {
                        WriteToFile($"Print {printerName} \n=> {ex.ToString()}");
                    }
                }
                printDoc.Dispose();
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
                target.Save(TelegramFileWatcherPath + file_name);   
                return file_name;

            }
            catch (Exception ex)
            {
                throw new Exception(
                  string.Format("Values are topmost={0} btm={1} left={2} right={3} croppedWidth={4} croppedHeight={5}", topmost, bottommost, leftmost, rightmost, croppedWidth, croppedHeight),
                  ex);

            }
        }

        // ============End Print Receipt ==========================

        public static void PrintKitchenMessage(string _data)
        {
            try
            {
                //sale data
                DataTable data = new DataTable();
                List<KitchenMessageModel> values = new List<KitchenMessageModel>();
                var _value = JsonSerializer.Deserialize<KitchenMessageModel>(_data);
                values.Add(_value);
                data = CreateDataTable(values);

                ReceiptListModel receipt = new ReceiptListModel();
                receipt = ReceiptLists.Where(r => r.ReceiptName.ToLower() == "Kitchen Message".ToLower()).FirstOrDefault();      
                
                foreach (var p in _value.printer_names.Split(','))
                {    
                    LocalReport report = new LocalReport();
                    report.ReportPath = string.Format(@"{0}\RDLC\{1}.rdlc", AppDomain.CurrentDomain.BaseDirectory, receipt.ReceiptFileName);
                    report.DataSources.Add(new ReportDataSource("Data", data));
                    Print(report,receipt, p);
                } 
                IsPrintSuccess = true;
            }
            catch (Exception ex)
            {
                //
                IsPrintSuccess = false;
                WriteToFile(ex.Message);
            }
        }

        public static async Task PrintKitchenOrder(string sale_id)
        {
            try
            {
                DynamicDataModel print_data = new DynamicDataModel();
                var data = await GetApiData($"sp_get_sale_product_for_print_to_kitchen", $"'{sale_id}','json'");
                if (data.Any())
                {
                    print_data = data.FirstOrDefault();
                }
                else
                {
                    return;
                }

                ReceiptListModel receipt = new ReceiptListModel();
                receipt = ReceiptLists.Where(r => r.ReceiptName.ToLower() == "Kitchen Order".ToLower()).FirstOrDefault();
               

                //sale data
                DataTable sale_data = new DataTable();
                List<SaleModel> sales = new List<SaleModel>();
                sales = JsonSerializer.Deserialize<List<SaleModel>>(print_data.sale_data);

                sale_data = CreateDataTable(sales);       
                //sale_product_data     
                List<SaleProductPrintQueueModel> sale_products = new List<SaleProductPrintQueueModel>();
                sale_products = JsonSerializer.Deserialize<List<SaleProductPrintQueueModel>>(print_data.sale_product_data);


                //loop to printer for print to document
                foreach (var p in sale_products.Select(r => new { r.printer_name, r.group_item_type_id }).Distinct().ToList())
                {
                    sale_products.ForEach(r => r.total_quantity = r.quantity);
                    switch (p.group_item_type_id)
                    {
                        case 1: //print all item in 1 document                                                                       
                            if (p.printer_name != Properties.Settings.Default.LabelPrinterName)
                            {
                                ProcessPrintKitchenOrder(p.printer_name, receipt, sale_data, CreateDataTable(sale_products.Where(r => r.printer_name == p.printer_name)), receipt.number_invoice_copies);                                 
                            }
                            else
                            {
                                //Print Label
                                ProcessPrintLabel(sale_data, sale_products.Where(r => r.printer_name == p.printer_name).ToList());     
                            }
                            break;
                        case 2: //print all item in 1 document
                            if (p.printer_name != Properties.Settings.Default.LabelPrinterName)
                            {
                                foreach (var d in sale_products.Where(r => r.printer_name == p.printer_name))
                                {
                                    ProcessPrintKitchenOrder(p.printer_name, receipt, sale_data, CreateDataTable(sale_products.Where(r => r.id == d.id)), 1);
                                }
                            }
                            else
                            {
                                //Print Label
                                ProcessPrintLabel(sale_data, sale_products.Where(r => r.printer_name == p.printer_name).ToList());
                            }
                            break;
                        case 3:
                            if (p.printer_name != Properties.Settings.Default.LabelPrinterName)
                            {
                                foreach (var d in sale_products.Where(r => r.printer_name == p.printer_name))
                                {
                                    int copy = (int)Math.Floor(d.quantity);
                                    if (copy > 0)
                                    {
                                        ProcessPrintKitchenOrder(p.printer_name, receipt, sale_data, CreateDataTable(sale_products.Where(r => r.id == d.id)), (Int16)copy);
                                    }

                                    if (d.quantity - copy > 0)
                                    {
                                        sale_products.ForEach(r => { r.group_item_type_id = 1; r.quantity = d.quantity - copy; });
                                        ProcessPrintKitchenOrder(p.printer_name, receipt, sale_data, CreateDataTable(sale_products.Where(r => r.id == d.id)), 1);
                                    }
                                }
                            }
                            else
                            {
                                //Print Label
                                ProcessPrintLabel(sale_data, sale_products.Where(r => r.printer_name == p.printer_name).ToList());
                            }
                            break;
                        default:
                            break;
                    }  
                }

                // update print status
                await  ExecuteSQLSatement(string.Format("exec sp_update_print_to_kitchen_status '{0}'", sale_id));  
                IsPrintSuccess = true;

            }
            catch (Exception ex)
            {
                IsPrintSuccess = false;
                WriteToFile(ex.Message);
            };
        }

        static void ProcessPrintLabel(DataTable sale_data, List<SaleProductPrintQueueModel> sale_product_print_queues)
        {
            foreach (var d in sale_product_print_queues)
            {
                var _sppq = sale_product_print_queues.Where(r => r.id == d.id).ToList();
                double _qty = d.quantity;
                for (double i = _qty; i > 0; i--)
                {
                    if (i < 1)
                    {
                        _sppq.ForEach(r => r.quantity = i);
                    }
                    else
                    {
                        _sppq.ForEach(r => r.quantity =  Convert.ToDouble((int)Math.Floor(i)));
                    }
                    PrintLabel(sale_data, CreateDataTable(_sppq));
                }
            }
        }

        static void ProcessPrintKitchenOrder(string printer_name, ReceiptListModel receipt, DataTable sale_data, DataTable sale_product_data, short copies = 1)
        {
            LocalReport report = new LocalReport();
            report.ReportPath = string.Format(@"{0}\RDLC\{1}.rdlc", AppDomain.CurrentDomain.BaseDirectory, receipt.ReceiptFileName);
            report.DataSources.Add(new ReportDataSource("Sale", sale_data));
            report.DataSources.Add(new ReportDataSource("SaleProduct", sale_product_data));

            Print(report, receipt,printer_name, copies);            
        }

        static void PrintLabel(DataTable sale_data, DataTable sale_product_data)
        {
            LocalReport report = new LocalReport();
            ReceiptListModel receipt = new ReceiptListModel();
            receipt = ReceiptLists.Where(r => r.ReceiptName.ToLower() == "Label".ToLower()).FirstOrDefault();

            report.ReportPath = string.Format(@"{0}\RDLC\{1}.rdlc", AppDomain.CurrentDomain.BaseDirectory, receipt.ReceiptFileName);
            report.DataSources.Add(new ReportDataSource("Sale", sale_data));
            report.DataSources.Add(new ReportDataSource("SaleProduct", sale_product_data));

            Print(report, receipt, Properties.Settings.Default.LabelPrinterName, 1);
        }

        public static async Task PrintInvoice(string sale_id, ReceiptListModel receipt, string printer_name, int copies)
        {
            try
            {
                DynamicDataModel receipt_data = new DynamicDataModel();
                var data = await GetApiData($"sp_get_sale_data_for_print_bill", $"'{sale_id}','json'");
                if (data.Any())
                {
                    receipt_data = data.FirstOrDefault();
                    LocalReport report = new LocalReport();

                    report.ReportPath = string.Format(@"{0}\RDLC\{1}.rdlc", AppDomain.CurrentDomain.BaseDirectory, receipt.InvoiceFileName);

                    //sale data
                    DataTable sale_data = new DataTable();
                    List<SaleModel> sales = new List<SaleModel>();
                    sales = JsonSerializer.Deserialize<List<SaleModel>>(receipt_data.sale_data);
                    sale_data = CreateDataTable(sales);
                    //sale_product_data
                    DataTable sale_product_data = new DataTable();
                    List<SaleProductModel> sale_products = new List<SaleProductModel>();
                    sale_products = JsonSerializer.Deserialize<List<SaleProductModel>>(receipt_data.sale_product_data);
                    sale_product_data = CreateDataTable(sale_products);


                    //Grand total data
                    DataTable grand_total_data = new DataTable();
                    List<GrandTotalModel> grand_totals = new List<GrandTotalModel>();
                    grand_totals = JsonSerializer.Deserialize<List<GrandTotalModel>>(receipt_data.grand_total_data);
                    grand_total_data = CreateDataTable(grand_totals);

                    //Setting data
                    DataTable setting_data = new DataTable();
                    List<SettingModel> settings = new List<SettingModel>();
                    settings = JsonSerializer.Deserialize<List<SettingModel>>(receipt_data.setting_data);
                    setting_data = CreateDataTable(settings);

                    //coupon voucher data
                    DataTable coupon_voucher_data = new DataTable();
                    List<CouponVoucherModel> coupon_voucher_list = new List<CouponVoucherModel>();
                    if (receipt_data.coupon_voucher_data != null)
                    {
                        coupon_voucher_list = JsonSerializer.Deserialize<List<CouponVoucherModel>>(receipt_data.coupon_voucher_data);
                    }
                    coupon_voucher_data = CreateDataTable(coupon_voucher_list);


                    report.DataSources.Add(new ReportDataSource("Sale", sale_data));
                    report.DataSources.Add(new ReportDataSource("SaleProduct", sale_product_data));
                    report.DataSources.Add(new ReportDataSource("GrandTotal", grand_total_data));
                    report.DataSources.Add(new ReportDataSource("Setting", setting_data));
                    report.DataSources.Add(new ReportDataSource("CouponVoucher", coupon_voucher_data));



                    Print(report, receipt, printer_name, Convert.ToInt16(copies));

                    if(   sale_data.Rows .Count >0)
                    {
                        DataRow row = sale_data.Rows[0];
                        TelegramMessageParamModel param = new TelegramMessageParamModel()
                        {
                            document_number = row["sale_number"].ToString(),
                            outlet_name = row["outlet_name_en"].ToString(),
                            station_name = row["station_name_en"].ToString(),
                            shift_name = row["shift_name"].ToString(),
                            printed_by = row["last_modified_by"].ToString(),
                            printed_date = Convert.ToDateTime(row["last_modified_date"].ToString()),

                        }; 
                        Thread t = ThreadStart(() => SendTelegramAlert(report, "PrintInvoiceResquestService", param));
                    }

                    IsPrintSuccess = true;
                }    
            }
            catch (Exception ex)
            {
                WriteToFile(ex.Message + ex.ToString());
                IsPrintSuccess = false;
            };
        }
       

      

        public static async Task PrintWaitingOrder(string sale_id, ReceiptListModel receipt, string printer_name, int copies)
        {
            try
            { 
                DynamicDataModel receipt_data = new DynamicDataModel();     
                var data = await GetApiData($"sp_get_sale_data_for_print_bill", $"'{sale_id}','json'");
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
                sales = JsonSerializer.Deserialize<List<SaleModel>>(receipt_data.sale_data);
                sale_data = CreateDataTable(sales);
                //sale_product_data
                DataTable sale_product_data = new DataTable();
                List<SaleProductModel> sale_products = new List<SaleProductModel>();
                sale_products = JsonSerializer.Deserialize<List<SaleProductModel>>(receipt_data.sale_product_data);
                sale_product_data = CreateDataTable(sale_products);


                //Grand total data
                DataTable grand_total_data = new DataTable();
                List<GrandTotalModel> grand_totals = new List<GrandTotalModel>();
                grand_totals = JsonSerializer.Deserialize<List<GrandTotalModel>>(receipt_data.grand_total_data);
                grand_total_data = CreateDataTable(grand_totals);

                //Setting data
                DataTable setting_data = new DataTable();
                List<SettingModel> settings = new List<SettingModel>();
                settings = JsonSerializer.Deserialize<List<SettingModel>>(receipt_data.setting_data);
                setting_data = CreateDataTable(settings);

                report.DataSources.Add(new ReportDataSource("Sale", sale_data));
                report.DataSources.Add(new ReportDataSource("SaleProduct", sale_product_data));
                report.DataSources.Add(new ReportDataSource("GrandTotal", grand_total_data));
                report.DataSources.Add(new ReportDataSource("Setting", setting_data));


                Print(report, receipt, printer_name, Convert.ToInt16(copies));

                IsPrintSuccess = true;
            }
            catch (Exception ex)
            {
                WriteToFile(ex.Message + ex.ToString());
                IsPrintSuccess = false;
            };
        }

       public static async Task PrintWifiPassword(string printer_name)
        {
            try
            {    
                ReceiptListModel receipt = ReceiptLists.FirstOrDefault(); 
                DynamicDataModel receipt_data = new DynamicDataModel(); 
                var data = await GetApiData($"sp_get_setting_data", $"'json'");
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
                settings = JsonSerializer.Deserialize<List<SettingModel>>(receipt_data.setting_data);
                setting_data = CreateDataTable(settings);
                report.DataSources.Add(new ReportDataSource("Setting", setting_data));

                Print(report, receipt,printer_name, 1);
                IsPrintSuccess = true;
            }
            catch (Exception ex)
            {
                WriteToFile(ex.Message + ex.ToString());
                IsPrintSuccess = false;
            };
        }

        public static async Task PrintReceipt(string sale_id, ReceiptListModel receipt, string printer_name, int copies, bool is_reprint = false)
        {
            try
            { 
                DynamicDataModel receipt_data = new DynamicDataModel(); 
                var data = await GetApiData($"sp_get_sale_data_for_print_bill", $"'{sale_id}','json'");
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
                sales = JsonSerializer.Deserialize<List<SaleModel>>(receipt_data.sale_data);
                sales.ForEach(r => r.is_reprint_receipt = is_reprint);
                sale_data = CreateDataTable(sales);
                //sale_product_data
                DataTable sale_product_data = new DataTable();
                List<SaleProductModel> sale_products = new List<SaleProductModel>();
                sale_products = JsonSerializer.Deserialize<List<SaleProductModel>>(receipt_data.sale_product_data);
                sale_product_data = CreateDataTable(sale_products);  

                //Grand total data
                DataTable grand_total_data = new DataTable();
                List<GrandTotalModel> grand_totals = new List<GrandTotalModel>();
                grand_totals = JsonSerializer.Deserialize<List<GrandTotalModel>>(receipt_data.grand_total_data);
                grand_total_data = CreateDataTable(grand_totals);


                //sale payment data
                DataTable sale_payment_data = new DataTable();
                List<SalePaymentModel> sale_payments = new List<SalePaymentModel>();
                sale_payments = JsonSerializer.Deserialize<List<SalePaymentModel>>(receipt_data.sale_payment_data);
                sale_payment_data = CreateDataTable(sale_payments);

                //sale payment change data
                DataTable sale_payment_change_data = new DataTable();
                List<SalePaymentChangeModel> sale_payment_change_list= new List<SalePaymentChangeModel>();
                if (receipt_data.sale_payment_change_data != null)
                {
                    sale_payment_change_list = JsonSerializer.Deserialize<List<SalePaymentChangeModel>>(receipt_data.sale_payment_change_data);
                }                 
                sale_payment_change_data = CreateDataTable(sale_payment_change_list);   

                //Setting data
                DataTable setting_data = new DataTable();
                List<SettingModel> settings = new List<SettingModel>();
                settings = JsonSerializer.Deserialize<List<SettingModel>>(receipt_data.setting_data);
                setting_data = CreateDataTable(settings);


                //coupon voucher data
                DataTable coupon_voucher_data = new DataTable();
                List<CouponVoucherModel> coupon_voucher_list = new List<CouponVoucherModel>();
                if (receipt_data.coupon_voucher_data != null)
                {
                    coupon_voucher_list = JsonSerializer.Deserialize<List<CouponVoucherModel>>(receipt_data.coupon_voucher_data);
                }
                coupon_voucher_data = CreateDataTable(coupon_voucher_list);



                report.DataSources.Add(new ReportDataSource("Sale", sale_data));
                report.DataSources.Add(new ReportDataSource("SaleProduct", sale_product_data));
                report.DataSources.Add(new ReportDataSource("GrandTotal", grand_total_data));
                report.DataSources.Add(new ReportDataSource("Setting", setting_data));
                report.DataSources.Add(new ReportDataSource("SalePayment", sale_payment_data));
                report.DataSources.Add(new ReportDataSource("SalePaymentChange", sale_payment_change_data));
                report.DataSources.Add(new ReportDataSource("CouponVoucher", coupon_voucher_data));


                Print(report, receipt, printer_name, Convert.ToInt16(copies));


                if (sale_data.Rows.Count > 0)
                {
                    DataRow row = sale_data.Rows[0];
                    TelegramMessageParamModel param = new TelegramMessageParamModel()
                    {
                        document_number = row["document_number"].ToString(),
                        outlet_name = row["outlet_name_en"].ToString(),
                        station_name = row["station_name_en"].ToString(),
                        shift_name = row["shift_name"].ToString(),
                        printed_by = row["last_modified_by"].ToString(),
                        printed_date = Convert.ToDateTime(row["last_modified_date"].ToString()),
                    };

                    Thread t = ThreadStart(() => SendTelegramAlert(report, is_reprint ? "RePrintReceiptService" : "PrintReceiptService", param));
                }
                IsPrintSuccess = true;
            }
            catch (Exception ex)
            {
                WriteToFile(ex.Message + ex.ToString());
                IsPrintSuccess = false;
            };
        }

        public static async Task PrintCouponVoucherReceipt(string id, ReceiptListModel receipt, string printer_name, int copies)
        {
            try
            {
                DynamicDataModel receipt_data = new DynamicDataModel();
                var data = await GetApiData($"sp_get_coupon_voucher_transaction_for_print", $"'{id}'");
                if (data.Any())
                {
                    receipt_data = data.FirstOrDefault();
                }
                else
                {
                    return;
                }
                LocalReport report = new LocalReport();
                report.ReportPath = string.Format(@"{0}\RDLC\{1}.rdlc", AppDomain.CurrentDomain.BaseDirectory, receipt.ReceiptFileName);

                //Setting data
                DataTable setting_data = new DataTable();
                List<SettingModel> settings = new List<SettingModel>();
                settings = JsonSerializer.Deserialize<List<SettingModel>>(receipt_data.setting_data);
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

                Print(report, receipt, printer_name, Convert.ToInt16(copies));

                IsPrintSuccess = true;
            }
            catch (Exception ex)
            {
                WriteToFile(ex.Message + ex.ToString());
                IsPrintSuccess = false;
            };
        }

        public static async Task PrintParkItemReceipt(string sale_id, ReceiptListModel receipt, string printer_name, bool is_reprint = false)
        {
            try
            {
                DynamicDataModel receipt_data = new DynamicDataModel();
                var data = await GetApiData($"sp_get_sale_data_for_print_bill", $"'{sale_id}','json'");
                if (data.Any())
                {
                    receipt_data = data.FirstOrDefault();
                }
                else
                {
                    return;
                }
                LocalReport report = new LocalReport();
                report.ReportPath = string.Format(@"{0}\RDLC\{1}.rdlc", AppDomain.CurrentDomain.BaseDirectory, receipt.ReceiptFileName);

                //sale data
                DataTable sale_data = new DataTable();
                List<SaleModel> sales = new List<SaleModel>();
                sales = JsonSerializer.Deserialize<List<SaleModel>>(receipt_data.sale_data);
                sales.ForEach(r => r.is_reprint_receipt = is_reprint);
                sale_data = CreateDataTable(sales);
                //sale_product_data
                DataTable sale_product_data = new DataTable();
                List<SaleProductModel> sale_products = new List<SaleProductModel>();
                sale_products = JsonSerializer.Deserialize<List<SaleProductModel>>(receipt_data.sale_product_data);
                sale_product_data = CreateDataTable(sale_products.Where(r=>r.is_park==true));

                //Setting data
                DataTable setting_data = new DataTable();
                List<SettingModel> settings = new List<SettingModel>();
                settings = JsonSerializer.Deserialize<List<SettingModel>>(receipt_data.setting_data);
                setting_data = CreateDataTable(settings);

                report.DataSources.Add(new ReportDataSource("Sale", sale_data));
                report.DataSources.Add(new ReportDataSource("SaleProduct", sale_product_data));
                report.DataSources.Add(new ReportDataSource("Setting", setting_data));
 
                Print(report, receipt, printer_name, Convert.ToInt16(receipt.number_receipt_copies));


                if (sale_data.Rows.Count > 0)
                {
                    DataRow row = sale_data.Rows[0];
                    TelegramMessageParamModel param = new TelegramMessageParamModel()
                    {
                        document_number = row["document_number"].ToString(),
                        outlet_name = row["outlet_name_en"].ToString(),
                        station_name = row["station_name_en"].ToString(),
                        shift_name = row["shift_name"].ToString(),
                        printed_by = row["last_modified_by"].ToString(),
                        printed_date = Convert.ToDateTime(row["last_modified_date"].ToString()),
                    };

                    Thread t = ThreadStart(() => SendTelegramAlert(report, is_reprint ? "RePrintParkItemReceiptService" : "PrintParkItemReceipt",param));
                }
                IsPrintSuccess = true;
            }
            catch (Exception ex)
            {
                WriteToFile(ex.Message + ex.ToString());
                IsPrintSuccess = false;
            };
        }


        public static async Task PrintDeletedOrder(string sale_id, ReceiptListModel receipt, string printer_name)
        {
            try
            {
                DynamicDataModel receipt_data = new DynamicDataModel();
                var data = await GetApiData($"sp_get_deleted_sale_data_for_print_bill", $"'{sale_id}','json'");
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
                sales = JsonSerializer.Deserialize<List<SaleModel>>(receipt_data.sale_data);
                sale_data = CreateDataTable(sales);
                //sale_product_data
                DataTable sale_product_data = new DataTable();
                List<SaleProductModel> sale_products = new List<SaleProductModel>();
                sale_products = JsonSerializer.Deserialize<List<SaleProductModel>>(receipt_data.sale_product_data);
                sale_product_data = CreateDataTable(sale_products);


                //Grand total data
                DataTable grand_total_data = new DataTable();
                List<GrandTotalModel> grand_totals = new List<GrandTotalModel>();
                grand_totals = JsonSerializer.Deserialize<List<GrandTotalModel>>(receipt_data.grand_total_data);
                grand_total_data = CreateDataTable(grand_totals);

                //Setting data
                DataTable setting_data = new DataTable();
                List<SettingModel> settings = new List<SettingModel>();
                settings = JsonSerializer.Deserialize<List<SettingModel>>(receipt_data.setting_data);
                setting_data = CreateDataTable(settings);

                report.DataSources.Add(new ReportDataSource("Sale", sale_data));
                report.DataSources.Add(new ReportDataSource("SaleProduct", sale_product_data));
                report.DataSources.Add(new ReportDataSource("GrandTotal", grand_total_data));
                report.DataSources.Add(new ReportDataSource("Setting", setting_data));

     
                Print(report, receipt,printer_name, 1);
                if (sale_data.Rows.Count > 0)
                {
                    DataRow row = sale_data.Rows[0];
                    TelegramMessageParamModel param = new TelegramMessageParamModel()
                    {
                        document_number = row["document_number"].ToString(),
                        outlet_name = row["outlet_name_en"].ToString(),
                        station_name = row["station_name_en"].ToString(),
                        shift_name = row["shift_name"].ToString(),
                        printed_by = row["last_modified_by"].ToString(),
                        printed_date = Convert.ToDateTime(row["last_modified_date"].ToString()),
                    };
                    Thread t = ThreadStart(() => SendTelegramAlert(report, "PrintDeletedOrderService",param));
                }
                IsPrintSuccess = true;
            }
            catch (Exception ex)
            {
                WriteToFile(ex.Message + ex.ToString());
                IsPrintSuccess = false;
            };
        }



        public static async Task PrintCloseWorkingDay(string working_day_id, ReceiptListModel receipt, string printer_name, string printed_by="",string language="en")
        {
            try
            {
                DynamicDataModel report_data = new DynamicDataModel();
                var data = await GetApiData($"sp_get_close_working_data_for_print", $"'{working_day_id}','json','{language}'");
                if (data.Any())
                {
                    report_data = data.FirstOrDefault();
                }
                else
                {
                    return;
                }
                var _translate_text = await GetTranslateText(language);
                LocalReport report = new LocalReport();     
                report.ReportPath = string.Format(@"{0}\RDLC\{1}.rdlc", AppDomain.CurrentDomain.BaseDirectory, receipt.ReceiptFileName);

                //Working day info
                DataTable working_day_data = new DataTable();
                List<WorkingDayModel> working_day = new List<WorkingDayModel>();
                working_day = JsonSerializer.Deserialize<List<WorkingDayModel>>(report_data.working_day_info);
                working_day_data = CreateDataTable(working_day);
                
                //Working day summaryu data
                DataTable working_day_summary_data = new DataTable();
                List<CloseWorkingDaySummaryDataModel> working_day_summary = new List<CloseWorkingDaySummaryDataModel>();
                working_day_summary = JsonSerializer.Deserialize<List<CloseWorkingDaySummaryDataModel>>(report_data.working_day_data);
                working_day_summary_data = CreateDataTable(working_day_summary);
               
                //Close sale data data
                DataTable close_sale_data = new DataTable();
                List<SaleModel> close_sales = new List<SaleModel>();
                close_sales = JsonSerializer.Deserialize<List<SaleModel>>(report_data.sale_data);
                close_sale_data = CreateDataTable(close_sales);

                //Setting data
                DataTable setting_data = new DataTable();
                List<SettingModel> settings = new List<SettingModel>();
                settings = JsonSerializer.Deserialize<List<SettingModel>>(report_data.setting_data);
                settings.ForEach(r => r.printed_by = printed_by);
                setting_data = CreateDataTable(settings);
 
                report.DataSources.Add(new ReportDataSource("WorkingDay", working_day_data));
                report.DataSources.Add(new ReportDataSource("Setting", setting_data));
                report.DataSources.Add(new ReportDataSource("CloseWorkingDayData", working_day_summary_data));
                report.DataSources.Add(new ReportDataSource("CloseSaleData", close_sale_data));
                report.DataSources.Add(new ReportDataSource("Translate", CreateDataTable(_translate_text)));

      
                Print(report, receipt,printer_name, 1);

                if (working_day.Any())
                {
                    var wd = working_day.FirstOrDefault();
                    TelegramMessageParamModel param = new TelegramMessageParamModel()
                    {
                        document_number =wd.working_day_number,
                        outlet_name = wd.outlet_name_en,
                        station_name = wd.closed_station_name_en,
                        printed_by = printed_by,
                    };

                    Thread t = ThreadStart(() => SendTelegramAlert(report, $"PrintCloseWorkingDayReportService",param));
                }
                IsPrintSuccess = true;
            }
            catch (Exception ex)
            {
                WriteToFile(ex.Message + ex.ToString());
                IsPrintSuccess = false;
            };
        }
        public static Thread ThreadStart(Action action)
        {
            Thread thread = new Thread(() => { action(); });
            thread.Start();
            return thread;
        }


        public static async Task PrintCloseWorkingDaySaleProduct(string working_day_id, ReceiptListModel receipt, string printer_name, string printed_by = "",string language="en")
        {
            try
            {  
                DynamicDataModel report_data = new DynamicDataModel();  
                var data = await GetApiData($"sp_get_close_working_sale_product_data_for_print", $"'{working_day_id}','{language}','json'");
                if (data.Any())
                {
                    report_data = data.FirstOrDefault();
                }
                else
                {
                    return;
                }
                var _translate_text = await GetTranslateText(language);
                LocalReport report = new LocalReport();  
                report.ReportPath = string.Format(@"{0}\RDLC\{1}.rdlc", AppDomain.CurrentDomain.BaseDirectory, receipt.ReceiptFileName);

                //Working day info
                DataTable working_day_data = new DataTable();
                List<WorkingDayModel> working_day = new List<WorkingDayModel>();
                working_day = JsonSerializer.Deserialize<List<WorkingDayModel>>(report_data.working_day_info);
                working_day_data = CreateDataTable(working_day);

                //Setting data
                DataTable setting_data = new DataTable();
                List<SettingModel> settings = new List<SettingModel>();
                settings = JsonSerializer.Deserialize<List<SettingModel>>(report_data.setting_data);
                settings.ForEach(r => r.printed_by = printed_by);
                setting_data = CreateDataTable(settings);

                //Sale Product Data
                DataTable sale_product_data= new DataTable();
                List<SaleProductModel> sale_products = new List<SaleProductModel>();
                sale_products = JsonSerializer.Deserialize<List<SaleProductModel>>(report_data.sale_product_data);
                sale_product_data = CreateDataTable(sale_products);

                //FOC Sale Product Data
                DataTable foc_sale_product_data= new DataTable();
                List<SaleProductModel> foc_sale_products = new List<SaleProductModel>();
                foc_sale_products = JsonSerializer.Deserialize<List<SaleProductModel>>(report_data.foc_sale_product_data);
                foc_sale_product_data = CreateDataTable(foc_sale_products);

                report.DataSources.Add(new ReportDataSource("WorkingDay", working_day_data));
                report.DataSources.Add(new ReportDataSource("Setting", setting_data));
                report.DataSources.Add(new ReportDataSource("SaleProductData", sale_product_data));
                report.DataSources.Add(new ReportDataSource("FOCSaleProductData", foc_sale_product_data));
                report.DataSources.Add(new ReportDataSource("Translate", CreateDataTable(_translate_text)));

          
                Print(report, receipt, printer_name, 1);

                var wd = working_day.FirstOrDefault();
                TelegramMessageParamModel param = new TelegramMessageParamModel()
                {
                    document_number = wd.working_day_number,
                    outlet_name = wd.outlet_name_en,
                    station_name = wd.closed_station_name_en,
                    printed_by = printed_by,
                };
                Thread t = ThreadStart(() => SendTelegramAlert(report, $"PrintCloseWorkingDaySaleProductReportService", param));  

                IsPrintSuccess = true;
            }
            catch (Exception ex)
            {
                WriteToFile(ex.Message + ex.ToString());
                IsPrintSuccess = false;
            };
        }

        public static async Task PrintCloseWorkingDaySaleTransaction(string working_day_id, ReceiptListModel receipt, string printer_name, string printed_by = "",string language="")
        {
            try
            { 
                DynamicDataModel report_data = new DynamicDataModel();  
                var data = await GetApiData($"sp_get_close_working_data_sale_transaction_for_print", $"'{working_day_id}','{language}','json'");
                if (data.Any())
                {
                    report_data = data.FirstOrDefault();
                }
                else
                {
                    return;
                }
                var _translate_text = await GetTranslateText(language);
                LocalReport report = new LocalReport();  
                report.ReportPath = string.Format(@"{0}\RDLC\{1}.rdlc", AppDomain.CurrentDomain.BaseDirectory, receipt.ReceiptFileName);

                //Working day info
                DataTable working_day_data = new DataTable();
                List<WorkingDayModel> working_day = new List<WorkingDayModel>();
                working_day = JsonSerializer.Deserialize<List<WorkingDayModel>>(report_data.working_day_info);
                working_day_data = CreateDataTable(working_day);

                //Setting data
                DataTable setting_data = new DataTable();
                List<SettingModel> settings = new List<SettingModel>();
                settings = JsonSerializer.Deserialize<List<SettingModel>>(report_data.setting_data);
                settings.ForEach(r => r.printed_by = printed_by);
                setting_data = CreateDataTable(settings);

                //Close sale data data
                DataTable close_sale_data = new DataTable();
                List<SaleModel> close_sales = new List<SaleModel>();
                close_sales = JsonSerializer.Deserialize<List<SaleModel>>(report_data.sale_data);
                close_sale_data = CreateDataTable(close_sales);

                report.DataSources.Add(new ReportDataSource("WorkingDay", working_day_data));
                report.DataSources.Add(new ReportDataSource("Setting", setting_data));
                report.DataSources.Add(new ReportDataSource("CloseSaleData", close_sale_data));
                report.DataSources.Add(new ReportDataSource("Translate", CreateDataTable(_translate_text)));
   

                Print(report, receipt, printer_name, 1);

                var wd = working_day.FirstOrDefault();
                TelegramMessageParamModel param = new TelegramMessageParamModel()
                {
                    document_number = wd.working_day_number,
                    outlet_name = wd.outlet_name_en,
                    station_name = wd.closed_station_name_en,
                    printed_by = printed_by,
                };
                Thread t = ThreadStart(() => SendTelegramAlert(report, $"PrintCloseWorkingDaySaleTransactionReportService", param)); 
                
                IsPrintSuccess = true;
            }
            catch (Exception ex)
            {
                WriteToFile(ex.Message + ex.ToString());
                IsPrintSuccess = false;
            };
        }  
        public static async Task PrintCloseCashierShiftSummary(string cashier_shift_id, ReceiptListModel receipt, string printer_name, string printed_by = "", string language = "en")
        {
            try
            {   
                DynamicDataModel report_data = new DynamicDataModel();   
                var data = await GetApiData($"sp_get_close_cashier_shift_data_for_print", $"'{cashier_shift_id}','json','{language}'");   
                if (data.Any())
                {
                    report_data = data.FirstOrDefault();
                }
                else
                {
                    return;
                }
                var _translate_text = await GetTranslateText(language);
                LocalReport report = new LocalReport();  
                report.ReportPath = string.Format(@"{0}\RDLC\{1}.rdlc", AppDomain.CurrentDomain.BaseDirectory, receipt.ReceiptFileName);

                //cashier shift info
                DataTable cashier_shift_data = new DataTable();
                List<CashierShiftModel> cashier_shift = new List<CashierShiftModel>();
                cashier_shift = JsonSerializer.Deserialize<List<CashierShiftModel>>(report_data.cashier_shift_info);
                cashier_shift_data = CreateDataTable(cashier_shift);

                //cashier shift summaryu data
                DataTable cashier_shift_summary_data = new DataTable();
                List<CloseCashierShiftSummaryDataModel> cashier_shift_summary = new List<CloseCashierShiftSummaryDataModel>();
                cashier_shift_summary= JsonSerializer.Deserialize<List<CloseCashierShiftSummaryDataModel>>(report_data.cashier_shift_data);
                cashier_shift_summary_data = CreateDataTable(cashier_shift_summary);

                //Close sale data data
                DataTable close_sale_data = new DataTable();
                List<SaleModel> close_sales = new List<SaleModel>();
                close_sales = JsonSerializer.Deserialize<List<SaleModel>>(report_data.sale_data);
                close_sale_data = CreateDataTable(close_sales);

                //Setting data
                DataTable setting_data = new DataTable();
                List<SettingModel> settings = new List<SettingModel>();
                settings = JsonSerializer.Deserialize<List<SettingModel>>(report_data.setting_data);
                settings.ForEach(r => r.printed_by = printed_by);
                setting_data = CreateDataTable(settings);

                report.DataSources.Add(new ReportDataSource("CashierShiftData",cashier_shift_data));
                report.DataSources.Add(new ReportDataSource("Setting", setting_data));
                report.DataSources.Add(new ReportDataSource("CloseCashierShiftData", cashier_shift_summary_data));
                report.DataSources.Add(new ReportDataSource("CloseSaleData", close_sale_data));
                report.DataSources.Add(new ReportDataSource("Translate", CreateDataTable(_translate_text)));     

                Print(report, receipt,printer_name, 1);

                var cs = cashier_shift.FirstOrDefault();
                TelegramMessageParamModel param = new TelegramMessageParamModel()
                {
                    document_number = cs.working_day_number,
                    outlet_name = cs.outlet_name_en,
                    station_name = cs.closed_station_name_en,
                    printed_by = printed_by,
                };
                Thread t = ThreadStart(() => SendTelegramAlert(report, $"PrintCloseCashierShiftSaleSummaryReportService", param));    

                IsPrintSuccess = true;
            }
            catch (Exception ex)
            {
                WriteToFile(ex.Message + ex.ToString());
                IsPrintSuccess = false;
            };
        }

        public static async Task PrintCloseCashierShiftSaleTransaction(string cashier_shift_id, ReceiptListModel receipt, string printer_name, string printed_by = "",string language="en")
        {
            try
            {  
                DynamicDataModel report_data = new DynamicDataModel();   
                var data = await GetApiData($"sp_get_close_cashier_shift_sale_transaction_for_print", $"'{cashier_shift_id}','{language}','json'");
                if (data.Any())
                {
                    report_data = data.FirstOrDefault();
                }
                else
                {
                    return;
                }
                var _translate_text = await GetTranslateText(language);
                LocalReport report = new LocalReport();   
                report.ReportPath = string.Format(@"{0}\RDLC\{1}.rdlc", AppDomain.CurrentDomain.BaseDirectory, receipt.ReceiptFileName); 

                //cashier shift info
                DataTable cashier_shift_data = new DataTable();
                List<CashierShiftModel> cashier_shift = new List<CashierShiftModel>();
                cashier_shift = JsonSerializer.Deserialize<List<CashierShiftModel>>(report_data.cashier_shift_info);
                cashier_shift_data = CreateDataTable(cashier_shift);  
               
                //Close sale data data
                DataTable close_sale_data = new DataTable();
                List<SaleModel> close_sales = new List<SaleModel>();
                close_sales = JsonSerializer.Deserialize<List<SaleModel>>(report_data.sale_data);
                close_sale_data = CreateDataTable(close_sales);

                //Setting data
                DataTable setting_data = new DataTable();
                List<SettingModel> settings = new List<SettingModel>();
                settings = JsonSerializer.Deserialize<List<SettingModel>>(report_data.setting_data);
                settings.ForEach(r => r.printed_by = printed_by);
                setting_data = CreateDataTable(settings);

                report.DataSources.Add(new ReportDataSource("CashierShiftData", cashier_shift_data));
                report.DataSources.Add(new ReportDataSource("Setting", setting_data));
                report.DataSources.Add(new ReportDataSource("CloseSaleData", close_sale_data));
                report.DataSources.Add(new ReportDataSource("Translate", CreateDataTable(_translate_text)));         

                Print(report, receipt,printer_name, 1);

                var cs = cashier_shift.FirstOrDefault();
                TelegramMessageParamModel param = new TelegramMessageParamModel()
                {
                    document_number = cs.working_day_number,
                    outlet_name = cs.outlet_name_en,
                    station_name = cs.closed_station_name_en,
                    printed_by = printed_by,
                };
                Thread t = ThreadStart(() => SendTelegramAlert(report, $"PrintCloseCashierShiftSaleTransactionService", param));

                IsPrintSuccess = true;
            }
            catch (Exception ex)
            {
                WriteToFile(ex.Message + ex.ToString());
                IsPrintSuccess = false;
            };
        }



        public static async Task PrintCloseCashierShiftSaleProduct(string cashier_shift_id, ReceiptListModel receipt, string printer_name, string printed_by = "",string language="en")
        {
            try
            { 
                DynamicDataModel report_data = new DynamicDataModel();    
                var data = await GetApiData($"sp_get_close_cashier_shift_sale_product_data_for_print", $"'{cashier_shift_id}','{language}','json'");
                if (data.Any())
                {
                    report_data = data.FirstOrDefault();
                }
                else
                {
                    return;
                }
                var _translate_text = await GetTranslateText(language);
                LocalReport report = new LocalReport();  
                report.ReportPath = string.Format(@"{0}\RDLC\{1}.rdlc", AppDomain.CurrentDomain.BaseDirectory, receipt.ReceiptFileName);  

                //cashier shift info
                DataTable cashier_shift_data = new DataTable();
                List<CashierShiftModel> cashier_shift = new List<CashierShiftModel>();
                cashier_shift = JsonSerializer.Deserialize<List<CashierShiftModel>>(report_data.cashier_shift_info);
                cashier_shift_data = CreateDataTable(cashier_shift);

                //Setting data
                DataTable setting_data = new DataTable();
                List<SettingModel> settings = new List<SettingModel>();
                settings = JsonSerializer.Deserialize<List<SettingModel>>(report_data.setting_data);
                settings.ForEach(r => r.printed_by = printed_by);
                setting_data = CreateDataTable(settings);

                //Sale Product Data
                DataTable sale_product_data = new DataTable();
                List<SaleProductModel> sale_products = new List<SaleProductModel>();
                sale_products = JsonSerializer.Deserialize<List<SaleProductModel>>(report_data.sale_product_data);
                sale_product_data = CreateDataTable(sale_products);

                //FOC Sale Product Data
                DataTable foc_sale_product_data = new DataTable();
                List<SaleProductModel> foc_sale_products = new List<SaleProductModel>();
                foc_sale_products = JsonSerializer.Deserialize<List<SaleProductModel>>(report_data.foc_sale_product_data);
                foc_sale_product_data = CreateDataTable(foc_sale_products);

                report.DataSources.Add(new ReportDataSource("CashierShiftData", cashier_shift_data));
                report.DataSources.Add(new ReportDataSource("Setting", setting_data));
         
                report.DataSources.Add(new ReportDataSource("SaleProductData", sale_product_data));
                report.DataSources.Add(new ReportDataSource("FOCSaleProductData", foc_sale_product_data));
                report.DataSources.Add(new ReportDataSource("Translate", CreateDataTable(_translate_text)));
          

                Print(report, receipt,printer_name, 1);

                var cs = cashier_shift.FirstOrDefault();
                TelegramMessageParamModel param = new TelegramMessageParamModel()
                {
                    document_number = cs.working_day_number,
                    outlet_name = cs.outlet_name_en,
                    station_name = cs.closed_station_name_en,
                    printed_by = printed_by,
                };
                Thread t = ThreadStart(() => SendTelegramAlert(report, $"PrintCloseCashierShiftSaleProductReportService", param));                 
                IsPrintSuccess = true;
            }
            catch (Exception ex)
            {
                WriteToFile(ex.Message + "\n" + ex.ToString());
                IsPrintSuccess = false;
            };
        }

      public  static async Task<List<DynamicDataModel>> GetApiData(string procedure_name, string parameters)
        {

            try
            {
                var client = new RestClient(Properties.Settings.Default.BaseAPIURL);

                var request = new RestRequest("Printing/GetPrintData");
                request.AddParameter("procedure_name", procedure_name);
                request.AddParameter("parameters", parameters);
                var response = await client.GetAsync(request); 
                if ( response.Content.ToString() != "")
                {
                    var result =JsonSerializer.Deserialize<List<DynamicDataModel>>( response.Content);  
                    return result;
                }     
                return new List<DynamicDataModel>();   
            }
            catch (Exception ex)
            {
                WriteToFile(ex.Message + "\n" + ex.ToString());
                return new List<DynamicDataModel>();   
            }
        }
        public static async Task<GetResponse> GetApi(string query)
        {

            try
            {
                System.Net.HttpStatusCode StatusCode = new System.Net.HttpStatusCode();
                var client = new RestClient(Properties.Settings.Default.BaseAPIURL);
                var request = new RestRequest(query);
                var resp = await client.GetAsync(request);

                StatusCode = resp.StatusCode;
                if (resp.IsSuccessful)
                {
                    var jsonString = resp.Content.ToString();
                    return new GetResponse(true, jsonString);
                }


                return new GetResponse(false, StatusCode);
            }
            catch (Exception ex)
            {
                WriteToFile(ex.Message + "\n" + ex.ToString());
                return new GetResponse(false, 0);
            }
        }



        public static async Task<List<DynamicDataModel>> GetTranslateText(string language_code = "en")
        {

            try
            {
                if (translate_caches.Where(r => r.language_code == language_code).Any())
                {
                    return translate_caches.Where(r => r.language_code == language_code).FirstOrDefault().translate_data;
                }
                else
                {
                    var client = new RestClient(Properties.Settings.Default.BaseAPIURL);
                    var request = new RestRequest("Printing/GetPrintData");
                    request.AddParameter("procedure_name", "sp_get_translate_text");
                    request.AddParameter("parameters", $"'{language_code}','close_cashier_shift_summary_report,shift_information,working_day_no,shift_no,sale_transaction,receipt_no,tbl_no,time,qty,amt,by,branch,outlet,status,close_working_day_summary_report,working_day_information,cash_drawer_name,opened_date,opened_by,closed_date,closed_by,printed_by,printed_on,sale_products,sale_product,amount,total,grand_total,product_name,summary_by_revenue_group,revenue_group,foc_sale_product,free_sale_product,close_cashier_shift_report,total_quantity,sub_total,item_discount,sale_discount','json'");
                    var response = await client.GetAsync(request);
                    if (response.Content.ToString() != "")
                    {
                        var result = System.Text.Json.JsonSerializer.Deserialize<List<DynamicDataModel>>(response.Content);  
                        if (!translate_caches.Where(r => r.language_code == language_code).Any())
                        {
                            translate_caches.Add(new TranslateCacheModel() { language_code = language_code, translate_data = result });
                        }
                            return result;
                    }
                    return new List<DynamicDataModel>();
                }  
            }
            catch (Exception ex)
            {
                WriteToFile(ex.Message + "\n" + ex.ToString());
                return new List<DynamicDataModel>();
            }
        }



        public static async Task ExecuteSQLSatement(string sql)
        {

            try
            {
                var client = new RestClient(Properties.Settings.Default.BaseAPIURL);      
                var request = new RestRequest("Printing/ExecuteSQLSatement");
                request.AddParameter("sql", sql);   
                var response = await client.GetAsync(request);
                
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


            try
            {
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
            catch { };
        }

        public static void OpenCashDrawer(string printer_name)
        {
            try
            {                                                                                              
                string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
                var doc = JsonDocument.Parse(Properties.Settings.Default.USBCashDrawer);
                var root = doc.RootElement;
                var _usb = root.GetProperty("is_usb").GetBoolean();
                var _port = root.GetProperty("port").GetString();
                  
                if (_usb)
                {
                    try
                    { 
                        FileSystem.FileOpen(1, path, OpenMode.Output);
                        FileSystem.PrintLine(1, Strings.Chr(27) + "p" + Strings.Chr(0) + Strings.Chr(25) + Strings.Chr(250));
                        Interaction.Shell("print /d:" + _port + " " + path, AppWinStyle.MinimizedNoFocus);
                        FileSystem.FileClose(1);
                    }
                    catch (Exception ex)
                    {
                        WriteToFile(ex.Message);
                        return;
                    }
                }
                else
                {
                    string DrawerCode = Strings.Chr(27).ToString() + Strings.Chr(112).ToString() + Strings.Chr(48).ToString() + Strings.Chr(64).ToString() + Strings.Chr(64).ToString();                    
                    PrintRaw(printer_name, DrawerCode);
                }
            }
            catch (Exception ex)
            {
               WriteToFile(ex.Message);  
            }
        }


        //open cach drawer
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct DOCINFO
        {
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pDocName;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pOutputFile;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pDataType;
        }

        // ----- Define interfaces to the functions supplied in the DLL.
        [DllImport("winspool.drv", EntryPoint = "OpenPrinterW", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool OpenPrinter(string printerName, ref IntPtr hPrinter, int printerDefaults);

        [DllImport("winspool.drv", EntryPoint = "ClosePrinter", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool ClosePrinter(IntPtr hPrinter);

        [DllImport("winspool.drv", EntryPoint = "StartDocPrinterW", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartDocPrinter(IntPtr hPrinter, int level, ref DOCINFO documentInfo);

        [DllImport("winspool.drv", EntryPoint = "EndDocPrinter", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndDocPrinter(IntPtr hPrinter);

        [DllImport("winspool.drv", EntryPoint = "StartPagePrinter", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.drv", EntryPoint = "EndPagePrinter", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.drv", EntryPoint = "WritePrinter", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool WritePrinter(IntPtr hPrinter, IntPtr buffer, int bufferLength, ref int bytesWritten);


        public static bool PrintRaw(string printerName, string origString)
        {
            bool functionReturnValue = false;
            try
            {
                // ----- Send a string of  raw data to  the printer.
                IntPtr hPrinter = default(IntPtr);
                DOCINFO spoolData = new DOCINFO();
                IntPtr dataToSend = default(IntPtr);
                int dataSize = 0;
                int bytesWritten = 0;

                // ----- The internal format of a .NET String is just
                //       different enough from what the printer expects
                //       that there will be a problem if we send it
                //       directly. Convert it to ANSI format before
                //       sending.
                dataSize = origString.Length;
                dataToSend = Marshal.StringToCoTaskMemAnsi(origString);

                // ----- Prepare information for the spooler.
                spoolData.pDocName = "OpenDrawer";
                // class='highlight'
                spoolData.pDataType = "RAW";

                try
                {
                    // ----- Open a channel to  the printer or spooler.
                    OpenPrinter(printerName, ref hPrinter, 0);

                    // ----- Start a new document and Section 1.1.
                    StartDocPrinter(hPrinter, 1, ref spoolData);
                    StartPagePrinter(hPrinter);

                    // ----- Send the data to the printer.
                    WritePrinter(hPrinter, dataToSend, dataSize, ref bytesWritten);

                    // ----- Close everything that we opened.
                    EndPagePrinter(hPrinter);
                    EndDocPrinter(hPrinter);
                    ClosePrinter(hPrinter);
                    functionReturnValue = true;
                }
                catch (Exception ex)
                {
                    WriteToFile(ex.Message + "\n" + ex.ToString());
                    functionReturnValue = false;
                }
                finally
                {
                    // ----- Get rid of the special ANSI version.
                    Marshal.FreeCoTaskMem(dataToSend);
                }
            }
            catch (Exception ex)
            {
                WriteToFile(ex.Message + "\n" + ex.ToString());
                functionReturnValue = false;
            }
            return functionReturnValue;
        }

    }
}
