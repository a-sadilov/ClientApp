using Client.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WebSocketSharp;

namespace Client.Models
{
    class WebSocketClient : ObservableObject
    {
        private static IPAddress _serverIp;
        private static int _serverPort;
        internal static WebSocket connectionSocket;

        [Range(1000, 65535)]
        public string ServerPort
        {
            get { return _serverPort.ToString(); }
            set
            {
                if (int.TryParse(value, result: out _serverPort))
                {
                    OnPropertyChanged();
                }
                else
                {
                    MessageBox.Show("Входная строка не может преобразоваться в Port");
                }
            }
        }

        [StringLength(32, MinimumLength = 8, ErrorMessage = "Wrong IP-Adress lenght")]
        public string ServerIp
        {
            get
            {
                if (_serverIp != null)
                {
                    return _serverIp.ToString();
                }
                else
                {
                    MessageBox.Show("Попытка взятия IP-сервера : IP not given");
                    return null;
                }
            }
            set
            {
                if (IPAddress.TryParse(value, out _serverIp))
                {
                    OnPropertyChanged();
                }
                else
                {
                    MessageBox.Show("Входная строка не может преобразоваться в IP");
                }
            }
        }
        public WebSocketClient()
        {
            _serverPort = 8000;
            _serverIp = IPAddress.Parse("192.168.0.107");
            connectionSocket = new WebSocket("ws://" + ServerIp + ":" + ServerPort + "/ServerCounter");
        } 
        public WebSocketClient(string serverIp, string serverPort)
        {
            ServerPort = serverPort;
            ServerIp = serverIp;
            connectionSocket = new WebSocket("ws://" + ServerIp + ":" + ServerPort + "/ServerCounter");
        }
        public bool Connect()
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
        public void Disconnect()
        {
            connectionSocket.Close();
        }
        public void Send(string data)
        {
            connectionSocket.Send(data);
        }

    }
}
