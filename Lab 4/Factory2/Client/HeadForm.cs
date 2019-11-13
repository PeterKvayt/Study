using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Server;

namespace Client
{
    public partial class HeadForm : Form
    {
        private NetworkStream stream { get; set; }

        private static Form enterForm;

        List<Report> reports = new List<Report> { };

        public HeadForm(NetworkStream stream, Form form)
        {
            this.stream = stream;
            enterForm = form;

            InitializeComponent();

            
        }

        private void HeadForm_Load(object sender, EventArgs e)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            reports = (List<Report>)formatter.Deserialize(stream);
            dataGridView1.DataSource = reports;
            List<string> nameReports = new List<string> { };
            foreach (var item in reports)
            {
                nameReports.Add(item.Id + " " + item.UserName + " ");
            }
            comboBoxReports.DataSource = nameReports;
            comboBoxReports.SelectedIndex = 0;
            richTextBoxReport.Text = reports[0].Comment;
        }

        private void HeadForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            enterForm.Close();
        }

        private void comboBoxReports_SelectedIndexChanged(object sender, EventArgs e)
        {
            richTextBoxReport.Text = reports[comboBoxReports.SelectedIndex].Comment;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //byte[] data = new byte[] { };
            //string[] nameReport = comboBoxReports.Text.Split(' ');
            //data = Encoding.Unicode.GetBytes(nameReport[0] + "~" + richTextBoxReport.Text);
            //stream.Write(data, 0, data.Length);
            UpdateReports();
        }

        void UpdateReports()
        {
            byte[] data = new byte[] { };
            string[] nameReport = comboBoxReports.Text.Split(' ');
            data = Encoding.Unicode.GetBytes(nameReport[0] + "~" + richTextBoxReport.Text);
            stream.Write(data, 0, data.Length);

            BinaryFormatter formatter = new BinaryFormatter();
            reports = (List<Report>)formatter.Deserialize(stream);
            dataGridView1.DataSource = reports;
            List<string> nameReports = new List<string> { };
            foreach (var item in reports)
            {
                nameReports.Add(item.Id + " " + item.UserName + " ");
            }
            comboBoxReports.DataSource = nameReports;
        }

        private void ToolStripMenuItemUpdate_Click(object sender, EventArgs e)
        {
            UpdateReports();
        }
    }
}
