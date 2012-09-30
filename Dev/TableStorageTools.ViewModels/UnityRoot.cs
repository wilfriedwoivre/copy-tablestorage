using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using TableStorageTools.Tools.MVVM;

namespace TableStorageTools.ViewModels
{
    public static class UnityRoot
    {
        public static void Configure(IUnityContainer container)
        {
            container.RegisterType<ViewModelBase, MainViewModel>();
        }
    }
}
