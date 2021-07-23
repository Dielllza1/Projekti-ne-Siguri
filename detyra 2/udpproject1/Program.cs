using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

class Program
{
    public static string SentMessage(string msg,string bajt)
    {
        string message = bajt + "*" + encrypt(bajt) + "*" + Encrypt(msg);
        byte[] bytes1 = Encoding.ASCII.GetBytes(message);
        string base64String = Convert.ToBase64String(bytes1, 0, bytes1.Length);
        return base64String;
    }
    static byte[] bytes = ASCIIEncoding.ASCII.GetBytes("12345678");
    static void Main(string[] args)
    {
        string bajt = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        IPAddress broadcast = IPAddress.Parse("127.0.0.1");

        IPEndPoint ep = new IPEndPoint(broadcast, 11000);
        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
        EndPoint senderRemote = (EndPoint)sender;

        var rsa = new RSACryptoServiceProvider();
        _privateKey = rsa.ToXmlString(true);
        _publicKey = rsa.ToXmlString(false);
        
        String line = Console.ReadLine();
        string base64String = SentMessage(line, bajt);
        byte[] sendbuf1 = Encoding.ASCII.GetBytes(base64String);
        s.SendTo(sendbuf1, ep);

        byte[] msg = new byte[1024];
        int receivedDataLength;
        receivedDataLength = s.ReceiveFrom(msg, ref senderRemote);
        Console.Write(Encoding.ASCII.GetString(msg, 0, receivedDataLength));

        String varg = Console.ReadLine();
        string base64 = SentMessage(varg, bajt);
        byte[] sendbuf2 = Encoding.ASCII.GetBytes(base64);
        s.SendTo(sendbuf2, ep);

        receivedDataLength = s.ReceiveFrom(msg, ref senderRemote);
        Console.Write(Encoding.ASCII.GetString(msg, 0, receivedDataLength));

        varg = Console.ReadLine();
        base64 = SentMessage(varg, bajt);
        byte[] sendbuf3 = Encoding.ASCII.GetBytes(base64);
        s.SendTo(sendbuf3, ep);


        receivedDataLength = s.ReceiveFrom(msg, ref senderRemote);
        Console.Write(Encoding.ASCII.GetString(msg, 0, receivedDataLength));

        varg = Console.ReadLine();
        base64 = SentMessage(varg, bajt);
        byte[] sendbuf4 = Encoding.ASCII.GetBytes(base64);
        s.SendTo(sendbuf4, ep);

        receivedDataLength = s.ReceiveFrom(msg, ref senderRemote);
        Console.Write(Encoding.ASCII.GetString(msg, 0, receivedDataLength));

        varg = Console.ReadLine();
        base64 = SentMessage(varg, bajt);
        byte[] sendbuf5 = Encoding.ASCII.GetBytes(base64);
        s.SendTo(sendbuf5, ep);

        receivedDataLength = s.ReceiveFrom(msg, ref senderRemote);
        Console.Write(Encoding.ASCII.GetString(msg, 0, receivedDataLength));

        varg = Console.ReadLine();
        base64 = SentMessage(varg, bajt);
        byte[] sendbuf6 = Encoding.ASCII.GetBytes(base64);
        s.SendTo(sendbuf6, ep);

        receivedDataLength = s.ReceiveFrom(msg, ref senderRemote);
        Console.Write(Encoding.ASCII.GetString(msg, 0, receivedDataLength));

        varg = Console.ReadLine();
        base64 = SentMessage(varg, bajt);
        byte[] sendbuf7 = Encoding.ASCII.GetBytes(base64);
        s.SendTo(sendbuf7, ep);


        receivedDataLength = s.ReceiveFrom(msg, ref senderRemote);
        Console.Write(Encoding.ASCII.GetString(msg, 0, receivedDataLength));

        varg = Console.ReadLine();
        base64 = SentMessage(varg, bajt);
        byte[] sendbuf8 = Encoding.ASCII.GetBytes(base64);
        s.SendTo(sendbuf8, ep);

        receivedDataLength = s.ReceiveFrom(msg, ref senderRemote);
        Console.Write(Encoding.ASCII.GetString(msg, 0, receivedDataLength));

        varg = Console.ReadLine();
        base64 = SentMessage(varg, bajt);
        byte[] sendbuf9 = Encoding.ASCII.GetBytes(base64);
        s.SendTo(sendbuf9, ep);

        receivedDataLength = s.ReceiveFrom(msg, ref senderRemote);
        Console.Write(Encoding.ASCII.GetString(msg, 0, receivedDataLength));

        varg = Console.ReadLine();
        base64 = SentMessage(varg, bajt);
        byte[] sendbuf10 = Encoding.ASCII.GetBytes(base64);
        s.SendTo(sendbuf10, ep);

        byte[] msgi = new byte[1024];
        int receivedDataLengthi; ;
        receivedDataLengthi = s.ReceiveFrom(msgi, ref senderRemote);
        Console.Write(Encoding.ASCII.GetString(msgi, 0, receivedDataLengthi));
        
        
        System.Threading.Thread.Sleep(1800000000);
    }
    public static string HideCharacter()
    {
        ConsoleKeyInfo key;
        string code = "";
        do
        {
            key = Console.ReadKey(true);

            if (Char.IsNumber(key.KeyChar))
            {
                Console.Write("*");
            }
            code += key.KeyChar;
        } while (key.Key != ConsoleKey.Enter);

        return code;

    }


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
            cryptoProvider.CreateEncryptor(bytes, bytes), CryptoStreamMode.Write);//CBC
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

    private static string _privateKey;
    private static string _publicKey;
    private static UnicodeEncoding _encoder = new UnicodeEncoding();

    public static string decrypt(string data)
    {
        var rsa = new RSACryptoServiceProvider();
        var dataArray = data.Split(new char[] { ',' });
        byte[] dataByte = new byte[dataArray.Length];
        for (int i = 0; i < dataArray.Length; i++)
        {
            dataByte[i] = Convert.ToByte(dataArray[i]);
        }

        rsa.FromXmlString(_privateKey);
        var decryptedByte = rsa.Decrypt(dataByte, false);
        return _encoder.GetString(decryptedByte);
    }

    public static string encrypt(string data)
    {
        var rsa = new RSACryptoServiceProvider();
        rsa.FromXmlString(_publicKey);
        var dataToEncrypt = _encoder.GetBytes(data);
        var encryptedByteArray = rsa.Encrypt(dataToEncrypt, false).ToArray();
        var length = encryptedByteArray.Count();
        var item = 0;
        var sb = new StringBuilder();
        foreach (var x in encryptedByteArray)
        {
            item++;
            sb.Append(x);

            if (item < length)
                sb.Append(",");
        }

        return sb.ToString();
    }
}