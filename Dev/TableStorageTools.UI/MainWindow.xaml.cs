using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TableStorageTools.ViewModels;
using TableStorageTools.Tools.Extensions;

namespace TableStorageTools.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += (sender, e) => this.EnsureViewModel<MainViewModel>().OnLoad();
            this.Unloaded += (sender, e) => this.EnsureViewModel<MainViewModel>().OnUnload();
        }
    }
}
