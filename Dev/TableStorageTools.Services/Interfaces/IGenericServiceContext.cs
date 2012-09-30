using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableStorageTools.Model;

namespace TableStorageTools.Services.Interfaces
{
    public interface IGenericServiceContext
    {
        List<AzureGenericEntity> GetEntities(string tableName);
        void SaveEntities(string tableName, List<AzureGenericEntity> entities, int count);
    }
}
