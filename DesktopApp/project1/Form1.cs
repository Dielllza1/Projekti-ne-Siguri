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

namespace project1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SimpleTcpServer server;

        private void button1_Click(object sender, EventArgs e)
        {
            if (server.IsListening)
            {
                if (!string.IsNullOrEmpty(textBox3.Text) && listClientIP.SelectedItem != null)
                 {
                    server.Send(listClientIP.SelectedItem.ToString(), textBox3.Text);
                    textBox2.Text += $"Server: {textBox3.Text}{Environment.NewLine}";
                    textBox3.Text = string.Empty;
                } 
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox2.Text += $"Starting...{Environment.NewLine}";
            server.Start();
            textBox2.Text += $"-------------------------------{Environment.NewLine}";
            textBox2.Text += $"Server has started.{Environment.NewLine}";
            button2.Enabled = false;
            button1.Enabled = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button1.Enabled = false;
            server = new SimpleTcpServer(textBox1.Text);
            server.Events.ClientConnected += Events_ClientConnected;
            server.Events.ClientDisconnected += Events_ClientDisconnected;
            server.Events.DataReceived += Events_DataReceived;
        }

        private void Events_DataReceived(object sender, DataReceivedEventArgs e)
        {   
            string enkriptimi = Encrypt(Encoding.UTF8.GetString(e.Data));
            string dekriptimi = Decrypt(enkriptimi);
            this.Invoke((MethodInvoker)delegate
            {
                textBox4.Text += $"Teksti i enkriptuar nga klienti {e.IpPort}: {enkriptimi}{Environment.NewLine}";
                textBox4.Text += $"Teksti i dekriptuar nga klienti {e.IpPort}: {dekriptimi}{Environment.NewLine}";
                textBox4.Text += $"-------------------------------{Environment.NewLine}";
            });
        }

        private void Events_ClientDisconnected(object sender, ClientDisconnectedEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                textBox2.Text += $"{e.IpPort}: disconnected.{Environment.NewLine}";
                textBox2.Text += $"-------------------------{Environment.NewLine}";
                listClientIP.Items.Remove(e.IpPort);
            });
        }

        private void Events_ClientConnected(object sender,ClientConnectedEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                textBox2.Text += $"{e.IpPort}: connected.{Environment.NewLine}";
                listClientIP.Items.Add(e.IpPort);
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
