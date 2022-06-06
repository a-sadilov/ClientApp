using Client.Core;
using Client.Views;
using System.Threading;
using System.Windows;
using System.Windows.Input;

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

        public void CloseApp(object obj)
        {
            MainWindow win = obj as MainWindow;
            win.Close();
        }

        private ICommand _closeCommand;

        public ICommand CloseAppCommand
        {
            get
            {
                if (_closeCommand == null)
                {
                    _closeCommand = new RelayCommand(p => CloseApp(p));
                }
                return _closeCommand;
            }
        }

        public void MaxApp(object obj)
        {
            MainWindow win = obj as MainWindow;

            if (win.WindowState == WindowState.Normal)
            {
                win.WindowState = WindowState.Maximized;
            }
            else if (win.WindowState == WindowState.Maximized)
            {
                win.WindowState = WindowState.Normal;
            }
        }

        private ICommand _maxCommand;
        public ICommand MaxAppCommand
        {
            get
            {
                if (_maxCommand == null)
                {
                    _maxCommand = new RelayCommand(p => MaxApp(p));
                }
                return _maxCommand;
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
                if (SettingsViewUserControl.client.connectionSocket.Connected)
                {
                    new Thread(() => CounterVM.CounterUpdater()).Start();
                }
            });
        }
    }
}

