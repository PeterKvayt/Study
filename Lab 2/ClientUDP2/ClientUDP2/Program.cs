using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientUDP2
{
    class Program
    {
        static int localPort = 4004; // порт приема сообщений
        static int remotePort = 4005; // порт для отправки сообщений
        //static string userName;
        static IPAddress[] addr = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
        static string localIpV6Address = addr[addr.Length - 1].ToString(); // local ipv6
        static string remoteIpV6Address = localIpV6Address;
        //static string localIpV4Address = new WebClient().DownloadString("http://icanhazip.com/"); // local ipv4
        static string localIpV4Address = "127.0.0.1";// local ipv4
        static string remoteIpV4Address = "127.0.0.1";
        //static string remoteIpV4Address = "178.120.75.34";
        static Socket listeningSocket;

        static void Main(string[] args)
        {
            //Console.Write("Введите порт для приема сообщений: ");
            //localPort = Int32.Parse(Console.ReadLine());
            //Console.Write("Введите порт для отправки сообщений: ");
            //remotePort = Int32.Parse(Console.ReadLine());
            //Console.Write("Введите свое имя: ");
            //userName = Console.ReadLine();
            Console.WriteLine("Для отправки сообщений введите сообщение и нажмите Enter");
            Console.WriteLine();

            try
            {
                //listeningSocket = new Socket(AddressFamily.InterNetworkV6, SocketType.Dgram, ProtocolType.Udp);
                listeningSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                Task listeningTask = new Task(Listen);
                listeningTask.Start();

                // отправка сообщений на разные порты
                while (true)
                {
                    string message = Console.ReadLine();

                    byte[] data = Encoding.Unicode.GetBytes(message);
                    //EndPoint remotePoint = new IPEndPoint(IPAddress.Parse(remoteIpV6Address), remotePort);
                    EndPoint remotePoint = new IPEndPoint(IPAddress.Parse(remoteIpV4Address), remotePort);
                    listeningSocket.SendTo(data, remotePoint);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Close();
            }
        }

        // поток для приема подключений
        private static void Listen()
        {
            try
            {
                //Прослушиваем по адресу
                //IPEndPoint localIP = new IPEndPoint(IPAddress.Parse(localIpV6Address), localPort);
                IPEndPoint localIP = new IPEndPoint(IPAddress.Parse(localIpV4Address), localPort);
                listeningSocket.Bind(localIP);

                while (true)
                {
                    // получаем сообщение
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0; // количество полученных байтов
                    byte[] data = new byte[256]; // буфер для получаемых данных

                    //адрес, с которого пришли данные
                    //EndPoint remoteIp = new IPEndPoint(IPAddress.IPv6Any, 0);
                    EndPoint remoteIp = new IPEndPoint(IPAddress.Any, 0);

                    do
                    {
                        bytes = listeningSocket.ReceiveFrom(data, ref remoteIp);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (listeningSocket.Available > 0);
                    // получаем данные о подключении
                    IPEndPoint remoteFullIp = remoteIp as IPEndPoint;

                    // выводим сообщение
                    Console.WriteLine("Message:{0} - {1}",  DateTime.Now.ToShortTimeString(), builder.ToString());
                    //listeningSocket.SendTo(Encoding.Unicode.GetBytes("well done"), remoteFullIp);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Close();
            }
        }
        // закрытие сокета
        private static void Close()
        {
            if (listeningSocket != null)
            {
                listeningSocket.Shutdown(SocketShutdown.Both);
                listeningSocket.Close();
                listeningSocket = null;
            }
        }
    }
}
