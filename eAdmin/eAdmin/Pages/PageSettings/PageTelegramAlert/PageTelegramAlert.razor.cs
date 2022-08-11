using System.Threading.Tasks;
using eModels;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System;

namespace eAdmin.Pages.PageSettings.PageTelegramAlert
{
    public class PageTelegramAlertCore    : PageCore
    {
       
        List<SettingModel> settings = new List<SettingModel>();
        List<BusinessBranchSettingModel> business_branch_settings = new List<BusinessBranchSettingModel>();
        public  List<TelegramAlertModel> telegram_alert_list = new List<TelegramAlertModel>();



        public bool is_message_format = true;
        protected override async Task OnInitializedAsync()
        {
            await onInit();    
        }
        async Task onInit()
        {
            is_loading = true;
            telegram_alert_list = new List<TelegramAlertModel>();
            string api_url = $"Setting?$expand=business_branch_settings&$filter=id eq 115";         
            var resp = await http.ApiGetOData(api_url);
            {
                settings = JsonSerializer.Deserialize<List<SettingModel>>(resp.Content.ToString());
                if (settings.Any())
                {
                    business_branch_settings = settings.FirstOrDefault().business_branch_settings;
                    foreach(var bs in business_branch_settings)
                    {
                        List<TelegramAlertModel> telegrams =  JsonSerializer.Deserialize<List<TelegramAlertModel>>(bs.setting_value);
                        foreach(var t in telegrams)
                        {
                            telegram_alert_list.Add(new TelegramAlertModel()
                            {
                                    business_branch_id = bs.business_branch_id,
                                    chat_id =  t.chat_id,
                                    token = t.token,
                                    actions = t.actions
                            });
                        } 
                    }
                }
            }
            is_loading = false;
        }
          

        public void OnCreateNewChannel_Click(Guid business_branch_id, TelegramAlertModel t )
        {
            telegram_alert_list.Add(new TelegramAlertModel()
            {
                business_branch_id = business_branch_id,
                chat_id = "",
                token = "",
                actions = t.actions
            });
        }
        public void OnDeletedChannel_Click(Guid business_branch_id, TelegramAlertModel t )
        {
            telegram_alert_list = telegram_alert_list.Where(r=>r.business_branch_id==business_branch_id).ToList();
            telegram_alert_list.Remove(t);


            //

            Console.WriteLine(telegram_alert_list.Where(r => r.business_branch_id == business_branch_id).Count());
        }
        public  async Task Save_Click()
        {
            is_saving = true;

            foreach(var bs in   business_branch_settings)
            {                   
                    bs.setting_value = JsonSerializer.Serialize(telegram_alert_list.Where(r => r.business_branch_id == bs.business_branch_id).ToList());                                 
            }

            if (settings.Any())
            {
                settings.FirstOrDefault().business_branch_settings = business_branch_settings;
                var resp = await http.ApiPost("save/setting/multiple", settings);
                if (resp.IsSuccess)
                {
                    await onInit();
                    toast.Add(lang["Save successfully"], MudBlazor.Severity.Success);
                }
                else
                {
                    toast.Add(lang["Save data fail"], MudBlazor.Severity.Warning);
                }
            }
            
            is_saving = false;
        }

        public void OnAllowSend_Click(TelegramActionModel action)
        {
            //
            action.allow_send = !action.allow_send;
        }

    }
}
