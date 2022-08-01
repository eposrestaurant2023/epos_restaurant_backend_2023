
using System.Collections.Generic;
using System.Text.Json;          

namespace eAPIClient
{
    
    public class CurrencyModel
    {
        public int? id { get; set; }
        public string currency_format { get; set; }
        public bool is_main { get; set; }
        public string symbol { get; set; }
        public bool is_prefix_symbol { get; set; }
    }
}
