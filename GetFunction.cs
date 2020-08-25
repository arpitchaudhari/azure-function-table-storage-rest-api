using System;
using System.Collections.Generic;
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
    public static class GetFunction
    {
        [FunctionName("GetFunction")]
        public static async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req, 
            IBinder binder,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            
            try
            {
                string partitionKey = req.Query["partitionKey"];
                string rowKey = req.Query["rowKey"];

                var table = await binder.BindAsync<LoggerEventModel>(new TableAttribute($"tablecurd",$"{partitionKey}",$"{rowKey}")
                {
                    Connection = "TableConnection"
                });
                return new OkObjectResult(table);
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