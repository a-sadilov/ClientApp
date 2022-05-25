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

namespace Client.Models
{
    class Client : INotifyPropertyChanged/*, IDataErrorInfo*/
    {
        

        /*public string Error => throw new NotImplementedException();

        public string this[string columnName]
        {
            get
            {
                string error = String.Empty;
                switch (columnName)
                {
                    case "IpAddress":
                        if ()
                }
            }
        }*/


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private static IPAddress _serverIp;
        private static int _serverPort = 8000;
        private static IPEndPoint _serverIpEndPoint;

        private Socket _connectionSocket;
        
        public Client(string serverIp, string serverPort)
        {
            ServerPort = serverPort;
            _connectionSocket = new Socket(AddressFamily.InterNetwork,
                                               SocketType.Stream,
                                               ProtocolType.Tcp);

            ServerIp = serverIp;
            _serverIpEndPoint = new IPEndPoint(_serverIp, _serverPort);
        }


        [Range(1, 25555)]
        //[DefaultValue("8000")]
        public string ServerPort
        {
            get
            {
               return _serverIp.ToString();
            }
            set
            {
                if (Int32.TryParse(value, out Client._serverPort))
                {
                    if(_serverIpEndPoint != null)
                        _serverIpEndPoint.Port = Client._serverPort;
                    OnPropertyChanged("serverPort");
                }
                else
                {
                    MessageBox.Show("Входная строка не может преобразоваться в Port");
                }
            }
        }
        //[DefaultValue("192.168.0.107")]
        [StringLength(32,MinimumLength = 8, ErrorMessage = "Wrong IP-Adress lenght")]
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
                    OnPropertyChanged("serverIp");
                }
                else
                {
                    MessageBox.Show("Входная строка не может преобразоваться в IP");
                }
            }
        }

        public bool ConnectSocket()
        {
            try
            {
                if (!_connectionSocket.Connected)
                {
                    _connectionSocket = new Socket(AddressFamily.InterNetwork,
                                               SocketType.Stream,
                                               ProtocolType.Tcp);

                    //_connectionSocket.ReceiveTimeout = 1;
                    //_connectionSocket.SendTimeout = 500;
                    _connectionSocket.Connect(_serverIpEndPoint);
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

        public bool SendAndGetResponse(ref byte[] sendBuf, ref byte[] outBuf)
        {
            if (_connectionSocket.Connected)
            {
                _connectionSocket.Send(sendBuf);
                _connectionSocket.Receive(outBuf);
                return true;
            }
            else
            {
                MessageBox.Show("Socket is closed");
                return false;
            }
        }

        public bool CloseSocket()
        {
            if (_connectionSocket.Connected)
            {
                _connectionSocket.Close();
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
