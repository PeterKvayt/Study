using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UDPSender
{
    class Program
    {
        static void Main(string[] args)
        {
            UdpClient sender = new UdpClient();

            byte[] message = Encoding.Unicode.GetBytes("message");
            sender.Send(message, message.Length, "127.0.0.1", 5001);
            sender.Close();
        }
    }
}
