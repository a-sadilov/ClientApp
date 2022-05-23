using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Client.Models
{
    class Client
    {
        private const int port = 8082;
        private const string server = "192.168.0.106";

        public void TranslateCommand()
        {
            try
            {
                string command = GetCommand();
                using (NetworkStream stream = EstablishNewConnectionStream())
                {
                    SendCommand(stream, command);
                    string response = ReceiveResponse(stream);
                    ShowResponse(response);
                }
            }

            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
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
            Console.WriteLine("Response received:");
            Console.WriteLine(response);
        }

        private static NetworkStream EstablishNewConnectionStream()
        {
            TcpClient client = new TcpClient();
            while(!client.Connected)
            {
                client.Connect(server, port);
            }
            NetworkStream stream = client.GetStream();
            return stream;
        }

        private static string GetCommand()
        {
            Console.WriteLine("Insert a command for sending to the server:");
            string command = Console.ReadLine();
            return command;
        }
        
    }

}
