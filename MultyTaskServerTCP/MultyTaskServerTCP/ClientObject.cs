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

        private static StudentsList studentsList { get; set; }

        public ClientObject(TcpClient client)
        {
            Client = client;
        }

        public void Process(object students)
        {
            studentsList = (StudentsList)students;
            NetworkStream stream = null;
            try
            {
                stream = Client.GetStream();
                while (true)
                {
                    byte[] data = new byte[256];
                    StringBuilder builder = new StringBuilder();
                    int dataLength = 0;
                    do
                    {
                        dataLength = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, dataLength));
                    }
                    while (stream.DataAvailable);

                    string response = builder.ToString();

                    if (!ResponsePerform(response, stream))
                    {
                        Send(DateTime.Now.ToShortTimeString() + ": " + response, stream);
                        //Console.WriteLine(DateTime.Now.ToShortTimeString() + ": " + response);
                        //data = Encoding.Unicode.GetBytes(response);
                        //stream.Write(data, 0, data.Length);
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
            message = message.ToLower();

            if (message.Contains("get"))
            {
                message = message.Replace("get ", "");
                Console.WriteLine(message);

                bool isWrong = false;

                try
                {
                    double number = Convert.ToDouble(message);
                    string query = message;
                    foreach (var item in studentsList.Students)
                    {
                        if (item.Group.Contains(query))
                        {
                            message = "\n" + DateTime.Now.ToShortTimeString() + ": " + "Student name : " + item.Name + "; Group number : " + item.Group;
                            Send(message, stream);
                            isWrong = true;
                        }
                    }
                    if (!isWrong)
                    {
                        message = DateTime.Now.ToShortTimeString() + ": No matches in group numbers!";
                        Send(message, stream);
                    }
                }
                catch (Exception)
                {
                    string query = message;

                    foreach(var item in studentsList.Students)
                    {
                        if (item.Name.ToLower().Contains(query))
                        {
                            message = "\n" + DateTime.Now.ToShortTimeString() + ": " + "Student name : " + item.Name + "; Group number : " + item.Group;
                            Send(message, stream);
                            isWrong = true;
                        }
                    }
                    if (!isWrong)
                    {
                        message = DateTime.Now.ToShortTimeString() + ": No matches in student names!";
                        Send(message, stream);
                    }
                }
                
                return true;
            }
            else
            {
                return false;
            }
        }

        private void Send(string message, NetworkStream stream)
        {
            byte[] data = Encoding.Unicode.GetBytes(message);
            stream.Write(data, 0, data.Length);
        }
    }
}
