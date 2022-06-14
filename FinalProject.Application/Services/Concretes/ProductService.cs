using FinalProject.Application.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Application.Services.Concretes
{
    public class ProductService<T> : IProductService<T> where T : class
    {
        public async Task<string> Consume(T request, string methodAdress, string token)
        {
            string BaseUrl = "http://service.stage.paximum.com/v2";
            HttpClient client = new HttpClient();
            // Update port # in the following line.
            client.BaseAddress = new Uri(BaseUrl + methodAdress);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            if (token != "")
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var loginJson = JsonConvert.SerializeObject(request);
            var buffer = Encoding.UTF8.GetBytes(loginJson);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var httpResponse = await client.PostAsync(client.BaseAddress, byteContent);

            string response = await httpResponse.Content.ReadAsStringAsync();

            return response;
        }
    }
}
