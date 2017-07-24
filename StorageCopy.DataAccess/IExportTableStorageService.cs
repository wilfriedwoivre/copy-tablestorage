using System.Collections.Generic;
using System.Threading.Tasks;

namespace StorageCopy.DataAccess
{
    public interface IExportTableStorageService
    {
        Task<bool> Copy(string source, string destination, List<string> tables);
    }
}