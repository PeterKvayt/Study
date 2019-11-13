using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server
{
    public partial class Form1 : Form
    {
        const string connectionString = @"Data Source=PETERKVAYTPC\SQLEXPRESS1;Initial Catalog=Factory;Integrated Security=True";
        static List<User> users = new List<User> { };
        static TcpListener server;
        static int port = 5001;
        static DataSet dataSet = new DataSet();
        static List<Report> reports = new List<Report> { };

        public Form1()
        {
            InitializeComponent();
            try
            {
                UpdateAllUsers(dataGridViewAllUsers);
                DataTableCollection tables = dataSet.Tables;
                DataTable table = tables[0].Clone();
                table.TableName = "ConnectedUsers";
                table.PrimaryKey =  new DataColumn[]{ table.Columns["Id"] };
                dataSet.Tables.Add(table);

                server = new TcpListener(IPAddress.Any, port);
                server.Start();
                textBoxLog.Text = "Ожидание подключений ...";
                Thread thread = new Thread(new ThreadStart(Listen));
                thread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Listen()
        {
            while (true)
            {
                try
                {
                    TcpClient client = server.AcceptTcpClient();
                    NetworkStream stream = client.GetStream();
                    
                    string userInfo = GetMessage(stream);
                    User user = GetUser(userInfo);

                    Weitrest weitrest = new Weitrest(client, user);

                    Thread thread = new Thread(new ParameterizedThreadStart(ServeClient));
                    thread.Start(weitrest);

                    
                }
                catch (Exception)
                {
                    this.Invoke(new MethodInvoker(() =>
                    {
                        textBoxLog.Text += "\n" + DateTime.Now.ToShortTimeString() + ": ошибка подключения!!!";
                    }));
                }
            }
        }

        private void ServeClient(object obj)
        {
            Weitrest weitrest = (Weitrest)obj;
            User user = weitrest.User;
            NetworkStream stream = weitrest.Client.GetStream();
            TcpClient client = weitrest.Client;

            if (user != null)
            {
                SendMessage(user.Role, stream);
                if (user.Role == "Управляющий")
                {
                    SendReports(stream);
                    AddToConnectedUsers(user);
                    RecieveMessage(stream, client, user, false);
                }
                else
                {
                    AddToConnectedUsers(user);
                    RecieveMessage(stream, client, user, true);
                }
            }
            else
            {
                SendMessage("Неверное имя пользователя или пароль", stream);
                this.Invoke(new MethodInvoker(() =>
                {
                    textBoxLog.Text += "\n" + DateTime.Now.ToShortTimeString() + ": Ошибка аутентификации пользователя";
                }));
            }
        }

        private User GetUser(string info)
        {
            string[] data = new string[] { };

            User result = null;
            data = info.Split(';');

            foreach (User item in users)
            {
                if (item.Name == data[0] && item.Password == data[1])
                {
                    result = item;
                    break;
                }
            }
           
            return result;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

        private static DataSet GetDataFromDB(string query)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    // Создаем объект DataAdapter
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    // Создаем объект Dataset
                    DataSet ds = new DataSet();
                    // Заполняем Dataset
                    adapter.Fill(ds);
                    return ds;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return null;
            }
        }

        private static void SetDataToDB(string query)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении пользователя: " + ex.ToString());
            }
        }

        private static void FillUsers(DataSet ds)
        {
            users.Clear();
            
            foreach (DataRow item in ds.Tables[0].Rows)
            {
                users.Add(new User(
                    int.Parse(item[0].ToString()),
                    item[1].ToString(),
                    item[2].ToString(),
                    item[3].ToString()
                    ));
            }
        }

        private static string GetMessage(NetworkStream stream)
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

        void RecieveMessage(NetworkStream stream, TcpClient client, User user, bool type)
        {
            while (client.Connected)
            {
                try
                {
                    string[] data = new string[] { };
                    string message = GetMessage(stream);
                    data = message.Split('~');
                    string query;
                    if (type)
                    {
                        query = "INSERT INTO Reports (UserId, Report, Profit) VALUES (" + user.Id + ", '" + data[0] + "', '" + data[1] + "')";
                        SetDataToDB(query);
                    }
                    else
                    {
                        query = "UPDATE Reports SET Reports.Report ='" + data[1] + "' WHERE Reports.Id = " + data[0];
                        SetDataToDB(query);
                        reports.Clear();
                        SendReports(stream);
                    }
                }
                catch (Exception)
                {
                    DisposeClient(user);
                }
            }
        }

        private static void SendMessage(string message, NetworkStream stream)
        {
            byte[] data = new byte[256];
            data = Encoding.Unicode.GetBytes(message);
            stream.Write(data, 0, data.Length);
        }

        private void buttonAddUser_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBoxUserName.Text) && !string.IsNullOrWhiteSpace(textBoxUserName.Text) &&
                !string.IsNullOrEmpty(textBoxUserPassword.Text) && !string.IsNullOrWhiteSpace(textBoxUserPassword.Text) &&
                !string.IsNullOrEmpty(comboBoxUserRole.Text) && !string.IsNullOrWhiteSpace(comboBoxUserRole.Text))
            {
                bool isUniq = true;
                foreach (var item in users)
                {
                    if (item.Name == textBoxUserName.Text)
                    {
                        isUniq = false;
                        break;
                    }
                }

                if (isUniq)
                {
                    string sqlExpression = "INSERT INTO Users (Name, Password, Role) VALUES ('" + textBoxUserName.Text + "', '" + textBoxUserPassword.Text + "', '" + comboBoxUserRole.Text + "')";
                    SetDataToDB(sqlExpression);
                    UpdateAllUsers(dataGridViewAllUsers);
                    textBoxLog.Text = textBoxLog.Text + "\n" + DateTime.Now.ToShortTimeString() +": добавлен новый пользовательпользователь " + textBoxUserName.Text;
                    textBoxUserName.Text = "";
                    textBoxUserPassword.Text = "";
                    
                }
                else
                {
                    MessageBox.Show("Пользователь с таким именем уже существует!");
                }
            }
        }

        private static void UpdateAllUsers(DataGridView grid)
        {
            dataSet = GetDataFromDB("SELECT * FROM Users");
            FillUsers(dataSet);
            grid.DataSource = dataSet.Tables[0];
        }

        private static void SendReports(NetworkStream stream)
        {
            DataSet ds = GetDataFromDB("SELECT * FROM vwReports");
            foreach (DataRow item in ds.Tables[0].Rows)
            {
                reports.Add(new Report(
                    int.Parse(item[0].ToString()),
                    item[1].ToString(),
                    item[2].ToString(),
                    item[3].ToString()
                    ));
            }

            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, reports);
        }

        private void DisposeClient(User user)
        {
            this.Invoke(new MethodInvoker(() =>
            {
                textBoxLog.Text += "\n" + DateTime.Now.ToShortTimeString() + ": " + user.Name + " отключился";
                dataSet.Tables[1].Rows.Remove(dataSet.Tables[1].Rows.Find(user.Id));
                
                dataGridViewConnectedUsers.DataSource = dataSet.Tables[1];
            }));
        }

        void AddToConnectedUsers(User user)
        {
            string[] parameters = { user.Id.ToString(), user.Name, user.Password, user.Role };
            dataSet.Tables[1].Rows.Add(parameters);
            this.Invoke(new MethodInvoker(() =>
            {
                textBoxLog.Text += "\n" + DateTime.Now.ToShortTimeString() + ": " + user.Name + " подключился";
                dataGridViewConnectedUsers.DataSource = dataSet.Tables[1];
            }));
        }
    }
}
