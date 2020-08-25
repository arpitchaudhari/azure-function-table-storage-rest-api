using Microsoft.Azure.Cosmos.Table;

namespace TableStorageREST_API
{
    public class LoggerEventModel : TableEntity
    {
        public string originalName { get; set; }

    }
}