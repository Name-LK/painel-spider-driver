using System;
using System.Net.Sockets;

namespace PainelSpider.core
{
    public class PainelSpider
    {
        private const string Host = "10.1.3.96";
        private const int Port = 2101;

        private TcpClient? _client;
        
        public void Connect()
        {
            try
            {
                Console.WriteLine($"Conectando ao painel spider no endereço {Host}");
                _client = new TcpClient(Host, Port);
                Console.WriteLine($"Device Connected Successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to connect: {ex.Message}");
            }
        }

        public void Disconnect()
        {
            if (_client != null && _client.Connected)
            {
                _client.Close();
            }
            Console.WriteLine("Device Disconnected");
        }

        private void EnsureConencted()
        {
            if (_client == null || !_client.Connected)
            {
                Console.WriteLine("Device is not connected. Reconnecting...");
                Disconnect();
                Connect();
            }
        }

        public void SetMessage()
        {
            
        }

        public void GetMessage()
        {
            
        }
    }
}