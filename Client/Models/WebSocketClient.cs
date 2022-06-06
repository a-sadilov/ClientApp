using Client.Models.Core;
using Client.Views;
using Newtonsoft.Json;

using System;
using System.Net;
using System.Windows;
using System.Windows.Threading;
using WebSocketSharp;


namespace Client.Models
{
    class WebSocketClient : ClientBase
    {
        internal static WebSocket connectionSocket;
        public WebSocketClient()
        {
            _serverPort = 8000;
            _serverIp = IPAddress.Parse("192.168.0.107");
            connectionSocket = new WebSocket("ws://" + ServerIp + ":" + ServerPort + "/ServerCounter");
            //connectionSocket.OnMessage += Recieve;
        } 
        public WebSocketClient(string serverIp, string serverPort)
        {
            ServerPort = serverPort;
            ServerIp = serverIp;
            connectionSocket = new WebSocket("ws://" + ServerIp + ":" + ServerPort + "/ServerCounter");
            //connectionSocket.OnMessage += Recieve;
        }
        public override bool Connect()
        {
            try
            {
                if (connectionSocket.ReadyState != WebSocketState.Open)
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
        /*public string Recieve(MessageEventArgs e)
        {
            var obj = JsonConvert.DeserializeObject<CurrentCount>(e.Data);
            int counter = obj.counter;
            return counter.ToString();
        }*/

        private static void WS_OnMessage(object sender, MessageEventArgs e)
        {
            var obj = JsonConvert.DeserializeObject<CurrentCount>(e.Data);
            /*int counter = obj.counter;
            Dispatcher?.Invoke(() => CounterViewUserControl.CounterLabel.Text = counter.ToString());*/
        }
    }
}
