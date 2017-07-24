using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Microsoft.Expression.Interactivity.Core;
using StorageCopy.DataAccess;
using StorageCopy.Models;

namespace StorageCopy.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private string _sourceTableStorage;
        public string SourceTableStorage
        {
            get => _sourceTableStorage;
            set
            {
                _sourceTableStorage = value;
                RaisePropertyChanged("SourceTableStorage");
            }
        }

        private bool _sourceIsDevelopmentStorage;
        public bool SourceIsDevelopmentStorage
        {
            get => _sourceIsDevelopmentStorage;
            set
            {
                _sourceIsDevelopmentStorage = value;
                SetDevelopmentStorage((entry) => { SourceTableStorage = entry; }, value);
                RaisePropertyChanged("SourceIsDevelopmentStorage");
            }
        }

        public ObservableCollection<SelectedItem> SourceTables { get; } = new ObservableCollection<SelectedItem>();

        private bool _destinationIsDevelopmentStorage;
        public bool DestinationIsDevelopmentStorage
        {
            get => _destinationIsDevelopmentStorage;
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
            get => _destinationTableStorage;
            set
            {
                _destinationTableStorage = value;
                RaisePropertyChanged("DestinationTableStorage");
            }
        }

        private bool _isBusy;
        public ObservableCollection<string> DestinationTables { get; } = new ObservableCollection<string>();

        public ICommand LoadTablesCommand { get; }
        public ICommand ExportTablesCommand { get; }

        public bool IsBusy
        {
            get => _isBusy;
            set { _isBusy = value; RaisePropertyChanged("IsBusy"); }
        }

        public MainViewModel()
        {
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
            Task.Run(() => LoadTables(SourceTableStorage, (items) =>
             {
                 sourceIsFinish = true;
                 IsBusy = !(sourceIsFinish && destinationIsFinish);
                 SourceTables.Clear();
                 foreach (var item in items)
                     SourceTables.Add(new SelectedItem { Value = item, IsSelected = false });
             }));
            Task.Run(() => LoadTables(DestinationTableStorage, (items) =>
            {
                destinationIsFinish = true;
                IsBusy = !(sourceIsFinish && destinationIsFinish);
                DestinationTables.Clear();
                foreach (var item in items)
                    DestinationTables.Add(item);
            }));
        }

        private void ExportTables()
        {
            if (string.IsNullOrEmpty(SourceTableStorage) || string.IsNullOrEmpty(DestinationTableStorage))
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

            Task.Run(() =>
                {
                    var exportService = new ExportTableStorageService();
                    exportService.Copy(SourceTableStorage, DestinationTableStorage,
                        SourceTables.Where(n => n.IsSelected).Select(n => n.Value).ToList()).Wait();
                }).ContinueWith((t) =>
            {
                Application.Current.Dispatcher.Invoke(() => IsBusy = false);
            });

        }

        private void SetDevelopmentStorage(Action<string> action, bool isActive)
        {

            if (action != null)
            {
                string value = isActive ? "UseDevelopmentStorage=true" : string.Empty;
                action(value);
            }
        }

        private async Task LoadTables(string cloudStorageAccount, Action<List<string>> addItem)
        {
            try
            {
                List<string> result = await new TableStorageService(cloudStorageAccount).GetTablesNames(null, 10);

                Application.Current.Dispatcher.Invoke(() => addItem?.Invoke(result));
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Your storage account must be formated like this : 'DefaultEndpointsProtocol=[https|http];AccountName={AccountStorageName};AccountKey={AccountStorageKey}'");

                Application.Current.Dispatcher.Invoke(() => addItem?.Invoke(new List<string>()));
            }
        }
    }
}
