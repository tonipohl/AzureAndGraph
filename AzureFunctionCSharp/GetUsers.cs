using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq;
using Microsoft.Graph.Models;
using Microsoft.Graph.Drives.Item.Items.Item.Workbook.Functions.Now;
using System.Collections.Generic;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace GraphSample
{
    public static class GetUsers
    {
        [FunctionName("GetUsers")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [DurableClient] IDurableClient client,
            ILogger log)
        {
            string msg = $"{System.DateTime.Now} - Start";
            Console.WriteLine(msg);
            log.LogInformation(msg);


            // v1: Get all users
            var users = await Graph.GraphClient.GetUsersAsync();
            foreach (var user in users)
            {
                Console.WriteLine($"User: {user.UserPrincipalName}, {user.DisplayName}");
            }
            msg = $"Total: {users.Count}";


            // v2: Wait for the result
            // https://learn.microsoft.com/en-us/azure/azure-functions/durable/durable-functions-http-features?tabs=csharp
            var start = await client.StartNewAsync("FanOutFanIn", null);
            start = await client.StartNewAsync("FanOutFanIn", null);
            start = await client.StartNewAsync("FanOutFanIn", null);
            msg = "Waited for the completion of FanOutFanIn.";


            // v3: Just start and don´t wait
            for (int i = 0; i < 10; i++)
            {
                client.StartNewAsync("FanOutFanIn", null);
            }
            msg = "Did execute FanOutFanIn multiple times.";


            msg = $"{System.DateTime.Now} - End - {msg}";
            Console.WriteLine(msg);
            log.LogInformation(msg);
            return new OkObjectResult(msg);
        }

        [FunctionName("FanOutFanIn")]
        public static async Task RunFanOutFanIn([OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            Console.WriteLine("FanOut");
            // Do something time costly...
        }

    }
}
