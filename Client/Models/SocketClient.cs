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
    public class SocketClient : ObservableObject
    {
        private static IPAddress _serverIp;
        private static int _serverPort;
        private static IPEndPoint _serverIpEndPoint;
        internal Socket connectionSocket;

        public SocketClient()
        {
            _serverPort = 8000;
            _serverIp = IPAddress.Parse("192.168.0.107");
            connectionSocket = new Socket(AddressFamily.InterNetwork,
                                               SocketType.Stream,
                                               ProtocolType.Tcp);

            _serverIpEndPoint = new IPEndPoint(_serverIp, _serverPort);
        }

        [Range(1000, 65535)]
        public string ServerPort
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

        public SocketClient(string serverIp, string serverPort)
        {
            ServerPort = serverPort;
            ServerIp = serverIp;
            connectionSocket = new Socket(AddressFamily.InterNetwork,
                                               SocketType.Stream,
                                               ProtocolType.Tcp);

            _serverIpEndPoint = new IPEndPoint(_serverIp, _serverPort);
        }



        public bool ConnectSocket()
        {
            try
            {
                if (!connectionSocket.Connected)
                {
                    connectionSocket = new Socket(AddressFamily.InterNetwork,
                                               SocketType.Stream,
                                               ProtocolType.Tcp);

                    //_connectionSocket.ReceiveTimeout = 1;
                    //_connectionSocket.SendTimeout = 500;
                    connectionSocket.Connect(_serverIpEndPoint);
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
        public bool SendRequest(ref byte[] sendBuf)
        {
            if (connectionSocket.Connected)
            {
                connectionSocket.Send(sendBuf);
                return true;
            }
            else
            {
                MessageBox.Show("Socket is closed");
                return false;
            }
        }


        /*public bool SendAndGetResponse(ref byte[] sendBuf, ref byte[] outBuf)
        {
            if (connectionSocket.Connected)
            {
                connectionSocket.Send(sendBuf);
                connectionSocket.Receive(outBuf);
                return true;
            }
            else
            {
                MessageBox.Show("Socket is closed");
                return false;
            }
        }*/

        public bool CloseSocket()
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








        public void TranslateCommand(string command)
        {
            try
            {
                using (NetworkStream stream = EstablishNewConnectionStream())
                {
                    SendCommand(stream, command);
                    string response = ReceiveResponse(stream);
                    ShowResponse(response);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Exception: {0}", e.Message);
            }
        }

        private static void SendCommand(NetworkStream stream, string command)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(command);
            stream.Write(buffer, 0, buffer.Length);
        }

        private static string ReceiveResponse(NetworkStream stream)
        {
            byte[] bytesForData = new byte[16];
            stream.Read(bytesForData, 0, bytesForData.Length);
            return Encoding.UTF8.GetString(bytesForData);
        }

        private static void ShowResponse(string response)
        {
            MessageBox.Show($"Response received: {0}", response );
        }

        private static NetworkStream EstablishNewConnectionStream()
        {
            TcpClient client = new TcpClient();
            while(!client.Connected)
            {
               // client.Connect(IpAdress, Port);
            }
            NetworkStream stream = client.GetStream();
            return stream;
        }
    }
}
