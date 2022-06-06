using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Client.ViewModel;

namespace Client.Views
{
    /// <summary>
    /// Логика взаимодействия для CounterViewUserControl.xaml
    /// </summary>
    public partial class CounterViewUserControl : UserControl
    {
        public CounterViewUserControl()
        {
            InitializeComponent();
            ClearButton.IsEnabled = false;
        }
        private void StartStopButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClearButton.IsEnabled = true;
                string cmd = StartStopButton.Content.ToString();
                byte[] messageToServer = Encoding.UTF8.GetBytes(cmd);
                if (SettingsViewUserControl.client.SendRequest(ref messageToServer))
                {
                    switch (cmd)
                    {
                        case "Start":
                            StartStopButton.Content = "Stop";
                            break;
                        case "Stop":
                            StartStopButton.Content = "Start";
                            break;
                    }
                }
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message.ToString());
                //MessageBox.Show(E.Message + ":\n" + E.StackTrace);
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string cmd = ClearButton.Content.ToString();
                byte[] messageToServer = Encoding.UTF8.GetBytes(cmd);
                if (SettingsViewUserControl.client.SendRequest(ref messageToServer))
                {
                    CounterLabel.Text = "0";
                    StartStopButton.Content = "Start";
                    ClearButton.IsEnabled = false;
                }
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message + ":\n" + E.StackTrace);
            }
        }
        public void CounterUpdater()
        {
            while (SettingsViewUserControl.client.connectionSocket.Connected)
            {
                try
                {
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    byte[] data = new byte[32];
                    do
                    {
                        bytes = SettingsViewUserControl.client.connectionSocket.Receive(data);
                        builder.Append(Encoding.UTF8.GetString(data, 0, bytes));
                    } while (SettingsViewUserControl.client.connectionSocket.Available > 0);

                    if (builder == null)
                    {
                        throw new Exception();
                    }
                    else
                    {
                        string message = builder.ToString();
                        Dispatcher?.Invoke(() => CounterLabel.Text = message);
                        Dispatcher?.Invoke(() => StartStopButton.Content = "Stop");
                        InitializeComponent();
                    }
                }
                catch (Exception)
                {
                    Thread.CurrentThread.Abort();
                }
            }
        }
    }
}
