using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableStorageTools.Services.Interfaces
{
    public interface IExportTableStorageService
    {
        Task<bool> Copy(string source, string destination, List<string> tables);
    }
}
