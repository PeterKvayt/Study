using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MulticastChat
{
    public partial class ChatForm : Form
    {
        private bool done = true;
        private UdpClient client;
        private IPAddress groupAddress;
        private int localPort;
        private int remotePort;
        private int ttl;
        private IPEndPoint remoteEP;
        private UnicodeEncoding encoding = new UnicodeEncoding();
        private string name;
        private string message;

        public ChatForm()
        {
            InitializeComponent();

            try
            {
                //Считываем конфигурационный файл приложения
                NameValueCollection configuration = ConfigurationSettings.AppSettings;
                groupAddress = IPAddress.Parse(configuration["groupAddress"]);
                localPort = int.Parse(configuration["LocalPort"]);
                remotePort = int.Parse(configuration["RemotePort"]);
                ttl = int.Parse(configuration["TTL"]);
            }
            catch
            {
                MessageBox.Show(this, "Ошибка в конфигурационном файле!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                buttonStart.Enabled = false;
            }
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            name = textName.Text;
            textName.ReadOnly = true;
            try
            {
                // Присоединяемся к группе рассылки
                client = new UdpClient(localPort);
                client.JoinMulticastGroup(groupAddress, ttl);
                remoteEP = new IPEndPoint(groupAddress, remotePort);

                // Запускаем поток, получающий сообщения
                Thread receiver = new Thread(new ThreadStart(Listener));
                receiver.IsBackground = true;
                receiver.Start();
                //Отправляем первое сообщение группе
                byte[] data = encoding.GetBytes("Пользователь " + name + " присоединился к чату");
                client.Send(data, data.Length, remoteEP);
                buttonStart.Enabled = false;
                buttonStop.Enabled = true;
                buttonSend.Enabled = true;
            }
            catch (SocketException ex)
            {
                MessageBox.Show(this, ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Listener()
        {
            done = false;
            try
            {
                while (!done)
                {
                    IPEndPoint ep = null;
                    byte[] buffer = client.Receive(ref ep);
                    message = encoding.GetString(buffer);
                    Invoke(new MethodInvoker(DisplayReceivedMessage));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayReceivedMessage()
        {
            string time = DateTime.Now.ToString("t");
            //textMessages.Text = time + " " + message + "\r\n" + textMessages.Text;
            textMessages.Text += "\r\n" + time + " " + message;
            statusBar.Text = "Последнее сообщение " + time;
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            try
            {
                //Отправляем сообщение группе
                byte[] data = encoding.GetBytes(name + ": " + textMessage.Text);
                client.Send(data, data.Length, remoteEP);
                textMessage.Clear();
                textMessage.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            StopListener();
        }

        private void StopListener()
        {
            //Отправляем группе сообщение о выходе
            byte[] data = encoding.GetBytes(name + " покинул чат");
            client.Send(data, data.Length, remoteEP);
            //Покидаем группу
            client.DropMulticastGroup(groupAddress);
            client.Close();
            //Останавливаем поток, получающий сообщения
            done = true;
            buttonStart.Enabled = true;
            buttonStop.Enabled = false;
            buttonSend.Enabled = false;
            textName.ReadOnly = false;
        }

        private void ChatForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!done)
            {
                StopListener();
            }
        }
    }
}
