using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class SpiderPanelSimulator
{
    private const int Porta = 2101; // Porta onde o painel estará escutando

    public static void Main()
    {
        TcpListener listener = new TcpListener(IPAddress.Any, Porta);
        
        try
        {
            listener.Start();
            Console.WriteLine($"📡 Simulador do Painel iniciado na porta {Porta}. Aguardando mensagens...");

            while (true)
            {
                using (TcpClient client = listener.AcceptTcpClient())
                using (NetworkStream stream = client.GetStream())
                {
                    Console.WriteLine("\n🔗 Conexão estabelecida com o SpiderDriver.");

                    byte[] buffer = new byte[1024];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    
                    if (bytesRead > 0)
                    {
                        // Converter para HEX
                        string hexMessage = BitConverter.ToString(buffer, 0, bytesRead).Replace("-", " ");
                        Console.WriteLine($"📥 Mensagem recebida (HEX): {hexMessage}");

                        // Extrair a parte do dado ASCII (se presente)
                        string asciiMessage = ExtraiaTextoASCII(buffer, bytesRead);
                        if (!string.IsNullOrEmpty(asciiMessage))
                        {
                            Console.WriteLine($"📝 Mensagem traduzida (ASCII): {asciiMessage}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Erro no servidor: {ex.Message}");
        }
        finally
        {
            listener.Stop();
            Console.WriteLine("🛑 Servidor finalizado.");
        }
    }

    private static string ExtraiaTextoASCII(byte[] data, int length)
    {
        // O texto ASCII enviado começa após os primeiros 13 bytes e termina antes do ETX (0x03)
        int startIndex = 13;
        int endIndex = Array.IndexOf(data, (byte)0x03, startIndex);

        if (endIndex == -1) endIndex = length; // Caso não encontre o ETX, considerar todo o restante

        if (startIndex < endIndex && endIndex <= length)
        {
            return Encoding.ASCII.GetString(data, startIndex, endIndex - startIndex);
        }

        return string.Empty;
    }
}
