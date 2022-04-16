using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace CPE4020Final_dotNET
{


    public partial class Form1 : Form
    {

        //   luis1 luis2 other1 other2
        string[] sensors = { "", "", "", "" };

        Thread _serverThread = null;
        TcpListener _listener;

        public Form1()
        {


            InitializeComponent();
            button1.Text = "Start API";
            button2.Text = "Stop API";
            label1.Text = ("temp sensor 1: null");
            label2.Text = ("temp sensor 2: null");
            label3.Text = ("API off");
        }

        delegate void SetChartCallback(string text);
        TcpListener server;
        TcpClient connclient;
        NetworkStream ns;
        WebClient client = null;
        Thread t = null;

        Int32 port = 2222;
        IPAddress localaddr = IPAddress.Parse("10.0.0.220");

        String webAddr1 = "10.0.0.185:2000";

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Start();
            Start();
            label3.Text = ("API on");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            Stop();
            label3.Text = ("API off");
            //client.Dispose();
        }

        public void Start(int port = 2222)
        {
            if (_serverThread == null)
            {
                IPAddress ipAddress = new IPAddress(0);
                _listener = new TcpListener(ipAddress, 2222);
                _serverThread = new Thread(ServerHandler);
                _serverThread.Start();
            }
        }

        public void Stop()
        {
            if (_serverThread != null)
            {
                _serverThread.Abort();
                _serverThread = null;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Create web client simulating IE6.
            WebClient client = new WebClient();
            client.Headers["User-Agent"] =
            "Mozilla/4.0 (Compatible; Windows NT 5.1; MSIE 6.0)";

            // Download data.
            try
            {
                byte[] arr = client.DownloadData("http://" + webAddr1 + "/api/sensor");

                // Write values.
                string resp = Encoding.UTF8.GetString(arr);
                Console.WriteLine(resp);
                String user = null;
                String data = null;

                //example: "user:luis1:17.71,user:luis2:45.09"
                string[] parameters = resp.Split(',');
                foreach (string parameter in parameters)
                {
                    string[] keypair = parameter.Split(':');

                    if (keypair[0].Equals("user"))
                    {
                        user = keypair[1];
                        data = keypair[2];
                    }

                    if (user != null)
                    {
                        if (data != null)
                        {
                            switch (user)
                            {
                                case "luis1":
                                    sensors[0] = data;
                                    break;

                                case "luis2":
                                    sensors[1] = data;
                                    break;

                                default:
                                    break;

                            }
                        }
                    }
                }
                label1.Text = ("temp sensor 1: " + sensors[0]);
                label2.Text = ("temp sensor 2: " + sensors[1]);

            }
            catch (WebException ee)
            {

            }
        }

        String ReadRequest(NetworkStream stream)
        {
            MemoryStream contents = new MemoryStream();
            var buffer = new byte[2048];
            do
            {
                var size = stream.Read(buffer, 0, buffer.Length);
                if (size == 0)
                {
                    return null;
                }
                contents.Write(buffer, 0, size);
            } while (stream.DataAvailable);
            var retVal = Encoding.UTF8.GetString(contents.ToArray());
            return retVal;
        }

        void ServerHandler(Object o)
        {
            _listener.Start();
            while (true)
            {
                TcpClient client = _listener.AcceptTcpClient();
                NetworkStream stream = client.GetStream();

                try
                {
                    var request = ReadRequest(stream);

                    var responseBuilder = new StringBuilder();
                    responseBuilder.AppendLine("HTTP/1.1 200 OK");
                    responseBuilder.AppendLine("Content-Type: text/html");
                    responseBuilder.AppendLine();
                    responseBuilder.AppendLine("<html><head><title>CPE4020 Final Project</title></head><body>CPE 4020 Final Project<br>Sensor1: " + sensors[0] + "<br>Sensor2: " + sensors[1] + "</body></html>");
                    responseBuilder.AppendLine("");
                    var responseString = responseBuilder.ToString();
                    var responseBytes = Encoding.UTF8.GetBytes(responseString);

                    stream.Write(responseBytes, 0, responseBytes.Length);

                }
                finally
                {
                    stream.Close();
                    client.Close();
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
