using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class ManagerForm : Form
    {
        private NetworkStream stream { get; set; }

        private static Form enterForm;

        public ManagerForm(NetworkStream stream, Form form)
        {
            this.stream = stream;
            enterForm = form;

            InitializeComponent();
        }

        private void buttonCalculate_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBoxExpenses.Text) && !string.IsNullOrWhiteSpace(textBoxProfit.Text) &&
                !string.IsNullOrEmpty(textBoxProfit.Text) && !string.IsNullOrWhiteSpace(textBoxExpenses.Text))
            {
                textBoxEfficiency.Text = CalculateEfficiency();
            }
        }

        private string CheckOnNumbers(string num)
        {
            try
            {
                double x;
                x = double.Parse(num);
                return num;
            }
            catch (Exception)
            {
                return "0";
            }
        }

        private void textBoxProfit_TextChanged(object sender, EventArgs e)
        {
            textBoxProfit.Text = CheckOnNumbers(textBoxProfit.Text);
        }

        private void textBoxExpenses_TextChanged(object sender, EventArgs e)
        {
            textBoxExpenses.Text = CheckOnNumbers(textBoxExpenses.Text);
        }

        private void ManagerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            enterForm.Close();
        }

        private void buttonSaveReport_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBoxEfficiency.Text) && !string.IsNullOrWhiteSpace(textBoxEfficiency.Text))
            {
                string efficiency = textBoxEfficiency.Text;
                string comment = textBoxReport.Text;

                string message = comment + "~" + efficiency;

                byte[] data = Encoding.Unicode.GetBytes(message);
                stream.Write(data, 0, data.Length);
                MessageBox.Show("Отчет успешно сохранен");
            }
            else
            {
                if (!string.IsNullOrEmpty(textBoxExpenses.Text) && !string.IsNullOrWhiteSpace(textBoxProfit.Text) &&
                    !string.IsNullOrEmpty(textBoxProfit.Text) && !string.IsNullOrWhiteSpace(textBoxExpenses.Text))
                {
                    string efficiency = CalculateEfficiency();
                    string comment = textBoxReport.Text;

                    string message = comment + "~" + efficiency;

                    byte[] data = Encoding.Unicode.GetBytes(message);
                    stream.Write(data, 0, data.Length);
                    MessageBox.Show("Отчет успешно сохранен");
                }
            }
        }

        private string CalculateEfficiency()
        {
            double expenses = double.Parse(textBoxExpenses.Text);
            double profit = double.Parse(textBoxProfit.Text);

            if (expenses == 0)
            {
                expenses = 1;
            }
            if (expenses < 0)
            {
                expenses *= -1;
            }
            if (profit < 0)
            {
                profit = 0;
            }

            return (profit / expenses * 100).ToString() + "%";
        }
    }
}
