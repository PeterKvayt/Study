using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NewClientTCP
{
    class Program
    {
        // Создаем клиента, используя конструктор по умолчанию 
        

        static void Main(string[] args)
        {
            try
            {
                TcpClient tcpClient = new TcpClient();
                // Подключаемся к серверу 
                tcpClient.Connect("127.0.0.1", 5001);
                // Создаем поток, соединенный с сервером 
                NetworkStream stream = tcpClient.GetStream();
                // Формируем сообщение. Преобразуем его в массив байтов
                while (true)
                {
                    Console.WriteLine("Write your message");
                    string message = Console.ReadLine();
                    byte[] data = Encoding.Unicode.GetBytes(message);
                    // Отправка сообщения 
                    stream.Write(data, 0, data.Length);
                    if (message == "exit")
                    {
                        break;
                    }
                    byte[] response = new byte[256];
                    int responseLength = stream.Read(response, 0, response.Length);
                    Console.WriteLine(Encoding.Unicode.GetString(response, 0, response.Length));
                }
                // Закрываем потоки 
                stream.Close();
                tcpClient.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Console.ReadKey();
            }
        }

    }
}

