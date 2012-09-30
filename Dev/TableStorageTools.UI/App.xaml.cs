using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TableStorageTools.Tools.Extensions;

namespace TableStorageTools.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var window = new MainWindow();
            window.SetUnityContainer(UnityRoot.Container);

            this.MainWindow = window;
            this.MainWindow.Show();

            base.OnStartup(e);
        }
    }
}
