using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MultyTaskServerTCP
{
    class ClientObject
    {
        public TcpClient Client { get; set; }

        public ClientObject(TcpClient client)
        {
            Client = client;
        }

        public void Process()
        {

            NetworkStream stream = null;
            try
            {
                stream = Client.GetStream();
                byte[] data = new byte[256];
                while (true)
                {
                    StringBuilder builder = new StringBuilder();
                    int dataLength = 0;
                    do
                    {
                        dataLength = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, data.Length));
                    }
                    while (stream.DataAvailable);

                    string response = builder.ToString();

                    if (!ResponsePerform(response, stream))
                    {
                        Console.WriteLine(DateTime.Now.ToShortTimeString() + ": " + response);
                        data = Encoding.Unicode.GetBytes(response);
                        stream.Write(data, 0, data.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                //throw;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
                if (Client != null)
                {
                    Client.Close();
                }
            }
        }

        private bool ResponsePerform(string message, NetworkStream stream)
        {
            if (message.ToLower().Contains("get"))
            {
                message = message.ToLower();
                message = message.Replace("get ", "");

                try
                {
                    double number = Convert.ToDouble(message.Trim());
                    foreach (var item in StudentsList.Students)
                    {
                        if (Convert.ToDouble(item.Group) == number)
                        {
                            message = DateTime.Now.ToShortTimeString() + ": " + "Student name : " + item.Name + "; Group number : " + item.Group;
                            byte[] data = Encoding.Unicode.GetBytes(message);
                            stream.Write(data, 0, data.Length);
                        }
                    }
                }
                catch (Exception)
                {
                    foreach (var item in StudentsList.Students)
                    {
                        if (item.Name == message.Trim())
                        {
                            message = DateTime.Now.ToShortTimeString() + ": " + "Student name : " + item.Name + "; Group number : " + item.Group;
                            byte[] data = Encoding.Unicode.GetBytes(message);
                            stream.Write(data, 0, data.Length);
                        }
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
