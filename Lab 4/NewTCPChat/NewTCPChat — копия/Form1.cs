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

namespace NewTCPChat
{
    public partial class Form1 : Form
    {
        static string userName;
        private const string host = "127.0.0.1";
        private const int port = 8888;
        static TcpClient client;
        static NetworkStream stream;
        Thread receiveThread;

        public Form1()
        {
            InitializeComponent();
        }

        public void SendMessage()
        {
            string message = textBoxMessage.Text;
            byte[] data = Encoding.Unicode.GetBytes(message);
            stream.Write(data, 0, data.Length);
            textBoxMessages.Text = DateTime.Now.ToShortTimeString() + " " + userName + " :  " + message + " \r\n" + textBoxMessages.Text;

        }

        // получение сообщений
        static void ReceiveMessage()
        {
            while (true)
            {
                try
                {
                    byte[] data = new byte[256]; // буфер для получаемых данных
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);
                    string message = builder.ToString();
                    ActiveForm.Controls["textBoxMessages"].Text = DateTime.Now.ToShortTimeString() + " " + userName + " :  " + message + " \r\n" + ActiveForm.Controls["textBoxMessages"].Text;
                    //Console.WriteLine(message);//вывод сообщения
                }
                catch
                {
                    //MessageBox.Show("Подключение прервано!");
                    ////Console.WriteLine("Подключение прервано!"); //соединение было прервано
                    ////Console.ReadLine();
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

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBoxName.Text) && !string.IsNullOrWhiteSpace(textBoxName.Text))
            {
                userName = textBoxName.Text;

                buttonConnect.Enabled = false;
                buttonSend.Enabled = true;
                textBoxName.Enabled = false;
                textBoxMessage.Enabled = true;

                client = new TcpClient();

                try
                {
                    client.Connect(host, port); //подключение клиента
                    stream = client.GetStream(); // получаем поток
                    string message = userName;
                    byte[] data = Encoding.Unicode.GetBytes(message);
                    stream.Write(data, 0, data.Length);

                    // запускаем новый поток для получения данных
                    receiveThread = new Thread(new ThreadStart(ReceiveMessage));
                    receiveThread.Start(); //старт потока
                    //textBoxMessages.Text = "Welcome, " + userName;
                    //SendMessage();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    //Console.WriteLine(ex.Message);
                    //Console.ReadKey();
                }
                //finally
                //{
                //    Disconnect();
                //}
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Disconnect();
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBoxMessage.Text) && !string.IsNullOrWhiteSpace(textBoxMessage.Text))
            {
                SendMessage();
                textBoxMessage.Text = "";
            }
        }
    }
}
