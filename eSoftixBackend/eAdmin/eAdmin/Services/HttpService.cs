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
namespace eAdmin.Services
{
    public interface IHttpService
    {
        Task<GetOdataResponse> ApiGetOData(string url);
        Task<GetResponse> ApiGet(string url);
        Task<PostReponse> ApiPost(string url, object obj = null);

        string ImageUrl(string image_path);

    }
    public class HttpService : IHttpService
    {

        private HttpClient http;
        private ILocalStorageService _localStorageService;

        private IConfiguration _configuration;
       
        public HttpService(
            HttpClient httpClient,
            ILocalStorageService localStorageService,
            IConfiguration configuration
        )
        {
            http = httpClient;
            _localStorageService = localStorageService;
            _configuration = configuration;
        }

        public string ImageUrl(string image_path)
        {
            image_path = $"{(string.IsNullOrEmpty(image_path) ? "placeholder.png" : image_path)}";
            return $"{_configuration.GetValue<string>("BaseUrl")}upload/{image_path}";
        }

        public async Task<GetResponse> ApiGet(string url)
        {
            HttpStatusCode StatusCode = new HttpStatusCode();
            string user = "";
            try
            {
                user = await _localStorageService.GetItemAsync<string>("_Authorization");
            }
            catch (Exception ex)
            {
                string msg = ex.Message;

            }

            UserModel current_login_user = new UserModel();
            if (!string.IsNullOrEmpty(user))
            {
                current_login_user =  JsonSerializer.Deserialize<UserModel>(EncryptProvider.Base64Decrypt(user));
            }

            if (current_login_user.id != 0 && current_login_user != null)
            {
                string pass = EncryptProvider.Base64Decrypt(current_login_user?.password);
                http.DefaultRequestHeaders.Add("ContentType", "application/json");
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes($"{current_login_user?.username}:{pass}");
                string val = System.Convert.ToBase64String(plainTextBytes);
                http.DefaultRequestHeaders.Remove("Authorization");
                http.DefaultRequestHeaders.Add("Authorization", "Basic " + val);
            }

            var resp = await http.GetAsync($"{_configuration.GetValue<string>("apiBaseUrl")}{url}");
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

            string user = await _localStorageService.GetItemAsync<string>("_Authorization");
            UserModel current_login_user = new UserModel();
            if (!string.IsNullOrEmpty(user))
            {
                current_login_user = JsonSerializer.Deserialize<UserModel>(EncryptProvider.Base64Decrypt(user));
            }

            if (current_login_user.id != 0 && current_login_user != null)
            {

                string pass = EncryptProvider.Base64Decrypt(current_login_user?.password);
                http.DefaultRequestHeaders.Add("ContentType", "application/json");
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes($"{current_login_user?.username}:{pass}");
                string val = System.Convert.ToBase64String(plainTextBytes);
                http.DefaultRequestHeaders.Remove("Authorization");
                http.DefaultRequestHeaders.Add("Authorization", "Basic " + val);
               
            }

          
                var resp = await http.GetAsync($"{_configuration.GetValue<string>("apiBaseUrl")}{url}");
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
            string base_url = _configuration.GetValue<string>("apiBaseUrl");
            string user = await _localStorageService.GetItemAsync<string>("_Authorization");
            UserModel current_login_user = new UserModel();
            if (!string.IsNullOrEmpty(user))
            {
                current_login_user = JsonSerializer.Deserialize<UserModel>(EncryptProvider.Base64Decrypt(user));
            }

            if (current_login_user.id != 0 && current_login_user != null)
            {



                string pass = EncryptProvider.Base64Decrypt(current_login_user?.password);
                http.DefaultRequestHeaders.Add("ContentType", "application/json");
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes($"{current_login_user?.username}:{pass}");
                string val = System.Convert.ToBase64String(plainTextBytes);
                
                    http.DefaultRequestHeaders.Remove("Authorization");
                    http.DefaultRequestHeaders.Add("Authorization", "Basic " + val);
                   
              
             
            }

            HttpRequestMessage requestMessage = new HttpRequestMessage();
            if (obj != null)
            {
                
                requestMessage = new HttpRequestMessage()
                {
                    Method = new HttpMethod("POST"),
                    RequestUri = new Uri($"{_configuration.GetValue<string>("apiBaseUrl")}{url}"),
                    Content = new StringContent(JsonSerializer.Serialize(obj))
                };
            }
            else
            {
                requestMessage = new HttpRequestMessage()
                {
                    Method = new HttpMethod("POST"),
                    RequestUri = new Uri($"{_configuration.GetValue<string>("apiBaseUrl")}{url}"),
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
