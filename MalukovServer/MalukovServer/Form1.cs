using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;


namespace MalukovServer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            PersonList = new Person[100];
            count =-1;
            string pathToFile = "list.txt";
            string data = File.ReadAllText(pathToFile);
            string[] readEveryLine = data.Split(new char[] { ' ', '\n', '\r' });
            int j = 0;
            string[] str = new string[100];
            foreach (var item in readEveryLine)
            {
                if (item != "")
                {
                    str[j] = item;
                    j++;
                }
            }
            bool flag = true;
            
            for(int i=0;i< j; )
            {
                if (flag)
                {
                    count++;
                    PersonList[count] = new Person(str[i] + ' ' + str[i + 1], str[i + 2]);

                    i += 3;
                    flag = false;
                }
                if (str[i + 1] == str[i + 1].TrimEnd(';'))
                    PersonList[count].addLogPas(str[i], str[i + 1]);
                else
                {
                    PersonList[count].addLogPas(str[i], str[i + 1].TrimEnd(';'));
                    flag = true;
                }
                i += 2;

            }
            richTextBox1.Text += string.Format("Загруженно {0} пользователей\r\n", count + 1);

        }
        void addText(string _s)
        {
            richTextBox1.Text += _s;
        }
        public  void New(string[] data)
        {
            int i = 1;
            count++;

            PersonList[count] = new Person(data[i] + ' ' + data[i + 1], data[i + 2]);
            PersonList[count].addLogPas(data[i + 3], data[i + 4]);
            richTextBox1.BeginInvoke((Action)(() =>
            {
                richTextBox1.Text += "Добавлен пользователь\r\n";
            }));

        }
        public  void Add(string[] data)
        {
            int i = 1;
            int j = 0;
            bool flag = false;
            for (;j<count+1;j++)
                if(PersonList[j].Name== data[i] + ' ' + data[i + 1]
                    && PersonList[j].Password==data[i+2])
                {
                    flag = true;
                    break;
                }
            if (flag)
            {
                PersonList[j].addLogPas(data[i + 3], data[i + 4]);
                richTextBox1.BeginInvoke((Action)(() =>
                {
                    
                
                richTextBox1.Text += "Добавлена учетная запись для существущего пользователя\r\n";
                }));
            }
        }
        private void connect_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = new Bitmap(Properties.Resources.w8);
            Thread connection = new Thread(Connect);
            connection.Start();

        }
        public  void Connect()
        {
            IPHostEntry ipHost = Dns.GetHostEntry("localhost");
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 11000);

            // Создаем сокет Tcp/Ip
            Socket sListener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Назначаем сокет локальной конечной точке и слушаем входящие сокеты
            sListener.Bind(ipEndPoint);
            sListener.Listen(10);

            while (true)
            {
                Socket handler = sListener.Accept();
                pictureBox1.BeginInvoke((Action)(() =>
                {


                    pictureBox1.Image = Properties.Resources.check;
                }));
                string data = null;

                // Мы дождались клиента, пытающегося с нами соединиться

                byte[] bytes = new byte[1024];
                int bytesRec = handler.Receive(bytes);
                for (int k = 0; k < bytesRec; k++)
                    bytes[k] -= 1;
                //command; name pas
                data += Encoding.UTF8.GetString(bytes, 0, bytesRec);
                string[] split = data.Split(' ');
                switch (split[0])
                {
                    case ("NEW"):
                        {
                            New(split);
                            break;
                        }
                    case ("ADD"):
                        {
                            Add(split);
                            break;
                        }
                    case ("GET"):
                        {
                            int i = 1;
                            int j = 0;
                            bool flag = false;
                            for (; j < count + 1; j++)
                                if (PersonList[j].Name == split[i] + ' ' + split[i + 1]
                                    && PersonList[j].Password == split[i + 2])
                                {
                                    flag = true;
                                    break;
                                }
                            if (flag)
                            {
                                byte[] msg = Encoding.UTF8.GetBytes(PersonList[j].ToString());
                                handler.Send(msg);
                                richTextBox1.BeginInvoke((Action)(() =>
                                {


                                    richTextBox1.Text += "Клиенту отправлена информация\r\n";
                                }));
                                
                            }
                            break;
                        }
                    default:
                        break;
                }

                string reply = "Спасибо за запрос в " + data.Length.ToString()
                        + " символов";


                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
                Thread.Sleep(1000);
                pictureBox1.BeginInvoke((Action)(() =>
                {


                    pictureBox1.Image = Properties.Resources.w8;
                }));
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Save_Click(object sender, EventArgs e)
        {
           
        }
    }
}
 public class Person
{
   public  string Name;
 public   string Password;
     public  Dictionary<string, string> LogPasList;
    public Person(string _n,string _p)
    {
        Name = _n;
        Password = _p;
        LogPasList = new Dictionary<string, string>();
    }
  public  void addLogPas(string _l, string _p)
    {
        LogPasList[_l] = _p;
    }
    public override string ToString()
    {
        string str = "";
        str += String.Format("{0} {1}\r\n", Name, Password);
        var data = LogPasList.ToArray();
        foreach (var item in data)
        {
            str += string.Format("{0} {1}\r\n", item.Key, item.Value);
        }
        str = str.TrimEnd('\r', '\n');
        str += ";\r\n";
        return str;
    }
}
