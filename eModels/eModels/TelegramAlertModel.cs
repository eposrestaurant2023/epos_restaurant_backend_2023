using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace eModels
{
 
    public  class TelegramAlertModel   : TelegramModel
    {
        [NotMapped,JsonIgnore]
        public Guid business_branch_id { get; set; }
        public List<TelegramActionModel> actions { get; set; }
    }
    public class TelegramModel
    {
        public string chat_id { get; set; }
        public string token { get; set; }
    }
    public class TelegramActionModel
    {
        public string    name { get; set; }
        public string title { get; set; }
        public string msg { get; set; }
        public bool allow_send { get; set; } = true;
        public int sort { get; set; } = 0;
    }
}
