using Client.Core;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Windows;

namespace Client.Models
{
    public abstract class ClientBase : ObservableObject
    {
        protected static IPAddress _serverIp;
        protected static int _serverPort;

        [Range(1000, 65535)]
        public virtual string ServerPort
        {
            get { return _serverPort.ToString(); }
            set
            {
                if (int.TryParse(value, out ClientBase._serverPort))
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
        public virtual string ServerIp
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

        public abstract bool IsConnected { get; }
        public abstract bool Connect();
        public abstract bool Disconnect();
        public abstract bool Send(string message);
        
    }
}