using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;


using System.IO;
using System.Net;

using System.Security.Cryptography;

namespace ServerApp
{
    public partial class frmServer : Form
    {
        private TcpListener tcpListener;
        private Thread listenThread;
        private int connectedClients = 0;
        private delegate void WriteMessageDelegate(string msg);
        RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
        SymmetricEncryptDecrypt symmetricEncryptDecrypt = new SymmetricEncryptDecrypt();
        AsymmetricEncryptDecrypt asymmetricEncryptDecrypt = new AsymmetricEncryptDecrypt();
        DES des = new DES();
        DES1 des1 = new DES1();
        Dictionary<TcpClient, int> clients = new Dictionary<TcpClient, int>();

        public frmServer()
        {
            InitializeComponent();
            Server();
        }

        private void Server()
        {
            this.tcpListener = new TcpListener(IPAddress.Loopback, 3000); // Change to IPAddress.Any for internet wide Communication
            this.listenThread = new Thread(new ThreadStart(ListenForClients));
            this.listenThread.Start();
        }

        private void ListenForClients()
        {
            this.tcpListener.Start();

            while (true) // Never ends until the Server is closed.
            {
                //blocks until a client has connected to the server
                TcpClient client = this.tcpListener.AcceptTcpClient();

                //create a thread to handle communication 
                //with connected client
                connectedClients++; // Increment the number of clients that have communicated with us.
                lblNumberOfConnections.Text = connectedClients.ToString();
                this.rtbClientConnect.AppendText("Client " + connectedClients + " đã kết nối đến server ✅" + Environment.NewLine);
                clients.Add(client, connectedClients);

                Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
                clientThread.Start(client);
            }
        }

        private void HandleClientComm(object client)
        {
            TcpClient tcpClient = (TcpClient)client;
            NetworkStream clientStream = tcpClient.GetStream();

            byte[] message = new byte[8192];
            int bytesRead;

            while (true)
            {
                bytesRead = 0;

                try
                {
                    //blocks until a client sends a message
                    bytesRead = clientStream.Read(message, 0, 4096);
                }
                catch
                {
                    //a socket error has occured
                    break;
                }

                //if (bytesRead == 0)
                //{
                //    //the client has disconnected from the server
                //    connectedClients--;
                //    lblNumberOfConnections.Text = connectedClients.ToString();
                //    break;
                //}

                //message has successfully been received
                ASCIIEncoding encoder = new ASCIIEncoding();

                // Convert the Bytes received to a string and display it on the Server Screen
                string msg = encoder.GetString(message, 0, bytesRead);
                int clientNumber = 0;
                clients.TryGetValue((TcpClient)client, out clientNumber);

                if (msg.StartsWith("SYMMETRIC"))
                {
                    Symmetric(msg, encoder, clientStream, clientNumber);
                }

                if(msg.StartsWith("ASYMMETRIC"))
                {
                    Asymmetric(msg, encoder, clientStream, clientNumber);
                }

                if (msg.StartsWith("TripleDES"))
                {
                    TripleDes(msg, encoder, clientStream, clientNumber);
                }

                if (msg.StartsWith("DES"))
                {
                    Des(msg, encoder, clientStream, clientNumber);
                }

                if(msg.Equals("CLOSING"))
                {
                    this.rtbClientConnect.AppendText("Client " + connectedClients + " đã ngắt kết nối đến server ❌" + Environment.NewLine);
                    connectedClients--;
                    lblNumberOfConnections.Text = connectedClients.ToString();
                    tcpClient.Close();
                }

                if(msg.Equals("GET_CLIENT_NUMBER"))
                {
                    Echo("CLIENT_NUMBER:" + connectedClients, encoder, clientStream);
                }
            }

            tcpClient.Close();
        }

        private void Symmetric(string msg, ASCIIEncoding encoder, NetworkStream clientStream, int clientNumber)
        {

            var decryptedText = symmetricEncryptDecrypt.Decrypt(msg.Split(':')[1], msg.Split(':')[2], msg.Split(':')[3]);
            WriteMessage("Client" + clientNumber + ": " + decryptedText);
            this.rtbClientConnect.AppendText("Client" + clientNumber + "đã gửi 1 tin nhắn có nội dung \"" + decryptedText + "\" được mã hóa bởi AES" + Environment.NewLine);

            // Now Echo the message back to client

            var (Key, IVBase64) = symmetricEncryptDecrypt.InitSymmetricEncryptionKeyIV();
            string encryptedText = symmetricEncryptDecrypt.Encrypt(decryptedText, IVBase64, Key);

            Echo("SYMMETRIC" + ":" + encryptedText + ":" + IVBase64 + ":" + Key, encoder, clientStream);
        }

        private void Asymmetric(string msg, ASCIIEncoding encoder, NetworkStream clientStream, int clientNumber)
        {
            string privateKey = msg.Split(':')[2];

            var decryptedText = asymmetricEncryptDecrypt.Decrypt(msg.Split(':')[1], privateKey);
            WriteMessage("Client" + clientNumber + ": " + decryptedText);
            this.rtbClientConnect.AppendText("Client" + clientNumber + "đã gửi 1 tin nhắn có nội dung \"" + decryptedText + "\" được mã hóa bởi RSA" + Environment.NewLine);

            // Now Echo the message back to client

            string privateKeyToClient = rsa.ToXmlString(true); // true to get the private key
            string publicKey = rsa.ToXmlString(false); // false to get the public key

            var encryptedText = asymmetricEncryptDecrypt.Encrypt(decryptedText, publicKey);

            Echo("ASYMMETRIC" + ":" + encryptedText + ":" + privateKeyToClient, encoder, clientStream);
        }

        private void Des(string msg, ASCIIEncoding encoder, NetworkStream clientStream, int clientNumber)
        {
            String decryptedText = des1.Decrypt(msg.Split(':')[1], msg.Split(':')[2]);
            WriteMessage("Client" + clientNumber + ": " + decryptedText);
            this.rtbClientConnect.AppendText("Client" + clientNumber + "đã gửi 1 tin nhắn có nội dung \"" + decryptedText + "\" được mã hóa bởi DES" + Environment.NewLine);
            
            String key = "password";
            string encryptedText = des1.Encrypt(decryptedText, key);
            Echo("DES" + ":" + encryptedText + ":" + key, encoder, clientStream);
        }

        private void TripleDes(string msg, ASCIIEncoding encoder, NetworkStream clientStream, int clientNumber)
        {
            String decryptedText = des.Decrypt(msg.Split(':')[1], msg.Split(':')[2]);
            WriteMessage("Client" + clientNumber + ": " + decryptedText);
            this.rtbClientConnect.AppendText("Client" + clientNumber + "đã gửi 1 tin nhắn có nội dung \"" + decryptedText + "\" được mã hóa bởi 3DES" + Environment.NewLine);
            String key = des.GetEncodedRandomString(32);
            string encryptedText = des.Encrypt(decryptedText, key);
            Echo("TripleDES" + ":" + encryptedText + ":" + key, encoder, clientStream);
        }

        private void WriteMessage(string msg)
        {
            if (this.rtbServer.InvokeRequired)
            {
                WriteMessageDelegate d = new WriteMessageDelegate(WriteMessage);
                this.rtbServer.Invoke(d, new object[] { msg });
            }
            else
            {
                this.rtbServer.AppendText(msg + Environment.NewLine);
            }
        }

        /// <summary>
        /// Echo the message back to the sending client
        /// </summary>
        /// <param name="msg">
        /// String: The Message to send back
        /// </param>
        /// <param name="encoder">
        /// Our ASCIIEncoder
        /// </param>
        /// <param name="clientStream">
        /// The Client to communicate to
        /// </param>
        private void Echo(string msg, ASCIIEncoding encoder, NetworkStream clientStream)
        {
            // Now Echo the message back
            byte[] buffer = encoder.GetBytes(msg);

            clientStream.Write(buffer, 0, buffer.Length);
            clientStream.Flush();
        }

        private void frmServer_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.tcpListener.Stop();
        }

        private void frmServer_Load(object sender, EventArgs e)
        {

        }
    }
}
