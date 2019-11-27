using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MailClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        void SendMail(string smtpServer,
             string from,
             string password,
             string to,
             string caption,
             string message,
             bool isHtml,
             string attachFile = null
             )
        {
            MailMessage mail = new MailMessage
            {
                From = new MailAddress(from),
                Subject = caption,
                Body = message,
                IsBodyHtml = isHtml
            };

            string[] recievers = to.Split(',');

            foreach (var item in recievers)
            {
                mail.To.Add(new MailAddress(item));
            }

            if (!string.IsNullOrEmpty(attachFile))
            {
                mail.Attachments.Add(new Attachment(attachFile));
            } 

            SmtpClient client = new SmtpClient
            {
                Host = smtpServer,
                Port = 25,
                EnableSsl = true,
                Credentials = new NetworkCredential(from, password),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            client.Send(mail);
            mail.Dispose();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                string caption, path;
                bool check = checkBox.IsChecked == true ? true : false;

                caption = string.IsNullOrEmpty(textBoxCaption.Text) && string.IsNullOrWhiteSpace(textBoxCaption.Text) != true ? textBoxCaption.Text : "Stock Caption";
                path = string.IsNullOrEmpty(textBoxPath.Text) && string.IsNullOrWhiteSpace(textBoxPath.Text) != true ? textBoxPath.Text : null;
                SendMail(
                    "smtp.mail.ru",
                    "nonealoneeee@mail.ru",
                    "9WwgEl?OCSOa$MKO",
                    textBoxMail.Text,
                    caption,
                    textBoxMessage.Text, 
                    check,
                    path
                    );
                MessageBox.Show("Message was delivered succesfully!!!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Some problems :\\ " + ex.ToString());
                //throw;
            }
        }
    }
}
