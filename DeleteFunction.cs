using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace TableStorageREST_API
{
    public static class DeleteFunction
    {
        [FunctionName("DeleteFunction")]
        public static async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = null)] HttpRequest req, 
            IBinder binder,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            try
            {
                string PrimaryKey = req.Query["PrimaryKey"];
                string RowKey = req.Query["RowKey"];

                var table = await binder.BindAsync<CloudTable>(new TableAttribute($"tablecurd")
                {
                    Connection = "TableConnection"
                });

                var item = new TableEntity(PrimaryKey, RowKey)
                {
                    ETag = "*"
                };

                var operation = TableOperation.Delete(item);
                await table.ExecuteAsync(operation);
                return new OkObjectResult("deleted");
            }
            catch (StorageException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                throw;
            }
        }
    }
}