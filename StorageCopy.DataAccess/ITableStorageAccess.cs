using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace StorageCopy.DataAccess
{
    public interface ITableStorageAccess
    {
        bool IsValidStorageAccount();
        CloudStorageAccount GetStorageAccount();
        Task<List<string>> GetTablesNames(string prefix, int count);
    }
}
