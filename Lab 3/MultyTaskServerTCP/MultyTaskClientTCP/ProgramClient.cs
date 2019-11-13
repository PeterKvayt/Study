using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MultyTaskClientTCP
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpClient client = null;
            try
            {
                client = new TcpClient("127.0.0.1", 5001);
                NetworkStream stream = client.GetStream();
                while (true)
                {
                    // Запрос на ввод сообщения
                    Console.Write("Введите сообщение: ");
                    string message = Console.ReadLine();

                    // Преобразуем сообщение в массив байтов
                    byte[] data = Encoding.Unicode.GetBytes(message);

                    // Отправляем сообщение
                    stream.Write(data, 0, data.Length);
                    // Получаем ответ сервера
                    data = new byte[256];
                    StringBuilder response = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        response.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);
                    message = response.ToString();
                    Console.WriteLine("Ответ сервера: {0}", message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
            finally
            {
                //client.Close();
            }
        }
    }
}
