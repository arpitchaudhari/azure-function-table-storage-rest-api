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
    public static class GetAllFunction
    {
        [FunctionName("GetAllFunction")]
        public static async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req, 
            IBinder binder,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            
            var table = await binder.BindAsync<CloudTable>(new TableAttribute($"tablecurd")
            {
                Connection = "TableConnection"
            });

            try
            {
                TableContinuationToken token = null;
                var entities = new List<LoggerEventModel>();
                do
                {
                    var queryResult = table.ExecuteQuerySegmented(new TableQuery<LoggerEventModel>(), token);
                    entities.AddRange(queryResult.Results);
                    token = queryResult.ContinuationToken;
                } while (token != null);


                return new OkObjectResult(entities);
            } catch (StorageException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                throw;
            }
        }
    }
}