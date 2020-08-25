# azure-function-table-storage-rest-api

Create `local.setting.json` file
```
{
    "IsEncrypted": false,
    "Values": {
        "AzureWebJobsStorage": "UseDevelopmentStorage=true",
        "FUNCTIONS_WORKER_RUNTIME": "dotnet",
        "TableConnection": "Your Connection String"
    }
}
```
