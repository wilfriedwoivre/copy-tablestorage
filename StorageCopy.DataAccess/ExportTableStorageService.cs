using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace StorageCopy.DataAccess
{
    public class ExportTableStorageService : IExportTableStorageService
    {
        private CloudStorageAccount sourceStorageAccount = null;
        private CloudStorageAccount destinationStorageAccount = null;
        private const int SegmentedCount = 100;
        public async Task<bool> Copy(string source, string destination, List<string> tables)
        {
            sourceStorageAccount = new TableStorageService(source).GetStorageAccount();
            destinationStorageAccount = new TableStorageService(destination).GetStorageAccount();

            if (sourceStorageAccount == null || destinationStorageAccount == null || !tables.Any())
                return false;

            //Task.Run(() => DeleteTableIfExists(tables)).Wait();
            Task.Run(() => CreateTablesIfNotExists(tables)).Wait();

            foreach (var table in tables)
            {
                string currentTable = table;
                await Task.Run(async () => await CopyTable(currentTable));
            }

            return false;
        }

        private void DeleteTableIfExists(List<string> tables)
        {
            var tableClient = destinationStorageAccount.CreateCloudTableClient();

            foreach (var table in tables)
            {
                var currentTable = tableClient.GetTableReference(table);
                currentTable.DeleteIfExists();
            }
        }

        private void CreateTablesIfNotExists(List<string> tables)
        {
            var tableClient = destinationStorageAccount.CreateCloudTableClient();

            foreach (var table in tables)
            {
                var currentTable = tableClient.GetTableReference(table);
                currentTable.CreateIfNotExists();
            }
        }

        private async Task CopyTable(string table)
        {
            var sourceTable = GetTable(sourceStorageAccount, table);
            var destTable = GetTable(destinationStorageAccount, table);

            var query = new TableQuery { TakeCount = SegmentedCount };
            var continuationToken = new TableContinuationToken();

            do
            {
                var result = await sourceTable.ExecuteQuerySegmentedAsync(query, continuationToken);
                continuationToken = result.ContinuationToken;

                foreach (var entity in result.Results)
                {
                    await destTable.ExecuteAsync(TableOperation.InsertOrReplace(entity));
                }

            } while (continuationToken != null);


        }

        private CloudTable GetTable(CloudStorageAccount storageAccount, string tableName)
        {
            var tableClient = storageAccount.CreateCloudTableClient();
            return tableClient.GetTableReference(tableName);
        }
    }
}