using System;
using System.Net; 
using System.Net.Sockets; 
using System.IO;
namespace MyServer
{
    public class Server
    {
        public IPAddress myIp; 
        public int port; 
        public bool serverStatus; 
        private TcpListener tcpListener;
        public Socket socketForclient;

        public NetworkStream networkStream;
        public StreamReader streamReader;
        public StreamWriter streamWriter; 

        public Server(IPAddress myIp, int port)
        {
            serverStatus = true;
            this.myIp = myIp;
            this.port = port;
        }

        public void startListening()
        {
            try
            {
                tcpListener = new TcpListener(myIp, port);
                tcpListener.Start();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Server couldn't start listening.");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                serverStatus = false;
            }
        }

        public void acceptClient()
        {
                try
                {
                    socketForclient = tcpListener.AcceptSocket();
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Server couldn't accept client.");
                    Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
                }
        }


        //allows server to exchange data with the client
        public void clientData()
        {
            networkStream = new NetworkStream(socketForclient);
            streamReader = new StreamReader(networkStream);
            streamWriter = new StreamWriter(networkStream);
        }


        public void disconect()
        {
            networkStream.Close();
            streamReader.Close();
            streamWriter.Close();
            socketForclient.Close();
        }

    }
}
