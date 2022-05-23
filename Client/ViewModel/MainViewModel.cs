using Client.Core;

namespace Client.ViewModel
{
    class MainViewModel : ObservableObject
    {
        public RelayCommand SettingsViewCommand { get; set; }
        public RelayCommand CounterViewCommand { get; set; }
        private object _currenView;
        public SettingsViewModel SettingsVM { get; set; }
        public CounterViewModel CounterVM { get; set; }
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
            SettingsVM = new SettingsViewModel();
            CounterVM = new CounterViewModel();
            CurrenView = SettingsVM;
            SettingsViewCommand = new RelayCommand(o =>
            {
                CurrenView = SettingsVM;
            });
            CounterViewCommand = new RelayCommand(o =>
            {
                CurrenView = CounterVM;
            });
        }
    }
}

