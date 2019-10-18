using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NewServerTCP
{
    class Program
    {
        static void Main()
        {
            try
            {
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");
                TcpListener server = new TcpListener(localAddr, 5001);
                // Запускаем сервер 
                server.Start();
                Console.WriteLine("Ожидание подключений... ");
                // Получаем входящее подключение 
                TcpClient client = server.AcceptTcpClient();
                // Получаем сетевой поток для чтения и записи 
                NetworkStream stream = client.GetStream();
                while (true)
                {
                    byte[] data = new byte[256];
                    int bytes = stream.Read(data, 0, data.Length); // Читаем сообщение
                    string message = Encoding.Unicode.GetString(data, 0, bytes);
                    if (message != "exit")
                    {
                        double result = 0;
                        string[] args = message.Split(new char[] { ' ' });
                        try
                        {
                            double d = Convert.ToDouble(args[0]);
                            foreach (var item in args)
                            {
                                try
                                {
                                    result += Convert.ToDouble(item);
                                }
                                catch (Exception)
                                {
                                    throw;
                                }
                            }
                            // Отображаем сообщение 
                            byte[] response = Encoding.Unicode.GetBytes("Result : " + result.ToString());
                            stream.Write(response, 0, response.Length);
                        }
                        catch (Exception)
                        {
                            // Отображаем сообщение 
                            byte[] response = Encoding.Unicode.GetBytes("Response : " + message);
                            stream.Write(response, 0, response.Length);
                        }
                        
                        
                    }
                    else
                    {
                        stream.Close();
                        client.Close();
                        // Закрываем слушающий объект 
                        server.Stop();
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                Console.ReadLine();
            }
        }
    }
}
