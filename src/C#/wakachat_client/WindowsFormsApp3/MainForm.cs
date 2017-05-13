using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace WindowsFormsApp3
{
    public partial class MainForm : Form
    {
        public static string getdata = null;
        public bool first = false;
        public string olddata = "";
        //int port = Convert.ToInt32("1060");
        IPEndPoint serveradd = new IPEndPoint(IPAddress.Parse("118.89.169.100"), 1060);
        Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        Socket client1 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);


        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Focus();
        }

        public delegate void ProcessDelegate();

        public void disconnect()
        {
            textBox1.Text = "Error";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length != 0) transmit(textBox1.Text);
            textBox1.Text = "";
            textBox1.Focus();
        }

        public void transmit(string text)
        {
            Thread t1 = new Thread(connect);
            t1.Start();
            Thread t2 = new Thread(send);
            t2.Start(text);
            Thread t3 = new Thread(rece);
            t3.Start();
        }

        public void connect(object obj)
        {
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try {
                client.Connect(serveradd);
            }
            catch (System.Exception ex)
            {
                ProcessDelegate error = new ProcessDelegate(disconnect);
                textBox1.Invoke(error);
                return;
            }
        }

        private void send(object obj)
        {
            Byte[] sendBytes = Encoding.UTF8.GetBytes((string)obj);
            while (true)
            {
                try {
                    client.Send(sendBytes);
                    break;
                }
                catch {}
            }

        }

        private void rece(object obj)
        {
            Byte[] receive = new Byte[1024];
            while (true)
            {
                try {
                    client.Receive(receive);
                    break;
                }
                catch {}
            }

            getdata = Encoding.UTF8.GetString(receive, 0, receive.Length).TrimEnd('\0');
            client.Close();
        }

        public void transmit1(string text)
        {
            Thread t1 = new Thread(connect1);
            t1.Start();
            Thread t2 = new Thread(send1);
            t2.Start(text);
            Thread t3 = new Thread(rece1);
            t3.Start();
        }

        public void connect1(object obj)
        {
            client1 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                client1.Connect(serveradd);
            }
            catch (System.Exception ex)
            {
                ProcessDelegate error = new ProcessDelegate(disconnect);
                textBox1.Invoke(error);
                return;
            }
        }

        private void send1(object obj)
        {
            Byte[] sendBytes = Encoding.UTF8.GetBytes((string)obj);
            while (true)
            {
                try
                {
                    client1.Send(sendBytes);
                    break;
                }
                catch { }
            }

        }

        private void rece1(object obj)
        {
            Byte[] receive = new Byte[1024];
            while (true)
            {
                try
                {
                    client1.Receive(receive);
                    break;
                }
                catch { }
            }

            getdata = Encoding.UTF8.GetString(receive, 0, receive.Length).TrimEnd('\0');
            client1.Close();
        }
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            if ((getdata != "")&& (getdata != null) && (getdata != olddata))
            {
                textBox2.Text = olddata;
                if (first)
                {
                    int temp = 0;
                    if (olddata == null)
                    {
                        temp = getdata.Length;
                    }
                    else
                    {
                        temp = getdata.Length - olddata.Length;
                    }
                    richTextBox1.Text = richTextBox1.Text + "\r\n" + getdata.Remove(0,olddata.Length);
                }
                else
                {
                    richTextBox1.Text = getdata;
                    first = true;
                }
                olddata = getdata;
                //getdata = "";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "adminwakafa")
            {
                transmit("0");
                textBox1.Text = "";
            }
            textBox1.Focus();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (textBox1.Text.Length != 0) transmit(textBox1.Text);
                textBox1.Text = "";
                textBox1.Focus();
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            transmit1("2");
            olddata = getdata;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer2.Enabled = false;
            Thread.Sleep(1000);
            if (client.Connected) client.Close();
            if (client1.Connected) client1.Close();
        }
    }
}
