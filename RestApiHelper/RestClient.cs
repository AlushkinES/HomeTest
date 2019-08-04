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
        private static readonly HttpClient client = new HttpClient();
        private string controller;

        public RestClient(string url, string item)
        {
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            controller = item;
        }
        
        public async Task<T> GetAllItems<T>(bool checkResponse = true)
        {
            var response = await client.GetAsync(controller);
            if (checkResponse) 
                CheckResponse(response);
            var result = await response.Content.ReadAsAsync<T>();
            return result;
        }

        public async Task<HttpResponseMessage> GetItem(string id, bool checkResponse = true)
        {
            var response = await client.GetAsync($"{controller}/{id}");
            if (checkResponse) 
                CheckResponse(response); 
            return response;
        }
        
        public async Task<HttpResponseMessage> CreateItem<T>(T content, bool checkResponse = true)
        {
            var response = await client.PostAsJsonAsync(controller, content);
            if (checkResponse) 
                CheckResponse(response);
            return response;
        }
        
        public async Task<HttpResponseMessage>  DeleteItem(string id, bool checkResponse = true)
        {
            var response = await client.DeleteAsync($"{controller}/{id}");
            if (checkResponse) 
                CheckResponse(response);
            return response;
        }
        
        public async Task<HttpResponseMessage>  UpdateItem<T>(string id, T content, bool checkResponse = true)
        {
            var stringContent = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
            var response = await PatchAsync($"{controller}/{id}", stringContent);
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

        private void CheckResponse(HttpResponseMessage message)
        {
            Assert.IsTrue(message.IsSuccessStatusCode, "Error with status code: " + message.StatusCode);
        }
    }
}