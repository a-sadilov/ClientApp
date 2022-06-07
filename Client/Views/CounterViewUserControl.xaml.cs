using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Client.Models;
using Client.Models.Core;
using Newtonsoft.Json;

namespace Client.Views
{
    /// <summary>
    /// Логика взаимодействия для CounterViewUserControl.xaml
    /// </summary>
    public partial class CounterViewUserControl : UserControl
    {
        private static CurrentCount _currentCount = new CurrentCount();
        public static CurrentCount CurrentCount
        {
            get { return _currentCount; }
            set
            { _currentCount = value;
            }
        }

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
                if (SettingsViewUserControl.connectionType == "Socket")
                {
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
                if (SettingsViewUserControl.connectionType == "WebSocket")
                {
                    if (SettingsViewUserControl.wsclient.Send(cmd))
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
                if (SettingsViewUserControl.connectionType == "Socket")
                {
                    if (SettingsViewUserControl.client.Send(cmd))
                    {
                        CounterLabel.Text = "0";
                        StartStopButton.Content = "Start";
                        ClearButton.IsEnabled = false;
                    }
                }
                if (SettingsViewUserControl.connectionType == "WebSocket")
                {
                    if (SettingsViewUserControl.wsclient.Send(cmd))
                    {
                        CounterLabel.Text = "0";
                        StartStopButton.Content = "Start";
                        ClearButton.IsEnabled = false;
                    }
                }
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message + ":\n" + E.StackTrace);
            }
        }
        public void CounterUpdater()
        {
            if (SettingsViewUserControl.connectionType == "Socket")
            {
                while (SettingsViewUserControl.client.IsConnected)
                {
                    try
                    {
                        string response = SettingsViewUserControl.client.Recieve();
                        //this.currentCount.counter = response;
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
                        Thread.CurrentThread.Abort();
                    }
                }
            }
            if (SettingsViewUserControl.connectionType == "WebSocket")
            {
                while (SettingsViewUserControl.wsclient.IsConnected)
                {
                    try
                    {
                        string answer = CurrentCount.counter.ToString();
                        if (answer == null)
                        {
                            throw new Exception();
                        }
                        else
                        {
                            Dispatcher?.Invoke(() => CounterLabel.Text = answer);
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
}
