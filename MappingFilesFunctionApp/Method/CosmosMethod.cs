using MappingFilesFunctionApp.Model;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MappingFilesFunctionApp.Method
{
    public class CosmosMethod
    {
        public  CosmosClient cosmosClient;

        public  Database database;

        public  Container container;

        private string databaseId = "product_db";

        private string containerId = "product_files";

        public async Task GetStartedDemoAsync()
        {
            // Create a new database
            await this.CreateDatabaseAsync();

            // Create a new container
            await this.CreateContainerAsync();
        }

        private async Task CreateDatabaseAsync()
        {
            this.database = await this.cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
            Console.WriteLine("Created Database: {0}\n", this.database.Id);
        }

        private async Task CreateContainerAsync()
        {
            this.container = await this.database.CreateContainerIfNotExistsAsync(containerId, "/PoNo");
            Console.WriteLine("Created Container: {0}\n", this.container.Id);
        }
    }
}
