using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Client.Models.Core;

namespace Client.Views
{
    /// <summary>
    /// Логика взаимодействия для CounterViewUserControl.xaml
    /// </summary>
    public partial class CounterViewUserControl : UserControl
    {
        public CurrentCount currentCount = new CurrentCount();
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
                if (SettingsViewUserControl.client.Send(cmd))
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
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string cmd = ClearButton.Content.ToString();
                if (SettingsViewUserControl.client.Send(cmd))
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
                    string response = SettingsViewUserControl.client.Recieve();
                    this.currentCount.Counter = response;
                    if (response == null)
                    {
                        throw new Exception();
                    }
                    else
                    {
                        Dispatcher?.Invoke(() => CounterLabel.Text = response);
                        Dispatcher?.Invoke(() => StartStopButton.Content = "Stop");
                        InitializeComponent();
                    }
                }
                catch (Exception)
                {
                    //Thread.CurrentThread.Abort();
                }
            }
        }
    }
}
