using SimpleTcp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace project
{
    
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SimpleTcpClient client;

        private void button1_Click(object sender, EventArgs e)
        {
            if (client.IsConnected)
            {
                 if (!string.IsNullOrEmpty(textBox3.Text))
                 {
                    client.Send(textBox3.Text);
                    textBox2.Text += $"Me: {textBox3.Text}{Environment.NewLine}";
                    textBox3.Text = string.Empty;
                 }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                client.Connect();
                button1.Enabled = true;
                button2.Enabled = false;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            client = new SimpleTcpClient(textBox1.Text);
            client.Events.Connected += Events_Connected;
            client.Events.Disconnected += Events_Disconnected;
            client.Events.DataReceived += Events_DataReceived;
            button1.Enabled = false;
        }

        private void Events_Disconnected(object sender, EventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                textBox2.Text += $"Server is disconnected.{Environment.NewLine}";
                textBox2.Text += $"----------------------{Environment.NewLine}";
            });
        }

        private void Events_DataReceived(object sender, DataReceivedEventArgs e)
        {
            string enkriptimi = Encrypt(Encoding.UTF8.GetString(e.Data));
            string dekriptimi = Decrypt(enkriptimi);
            this.Invoke((MethodInvoker)delegate
            {
                textBox4.Text += $"Teksti i enkriptuar nga serveri : {enkriptimi}{Environment.NewLine}";
                textBox4.Text += $"Teksti i dekriptuar nga serveri : {dekriptimi}{Environment.NewLine}";
                textBox4.Text += $"---------------------------------{Environment.NewLine}";
            });
        }

        private void Events_Connected(object sender, EventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                textBox2.Text += $"Server is connected.{Environment.NewLine}";
            });
        }

        static byte[] bytes = ASCIIEncoding.ASCII.GetBytes("ZeroCool");

        public static string Encrypt(string originalString)
        {
            if (String.IsNullOrEmpty(originalString))
            {
                throw new ArgumentNullException
                       ("The string which needs to be encrypted can not be null.");
            }
            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream,
                cryptoProvider.CreateEncryptor(bytes, bytes), CryptoStreamMode.Write);
            StreamWriter writer = new StreamWriter(cryptoStream);
            writer.Write(originalString);
            writer.Flush();
            cryptoStream.FlushFinalBlock();
            writer.Flush();
            return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
        }

        public static string Decrypt(string cryptedString)
        {
            if (String.IsNullOrEmpty(cryptedString))
            {
                throw new ArgumentNullException
                   ("The string which needs to be decrypted can not be null.");
            }
            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            MemoryStream memoryStream = new MemoryStream
                    (Convert.FromBase64String(cryptedString));
            CryptoStream cryptoStream = new CryptoStream(memoryStream,
                cryptoProvider.CreateDecryptor(bytes, bytes), CryptoStreamMode.Read);
            StreamReader reader = new StreamReader(cryptoStream);
            return reader.ReadToEnd();
        }
    }
}
