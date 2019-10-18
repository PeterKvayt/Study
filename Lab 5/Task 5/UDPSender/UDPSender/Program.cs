using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UDPSender
{
    class Program
    {

        static void Main(string[] args)
        {
            UdpClient udpClient = new UdpClient(5010);

            byte[] sendBytes = Encoding.Unicode.GetBytes("My message");

            udpClient.Send(sendBytes, sendBytes.Length, "127.0.0.1", 5001);

            udpClient.Close();
        }
    }
}
