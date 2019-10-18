using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UDPReciever
{
    class Program
    {
        static void Main(string[] args)
        {
            UdpClient receiver = new UdpClient(5001); // UdpClient для получения данных
            IPEndPoint remoteIp = new IPEndPoint(IPAddress.Parse("127.0.0.1"),5002); // адрес входящего подключения
            try
            {
                receiver.Connect(remoteIp);
                while (true)
                {
                    byte[] data = receiver.Receive(ref remoteIp); // получаем данные
                    string message = Encoding.Unicode.GetString(data);
                    Console.WriteLine("Собеседник: {0}", message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                receiver.Close();
            }
        }
    }
}
