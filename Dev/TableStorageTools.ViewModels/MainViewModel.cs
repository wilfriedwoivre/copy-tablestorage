using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Expression.Interactivity.Core;
using TableStorageTools.Model;
using TableStorageTools.Services.Interfaces;
using TableStorageTools.Tools.MVVM;
using Microsoft.Practices.Unity;

namespace TableStorageTools.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        #region Properties
        private string _sourceTableStorage;
        public string SourceTableStorage
        {
            get
            {
                return _sourceTableStorage;
            }
            set
            {
                _sourceTableStorage = value;
                RaisePropertyChanged("SourceTableStorage");
            }
        }

        private bool _sourceIsDevelopmentStorage;
        public bool SourceIsDevelopmentStorage
        {
            get
            {
                return _sourceIsDevelopmentStorage;
            }
            set
            {
                _sourceIsDevelopmentStorage = value;
                SetDevelopmentStorage((entry) => { SourceTableStorage = entry; }, value);
                RaisePropertyChanged("SourceIsDevelopmentStorage");
            }
        }

        private readonly ObservableCollection<SelectedItem> _sourceTables = new ObservableCollection<SelectedItem>();
        public ObservableCollection<SelectedItem> SourceTables { get { return _sourceTables; } }

        private bool _destinationIsDevelopmentStorage;
        public bool DestinationIsDevelopmentStorage
        {
            get { return _destinationIsDevelopmentStorage; }
            set
            {
                _destinationIsDevelopmentStorage = value;
                SetDevelopmentStorage((entry) => { DestinationTableStorage = entry; }, value);
                RaisePropertyChanged("DestinationIsDevelopmentStorage");
            }
        }

        private string _destinationTableStorage;
        public string DestinationTableStorage
        {
            get { return _destinationTableStorage; }
            set
            {
                _destinationTableStorage = value;
                RaisePropertyChanged("DestinationTableStorage");
            }
        }

        private readonly ObservableCollection<string> _destinationTables = new ObservableCollection<string>();
        private bool _isBusy;
        public ObservableCollection<string> DestinationTables { get { return _destinationTables; } }

        public ICommand LoadTablesCommand { get; private set; }
        public ICommand ExportTablesCommand { get; private set; }

        public bool IsBusy
        {
            get { return _isBusy; }
            set { _isBusy = value; RaisePropertyChanged("IsBusy"); }
        }
        #endregion

        private IUnityContainer _container;

        public MainViewModel(IUnityContainer container)
        {
            _container = container;
            LoadTablesCommand = new ActionCommand(LoadTables);
            ExportTablesCommand = new ActionCommand(ExportTables);
        }

        public override void OnLoad()
        {
        }

        public override void OnUnload()
        {
        }

        private void LoadTables()
        {
            IsBusy = true;
            bool sourceIsFinish = false;
            bool destinationIsFinish = false;
            LoadTables(SourceTableStorage, (items) =>
                                               {
                                                   sourceIsFinish = true;
                                                   IsBusy = !(sourceIsFinish && destinationIsFinish);
                                                   foreach (var item in items)
                                                       SourceTables.Add(new SelectedItem { Value = item, IsSelected = false });
                                               });
            LoadTables(DestinationTableStorage, (items) =>
                                                    {
                                                        destinationIsFinish = true;
                                                        IsBusy = !(sourceIsFinish && destinationIsFinish);
                                                        foreach (var item in items)
                                                            DestinationTables.Add(item);
                                                    });
        }

        private async void ExportTables()
        {
            if (string.IsNullOrEmpty(SourceTableStorage) ||string.IsNullOrEmpty(DestinationTableStorage))
            {
                MessageBox.Show("Define tables storage before execute this action");
                return;
            }
            if (!SourceTables.Any(n => n.IsSelected))
            {
                MessageBox.Show("Please select tables that you would copy.");
                return;
            }

            IsBusy = true;
            await _container.Resolve<IExportTableStorageService>().Copy(SourceTableStorage, DestinationTableStorage,
                                                                             SourceTables.Where(n => n.IsSelected).Select(n => n.Value).ToList());

            IsBusy = false;
        }

        private void SetDevelopmentStorage(Action<string> action, bool isActive)
        {

            if (action != null)
            {
                string value = isActive ? "UseDevelopmentStorage=true" : string.Empty;
                action(value);
            }
        }

        private async void LoadTables(string cloudStorageAccount, Action<List<string>> addItem)
        {
            try
            {
                List<string> result =
                    await
                    _container.Resolve<ITableStorageService>(new ParameterOverride("storageAccount", cloudStorageAccount))
                        .GetTablesNames(null, 10);

                if (addItem != null)
                {
                    addItem(result);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(
                    "Your storage account must be formated like this : 'DefaultEndpointsProtocol=[https|http];AccountName={AccountStorageName};AccountKey={AccountStorageKey}'");

                addItem(new List<string>());
            }
        }
    }
}
