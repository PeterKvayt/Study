using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Factory
{
    class Program
    {
        static TcpListener server;
        static int port = 5001;

        static void Main(string[] args)
        {
            try
            {
                server = new TcpListener(IPAddress.Any, port);
                server.Start();
                Console.WriteLine("Waiting for connections ...");
                Thread thread = new Thread(new ThreadStart(Listen));
                thread.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ReadKey();
            }
        }

        static private void Listen()
        {
            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("New client connected");
                NetworkStream stream = client.GetStream();
                SendProducts(stream);
            }
        }

        private static void SendProducts(NetworkStream stream)
        {
            List<Product> products = new List<Product> { };
            string connectionString = @"Data Source=PETERKVAYTPC\SQLEXPRESS1;Initial Catalog=Factory;Integrated Security=True";
            string sql = "SELECT * FROM Production";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    // Создаем объект DataAdapter
                    SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                    // Создаем объект Dataset
                    DataSet ds = new DataSet();
                    // Заполняем Dataset
                    adapter.Fill(ds);
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        products.Add(new Product(
                            int.Parse(item[0].ToString()),
                            item[1].ToString(),
                            int.Parse(item[2].ToString()),
                            int.Parse(item[3].ToString())
                            ));
                    }
                    Send(products, stream);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ReadKey();
            }
        }

        private static void Send(List<Product> products, NetworkStream stream)
        {
            try
            {
                //byte[] b = Encoding.Unicode.GetBytes(products.Count.ToString());
                //stream.Write(b, 0, b.Length);

                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, products);
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ReadKey();
            }
        }
    }
}
