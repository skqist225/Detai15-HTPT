using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace ClientApp
{
    public partial class frmClient : Form
    {
        private TcpClient client = new TcpClient();
        private IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3000);
        private string Key = "";
        private string IVBase64 = "";
        private bool isConnectionEstablished = false;
        RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
        SymmetricEncryptDecrypt symmetricEncryptDecrypt = new SymmetricEncryptDecrypt();
        AsymmetricEncryptDecrypt asymmetricEncryptDecrypt = new AsymmetricEncryptDecrypt();


        public frmClient()
        {
            InitializeComponent();
            try
            {
                client.Connect(serverEndPoint);
                isConnectionEstablished = true;
                this.cmbEncType.SelectedIndex = 0;
            } catch (Exception ex)
            {
                try
                {
                    isConnectionEstablished = false;
                    MessageBox.Show("Không thể kết nối kết tới server!");
                } finally
                {
                    //this.Dispose();
                    //Application.Exit();
                }
            }
        }

        private void SendMessage(string msg)
        {
            NetworkStream clientStream = client.GetStream();

            ASCIIEncoding encoder = new ASCIIEncoding();
            byte[] buffer = encoder.GetBytes(msg);

            clientStream.Write(buffer, 0, buffer.Length);
            clientStream.Flush();

            // Receive the TcpServer.response.

            // Buffer to store the response bytes.
            Byte[] data = new Byte[4096];

            // String to store the response ASCII representation.
            String responseData = String.Empty;

            // Read the first batch of the TcpServer response bytes.
            Int32 bytes = clientStream.Read(data, 0, data.Length);
            responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);


            if (responseData.StartsWith("SYMMETRIC"))
            {
                var decryptedText = symmetricEncryptDecrypt.Decrypt(responseData.Split(':')[1], responseData.Split(':')[2], responseData.Split(':')[3]);
                richTextBox1.AppendText(Environment.NewLine + "From Server: " + decryptedText);
                return;
            }
            else if (responseData.StartsWith("ASYMMETRIC"))
            {
                string privateKey = responseData.Split(':')[2];

                var decryptedText = asymmetricEncryptDecrypt.Decrypt(responseData.Split(':')[1], privateKey);
                richTextBox1.AppendText(Environment.NewLine + "From Server: " + decryptedText);
                return;
            }
            else
            {

            }

            richTextBox1.AppendText(Environment.NewLine + "From Server: " + responseData);
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (isConnectionEstablished)
            {

                string encTypeTXT = this.cmbEncType.Text.Trim();

                if(encTypeTXT == "Mã hóa khóa đối xứng")
                {
                    SendMessage("SYMMETRIC" + ":" + tbEncText.Text + ":" + IVBase64 + ":" + Key);
                } else if(encTypeTXT == "Mã hóa khóa bất đối xứng")
                {
                    string privateKey = rsa.ToXmlString(true); // true to get the private key
                    SendMessage("ASYMMETRIC" + ":" + tbEncText.Text + ":" + privateKey);
                } else
                {
                    // More algorithm here
                }

            } else
            {
                MessageBox.Show("Không thể kết nối tới server!");
            }

            
        }

        private void tbInput_KeyPress(object sender, KeyPressEventArgs e)
        {
           
        }

        private void tbInput_KeyUp(object sender, KeyEventArgs e)
        {

            var encryptedText = "";
            string encTypeTXT = this.cmbEncType.Text.Trim();

            if (encTypeTXT == "Mã hóa khóa đối xứng")
            {
                (Key, IVBase64) = symmetricEncryptDecrypt.InitSymmetricEncryptionKeyIV();

                 encryptedText = symmetricEncryptDecrypt.Encrypt(tbInput.Text.ToString().Trim(), IVBase64, Key);
            }
            else if (encTypeTXT == "Mã hóa khóa bất đối xứng")
            {
                string publicKey = rsa.ToXmlString(false); // false to get the public key
                encryptedText = asymmetricEncryptDecrypt.Encrypt(tbInput.Text.ToString().Trim(), publicKey);
            }
            else
            {
                // More algorithm here
            }

            tbEncText.Text = encryptedText.Trim();
        }

        private void frmClient_Load(object sender, EventArgs e)
        {

        }

        private void frmClient_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if(isConnectionEstablished)
            //{
            //    SendMessage("CLOSING");
            //} else
            //{
            //    return;
            //}
        }

        private void frmClient_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (isConnectionEstablished)
            {
                SendMessage("CLOSING");
            }
            else
            {
                return;
            }
        }
    }


}
