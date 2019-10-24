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

namespace FormTCPChat
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

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBoxName.Text))
            {
                userName = textBoxName.Text;
                buttonConnect.Enabled = false;
                //buttonDisconnect.Enabled = true;
                buttonSend.Enabled = true;
                textBoxName.Enabled = false;
                textBoxMessage.Enabled = true;

                client = new TcpClient();

                try
                {
                    client.Connect(host, port); //подключение клиента
                    stream = client.GetStream(); // получаем поток

                    byte[] data = Encoding.Unicode.GetBytes(userName);
                    stream.Write(data, 0, data.Length);

                    // запускаем новый поток для получения данных
                    receiveThread = new Thread(new ThreadStart(ReceiveMessage));
                    receiveThread.Start(); //старт потока
                    textBoxMessages.Text += "Welcome , "+ userName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.ToString());
                }
                //finally
                //{
                //    Disconnect();
                //}
            }
        }

        // отправка сообщений
        static void SendMessage(string message)
        {
            byte[] data = Encoding.Unicode.GetBytes(message);
            stream.Write(data, 0, data.Length);
        }

        // получение сообщений
        void ReceiveMessage()
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
                    textBoxMessages.Text =  message;
                }
                catch
                {
                    MessageBox.Show("Подключение прервано!");
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

        private void buttonSend_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBoxMessage.Text) && !string.IsNullOrWhiteSpace(textBoxMessage.Text))
            {
                try
                {
                    SendMessage(textBoxMessage.Text);
                    textBoxMessage.Text = "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.ToString());
                }
            }
        }

        private void buttonDisconnect_Click(object sender, EventArgs e)
        {
            textBoxName.Text = "";
            buttonConnect.Enabled = true;
            //buttonDisconnect.Enabled = false;
            buttonSend.Enabled = false;
            textBoxMessage.Enabled = false;
        }
    }
}
