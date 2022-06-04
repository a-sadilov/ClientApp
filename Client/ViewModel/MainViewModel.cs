using Client.Core;
using Client.Views;
using System.Threading;

namespace Client.ViewModel
{
    class MainViewModel : ObservableObject
    {
        //private static Models.Client client = new Models.Client();
        public RelayCommand SettingsViewCommand { get; set; }
        public RelayCommand CounterViewCommand { get; set; }
        private object _currenView;
        public SettingsViewUserControl SettingsVM { get; set; }
        public CounterViewUserControl CounterVM { get; set; }
        public object CurrenView
        {
            get { return _currenView; }
            set
            {
                _currenView = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            SettingsVM = new SettingsViewUserControl();
            CounterVM = new CounterViewUserControl();
            CurrenView = SettingsVM;
            SettingsViewCommand = new RelayCommand(o =>
            {
                CurrenView = SettingsVM;
                
            });
            CounterViewCommand = new RelayCommand(o =>
            {
                CurrenView = CounterVM;
                new Thread(() => CounterVM.CounterUpdater()).Start();
            });
        }
    }
}

