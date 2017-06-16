using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;

namespace MalukovKlient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender1, EventArgs e)//new
        {
            int port = 11000;
            byte[] bytes = new byte[1024];

            // Соединяемся с удаленным устройством

            // Устанавливаем удаленную точку для сокета
            IPHostEntry ipHost = Dns.GetHostEntry("localhost");
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, port);

            Socket sender = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Соединяем сокет с удаленной точкой
            sender.Connect(ipEndPoint);
            string message = string.Format("NEW {0} {1} {2} {3}",textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text);

            
            byte[] msg = Encoding.UTF8.GetBytes(message);
            for (int i = 0; i < msg.Length; i++)
                msg[i] += 1;
            // Отправляем данные через сокет
            int bytesSent = sender.Send(msg);

            // Получаем ответ от сервера
            int bytesRec = sender.Receive(bytes);

            richTextBox1.Text += Encoding.UTF8.GetString(bytes, 0, bytesRec) + "\r\n";

            // Используем рекурсию для неоднократного вызова SendMessageFromSocket()


            // Освобождаем сокет
            sender.Shutdown(SocketShutdown.Both);
            sender.Close();
        }

        private void button3_Click(object sender1, EventArgs e)//add
        {
            int port = 11000;
            byte[] bytes = new byte[1024];

            // Соединяемся с удаленным устройством

            // Устанавливаем удаленную точку для сокета
            IPHostEntry ipHost = Dns.GetHostEntry("localhost");
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, port);

            Socket sender = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Соединяем сокет с удаленной точкой
            sender.Connect(ipEndPoint);
            string message = string.Format("ADD {0} {1} {2} {3}", textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text);
            

            byte[] msg = Encoding.UTF8.GetBytes(message);
            for (int i = 0; i < msg.Length; i++)
                msg[i] += 1;
            // Отправляем данные через сокет
            int bytesSent = sender.Send(msg);

            // Получаем ответ от сервера
            int bytesRec = sender.Receive(bytes);

            richTextBox1.Text += Encoding.UTF8.GetString(bytes, 0, bytesRec) + "\r\n";

            // Используем рекурсию для неоднократного вызова SendMessageFromSocket()


            // Освобождаем сокет
            sender.Shutdown(SocketShutdown.Both);
            sender.Close();
        }

        private void button2_Click(object sender1, EventArgs e)//get
        {
            int port = 11000;
            byte[] bytes = new byte[1024];

            // Соединяемся с удаленным устройством

            // Устанавливаем удаленную точку для сокета
            IPHostEntry ipHost = Dns.GetHostEntry("localhost");
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, port);

            Socket sender = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Соединяем сокет с удаленной точкой
            sender.Connect(ipEndPoint);
            string message = string.Format("GET {0} {1} {2} {3}", textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text);


            byte[] msg = Encoding.UTF8.GetBytes(message);
            for (int i = 0; i < msg.Length; i++)
                msg[i] += 1;

            // Отправляем данные через сокет
            int bytesSent = sender.Send(msg);

            // Получаем ответ от сервера
            int bytesRec = sender.Receive(bytes);

            richTextBox1.Text += Encoding.UTF8.GetString(bytes, 0, bytesRec) + "\r\n";

            // Используем рекурсию для неоднократного вызова SendMessageFromSocket()


            // Освобождаем сокет
            sender.Shutdown(SocketShutdown.Both);
            sender.Close();
        }
    }
}
