using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientSide
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Socket socketSend;
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //建立一个负责通讯的Socket
                socketSend = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                IPAddress ip = IPAddress.Parse(txtIp.Text);

                IPEndPoint point = new IPEndPoint(ip, Convert.ToInt32(txtPort.Text));

                //获得要连接的远程服务端的IP地址和端口号
                socketSend.Connect(point);

                ShowMessage("Successfully connected!!");

                Thread th = new Thread(Recieve);
                th.Start(socketSend);
                th.IsBackground = true;
            }
            catch
            {

            }
        }

        public void ShowMessage(string str)
        {
            txtSend.AppendText(str + "\r\n");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            byte[] buffer=Encoding.UTF8.GetBytes(txtSend.Text.Trim());
            socketSend.Send(buffer);
        }

        public void Recieve()
        {
            try
            {
                while (true)
                {
                    byte[] buffer = new byte[1024 * 1024 * 5];
                    int r = socketSend.Receive(buffer);

                    if (r == 0)
                    {
                        break;
                    }
                    string s = Encoding.UTF8.GetString(buffer, 0, r);
                    ShowMessage(socketSend.RemoteEndPoint + ":" + s);
                }
            }
            catch
            {

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
        }
    }
}
