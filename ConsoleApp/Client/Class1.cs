using System;
using System.IO;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net;
namespace MyClient
{
    public class Client
    {
        public string myIp; 
        public int port;
        private TcpClient socketForServer;
        public bool clientStatus = true;

        public NetworkStream networkStream;
        public StreamReader streamReader;
        public StreamWriter streamWriter;

        public Client(string myIp, int port)
        {
            this.myIp = myIp;
            this.port = port;
        }


        public void ConnectToServer()
        {
                try
                {
                    socketForServer = new TcpClient(myIp.ToString(), port);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Failed to connect to server.");
                    Thread.Sleep(2000);
                    Thread.Sleep(2000);
                    Thread.Sleep(2000);
                    Environment.Exit(-1);
                }
            
        }

        public void serverData()
        {
            networkStream = socketForServer.GetStream();
            streamReader = new StreamReader(networkStream);
            streamWriter = new StreamWriter(networkStream);
        }


        public void disconect()
        {
            streamWriter.Close();
            networkStream.Close();
            streamReader.Close();
            socketForServer.Close();
        }
    }
}
