using System.Windows;
using StorageCopy.ViewModels;

namespace StorageCopy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ViewModelBase viewModel => new MainViewModel();
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = viewModel;
            this.Loaded += (sender, e) => viewModel.OnLoad();
            this.Unloaded += (sender, e) => viewModel.OnUnload();
        }
    }
}
