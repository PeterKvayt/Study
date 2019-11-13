using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BrowserLite
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            WebClient client = new WebClient();
            try
            {
                using (Stream stream = client.OpenRead(textBox1.Text))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string newLine;
                        string html = null;
                        while ((newLine = reader.ReadLine()) != null)
                        {
                            html += newLine;
                        }
                        webBrowser.DocumentText = html;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
        }
    }
}
