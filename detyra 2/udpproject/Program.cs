using System;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Security.Cryptography.Xml;
using System.IO;
using System.Linq;

public class UdpSrvrSample
{
    public static string  ReceiveMessage(string msg)
    {
        byte[] newBytes = Convert.FromBase64String(msg);
        string frombase64 = Encoding.UTF8.GetString(newBytes, 0, newBytes.Length);
        string[] message1 = frombase64.Split("*");
        string message = Decrypt(message1[2]);
        return message;
    }

    static byte[] bytes = ASCIIEncoding.ASCII.GetBytes("12345678");
    public static void Main()
    {
        string bajt = Encoding.UTF8.GetString(bytes, 0, bytes.Length);

        var rsa = new RSACryptoServiceProvider();
        _privateKey = rsa.ToXmlString(true);
        _publicKey = rsa.ToXmlString(false);


        RSACryptoServiceProvider objrsa = new RSACryptoServiceProvider();

        XmlDocument doc = new XmlDocument();
        doc.Load(@"C:\Users\Admin\Desktop\udpproject\udpproject\users.xml");


        byte[] data=new byte[1024];
        IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 11000);
        UdpClient newsock = new UdpClient(ipep);

        Console.WriteLine("Waiting for a client...");
        Console.WriteLine("---------------------------");
        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 11000);

        byte[] dat = newsock.Receive(ref sender);

        string message = Encoding.ASCII.GetString(dat, 0, dat.Length);
        //string IV = message1[0];
        //string key = decrypt(message1[1]);
        string msg=ReceiveMessage(message);
        Console.WriteLine(msg);
        string res = "OK";
        if (msg == "regjistrohu")
        {
            try
            {
                string welcome = "Shkruaje emrin: ";
                data = Encoding.ASCII.GetBytes(welcome);
                newsock.Send(data, data.Length, sender);

                byte[] da = newsock.Receive(ref sender);
                String emri = Encoding.ASCII.GetString(da, 0, da.Length);
                string emri_ = ReceiveMessage(emri);

                welcome = "Shkruaje mbiemrin: ";
                data = Encoding.ASCII.GetBytes(welcome);
                newsock.Send(data, data.Length, sender);

                da = newsock.Receive(ref sender);
                String mbiemri = Encoding.ASCII.GetString(da, 0, da.Length);
                string mbiemri_ = ReceiveMessage(mbiemri);

                welcome = "Shkruaje username: ";
                data = Encoding.ASCII.GetBytes(welcome);
                newsock.Send(data, data.Length, sender);

                da = newsock.Receive(ref sender);
                string username = Encoding.ASCII.GetString(da, 0, da.Length);
                string username_ = ReceiveMessage(username);

                welcome = "Shkruaje password: ";
                data = Encoding.ASCII.GetBytes(welcome);
                newsock.Send(data, data.Length, sender);

                da = newsock.Receive(ref sender);
                String pwd = Encoding.ASCII.GetString(da, 0, da.Length);
                string pwd_ = ReceiveMessage(pwd);

                string fatura = "Shkruaje llojin e fatures: ";
                byte[] array = Encoding.ASCII.GetBytes(fatura);
                newsock.Send(array, array.Length, sender);

                byte[] array1 = newsock.Receive(ref sender);
                string lloji = Encoding.ASCII.GetString(array1, 0, array1.Length);
                string lloji_ = ReceiveMessage(lloji);

                string viti = "Shkruaje vitin: ";
                byte[] data1 = Encoding.ASCII.GetBytes(viti);
                newsock.Send(data1, data1.Length, sender);

                byte[] data2 = newsock.Receive(ref sender);
                string viti_ = Encoding.ASCII.GetString(data2, 0, data2.Length);
                string viti__ = ReceiveMessage(viti_);

                string muaji = "Shkruaje muajin: ";
                array = Encoding.ASCII.GetBytes(muaji);
                newsock.Send(array, array.Length, sender);

                array1 = newsock.Receive(ref sender);
                string month = Encoding.ASCII.GetString(array1, 0, array1.Length);
                string month_ = ReceiveMessage(month);

                string vlera = "Shkruaje vleren ne euro: ";
                data1 = Encoding.ASCII.GetBytes(vlera);
                newsock.Send(data1, data1.Length, sender);

                data2 = newsock.Receive(ref sender);
                string vlera_euro = Encoding.ASCII.GetString(data2, 0, data2.Length);
                string vlera_euro_ = ReceiveMessage(vlera_euro);

                string dita = "Shkruaje diten: ";
                data1 = Encoding.ASCII.GetBytes(dita);
                newsock.Send(data1, data1.Length, sender);

                data2 = newsock.Receive(ref sender);
                string date = Encoding.ASCII.GetString(data2, 0, data2.Length);
                string date_ = ReceiveMessage(date);

                String salt = new Random().Next(100000, 1000000).ToString();
                String saltedpwd = salt + pwd_;
                String saltedhashpwd = calculateHash(saltedpwd);

                addRecordToXml(emri_, mbiemri_, username_, saltedhashpwd, salt, lloji_, viti__, month_, vlera_euro_, date_);

                Console.WriteLine("OK");
                res = "OK";
                data1 = Encoding.ASCII.GetBytes(res);
                newsock.Send(data1, data1.Length, sender);
            }

            catch
            {
                Console.WriteLine("ERROR");
                res = "ERROR";
                byte[] data1 = Encoding.ASCII.GetBytes(res);
                newsock.Send(data1, data1.Length, sender);
            }

        }
        else if (msg == "login")
        {
            string welcome = "Shkruaje username: ";
            byte[] data1 = Encoding.ASCII.GetBytes(welcome);
            newsock.Send(data1, data1.Length, sender);

            byte[] data2 = newsock.Receive(ref sender);
            string username = Encoding.ASCII.GetString(data2, 0, data2.Length);
            string username_ = ReceiveMessage(username);

            var root = doc.SelectSingleNode("users");
            var tgtnode = root.SelectSingleNode("user[username='" + username_ + "']");
            if (tgtnode == null)
            {
                Console.WriteLine("ERROR");
                res = "ERROR";
                byte[] d = Encoding.ASCII.GetBytes(res);
                newsock.Send(d, d.Length, sender);

                System.Environment.Exit(-1);
            }

            welcome = "Shkruaje password: ";
            data1 = Encoding.ASCII.GetBytes(welcome);
            newsock.Send(data1, data1.Length, sender);

            data2 = newsock.Receive(ref sender);
            string pwd = Encoding.ASCII.GetString(data2, 0, data2.Length);
            string pwd_ = ReceiveMessage(pwd);

            try
            {
                string salt1 = tgtnode.ChildNodes[4].InnerText;
                string saltpwd = salt1 + pwd_;
                string salthashpwd = calculateHash(saltpwd);
                string tgtnode1 = tgtnode.ChildNodes[3].InnerText;

                if (salthashpwd == tgtnode1)
                {
                    Console.WriteLine("OK");

                    SignedXml objsignedXml = new SignedXml(doc);
                    Reference referenca = new Reference();
                    referenca.Uri = "";

                    XmlDsigEnvelopedSignatureTransform xmlDsigEnvelopedSignatureTransform = new XmlDsigEnvelopedSignatureTransform();
                    referenca.AddTransform(xmlDsigEnvelopedSignatureTransform);
                    objsignedXml.AddReference(referenca);

                    KeyInfo ki = new KeyInfo();
                    ki.AddClause(new RSAKeyValue(objrsa));

                    objsignedXml.KeyInfo = ki;
                    objsignedXml.SigningKey = objrsa;
                    objsignedXml.ComputeSignature();

                    XmlElement signatureNode = objsignedXml.GetXml();
                    XmlElement rootNode = doc.DocumentElement;
                    rootNode.AppendChild(signatureNode);
                    doc.Save("fatura_nenshkruar.xml");

                    Console.WriteLine("fatura u nenshkrua!");
                    string msgi = $"OK\nfatura u nenshkrua!";
                    byte[] data5 = Encoding.ASCII.GetBytes(msgi);
                    newsock.Send(data5, data5.Length, sender);
                }
                else
                {
                    Console.WriteLine("ERROR");
                    res = "ERROR";
                    byte[] d = Encoding.ASCII.GetBytes(res);
                    newsock.Send(d, d.Length, sender);
                }
            }
            catch
            {
                Console.WriteLine("ERROR");
                res = "ERROR";
                byte[] d = Encoding.ASCII.GetBytes(res);
                newsock.Send(d, d.Length, sender);
            }
        }
        else if (msg == "verifiko")
        {
            doc.Load("fatura_nenshkruar.xml");

            SignedXml objsignedXml = new SignedXml(doc);
            XmlNodeList signatureNodes = doc.GetElementsByTagName("Signature");
            XmlElement signatureNode = (XmlElement)signatureNodes[0];

            objsignedXml.LoadXml(signatureNode);

            if (objsignedXml.CheckSignature() == true)
            {
                Console.WriteLine("Nenshkrimi eshte valid!");
                string msgn = "Nenshkrimi eshte valid!";
                data = Encoding.ASCII.GetBytes(msgn);
                newsock.Send(data, data.Length, sender);
            }
            else
            {
                Console.WriteLine("Nenshkrimi NUK eshte valid!");
                string msga = "Nenshkrimi NUK eshte valid!";
                data = Encoding.ASCII.GetBytes(msga);
                newsock.Send(data, data.Length, sender);
            }
        }
        else
        {
            Console.WriteLine("ERROR");
            res = "ERROR";
            byte[] d = Encoding.ASCII.GetBytes(res);
            newsock.Send(d, d.Length, sender);
        }
        
    }


    static void addRecordToXml(String input, String input1,String input2,String input3,String input4, String input5, String input6, String input7, String input8, String input9)
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(@"C:\Users\Admin\Desktop\udpproject\udpproject\users.xml");
        XmlNode root = doc.SelectSingleNode("users");
        XmlElement user = doc.CreateElement("user");
        root.AppendChild(user);

        XmlAttribute id = doc.CreateAttribute("id");
        id.Value = doc.SelectNodes("users/user").Count.ToString();
        user.Attributes.Append(id);

        XmlElement emri = doc.CreateElement("emri");
        emri.InnerText = input;
        user.AppendChild(emri);

        XmlElement mbiemri = doc.CreateElement("mbiemri");
        mbiemri.InnerText = input1;
        user.AppendChild(mbiemri);

        XmlElement username = doc.CreateElement("username");
        username.InnerText = input2;
        user.AppendChild(username);

        XmlElement pwd = doc.CreateElement("password");
        pwd.InnerText = input3;
        user.AppendChild(pwd);

        XmlElement salt = doc.CreateElement("salt");
        salt.InnerText = input4;
        user.AppendChild(salt);

        XmlElement lloji = doc.CreateElement("lloji_fatures");
        lloji.InnerText = input5;
        user.AppendChild(lloji);

        XmlElement viti = doc.CreateElement("viti");
        viti.InnerText = input6;
        user.AppendChild(viti);

        XmlElement muaji = doc.CreateElement("muaji");
        muaji.InnerText = input7;
        user.AppendChild(muaji);

        XmlElement vlera = doc.CreateElement("vlera_euro");
        vlera.InnerText = input8;
        user.AppendChild(vlera);

        XmlElement dita = doc.CreateElement("dita");
        dita.InnerText = input9;
        user.AppendChild(dita);

        doc.Save(@"C:\Users\Admin\Desktop\udpproject\udpproject\users.xml");
    }

    public static String calculateHash(String saltedpwd)
    {
        byte[] bytesaltedpwd = Encoding.UTF8.GetBytes(saltedpwd);

        SHA1CryptoServiceProvider objHash = new SHA1CryptoServiceProvider();
        byte[] bytesaltedhashpwd = objHash.ComputeHash(bytesaltedpwd);

        return Convert.ToBase64String(bytesaltedhashpwd);
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
