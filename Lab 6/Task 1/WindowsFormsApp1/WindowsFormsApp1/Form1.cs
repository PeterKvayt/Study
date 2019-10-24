using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;
using Newtonsoft.Json.Converters;
namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        HttpWebRequest request;
        WebResponse response;

        string query;

        double sum;

        dynamic parsedJson;

        string[] valuts = { "USD", "EUR", "BYN", "RUB" };

        public object JsonValue { get; private set; }

        public Form1()
        {
            InitializeComponent();

            try
            {
                query = "https://belarusbank.by/api/kursExchange?city=Пинск";

                request = (HttpWebRequest)WebRequest.Create(query);

                response = request.GetResponse();

                StreamReader stream = new StreamReader(response.GetResponseStream());
                string jsonFile = stream.ReadToEnd();
                jsonFile = jsonFile.Remove(0, 1);
                jsonFile = jsonFile.Remove(jsonFile.Length - 1);
                jsonFile = jsonFile.Replace("\\/", " ");
                jsonFile = jsonFile.Remove(jsonFile.Length - (jsonFile.Length / 2 + 19));
                parsedJson = Newtonsoft.Json.Linq.JObject.Parse(jsonFile);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString());
            }
        }

        private void textBoxSumm_Click(object sender, EventArgs e)
        {
            if (textBoxSumm.Text == "Введите сумму")
            {
                textBoxSumm.Text = "";
                textBoxSumm.ForeColor = Color.Black;
            }
        }

        private void textBoxSumm_Leave(object sender, EventArgs e)
        {
            if (textBoxSumm.Text == "")
            {
                textBoxSumm.ForeColor = Color.Gray;
                textBoxSumm.Text = "Введите сумму";
            }
        }

        private void textBoxSumm_TextChanged(object sender, EventArgs e)
        {
            bool isNumber = double.TryParse(textBoxSumm.Text, out sum);

            if (!isNumber)
            {
                textBoxSumm.Text = "";
            }
        }

        private void comboBoxChooseOurMoney_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxChooseConvertableMoney.Enabled = true;
            comboBoxChooseConvertableMoney.Items.Clear();
            comboBoxChooseConvertableMoney.Items.AddRange(valuts);
            comboBoxChooseConvertableMoney.Items.Remove(comboBoxChooseOurMoney.SelectedItem);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBoxSumm.Text != "" &&
                textBoxSumm.Text != "Введите сумму" &&
                textBoxSumm.Text != "0" &&
                !textBoxSumm.Text.Contains("-") &&
                comboBoxChooseOurMoney.Text != "Выберите валюту" &&
                comboBoxChooseOurMoney.Text != "" &&
                comboBoxChooseConvertableMoney.Text != "" &&
                comboBoxChooseConvertableMoney.Text != "Выберите валюту"
                )
            {
                try
                {
                    double sum = Convert.ToDouble(textBoxSumm.Text);
                    if (comboBoxChooseOurMoney.Text == "BYN")
                    {
                        ConvertMoney(sum);
                    }
                    else
                    {
                        string curs = (string)parsedJson[comboBoxChooseOurMoney.Text + "_in"];
                        curs = curs.Replace(".", ",");
                        double byns = sum * Convert.ToDouble(curs);
                        if (comboBoxChooseConvertableMoney.Text != "BYN")
                        {
                            ConvertMoney(byns);
                        }
                        else
                        {
                            if (comboBoxChooseOurMoney.Text == "RUB")
                            {
                                byns /= 100;
                            }
                            textBoxResult.Text = byns.ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.ToString());
                }
            }
        }

        private void ConvertMoney(double sum)
        {
            string curs = (string)parsedJson[comboBoxChooseConvertableMoney.Text + "_out"];
            curs = curs.Replace(".", ",");
            double s = Convert.ToDouble(curs);

            double result = sum / s;
            
            if (comboBoxChooseConvertableMoney.Text == "RUB" && comboBoxChooseOurMoney.Text != "RUB")
            {
                result *= 100;
            }
            if (comboBoxChooseOurMoney.Text == "RUB")
            {
                result /= 100;
            }
            textBoxResult.Text = result.ToString();
        }
    }
}
