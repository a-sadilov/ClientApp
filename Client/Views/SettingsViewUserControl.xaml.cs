using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using System.Windows.Shapes;


namespace Client.Views
{
    /// <summary>
    /// Логика взаимодействия для SettingsViewUserControl.xaml
    /// </summary>
    public partial class SettingsViewUserControl : UserControl
    {
        private static Models.Client client;
        public SettingsViewUserControl()
        {
            InitializeComponent();
            
        }

        private void ButtonConnect_Click(object sender, EventArgs e)
        {
            try
            {
                switch (btnConnect.Content.ToString())
                {
                    case "Disconnect":
                        client.CloseSocket();
                        btnConnect.Content = "Connect";
                        textBoxPort.IsEnabled = true;
                        textBoxServerIp.IsEnabled = true;
                        break;
                    case "Connect":
                        client = new Models.Client(textBoxServerIp.Text, textBoxPort.Text);
                        if (client.ConnectSocket())
                        {
                            btnConnect.Content = "Disconnect";
                            textBoxPort.IsEnabled = false;
                            textBoxServerIp.IsEnabled = false;
                        }
                        break;
                }
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message.ToString());
            }
        }

        private void sendButton_Click(object sender, RoutedEventArgs e)
        {
            byte[] messageToServer = Encoding.UTF8.GetBytes(textBoxServerIp.Text);
            byte[] rxBuf = new byte[1024];

            if (client.SendAndGetResponse(ref messageToServer, ref rxBuf))
            {
                string serverResponse = Encoding.UTF8.GetString(rxBuf, 0, rxBuf.Count());

            }
            client.CloseSocket();
        }
    }


}
