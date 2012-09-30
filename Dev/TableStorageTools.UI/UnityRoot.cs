using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;

namespace TableStorageTools.UI
{
    public static class UnityRoot
    {
        private static readonly IUnityContainer _container = new UnityContainer();

        public static IUnityContainer Container { get { return _container; } }

        static UnityRoot()
        {
            Configure();
        }

        public static void EnsureInitialized()
        {
        }

        private static void Configure()
        {
            TableStorageTools.Services.UnityRoot.Configure(_container);
        }
    }
}
