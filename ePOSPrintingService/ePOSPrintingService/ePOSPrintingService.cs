using ePOSPrintingService.Models;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
 

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
                System.Threading.Thread.Sleep(250);

                string file_path = e.FullPath;
                ActionModel action = new ActionModel();
                using (Stream f = new FileStream(file_path,
                                 FileMode.Open,
                                 FileAccess.Read))
                {

                    string json = new StreamReader(f).ReadToEnd();

                    if (json != "")
                    {
                        action = JsonConvert.DeserializeObject<ActionModel>(json);
                    }


                    switch (action.action)
                    {
                        case "print_request_bill":
                            ReceiptListModel invoice = Program.ReceiptLists.Where(r => r.ReceiptName.ToLower() == action.receipt_name.ToLower()).FirstOrDefault();
                            Program.PrintIvoice(action.sale_id, invoice, Program.CashierPrinter);
                            break;
                        case "print_receipt":
                            ReceiptListModel receipt = Program.ReceiptLists.Where(r => r.ReceiptName.ToLower() == action.receipt_name.ToLower()).FirstOrDefault();
                            Program.PrintReceipt(action.sale_id, receipt, Program.CashierPrinter);
                            break;
                        case "reprint_receipt":
                            ReceiptListModel reprint_receipt = Program.ReceiptLists.Where(r => r.ReceiptName.ToLower() == action.receipt_name.ToLower()).FirstOrDefault();
                            Program.PrintReceipt(action.sale_id, reprint_receipt, Program.CashierPrinter, true);
                            break;
                        case "print_to_kitchen":

                            Program.PrintKitchenOrder(action.sale_id);
                            break;


                        default:

                            break;
                    }
                }



            }
            catch (Exception ex)
            {
                Program.WriteToFile(ex.Message);
            }
        }


    }
}
