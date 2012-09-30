using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using TableStorageTools.Services.Interfaces;

namespace TableStorageTools.Services.Implementations
{
    public class ExportTableStorageService : IExportTableStorageService
    {
        private IUnityContainer _container;

        private CloudStorageAccount _sourceStorageAccount;
        private CloudStorageAccount _destinationStorageAccount;

        private int _step = 5;

        public ExportTableStorageService(IUnityContainer container)
        {
            _container = container;
        }

        public async Task<bool> Copy(string source, string destination, List<string> tables)
        {
            _sourceStorageAccount = _container.Resolve<ITableStorageService>(new ParameterOverride("storageAccount", source)).GetStorageAccount();
            _destinationStorageAccount = _container.Resolve<ITableStorageService>(new ParameterOverride("storageAccount", destination)).GetStorageAccount();

            if (_sourceStorageAccount == null || _destinationStorageAccount == null || !tables.Any())
                return false;
            await Task.Run(() => DeleteTableIfExists(tables));
            await Task.Run(() => CreateTablesIfNotExists(tables));
            foreach (var table in tables)
            {
                string currentTable = table;
                await Task.Run(() => CopyTable(currentTable));
            }

            return false;
        }

        private void DeleteTableIfExists(List<string> tables)
        {
            var tableClient = _destinationStorageAccount.CreateCloudTableClient();

            foreach (var table in tables)
            {
                tableClient.DeleteTableIfExist(table);
            }
        }

        private void CreateTablesIfNotExists(List<string> tables)
        {
            var tableClient = _destinationStorageAccount.CreateCloudTableClient();

            foreach (var table in tables)
            {
                tableClient.CreateTableIfNotExist(table);
            }
        }

        private void CopyTable(string table)
        {
            var entities =
                _container.Resolve<IGenericServiceContext>(new ParameterOverride("storageAccount", _sourceStorageAccount))
                    .GetEntities(table);

            _container.Resolve<IGenericServiceContext>(new ParameterOverride("storageAccount", _destinationStorageAccount))
                    .SaveEntities(table, entities, 1);
        }
    }
}
