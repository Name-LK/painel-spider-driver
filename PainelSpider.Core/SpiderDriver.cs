using System.Net.Sockets;
using System.Text;

namespace AutoloadV4.EdgeGateway.Drivers.SpiderConsole
{
    public class SpiderDriver
    {
        private TcpClient? _client;
        private NetworkStream? _Stream;
        public DeviceStatus Status = new DeviceStatus();

        public enum DeviceStatus
        {
            CONNECTING = 0,
            CONNECTED = 1,
            DISCONNECTED = 2,
            ERROR = 4
        }

        public void Connect()
        {
            var host = "localhost";
            var port = 2101;
            Status = DeviceStatus.CONNECTING;
            Console.WriteLine($"Connecting to spider device at {host}:{port}");
            _client = new TcpClient();
            _client.Connect(host, port);
            _Stream = _client.GetStream();
            Status = DeviceStatus.CONNECTED;
            Console.WriteLine($"Device connected Successfully");
            
        }

        public void Disconnect()
        {
            if (_client != null && _client.Connected)
            {
                _client.Close();
            }
            Status = DeviceStatus.DISCONNECTED;
            Console.WriteLine($"Device Disconnected");
        }

        private void EnsureConnected()
        {
            if (Status != DeviceStatus.CONNECTED)
            {
                Console.WriteLine("Device is not connected, Reconnecting...");
                Disconnect();
                Connect();
            }
        }

        public static byte[] CriarMensagem(string mensagem)
    {
        // Definição dos cabeçalhos conforme o protocolo
        byte SOH = 0x01;
        byte STX = 0x02;
        byte CLASSE_ORIGEM = 0x50;
        byte GRUPO_ORIGEM = 0x01;
        byte ID_ORIGEM = 0x01;
        byte CLASSE_DESTINO = 0xAA;
        byte GRUPO_DESTINO = 0x01;
        byte ID_DESTINO = 0x01;
        byte CMD_QUICK_MESSAGE = 0x82;
        byte NFR = 0x01;
        byte NFRAME = 0x01;
        byte ETX = 0x03;

        // Converter a string para bytes ASCII
        byte[] dados = Encoding.ASCII.GetBytes(mensagem);
        ushort tamanhoDados = (ushort)dados.Length;

        // Criar um buffer para a mensagem completa
        byte[] mensagemBytes = new byte[13 + dados.Length + 3]; // 13 bytes fixos + dados + ETX + CRC

        // Montar a mensagem no buffer
        int index = 0;
        mensagemBytes[index++] = SOH;
        mensagemBytes[index++] = STX;
        mensagemBytes[index++] = CLASSE_ORIGEM;
        mensagemBytes[index++] = GRUPO_ORIGEM;
        mensagemBytes[index++] = ID_ORIGEM;
        mensagemBytes[index++] = CLASSE_DESTINO;
        mensagemBytes[index++] = GRUPO_DESTINO;
        mensagemBytes[index++] = ID_DESTINO;
        mensagemBytes[index++] = CMD_QUICK_MESSAGE;
        mensagemBytes[index++] = NFR;
        mensagemBytes[index++] = NFRAME;
        mensagemBytes[index++] = (byte)(tamanhoDados >> 8);  // TAM_DATA High Byte
        mensagemBytes[index++] = (byte)(tamanhoDados & 0xFF); // TAM_DATA Low Byte

        // Adicionar os dados (texto)
        Array.Copy(dados, 0, mensagemBytes, index, dados.Length);
        index += dados.Length;

        // Adicionar ETX (Fim de Texto)
        mensagemBytes[index++] = ETX;

        // Calcular CRC16 e adicionar ao final
        ushort crc = CalcularCRC16(mensagemBytes, index);
        mensagemBytes[index++] = (byte)(crc >> 8); // CRC High Byte
        mensagemBytes[index++] = (byte)(crc & 0xFF); // CRC Low Byte

        return mensagemBytes;
    }

    private static ushort CalcularCRC16(byte[] data, int length)
    {
        ushort crc = 0xFFFF;
        const ushort polinomio = 0x1021;

        for (int i = 0; i < length; i++)
        {
            crc ^= (ushort)(data[i] << 8);
            for (int j = 0; j < 8; j++)
            {
                if ((crc & 0x8000) != 0)
                    crc = (ushort)((crc << 1) ^ polinomio);
                else
                    crc <<= 1;
            }
        }
        return crc;
    }

    public void SetMessage(string message)
    {
        EnsureConnected();
        byte[] formatedMessage = CriarMensagem(message);
        Console.WriteLine($"Message sent (HEX): {BitConverter.ToString(formatedMessage).Replace("-", " ")})");

        try
        {
            _Stream.Write(formatedMessage, 0, formatedMessage.Length);
            Console.WriteLine($"Message '{message}' sent to Panel");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error Connecting: {e.Message}");
        }
    }
    }
}