using Factory;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class Form1 : Form
    {
        static TcpClient client;

        static int port = 5001;

        static List<Product> products = new List<Product> { };

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                client = new TcpClient("127.0.0.1", port);
                NetworkStream stream = client.GetStream();

                BinaryFormatter formatter = new BinaryFormatter();
                products = (List<Product>)formatter.Deserialize(stream);

                dataGridViewProducts.DataSource = products;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                Environment.Exit(0);
            }
        }

        private string GetMessage(NetworkStream stream)
        {
            StringBuilder builder = new StringBuilder();
            do
            {
                byte[] response = new byte[1024];
                int bytes = stream.Read(response, 0, response.Length);
                string message = Encoding.Unicode.GetString(response);
                builder.Append(response);
            } while (stream.DataAvailable);

            return builder.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double otherExpenses = 0;
            if (!string.IsNullOrEmpty(textBoxOtherExpenses.Text) && 
                !string.IsNullOrWhiteSpace(textBoxOtherExpenses.Text) &&
                double.TryParse(textBoxOtherExpenses.Text, out otherExpenses))
            {
                double productsExpenses = 0;
                double productsCost = 0;
                foreach (var item in products)
                {
                    productsExpenses += item.ProduceCost;
                    productsCost += item.Cost;
                }

                double result = (productsCost / (productsExpenses + otherExpenses)) * 100;

                textBoxResult.Text = result.ToString() + " %";
            }
        }

        private void textBoxOtherExpenses_TextChanged(object sender, EventArgs e)
        {
            double otherExpenses = 0;
            if (string.IsNullOrEmpty(textBoxOtherExpenses.Text) ||
                string.IsNullOrWhiteSpace(textBoxOtherExpenses.Text) ||
                !double.TryParse(textBoxOtherExpenses.Text, out otherExpenses))
            {
                textBoxOtherExpenses.Text = "0";
            }
        }
    }
}
