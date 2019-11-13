using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class Form1 : Form
    {
        static TcpClient client = new TcpClient();
        //static TcpListener listener = new TcpListener(IPAddress.Any, 5002);
        const int port = 5001;
        const string host = "127.0.0.1";
        static NetworkStream stream;

        public Form1()
        {
            InitializeComponent();
            //listener.Start();
        }

        private static void SendMessage(string message)
        {
            //byte[] data = new byte[256];
            byte[] data = Encoding.Unicode.GetBytes(message);
            stream.Write(data, 0, data.Length);
        }

        private static string RecieveMessage(NetworkStream newStream)
        {
            string response = "";
            byte[] message = new byte[256];
            StringBuilder builder = new StringBuilder();
            int bytes = 0;

            do
            {
                bytes = stream.Read(message, 0, message.Length);
                response = Encoding.Unicode.GetString(message, 0, bytes);
                builder.Append(response);
            }
            while (stream.DataAvailable);

            response = builder.ToString();

            return response;
        }

        private void Listen()
        {
            while (true)
            {
                string message = RecieveMessage(stream);

                if (message == "Менеджер")
                {
                    this.Invoke(new MethodInvoker(() =>
                    {
                        this.Hide();
                        this.ShowInTaskbar = false;
                        ManagerForm form = new ManagerForm(stream, this);
                        form.Show();
                    }));
                    
                    break;
                }
                if (message == "Управляющий")
                {
                    this.Invoke(new MethodInvoker(() =>
                    {
                        this.Hide();
                        this.ShowInTaskbar = false;
                        HeadForm form = new HeadForm(stream, this);
                        form.Show();
                    }));

                    break;
                }
                else
                {
                    MessageBox.Show(message);
                }
            }
        }

        private void buttonEnter_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBoxName.Text) && !string.IsNullOrWhiteSpace(textBoxName.Text) &&
                !string.IsNullOrEmpty(textBoxPass.Text) && !string.IsNullOrWhiteSpace(textBoxPass.Text))
            {
                try
                {
                    if (!client.Connected)
                    {
                        client.Connect(IPAddress.Parse(host), port);
                        stream = client.GetStream();
                        Thread thread = new Thread(new ThreadStart(Listen));
                        thread.Start();
                    }
                    string message = textBoxName.Text + ";" + textBoxPass.Text;
                    SendMessage(message);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка входа: " + ex.ToString());
                }
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
