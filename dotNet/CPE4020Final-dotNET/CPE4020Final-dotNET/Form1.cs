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



namespace CPE4020Final_dotNET
{


    public partial class Form1 : Form
    {

        //   luis1 luis2 other1 other2
        string[] sensors = { "", "", "", "" };

        public Form1()
        {


            InitializeComponent();
            button1.Text = "Start API";
            button2.Text = "Stop API";
            label1.Text = ("temp sensor 1: null");
            label2.Text = ("temp sensor 2: null");
            label3.Text = ("API off");        }

        delegate void SetChartCallback(string text);
        TcpListener server;
        TcpClient connclient;
        NetworkStream ns;
        WebClient client = null;
        Thread t = null;

        Int32 port = 2222;
        IPAddress localaddr = IPAddress.Parse("10.0.0.220");

        String webAddr = "10.0.0.185:2000";

        private void startServer()
        {
            server = new TcpListener(localaddr, port);
            server.Start();
            connclient = server.AcceptTcpClient();
            ns = connclient.GetStream();
            t = new Thread(DoWork);
            t.Start();
        }

        public void DoWork()
        {
            byte[] bytes = new byte[256];

            try
            {
                //read bytes
                int bytesRead = ns.Read(bytes, 0, bytes.Length);

                //respond
                byte[] msg = System.Text.Encoding.ASCII.GetBytes("OK");
                ns.Write(msg, 0, msg.Length);
                Console.WriteLine("Sent: {0}", "OK");

                //add temp to chart
                //this.AddTempPoint(Encoding.ASCII.GetString(bytes, 0, bytesRead));

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

            }

            connclient.Close();
            server.Stop();
            startServer();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Start();
            label3.Text = ("API on");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            label3.Text = ("API off");
            //client.Dispose();
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
                byte[] arr = client.DownloadData("http://" + webAddr + "/api/sensor");

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

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
