using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace StorageCopy.DataAccess
{
    public class TableStorageService : ITableStorageAccess
    {
        private string _storageAccount { get; }

        public TableStorageService(string storageAccount)
        {
            _storageAccount = storageAccount;
        }

        public bool IsValidStorageAccount()
        {
            CloudStorageAccount storage;
            return !string.IsNullOrWhiteSpace(_storageAccount) &&
                   CloudStorageAccount.TryParse(_storageAccount, out storage);
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

                    result.AddRange(tableClient.ListTables(prefix).Select(n => n.Name));
                });
                return result;
            }
            return null;
        }


    }
}