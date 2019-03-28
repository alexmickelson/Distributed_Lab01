using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Shared;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
using System.Collections.Generic;

namespace Distributed_Lab01
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Worker Started");
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddHttpClient();
            var services = serviceCollection.BuildServiceProvider();
            var httpClientFactory = services.GetRequiredService<IHttpClientFactory>();
            var client = httpClientFactory.CreateClient();

            Console.WriteLine("Sleeping...");
            Thread.Sleep(TimeSpan.FromSeconds(5));


            var work = await client.GetStringAsync("http://server/api/values/getwork");
            var listOfWork = JsonConvert.DeserializeObject<List<MessageDto>>(work);
            Console.WriteLine(listOfWork[0].Key);
            Console.WriteLine(listOfWork[0].Value);


            var response = await client.GetStringAsync("http://server/api/values/Get");
            Console.WriteLine(response);
            
            var content = new StringContent(JsonConvert.SerializeObject(listOfWork), Encoding.UTF8, "application/json");
            var response3 = await client.PostAsync("http://server/api/values/post", content);
            var stringResponse = await response3.Content.ReadAsStringAsync();

            var responseMessage = JsonConvert.DeserializeObject<List<MessageDto>>(stringResponse);
            foreach(var res in responseMessage)
            {
                Console.WriteLine("Result from server: " + res.Result);
            }
            
        }
    }
}
