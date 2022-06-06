using Client.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using System.Windows.Shapes;


namespace Client.Views
{
    public partial class SettingsViewUserControl : UserControl
    {
        internal static Models.SocketClient client = new Models.SocketClient();
        internal static Models.WebSocketClient wsclient = new Models.WebSocketClient();
        public SettingsViewUserControl()
        {
            InitializeComponent();
        }

        private void ButtonConnect_Click(object sender, EventArgs e)
        {
            try
            {
                TextBlock sl = (TextBlock)ConnectionTypeList.SelectedItem;
                switch (sl.Text)
                {
                    case "Socket":
                        switch (btnConnect.Content.ToString())
                        {
                            case "Disconnect":
                                {
                                    client.CloseSocket();
                                    btnConnect.Content = "Connect";
                                    textBoxPort.IsEnabled = true;
                                    textBoxServerIp.IsEnabled = true;
                                    break;
                                }
                            case "Connect":
                                {
                                    client = new Models.SocketClient(textBoxServerIp.Text, textBoxPort.Text);
                                    if (client.ConnectSocket())
                                    {
                                        btnConnect.Content = "Disconnect";
                                        textBoxPort.IsEnabled = false;
                                        textBoxServerIp.IsEnabled = false;
                                    }
                                    break;
                                }
                        }
                        break;
                    case "WebSocket":
                        switch (btnConnect.Content.ToString())
                        {
                            case "Disconnect":
                                {
                                    wsclient.Disconnect();
                                    btnConnect.Content = "Connect";
                                    textBoxPort.IsEnabled = true;
                                    textBoxServerIp.IsEnabled = true;
                                    break;
                                }
                            case "Connect":
                                {
                                    wsclient = new Models.WebSocketClient(textBoxServerIp.Text, textBoxPort.Text);
                                    if (wsclient.Connect())
                                    {
                                        btnConnect.Content = "Disconnect";
                                        textBoxPort.IsEnabled = false;
                                        textBoxServerIp.IsEnabled = false;
                                    }
                                    break;
                                }
                        }
                        break;
                }
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message.ToString());
            }
        }
    }
}
