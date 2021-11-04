using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace ePOSClientSyncService
{
    class Program
    {
      
        static void Main(string[] args)
        {
            using var watcher = new FileSystemWatcher(@"C:\epossync\");

            watcher.NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size;

         
            watcher.Created +=OnCreatedAsync;
          
         
            watcher.Filter = "*.txt";
            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;

            Console.WriteLine("Press enter to exit.");
            Console.ReadLine();
        }


   
       
        private static void OnCreatedAsync(object sender, FileSystemEventArgs e)
        {
               HttpClient client;
            string value = e.Name.Replace(".txt","").Split(',')[0];

            try
            {

                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                client = new HttpClient(clientHandler);

                Task.Run(() =>   client.GetAsync($"http://customer.esoftix.com:9597/client/api/Sync/SyncSale?saleId={value}"));


                File.Delete(e.FullPath);

                Console.WriteLine("sale synch complete " + value);
            }
            catch (Exception ex)
            {

                
            }

            Console.WriteLine(value);
        }


       

    }
}
