using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace ConsoleAppClientTCP
{
    class Program
    {
        // адрес и порт сервера, к которому будем подключаться
        static int port = 8001; // порт сервера
        //static string address = "192.168.1.6"; // адрес сервера
        static string address = "fd80:7d14:74d4:ed00:705f:be9c:40f5:9e09"; // адрес сервера
        static bool isWork = true;
        static void Main(string[] args)
        {
            try
            {
                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);

                Socket socket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
                // подключаемся к удаленному хосту
                socket.Connect(ipPoint);
                byte[] data;
                while (isWork)
                {
                    Console.Write("Введите сообщение:");
                    string message = Console.ReadLine();
                    if (message == "Sum")
                    {
                        Console.Write("Введите пер:");
                    }
                    socket.Send(Encoding.Unicode.GetBytes(message));

                    // получаем ответ
                    data = new byte[256]; // буфер для ответа
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0; // количество полученных байт

                    do
                    {
                        
                        bytes = socket.Receive(data, data.Length, 0);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (socket.Available > 0);
                    Console.WriteLine("ответ сервера: " + builder.ToString());

                    if (message == "exit")
                    {
                        isWork = false;
                        // закрываем сокет
                        socket.Shutdown(SocketShutdown.Both);
                        socket.Close();
                    }
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
