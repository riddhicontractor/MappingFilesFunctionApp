using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using MappingFilesFunctionApp.Model;
using Microsoft.Azure.Cosmos;
using MappingFilesFunctionApp.Method;
using System.Net;

namespace MappingFilesFunctionApp
{
    public static class Function1
    {
        private static string connectionstring = "AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";

        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, nameof(HttpMethods.Post), Route = null)] HttpRequest req)
        {
            ObjectResult result;
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            dynamic data = JsonConvert.DeserializeObject<Product>(requestBody);

            var pro = new Product
            {
                Id = data.Id,
                PoNo = data.PoNo,
                SupplierName = data.SupplierName,
                Date = data.Date,
                OrderValue = data.OrderValue,
                Status = data.Status
            };

            using FileStream filestream = File.Create(@"D:\" + pro.Id + ".json");
            await System.Text.Json.JsonSerializer.SerializeAsync(filestream, pro);

            CosmosMethod m = new CosmosMethod();

            Demo.RedirectAssembly();

            m.cosmosClient = new CosmosClient(connectionstring);

           // m.cosmosClient = new CosmosClient("https://localhost:8081/", "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==", new CosmosClientOptions() { AllowBulkExecution = true, ConnectionMode = ConnectionMode.Gateway });
            
            await m.GetStartedDemoAsync();

            await AddItemsToContainerAsync();

            async Task AddItemsToContainerAsync()
            {
                try
                {
                    // Read the item to see if it exists.  
                    ItemResponse<Product> dataResponse = await m.container.ReadItemAsync<Product>(data.Id, new PartitionKey(data.PoNo));
                    Console.WriteLine("Item in database with entitytype: {0} already exists\n", dataResponse.Resource.Id);
                }
                catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    // Create an item in the container representing the Log Data. Note we provide the value of the partition key for this item, which is "EntityType"
                    ItemResponse<Product> dataResponse = await m.container.CreateItemAsync<Product>(data, new PartitionKey(data.PoNo));

                    // Note that after creating the item, we can access the body of the item with the Resource property off the ItemResponse. We can also access the RequestCharge property to see the amount of RUs(request unit) consumed on this request.
                    Console.WriteLine("Created item in database with entitytype: {0} Operation consumed {1} RUs.\n", dataResponse.Resource.Id, dataResponse.RequestCharge);
                }
            }

            string messageBody = JsonConvert.SerializeObject(data);
            result = new OkObjectResult(messageBody);
            Console.WriteLine($"Message: {messageBody}");
            return result;
        }
    }
}
