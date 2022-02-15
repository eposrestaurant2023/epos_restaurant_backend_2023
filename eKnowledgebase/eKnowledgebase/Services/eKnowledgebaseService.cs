using eKnowledgebase.Models;
using System.Text.Json.Serialization;

namespace eKnowledgebase.Services
{
    public class eKnowledgebaseService
    {
        private IConfiguration config;
        private HttpClient http;
         
        public eKnowledgebaseService(IConfiguration config, HttpClient http)
        {
            this.config = config;
            this.http = http;

        }
        public async Task<eKnowledgebaseModel> GetByID(Guid id)
        {
            string url = $"{config.GetValue<string>("api_url")}eKnowledgeBase({id})";
            var data = await http.GetFromJsonAsync<eKnowledgebaseModel>(url);
            if (data == null)
            {
                return new();
            }
            return data;

        }

        public async Task<List<eKnowledgebaseModel>> GetModulesAsync()

           
        {
            string url = $"{config.GetValue<string>("api_url")}eKnowledgeBase?$filter=parent_id eq null and is_deleted eq false";
            var data  = await http.GetFromJsonAsync<OdataResponse>(url);
           if (data == null)
            {
                return new();
            }
            return data.value;

        }
        public async Task<List<eKnowledgebaseModel>> GetDataByParentID(Guid? parent_id)
        {
            string url = $"{config.GetValue<string>("api_url")}eKnowledgeBase?$filter=parent_id eq {parent_id}";
            var data = await http.GetFromJsonAsync<OdataResponse>(url);
            if (data == null)
            {
                return new();
            }
            return data.value;

        }
    }
    public class OdataResponse
    {
        [JsonPropertyName("@odata.context")]
        public string OdataContext { get; set; }
        public List<eKnowledgebaseModel> value { get; set; }
    }


}
