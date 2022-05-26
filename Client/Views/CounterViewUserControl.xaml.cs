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
using Client.ViewModel;

namespace Client.Views
{
    /// <summary>
    /// Логика взаимодействия для CounterViewUserControl.xaml
    /// </summary>
    public partial class CounterViewUserControl : UserControl
    {
        internal static Models.Client client;
        public CounterViewUserControl(Models.Client _client)
        {
            InitializeComponent();
            client = _client;
        }

        private void StartStopButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClearButton.IsEnabled = true;
                string cmd = ClearButton.Content.ToString();
                byte[] messageToServer = Encoding.UTF8.GetBytes(cmd);
                byte[] rxBuf = new byte[12];
                if (client.SendAndGetResponse(ref messageToServer, ref rxBuf))
                {
                    string serverResponse = Encoding.UTF8.GetString(rxBuf, 0, rxBuf.Count());
                    if (serverResponse == "200OK_" + cmd)
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
                byte[] messageToServer = Encoding.UTF8.GetBytes(cmd);
                byte[] rxBuf = new byte[12];
                if (client.SendAndGetResponse(ref messageToServer, ref rxBuf))
                {
                    string serverResponse = Encoding.UTF8.GetString(rxBuf, 0, rxBuf.Count());
                    if(serverResponse == "200OK_" + cmd)
                    {
                        CounterLabel.Text = "0";
                        ClearButton.IsEnabled = false;
                        StartStopButton.Content = "Start";
                    }
                }
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message.ToString());
            }
        }
    }
}
