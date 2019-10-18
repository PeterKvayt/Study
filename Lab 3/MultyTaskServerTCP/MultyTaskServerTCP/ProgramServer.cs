using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace MultyTaskServerTCP
{
    class Program
    {
        public static TcpListener server = null;

        static void Main(string[] args)
        {
            try
            {
                Student[] students =
                {
                    new Student("Bob","1"),
                    new Student("Richard","2"),
                    new Student("Nick","3"),
                    new Student("Rob", "10"),
                    new Student("Yan", "11")

                };

                StudentsList list = new StudentsList(students);

                //StudentsList listOfStudents = new StudentsList();

                //foreach (var item in students)
                //{
                //    StudentsList.
                //        SetStudents
                //        (item);

                //}
                //Student.Students.AddRange(students);

                server = new TcpListener(IPAddress.Parse("127.0.0.1"), 5001);
                server.Start();

                Console.WriteLine("Waiting for connections...");

                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    ClientObject clientObject = new ClientObject(client);

                    Thread thread = new Thread(new ParameterizedThreadStart(clientObject.Process));
                    thread.Start(list);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ReadKey();
                throw;
            }
            finally
            {
                if (server != null)
                {
                    server.Stop();
                }
            }
        }
    }
}
