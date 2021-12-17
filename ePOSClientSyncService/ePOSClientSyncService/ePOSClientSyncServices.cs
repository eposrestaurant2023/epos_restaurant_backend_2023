using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace ePOSClientSyncService
{
    partial class ePOSClientSyncServices : ServiceBase
    {
        public ePOSClientSyncServices()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            var watcher = new FileSystemWatcher(Properties.Settings.Default.file_watcher_path);

            watcher.NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size;


            watcher.Created += OnCreatedAsync;


            watcher.Filter = "*.txt";
            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;



            var watcherRemoteData = new FileSystemWatcher(Properties.Settings.Default.file_watcher_path);

            watcherRemoteData.NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size;


            watcherRemoteData.Created += OnSyncFromRemoteServerAsync;


            watcherRemoteData.Filter = "*.bat";
            watcherRemoteData.IncludeSubdirectories = true;
            watcherRemoteData.EnableRaisingEvents = true;


         
        }

        private static void OnCreatedAsync(object sender, FileSystemEventArgs e)
        {
            string value = e.Name.Replace(".txt", "").Split(',')[0];

            try
            {
                var client = new RestClient(Properties.Settings.Default.api_url);


                var data = GetDataforSync();
                if (data.Any())
                {




                    foreach (var r in data)
                    {
                        string _query = "";
                        switch (r.transaction_type.ToString())
                        {
                            case "working_day":
                                _query = $"Sync/SyncWorkingDay?workingDayId={r.id.ToString()}";
                                break;
                            case "cash_drawer_amount":
                                _query = $"Sync/SyncCashDrawerAmount?id={r.id.ToString()}";
                                break;
                            case "cashier_shift":
                                _query = $"Sync/SyncCashierShift?id={r.id.ToString()}";
                                break;
                            case "sale":
                                _query = $"Sync/SyncSale?saleId={r.id.ToString()}";
                                break;
                            case "history":
                                _query = $"Sync/SyncHistory?id={r.id.ToString()}";
                                break;
                            case "customer":
                                _query = $"Sync/SyncCustomer?id={r.id.ToString()}";
                                break;
                            default:
                                break;
                        }

                        if (!string.IsNullOrEmpty(_query))
                        {
                            var request = new RestRequest(_query);
                            var response = client.Get(request);
                        }
                    }
                }


                System.Threading.Thread.Sleep(1000);

                File.Delete(e.FullPath);

                Console.WriteLine("sale synch complete " + value);
            }
            catch (Exception ex)
            {


            }

            Console.WriteLine(value);
        }

        private static void OnSyncFromRemoteServerAsync(object sender, FileSystemEventArgs e)
        {
            string value = e.Name.Replace(".txt", "").Split(',')[0];

            try
            {
                var client = new RestClient(Properties.Settings.Default.api_url);

                var request = new RestRequest("Sync/SyncRemoteData?business_branch_id=" + Properties.Settings.Default.business_branch_id);
                var response = client.Get(request);

                System.Threading.Thread.Sleep(1000);

                File.Delete(e.FullPath);

             
            }
            catch (Exception ex)
            {
                Program.WriteToFile(string.Format(@"Sync remote error on  {0:yyyy-MM-dd HH:mm tt}, {1}", DateTime.Now, ex.ToString()));

            }

            Console.WriteLine(value);
        }


        static List<DataForSyncModel> GetDataforSync()
        {

            try
            {
                var client = new RestClient(Properties.Settings.Default.api_url);

                var request = new RestRequest("Sync/GetDataForSynchronize");
                var response = client.Get(request);
                if (response.Content.ToString() != "")
                {
                 
                    List<DataForSyncModel> datas = System.Text.Json.JsonSerializer.Deserialize<List<DataForSyncModel>>(response.Content);
                    return datas;

                }

                return new List<DataForSyncModel>();



            }
            catch (Exception ex)
            {
                return new List<DataForSyncModel>();

            }
        }  


        public void OnDebug()
        {
            OnStart(null);
        }

        protected override void OnStop()
        {
            Program.WriteToFile(string.Format(@"Sevice Stopped at {0:yyyy-MM-dd HH:mm tt}", DateTime.Now));
        }

    }

    class DataForSyncModel
    {
        public string transaction_type { get; set; }
        public string id { get; set; }
    }
}
