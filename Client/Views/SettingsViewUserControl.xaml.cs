using System;
using System.Windows;
using System.Windows.Controls;


namespace Client.Views
{
    public partial class SettingsViewUserControl : UserControl
    {
        internal static Models.SocketClient client = new Models.SocketClient();
        internal static Models.WebSocketClient wsclient = new Models.WebSocketClient();
        public static string connectionType;
        public SettingsViewUserControl()
        {
            InitializeComponent();
        }

        private void ButtonConnect_Click(object sender, EventArgs e)
        {
            try
            {
                TextBlock sl = (TextBlock)ConnectionTypeList.SelectedItem;
                if (sl == null)
                    throw new Exception("Выберите тип подключения к серверу");
                switch (sl.Text)
                {
                    case "Socket":
                        connectionType = "Socket";

                        switch (btnConnect.Content.ToString())
                        {
                            case "Disconnect":
                                {
                                    client.Disconnect();
                                    btnConnect.Content = "Connect";
                                    textBoxPort.IsEnabled = true;
                                    textBoxServerIp.IsEnabled = true;
                                    break;
                                }
                            case "Connect":
                                {
                                    client = new Models.SocketClient(textBoxServerIp.Text, textBoxPort.Text);
                                    if (client.Connect())
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
                        connectionType = "WebSocket";
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
