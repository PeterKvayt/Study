using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;


namespace ConsoleAppServerTCP
{
    class Program
    {

        static int port = 8001; // порт для приема входящих запросов
        //static bool isWork = true;
        static void Main(string[] args)
        {
            //Console.WriteLine(Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString());

            // получаем адреса для запуска сокета
            //IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString()), port);
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("fd80:7d14:74d4:ed00:705f:be9c:40f5:9e09"), port);

            // создаем сокет
            Socket listenSocket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                // связываем сокет с локальной точкой, по которой будем принимать данные
                listenSocket.Bind(ipPoint);

                // начинаем прослушивание
                listenSocket.Listen(10);

                Console.WriteLine("Сервер запущен. Ожидание подключений...");

                while (true)
                {
                    Socket handler = listenSocket.Accept();
                    // получаем сообщение
                    string message = null;
                    int bytes = 0; // количество полученных байтов
                    byte[] data = new byte[256]; // буфер для получаемых данных

                    do
                    {
                        bytes = handler.Receive(data);
                        message = Encoding.Unicode.GetString(data, 0, bytes);
                        
                        Console.WriteLine(DateTime.Now.ToShortTimeString() + ": " + message);

                        handler.Send(Encoding.Unicode.GetBytes("ваше сообщение доставлено"));
                    }
                    while (message != "exit");
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadKey();
        }
    }
}
