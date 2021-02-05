using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using NETCore.Encrypt;
 
using System.Linq;
using eAPIClient.Models;
using System.Text.Json;
namespace eAPIClient.Services
{
    public interface IHttpService
    {
        Task<GetOdataResponse> ApiGetOData(string url);
        Task<GetResponse> ApiGet(string url);
        Task<PostReponse> ApiPost(string url, object obj = null);

        

    }
    public class HttpService : IHttpService
    {

        private HttpClient http;
 

        private IConfiguration _configuration;
       
        public HttpService(
            HttpClient httpClient,
            IConfiguration configuration
        )
        {
            http = httpClient;
         
            _configuration = configuration;
        }

       

        public async Task<GetResponse> ApiGet(string url)
        {
            HttpStatusCode StatusCode = new HttpStatusCode();       
            http.DefaultRequestHeaders.Add("ContentType", "application/json"); 
            string val = _configuration.GetValue<string>("autherize_key");
            http.DefaultRequestHeaders.Remove("Authorization");
            http.DefaultRequestHeaders.Add("Authorization", "Basic " + val);


            var resp = await http.GetAsync($"{_configuration.GetValue<string>("server_api_url")}{url}");
           
            StatusCode = resp.StatusCode;
            if (resp.IsSuccessStatusCode)
            {
                var jsonString = await resp.Content.ReadAsStringAsync();
                return new GetResponse(true, jsonString);
            }
            return new GetResponse(false, StatusCode);

        }

        public async Task<GetOdataResponse> ApiGetOData(string url)
        {     
            http.DefaultRequestHeaders.Add("ContentType", "application/json");
            string val = _configuration.GetValue<string>("autherize_key");
            http.DefaultRequestHeaders.Remove("Authorization");
            http.DefaultRequestHeaders.Add("Authorization", "Basic " + val);


            var resp = await http.GetAsync($"{_configuration.GetValue<string>("server_api_url")}{url}");
                if (resp.IsSuccessStatusCode)
                {
                    var jsonString = await resp.Content.ReadAsStringAsync();
                    var con = JsonSerializer.Deserialize<GetOdataResponse>(jsonString);
                    return new GetOdataResponse(true, con.Count, con.Content);
                }
         
            return new GetOdataResponse(false);
        }

        public async Task<PostReponse> ApiPost(string url, object obj = null)
        {
            HttpStatusCode StatusCode = new HttpStatusCode();      
            http.DefaultRequestHeaders.Add("ContentType", "application/json"); 
            string val = _configuration.GetValue<string>("autherize_key");    
            http.DefaultRequestHeaders.Remove("Authorization");
            http.DefaultRequestHeaders.Add("Authorization", "Basic " + val);

            HttpRequestMessage requestMessage = new HttpRequestMessage();
            if (obj != null)
            {     
                requestMessage = new HttpRequestMessage()
                {
                    Method = new HttpMethod("POST"),
                    RequestUri = new Uri($"{_configuration.GetValue<string>("server_api_url")}{url}"),
                    Content = new StringContent(JsonSerializer.Serialize(obj))
                };
            }
            else
            {
                requestMessage = new HttpRequestMessage()
                {
                    Method = new HttpMethod("POST"),
                    RequestUri = new Uri($"{_configuration.GetValue<string>("server_api_url")}{url}"),
                };
            }
            if (requestMessage.Content != null)
            {
                requestMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            }
            var response = await http.SendAsync(requestMessage);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();  
                return new PostReponse(true, responseBody);
            }
            else
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                PostReponse con = new PostReponse();
                StatusCode = response.StatusCode;
                if (responseBody.Contains("@odata.context"))
                {
                    con = JsonSerializer.Deserialize<PostReponse>(responseBody);
                    return new PostReponse() { IsSuccess = false, Content = con.value, status_code = Convert.ToInt32((HttpStatusCode)StatusCode) };
                }
                else
                {
                    return new PostReponse(false, responseBody, Convert.ToInt32((HttpStatusCode)StatusCode));
                }
            }
        }
    }

}
