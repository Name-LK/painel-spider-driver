using System;
using System.Net.Sockets;
using System.Text;
using AutoloadV4.EdgeGateway.Drivers.SpiderConsole;

class Program
{
    public static void Main()
    {
        SpiderDriver driver = new SpiderDriver();
        driver.Connect();
        Console.WriteLine("Digite a mensagem a ser enviada");
        string mensagem = Console.ReadLine();

        driver.SetMessage(mensagem);
    }
}
