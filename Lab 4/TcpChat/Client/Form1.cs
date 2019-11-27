using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class Form1 : Form
    {
        static string userName;
        private const string host = "127.0.0.1";
        private const int port = 8888;
        static TcpClient client;
        static NetworkStream stream;

        public Form1()
        {
            InitializeComponent();
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            try
            {
                buttonSend.Enabled = true;
                textBoxMessage.Enabled = true;
                buttonConnect.Enabled = false;
                userName = textBoxName.Text;
                client = new TcpClient();
                client.Connect(host, port); //подключение клиента
                stream = client.GetStream(); // получаем поток

                string message = userName;
                byte[] data = Encoding.Unicode.GetBytes(message);
                stream.Write(data, 0, data.Length);

                // запускаем новый поток для получения данных
                Thread receiveThread = new Thread(new ParameterizedThreadStart(ReceiveMessage));
                receiveThread.Start(this); //старт потока
                textBoxMessages.Text = textBoxMessages.Text + "Добро пожаловать, " + userName;
                //Console.WriteLine("Добро пожаловать, {0}", userName);
                //SendMessage();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        // отправка сообщений
        static void SendMessage(string message)
        {
            //Console.WriteLine("Введите сообщение: ");

            //while (true)
            //{
            //    string message = Console.ReadLine();
            //    byte[] data = Encoding.Unicode.GetBytes(message);
            //    stream.Write(data, 0, data.Length);
            //}
            byte[] data = Encoding.Unicode.GetBytes(message);
            stream.Write(data, 0, data.Length);
        }
        // получение сообщений
        static void ReceiveMessage(object contrl)
        {
            Control that = (Control)contrl;
            while (true)
            {
                try
                {
                    byte[] data = new byte[64]; // буфер для получаемых данных
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);

                    string message = builder.ToString();
                    that.Invoke(new MethodInvoker(() =>
                    {
                        Control chat = (Control)that.Controls[0];
                        string time = DateTime.Now.ToShortTimeString();
                        chat.Text = time + " " + message + "\r\n" + chat.Text;
                    }));
                }
                catch
                {
                    MessageBox.Show("Подключение прервано!");
                    //Console.WriteLine("Подключение прервано!"); //соединение было прервано
                    //Console.ReadLine();
                    Disconnect();
                }
            }
        }

        static void Disconnect()
        {
            if (stream != null)
                stream.Close();//отключение потока
            if (client != null)
                client.Close();//отключение клиента
            Environment.Exit(0); //завершение процесса
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Disconnect();
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            textBoxMessages.Text = DateTime.Now.ToShortTimeString() + userName + ": " + textBoxMessage.Text + "\r\n" + textBoxMessages.Text;
            SendMessage(textBoxMessage.Text);
            textBoxMessage.Text = "";
        }
    }
}
