using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;

namespace RestApiHelper
{
    public class RestClient
    {
        private static HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:3030/")
        };

        public RestClient()
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }
        
        public static async Task<HttpResponseMessage> GetAllItems(string path, bool checkResponse = true)
        {

            var response = await client.GetAsync(path);
            if (checkResponse) 
                CheckResponse(response); 
            return response;
        }

        public static async Task<HttpResponseMessage> GetItem(string path, string id, bool checkResponse = true)
        {
            var response = await client.GetAsync($"{path}/{id}");
            if (checkResponse) 
                CheckResponse(response); 
            return response;
        }
        
        public static async Task<HttpResponseMessage> CreateItem<T>(string path, T content, bool checkResponse = true)
        {
            var response = await client.PostAsJsonAsync(path, content);
            if (checkResponse) 
                CheckResponse(response);
            return response;
        }
        
        public static async Task<HttpResponseMessage>  DeleteItem(string path, string id, bool checkResponse = true)
        {
            var response = await client.DeleteAsync($"{path}/{id}");
            if (checkResponse) 
                CheckResponse(response);
            return response;
        }
        
        public static async Task<HttpResponseMessage>  UpdateItem<T>(string path, string id, T content, bool checkResponse = true)
        {
            var stringContent = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
            var response = await PatchAsync($"{path}/{id}", stringContent);
            if (checkResponse) 
                CheckResponse(response);
            return response;
        }

        private static Task<HttpResponseMessage> PatchAsync(string requestUri, HttpContent content)
        {
            var request = new HttpRequestMessage
            {
                Method = new HttpMethod("PATCH"),
                RequestUri = new Uri(client.BaseAddress + requestUri),
                Content = content,
            };

            return client.SendAsync(request);
        }

        private static void CheckResponse(HttpResponseMessage message)
        {
            Assert.IsTrue(message.IsSuccessStatusCode, "Error with status code: " + message.StatusCode);
        }
    }
}