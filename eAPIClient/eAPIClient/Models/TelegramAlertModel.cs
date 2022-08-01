
using System.Collections.Generic;
using System.Text.Json;          

namespace eAPIClient
{
    
    public class TelegramAlertModel : TelegramModel
    {   
        public List<TelegramActionModel> actions { get; set; }

    }

    public class TelegramActionModel
    {
        public string name { get; set; }
        public string title { get; set; }
        public string msg { get; set; }
        public bool allow_send { get; set; }
        public int sort { get; set; }
    }
    public class TelegramModel
    {
        public string chat_id { get; set; }
        public string token { get; set; } 
    }


}
