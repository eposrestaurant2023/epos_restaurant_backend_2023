
namespace Reporting
{     
    public class GetResponse
    {

        public GetResponse() { }
        public GetResponse(bool status)
        {
            IsSuccess = status;
        }
        public GetResponse(bool status, object content, int _status_code = 200)
        {
            IsSuccess = status;
            Content = content;
            status_code = _status_code;

        }
        public object Content { get; set; }
        public bool IsSuccess { get; set; } = true;
        public int status_code { get; set; }

    }
    public class ConfigDataModel
    {
        public string id { get; set; }
        public string config_type { get; set; }
        public string data { get; set; }
    }

   
    
}
