using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace eModels
{
    public class GetOdataResponse
    {

        public GetOdataResponse() { }
        public GetOdataResponse(bool status)
        {
            IsSuccess = status;
        }
        public GetOdataResponse(bool status, int count, object content)
        {
            Count = count;
            IsSuccess = status;
            Content = content;
        }

        [JsonPropertyName("value")]
        public object Content { get; set; }
        [JsonPropertyName("@odata.count")] public int Count { get; set; } = 0;
        public bool IsSuccess { get; set; } = true;

    }

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

    public class PostReponse
    {

        public PostReponse() { }
        public PostReponse(bool status)
        {
            IsSuccess = status;
        }

        public PostReponse(bool status, string _content, int _status_code = 200)
        {
            IsSuccess = status;
            Content = _content;
            status_code = _status_code;
        }

        public bool IsSuccess { get; set; } = true;
        public string Content { get; set; } = "";
        public string value { get; set; } = "";
        public int status_code { get; set; }

    }
}
