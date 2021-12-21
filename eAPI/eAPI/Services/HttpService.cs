using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using NETCore.Encrypt;
 
using System.Linq;
using eModels;
using System.Text.Json;
using Microsoft.AspNetCore.Components;

namespace eAPI.Services
{
    public interface IHttpService
    {


        Task<GetResponse> eSoftixApiGet(string url);
        Task<PostReponse> eSoftixApiPost(string url, object obj = null);
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
 

        public async Task<GetResponse> eSoftixApiGet(string url)
        {
            HttpStatusCode StatusCode = new HttpStatusCode();

            http.DefaultRequestHeaders.Add("ContentType", "application/json");
           
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes($"esoftix:MN[D8beAt:AWeW5");
            //var plainTextBytes = System.Text.Encoding.UTF8.GetBytes($"esoftix:123456");
            string val = System.Convert.ToBase64String(plainTextBytes);
            http.DefaultRequestHeaders.Remove("Authorization");
            http.DefaultRequestHeaders.Add("Authorization", "Basic " + val);

            try
            {
                url = $"{_configuration.GetValue<string>("apieSoftixUrl")}{url}";
                var resp = await http.GetAsync(url);
                StatusCode = resp.StatusCode;
                if (resp.IsSuccessStatusCode)
                {
                    var jsonString = await resp.Content.ReadAsStringAsync();
                    return new GetResponse(true, jsonString);
                }
            }
            catch(Exception ex)
            {

                return new GetResponse(false, null, 503);
            }
            return new GetResponse(false, StatusCode);
        }

        public async Task<PostReponse> eSoftixApiPost(string url, object obj = null)
        {
            HttpStatusCode StatusCode = new HttpStatusCode();
            string base_url = _configuration.GetValue<string>("apieSoftixUrl");


            http.DefaultRequestHeaders.Add("ContentType", "application/json");
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes($"esoftix:MN[D8beAt:AWeW5");
            string val = System.Convert.ToBase64String(plainTextBytes);

            http.DefaultRequestHeaders.Remove("Authorization");
            http.DefaultRequestHeaders.Add("Authorization", "Basic " + val);

            HttpRequestMessage requestMessage = new HttpRequestMessage();
            if (obj != null)
            {

                requestMessage = new HttpRequestMessage()
                {
                    Method = new HttpMethod("POST"),
                    RequestUri = new Uri($"{_configuration.GetValue<string>("apieSoftixUrl")}{url}"),
                    Content = new StringContent(JsonSerializer.Serialize(obj))
                };
            }
            else
            {
                requestMessage = new HttpRequestMessage()
                {
                    Method = new HttpMethod("POST"),
                    RequestUri = new Uri($"{_configuration.GetValue<string>("apieSoftixUrl")}{url}"),
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
