using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.ComponentModel;
using System.Windows;
using System.ComponentModel.DataAnnotations;
using Client.Core;

namespace Client.Models
{
    public class SocketClient : ClientBase
    {
        private static IPEndPoint _serverIpEndPoint;
        internal Socket connectionSocket;

        [Range(1000, 65535)]
        public override string ServerPort
        {
            get{ return _serverPort.ToString();}
            set
            {
                if (Int32.TryParse(value, out SocketClient._serverPort))
                {
                    if (_serverIpEndPoint != null)
                        _serverIpEndPoint.Port = SocketClient._serverPort;
                    OnPropertyChanged();
                }
                else
                {
                    MessageBox.Show("Entered string was not converted to Port");
                }
            }
        }

        [StringLength(32, MinimumLength = 8, ErrorMessage = "Wrong IP-Adress lenght")]
        public override string ServerIp
        {
            get
            {
                if (_serverIp != null)
                {
                    return _serverIp.ToString();
                }
                else
                {
                    MessageBox.Show("Попытка взятия IP-сервера : IP не задан");
                    return null;
                }
            }
            set
            {
                if (IPAddress.TryParse(value, out _serverIp))
                {
                    if (_serverIpEndPoint != null)
                        _serverIpEndPoint.Address = _serverIp;
                    OnPropertyChanged();
                }
                else
                {
                    MessageBox.Show("Входная строка не может преобразоваться в IP");
                }
            }
        }

        public override bool IsConnected => this.connectionSocket.Connected;

        public SocketClient()
        {
            _serverPort = 8000;
            _serverIp = Dns.GetHostEntry(Dns.GetHostName()).AddressList[1];
            connectionSocket = new Socket(AddressFamily.InterNetwork,
                                               SocketType.Stream,
                                               ProtocolType.Tcp);

            _serverIpEndPoint = new IPEndPoint(_serverIp, _serverPort);
        }
        public SocketClient(string serverIp, string serverPort)
        {
            ServerPort = serverPort;
            ServerIp = serverIp;
            connectionSocket = new Socket(AddressFamily.InterNetwork,
                                               SocketType.Stream,
                                               ProtocolType.Tcp);

            _serverIpEndPoint = new IPEndPoint(_serverIp, _serverPort);
        }

        public override bool Connect()
        {
            try
            {
                if (!connectionSocket.Connected)
                {
                    connectionSocket = new Socket(AddressFamily.InterNetwork,
                                               SocketType.Stream,
                                               ProtocolType.Tcp);
                    connectionSocket.Connect(_serverIpEndPoint);
                    return true;
                }
                else
                {
                    MessageBox.Show("Already connected");
                }
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message.ToString());
                return false;
            }
        }
        public override bool Disconnect()
        {
            if (connectionSocket.Connected)
            {
                connectionSocket.Shutdown(SocketShutdown.Both);
                connectionSocket.Close();
                return true;
            }
            else
            {
                return false;
            }
        }

        public override bool Send(string message)
        {
            byte[] sendBuf = Encoding.UTF8.GetBytes(message);
            if (this.connectionSocket.Connected)
            {
                connectionSocket.Send(sendBuf);
                return true;
            }
            else
            {
                MessageBox.Show("Client is not connected");
                return false;
            }
        }

        public string Recieve()
        {
            StringBuilder builder = new StringBuilder();
            int bytes = 0;
            byte[] data = new byte[15];
            bytes = this.connectionSocket.Receive(data);
            builder.Append(Encoding.UTF8.GetString(data, 0, bytes));
            return builder.ToString();
        }

    }
}
