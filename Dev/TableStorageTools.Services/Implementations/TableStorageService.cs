using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using TableStorageTools.Services.Interfaces;

namespace TableStorageTools.Services.Implementations
{
    public class TableStorageService : ITableStorageService
    {
        private string _storageAccount { get; set; }

        public TableStorageService(string storageAccount)
        {
            this._storageAccount = storageAccount;
        }

        public bool IsValidStorageAccount()
        {
            CloudStorageAccount storage;
            return !string.IsNullOrWhiteSpace(_storageAccount) && CloudStorageAccount.TryParse(_storageAccount, out storage);
        }

        public CloudStorageAccount GetStorageAccount()
        {
            return IsValidStorageAccount() ? CloudStorageAccount.Parse(_storageAccount) : null;
        }

        public async Task<List<string>> GetTablesNames(string prefix, int count)
        {
            if (IsValidStorageAccount())
            {
                List<string> result = new List<string>();
                await Task.Run(() =>
                {
                    CloudStorageAccount csa = CloudStorageAccount.Parse(_storageAccount);
                    var tableClient = csa.CreateCloudTableClient();

                    ResultSegment<string> segmentedResult = null;
                    segmentedResult = string.IsNullOrWhiteSpace(prefix) ? tableClient.ListTablesSegmented(count, null) : tableClient.ListTablesSegmented(prefix, count, null);

                    result.AddRange(segmentedResult.Results);

                    while (segmentedResult.ContinuationToken != null)
                    {
                        segmentedResult = segmentedResult.GetNext();
                        result.AddRange(segmentedResult.Results);
                    }
                });
                return result;
            }
            else
            {
                return null;
            }
        }
    }
}
