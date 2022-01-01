﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using eAPIClient.Controllers;
using eAPIClient.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace eAPIClient.Services
{

    public class SyncService
    {
        public IConfiguration config { get; }
        private readonly ApplicationDbContext db;
        private readonly IHttpService http;
        public SyncService(ApplicationDbContext _db, IConfiguration _config, IHttpService _http)
        {
            db = _db;
            config = _config;
            http = _http;
        }          
             

        public async Task<bool> SyncSetting()
        {
            string business_branch_id = config.GetValue<string>("business_branch_id");
            var prepare_sync_response = await http.ApiPost("GetData", new FilterModel() { procedure_name = "sp_prepare_sync_config_data", procedure_parameter = $"'{business_branch_id}'" });
            if (prepare_sync_response.IsSuccess)
            {
                List<ConfigDataModel> config_datas = new();
                string url = "Configdata?$select=id,data,config_type,note,is_local_setting";
                url = url + $"&$filter=business_branch_id eq {business_branch_id}";
                var resp = await http.ApiGetOData(url);
                if (resp.IsSuccess)
                {

                    config_datas =  JsonSerializer.Deserialize<List<ConfigDataModel>>(resp.Content.ToString());
                    string _deleteQuery = string.Format("delete tbl_config_data where is_local_setting=0; ");
                    db.Database.ExecuteSqlRaw(_deleteQuery);
                    db.ConfigDatas.AddRange(config_datas.Where(r => r.is_local_setting == false));
                    db.SaveChanges();
                }

               
                
            }

            return false;

        }

     

    }
}
