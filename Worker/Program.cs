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
            var rand = new Random();
            var timeOut = rand.Next(5,8);

            Console.WriteLine("Worker Started");
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddHttpClient();
            var services = serviceCollection.BuildServiceProvider();
            var httpClientFactory = services.GetRequiredService<IHttpClientFactory>();
            var client = httpClientFactory.CreateClient();

            Console.WriteLine("Sleeping...");
            Thread.Sleep(TimeSpan.FromSeconds(timeOut));


            var work = await client.GetStringAsync("http://server/api/values/getwork");
            var listOfWork = JsonConvert.DeserializeObject<List<WorkItem>>(work);

            Console.WriteLine("Recieved Work:");
            foreach(var w in listOfWork)
            {
                Console.WriteLine(" "+w.Key);
                Console.WriteLine(" " + w.Value);

            }

            
            
            var content = new StringContent(JsonConvert.SerializeObject(listOfWork), Encoding.UTF8, "application/json");
            HttpResponseMessage response3 = new HttpResponseMessage(System.Net.HttpStatusCode.NoContent);
            while(response3.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                response3 = await client.PostAsync("http://server/api/values/post", content);
                if (response3.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    Console.WriteLine("BATCH FAILED trying again in 10 second");
                }
                Thread.Sleep(timeOut* 200);
            }
            Console.WriteLine("BATCH SUCCESSFUL");
            var stringResponse = await response3.Content.ReadAsStringAsync();

            var responseMessage = JsonConvert.DeserializeObject<List<WorkItem>>(stringResponse);
            foreach(var res in responseMessage)
            {
                Console.WriteLine("Result from server: " + res.Result);
            }
            
        }
    }
}
