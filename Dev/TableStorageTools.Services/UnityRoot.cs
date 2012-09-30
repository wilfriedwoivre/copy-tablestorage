using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using TableStorageTools.Services.Implementations;
using TableStorageTools.Services.Interfaces;

namespace TableStorageTools.Services
{
    public static class UnityRoot
    {
        public static void Configure(IUnityContainer container)
        {
            container.RegisterType<ITableStorageService, TableStorageService>(new InjectionConstructor(""));
            container.RegisterType<IExportTableStorageService, ExportTableStorageService>();
            container.RegisterType<IGenericServiceContext, GenericServiceContext>();
        }
    }
}
