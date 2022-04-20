using ePOSPrintingService.Models;    
using System; 
using System.Data; 
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;

namespace ePOSPrintingService
{
    public partial class ePOSPrintingService : ServiceBase
    { 
        public ePOSPrintingService()
        {
            InitializeComponent(); 
        }

        public void OnDebug()
        {
            OnStart(null);
        }

        protected override void OnStart(string[] args)
        {

            try
            {
                string folder = string.Format(@"{0}", Program.FileWatcherPath);
                if (!Directory.Exists(folder))
                {
                    DirectoryInfo di = Directory.CreateDirectory(folder);
                }

                Program.FileWatcherPath = folder; 
                var fileSystemWatcher = new FileSystemWatcher();
                // tell the watcher where to look
                fileSystemWatcher.Path = folder;

                fileSystemWatcher.NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size;
                // Associate event handlers with the events
                fileSystemWatcher.Created += FileSystemWatcher_Created;

                // You must add this line - this allows events to fire.
                fileSystemWatcher.EnableRaisingEvents = true;

            }
            catch (Exception ex)
            {
                Program.WriteToFile(ex.Message);
            }

            Program.WriteToFile(string.Format(@"Sevice Started at {0:yyyy-MM-dd HH:mm tt}", DateTime.Now));
        }

        protected override void OnStop()
        {
            Program.WriteToFile(string.Format(@"Sevice Stopped at {0:yyyy-MM-dd HH:mm tt}", DateTime.Now));
        }

        private void FileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            try
            {

                Thread.Sleep(250);
                string file_path = e.FullPath;
                if (File.Exists(file_path))
                {
                    ActionModel action = new ActionModel();
                    using (Stream f = new FileStream(file_path, FileMode.Open, FileAccess.Read))
                    {
                        string json = new StreamReader(f).ReadToEnd();
                        if (json != "")
                        {
                            action = JsonSerializer.Deserialize<ActionModel>(json);
                        }
                        switch (action.action)
                        {
                            case "print_request_bill":
                                ReceiptListModel invoice = Program.ReceiptLists.Where(r => r.ReceiptName.ToLower() == action.receipt_name.ToLower()).FirstOrDefault();
                                Task.Factory.StartNew(async () =>
                                {
                                    await Program.PrintInvoice(action.sale_id, invoice, Program.CashierPrinter, action.get_copy);
                                });
                                break;

                            case "print_receipt":
                                ReceiptListModel receipt = Program.ReceiptLists.Where(r => r.ReceiptName.ToLower() == action.receipt_name.ToLower()).FirstOrDefault();
                                Task.Factory.StartNew(async () =>
                                {
                                    await Program.PrintReceipt(action.sale_id, receipt, Program.CashierPrinter, action.get_copy);
                                });
                                break;

                            case "print_deleted_sale_order":
                                ReceiptListModel deleted_report = Program.ReceiptLists.Where(r => r.ReceiptName.ToLower() == action.receipt_name.ToLower()).FirstOrDefault();
                                Task.Factory.StartNew(async () =>
                                {
                                    await Program.PrintDeletedOrder(action.sale_id, deleted_report, Program.CashierPrinter);
                                });
                                break;

                            case "reprint_receipt":
                                ReceiptListModel reprint_receipt = Program.ReceiptLists.Where(r => r.ReceiptName.ToLower() == action.receipt_name.ToLower()).FirstOrDefault();
                                Task.Factory.StartNew(async () =>
                                {
                                    await Program.PrintReceipt(action.sale_id, reprint_receipt, Program.CashierPrinter, action.get_copy, true);
                                });
                                break;

                            case "print_to_kitchen":
                                Task.Factory.StartNew(async () =>
                                {
                                    await Program.PrintKitchenOrder(action.sale_id);
                                });
                                break;

                            case "kitchen_message":
                                Program.PrintKitchenMessage(action.data);
                                break;

                            case "print_close_working_day_summary":
                                ReceiptListModel close_working_day_report = Program.ReceiptLists.Where(r => r.ReceiptName.ToLower() == action.receipt_name.ToLower()).FirstOrDefault();
                                Task.Factory.StartNew(async () =>
                                {
                                    await Program.PrintCloseWorkingDay(action.id, close_working_day_report, Program.CashierPrinter, action.printed_by, action.language);
                                });
                                break;

                            case "print_close_working_day_sale_product":
                                ReceiptListModel close_working_day_sale_product_report = Program.ReceiptLists.Where(r => r.ReceiptName.ToLower() == action.receipt_name.ToLower()).FirstOrDefault();
                                Task.Factory.StartNew(async () =>
                                {
                                    await Program.PrintCloseWorkingDaySaleProduct(action.id, close_working_day_sale_product_report, Program.CashierPrinter, action.printed_by, action.language);
                                });
                                break;

                            case "print_close_working_day_sale_transaction":
                                ReceiptListModel close_working_day_sale_transaction_report = Program.ReceiptLists.Where(r => r.ReceiptName.ToLower() == action.receipt_name.ToLower()).FirstOrDefault();
                                Task.Factory.StartNew(async () =>
                                {
                                    await Program.PrintCloseWorkingDaySaleTransaction(action.id, close_working_day_sale_transaction_report, Program.CashierPrinter, action.printed_by, action.language);
                                });
                                break;

                            case "print_close_cashier_shift_summary":
                                ReceiptListModel close_cashift_shift_summary_report = Program.ReceiptLists.Where(r => r.ReceiptName.ToLower() == action.receipt_name.ToLower()).FirstOrDefault();
                                Task.Factory.StartNew(async () =>
                                {
                                    await Program.PrintCloseCashierShiftSummary(action.id, close_cashift_shift_summary_report, Program.CashierPrinter, action.printed_by, action.language);
                                });
                                break;

                            case "print_close_cashier_shift_sale_transaction":
                                ReceiptListModel close_cashift_shift_sale_transaction_report = Program.ReceiptLists.Where(r => r.ReceiptName.ToLower() == action.receipt_name.ToLower()).FirstOrDefault();
                                Task.Factory.StartNew(async () =>
                                {
                                    await Program.PrintCloseCashierShiftSaleTransaction(action.id, close_cashift_shift_sale_transaction_report, Program.CashierPrinter, action.printed_by, action.language);
                                });
                                break;

                            case "print_close_cashier_shift_sale_product":
                                ReceiptListModel close_cashift_shift_sale_product_report = Program.ReceiptLists.Where(r => r.ReceiptName.ToLower() == action.receipt_name.ToLower()).FirstOrDefault();
                                Task.Factory.StartNew(async () =>
                                {
                                    await Program.PrintCloseCashierShiftSaleProduct(action.id, close_cashift_shift_sale_product_report, Program.CashierPrinter, action.printed_by, action.language);
                                });
                                break;

                            case "print_waiting_order":
                                ReceiptListModel waiting_order = Program.ReceiptLists.Where(r => r.ReceiptName.ToLower() == action.receipt_name.ToLower()).FirstOrDefault();
                                Task.Factory.StartNew(async () =>
                                {
                                    await Program.PrintWaitingOrder(action.sale_id, waiting_order, Program.CashierPrinter, action.get_copy);
                                });
                                break;

                            case "print_park":
                                ReceiptListModel park = Program.ReceiptLists.Where(r => r.ReceiptName.ToLower() == action.receipt_name.ToLower()).FirstOrDefault();
                                Task.Factory.StartNew(async () =>
                                {
                                    await Program.PrintParkItemReceipt(action.sale_id, park, Program.CashierPrinter, action.is_reprint);
                                });
                                break;

                            case "print_wifi_password":
                                Task.Factory.StartNew(async () =>
                                {
                                    await Program.PrintWifiPassword();
                                });
                                break;

                            case "open_cashdrawer":
                                Program.OpenCashDrawer();
                                break;
                            default:

                                break;
                        }
                    }

                    Thread.Sleep(250);
                    File.Delete(file_path);
                }

            }
            catch (Exception ex)
            {
                Program.WriteToFile(ex.Message);
            }
        }   
        public Thread ThreadStart(Action action)
        {
            Thread thread = new Thread(() => { action(); });
            thread.Start();
            return thread;
        }  
    }
}
