using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace MyServer
{
    class Program
    { 
        static void Main(string[] args)
        {
            Console.Title = "Server";

            IPAddress myIp = IPAddress.Parse("127.0.0.1");
            Int32 port = 9000;
            Server server = new Server(myIp, port);

            server.startListening();
            if (server.serverStatus == false)
            {
                Environment.Exit(-1);
            }

            Console.WriteLine("Starting...");
            Console.WriteLine("-------------------");
            Console.WriteLine("Server has started.");

            Console.WriteLine("Waiting for connection.");
            server.acceptClient();
            Console.WriteLine("Client connected.");
            Console.WriteLine("------------------");

            string messageFromClient = "";
            string enkriptimi = "";
            string dekriptimi = "";
            string messageToClient = "";

            try
            {
                server.clientData();

                //while the server is on
                while (server.serverStatus)
                {
                    //As as the client connects
                    if (server.socketForclient.Connected)
                    {
                        //expecting a message from the client
                        messageFromClient = server.streamReader.ReadLine();

                        enkriptimi = Encrypt(messageFromClient);
                        dekriptimi = Decrypt(enkriptimi);
                        Console.WriteLine("Teksti i enkriptuar nga klienti: " + enkriptimi);
                        Console.WriteLine("Teksti i dekriptuar nga klienti: " + dekriptimi);
                        Console.WriteLine("--------------------------------");

                        if (messageFromClient == "exit")
                        {
                            // client can´t close socket connection, only the server
                            server.serverStatus = false;
                            server.streamReader.Close();
                            server.streamWriter.Close();
                            server.networkStream.Close();
                            return;
                        }

                        // if the client didn´t say no, now its my turn to talk
                        Console.Write("Server: ");
                        messageToClient = Console.ReadLine();
                        Console.WriteLine("---------------------");
                        server.streamWriter.WriteLine(messageToClient);
                        server.streamWriter.Flush();
                    }
                }

                server.disconect();
            }
            catch (Exception e)
            {
                Console.WriteLine("Problem reading from client.");
                Console.WriteLine(e.Message + "\n" + e.StackTrace);
            }
            
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
            return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);//ToHexString
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
                    (Convert.FromBase64String(cryptedString));  //FromHexString
            CryptoStream cryptoStream = new CryptoStream(memoryStream,
                cryptoProvider.CreateDecryptor(bytes, bytes), CryptoStreamMode.Read);
            StreamReader reader = new StreamReader(cryptoStream);
            return reader.ReadToEnd();
        }
    }
}
