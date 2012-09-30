using Microsoft.WindowsAzure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableStorageTools.Services.Interfaces
{
    public interface ITableStorageService
    {
        bool IsValidStorageAccount();
        CloudStorageAccount GetStorageAccount();
        Task<List<string>> GetTablesNames(string prefix, int count);
    }
}
