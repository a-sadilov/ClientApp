using Client.Models.Core;
using Client.Views;
using Newtonsoft.Json;

using System;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using WebSocketSharp;


namespace Client.Models
{
    public class WebSocketClient : ClientBase
    {
        public static WebSocket connectionSocket;
        public static CurrentCount number;
        public override bool IsConnected
        {
            get
            {
                if (connectionSocket.ReadyState == WebSocketState.Open)
                {
                    return true;
                }
                else return false;
            } 
        }

        public WebSocketClient()
        {
            _serverPort = 8000;
            _serverIp = Dns.GetHostEntry(Dns.GetHostName()).AddressList[1];
            connectionSocket = new WebSocket("ws://" + ServerIp + ":" + ServerPort + "/ServerCounter");
            connectionSocket.OnMessage += WS_OnMessage;
        }
        public WebSocketClient(string serverIp, string serverPort)
        {
            ServerPort = serverPort;
            ServerIp = serverIp;
            connectionSocket = new WebSocket("ws://" + ServerIp + ":" + ServerPort + "/ServerCounter");
            connectionSocket.OnMessage += WS_OnMessage;
        }
        public override bool Connect()
        {
            try
            {
                if (!this.IsConnected)
                {

                    connectionSocket.Connect();
                    return true;
                }
                else
                {
                    MessageBox.Show("Already connected");
                }
                return false;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message.ToString());
                return false;
            }
        }
        public override bool Disconnect()
        {
            connectionSocket.Close();
            if(connectionSocket.ReadyState == WebSocketState.Closed)
            {
                return true;
            }
            else if(connectionSocket.ReadyState == WebSocketState.Closing) { return true; }
            else return false;
        }
        public override bool Send(string data)
        {
            ClientCommand clientCmd = null;
            switch (data)
            {
                case "Start":
                    clientCmd = new ClientCommand(Command.Start);
                    break;
                case "Stop":
                    clientCmd = new ClientCommand(Command.Stop);
                    break;
                case "Clear":
                    clientCmd = new ClientCommand(Command.Clear);
                    break;
            }
            string json = JsonConvert.SerializeObject(clientCmd);
            if (connectionSocket.ReadyState == WebSocketState.Open)
            {
                connectionSocket.Send(json);
                return true;
            }
            else return false;
        }


        private void WS_OnMessage(object sender, MessageEventArgs e)
        {
            number = JsonConvert.DeserializeObject<CurrentCount>(e.Data);
            CounterViewUserControl.CurrentCount = number;
        }
    }
}
