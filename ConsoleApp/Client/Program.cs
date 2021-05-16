using System;
using System.IO;
using System.Text;
using System.Net;
using System.Threading;
using System.Security.Cryptography;

namespace MyClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Client";
            string myIp = "127.0.0.1";
            int port = 9000;

            Client client = new Client(myIp, port);

            client.ConnectToServer();
            Console.WriteLine("Server is connected.");
            Console.WriteLine("--------------------");

            client.serverData();
            try
            {

                string enkriptimi = "";
                string dekriptimi = "";
                string messageFromServer = "";

                while (client.clientStatus)
                {
                    Console.Write("Client: ");
                    string messageToServer = Console.ReadLine();
                    Console.WriteLine("--------------------");

                    if (messageToServer == "exit")
                    {
                        client.clientStatus = false;
                        client.streamWriter.WriteLine("bye");
                        client.streamWriter.Flush();
                    }
                    if (messageToServer != "bye")
                    {
                        client.streamWriter.WriteLine(messageToServer);
                        client.streamWriter.Flush();
                        messageFromServer = client.streamReader.ReadLine();

                        enkriptimi = Encrypt(messageFromServer);
                        dekriptimi = Decrypt(enkriptimi);
                        Console.WriteLine("Teksti i enkriptuar nga serveri: " + enkriptimi);
                        Console.WriteLine("Teksti i dekriptuar nga serveri: " + dekriptimi);
                        Console.WriteLine("--------------------------------");
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Problem reading from server.");
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }

            client.disconect();
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
