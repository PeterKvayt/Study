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
            message = message.ToLower();

            if (message.Contains("get"))
            {
                message = message.Replace("get ", "");
                Console.WriteLine(message);

                try
                {
                    double number = Convert.ToDouble(message);
                    //byte[] data = new byte[256];
                    foreach (var item in studentsList.Students)
                    {
                        if (Convert.ToDouble(item.Group) == number)
                        {
                            message = DateTime.Now.ToShortTimeString() + ": " + "Student name : " + item.Name + "; Group number : " + item.Group;
                            break;
                        }
                        else
                        {
                            message = DateTime.Now.ToShortTimeString() + ": No matches in group number!";
                        }
                    }
                    //byte[] data = Encoding.Unicode.GetBytes(message);
                    //stream.Write(data, 0, data.Length);

                }
                catch (Exception)
                {
                    string query = message;

                    foreach(var item in studentsList.Students)
                    {
                        //Student item = studentsList.Students[i];
                        string name = item.Name.ToLower();
                        Console.WriteLine(name+":"+query);
                        bool flag = query == name ? true : false;
                        Console.WriteLine(flag);
                        if (flag)
                        {
                            message = DateTime.Now.ToShortTimeString() + ": " + "Student name : " + item.Name + "; Group number : " + item.Group;
                            break;
                        }
                        else
                        {
                            message = DateTime.Now.ToShortTimeString() + ": No matches in student name!";
                        }
                    }
                }
                byte[] data = Encoding.Unicode.GetBytes(message);
                stream.Write(data, 0, data.Length);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
