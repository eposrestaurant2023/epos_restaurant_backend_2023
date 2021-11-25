using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using RestSharp;

namespace ePOSClientSyncService
{
    class Program
    {
        static string DBConnection = "Data Source=192.168.10.13,1437;Initial Catalog=epos_restaurant_client_2021_db;Persist Security Info=True;User ID=sa;Password=admin@eSoftix";

        static void Main(string[] args)
        {
            var watcher = new FileSystemWatcher(@"C:\epossync\");

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

            Console.WriteLine("Press enter to exit.");
            Console.ReadLine();
        }




        private static void OnCreatedAsync(object sender, FileSystemEventArgs e)
        { 
            string value = e.Name.Replace(".txt", "").Split(',')[0];

            try
            {
                var client = new RestClient("http://localhost:7072/api/");
              

                DataTable dt = ExecuteDataTable("exec sp_get_data_for_synchronize");
                foreach(DataRow r in dt.Rows)
                {
                    string _query = "";
                    switch (r["transaction_type"].ToString())
                    {
                        case "working_day":
                             _query = $"Sync/SyncWorkingDay?workingDayId={r["id"].ToString()}";
                            break;
                        case "cash_drawer_amount":
                             _query = $"Sync/SyncCashDrawerAmount?id={r["id"].ToString()}";
                            break;
                        case "cashier_shift":
                            _query = $"Sync/SyncCashierShift?id={r["id"].ToString()}";
                            break;
                        case "sale":
                             _query = $"Sync/SyncSale?saleId={r["id"].ToString()}";
                            break;
                        case "history":
                            _query = $"Sync/SyncHistory?historyId={r["id"].ToString()}";
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



                File.Delete(e.FullPath);

                Console.WriteLine("sale synch complete " + value);
            }
            catch (Exception ex)
            {


            }

            Console.WriteLine(value);
        }



        static DataTable ExecuteDataTable(string sql)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(DBConnection))
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    SqlTransaction trans = con.BeginTransaction();
                    SqlCommand cmd = new SqlCommand(sql, con, trans);
                    try
                    {
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                        trans.Commit();
                        return dt;
                    }
                    catch (Exception ex)
                    {

                        trans.Rollback();
                        //WriteToFile(ex.Message);
                        return null;
                    }
                    finally
                    {
                        cmd.Dispose();
                        trans.Dispose();
                        if (con.State == ConnectionState.Open)
                            con.Close();
                    }
                }
            }
            catch (SqlException ex)
            {

                //WriteToFile(ex.Message);
                return null;
            }
        }

    }
}
