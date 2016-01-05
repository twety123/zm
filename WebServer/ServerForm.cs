using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebServer
{
    public partial class ServerForm : Form
    {
        public ServerForm()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        /// <summary>
        /// 启动socket服务按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_begin_Click(object sender, EventArgs e)
        {
            //1，设置顶级的监听端口的socket对象
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipAddress;
            //判断输入的IP地址是否合法
            if (!IPAddress.TryParse(txt_IP.Text.Trim(), out ipAddress))
            {
                return;
            }
            int port;
            if (!int.TryParse(txt_Port.Text.Trim(), out port))
            {
                return;
            }
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, port);

            //开始顶级socket的半丁和监听
            try
            {
                serverSocket.Bind(ipEndPoint);
                serverSocket.Listen(10);
                SetLogText("服务器已经开启...");
                //
                Thread thread = new Thread(Listen);
                //设置为后台线程
                thread.IsBackground = true;
                thread.Start(serverSocket);
            }
            catch (Exception ex)
            {
                SetLogText("服务器已经开启，您无需重复开启！");
                SetLogText(" >详细信息：\r\n " + ex.Message);
            }
        }

        private void SetLogText(string msg)
        {
            txt_log.AppendText(msg + "\r\n");
        }

        private void Listen(object o)
        {
            Socket serverSocket = o as Socket;
            while (true)
            {
                //讲服务监听到的连接，转换成一个socket对象，进行http请求接收和响应
                Socket connSocket = serverSocket.Accept();
                SetLogText(connSocket.RemoteEndPoint + ":已建立连接！");
                try
                {
                    //声明接收http请求的二进制数组
                    byte[] buffer = new byte[1024 * 1024];
                    //将接收到的二进制字节存放到声明的二进制数组中
                    int realLen = connSocket.Receive(buffer);
                    if (realLen <= 0)
                    {
                        connSocket.Shutdown(SocketShutdown.Both);

                        SetLogText(connSocket.RemoteEndPoint + ":0字节请求，当前连接已经关闭！");
                        connSocket.Close();
                        return;
                    }
                    //如果当前接收的http请求是正常的，则进行http请求报文的分析，并生成http响应报文
                    string content = Encoding.UTF8.GetString(buffer, 0, realLen);
                    SetLogText(content);
                    Request request = new Request(content);

                }
                catch (Exception)
                {
                    SetLogText("当前连接发生异常，请重启服务！");
                    connSocket.Close();
                    break;
                }
            }
        }

        /// <summary>
        /// 判断请求的是动态页面还是静态页面，并分别针对，进行http响应处理
        /// </summary>
        /// <param name="rawUrl"></param>
        /// <param name="connSocket"></param>
        private void RequestStaticOrDynamicPage(string rawUrl, Socket connSocket)
        {
            string ext = Path.GetExtension(rawUrl);
            switch (ext)
            {
                case ".aspx":
                case ".asp":
                case ".php":
                case ".jsp":
                    break;
                default:
                    ProcessStaticPageRequest(rawUrl, connSocket);
                    break;
            }
        }

        /// <summary>
        /// 处理http静态页面的请求
        /// </summary>
        /// <param name="rawUrl"></param>
        /// <param name="connSocket"></param>
        private void ProcessStaticPageRequest(string rawUrl, Socket connSocket)
        {
            //拼接屋里路径的字符串，检测当前物理路径的文件是否存在
            //注意Path.Combine()方法中，第二个开始后的参数，开头的/要去掉，否则拼接出来的路径将从后面的
            //以/的字符串开始进行拼接，也就是忽略掉，/前面的拼接路径字符串
            rawUrl = rawUrl.TrimStart('/');
            //完整路径
            string physicalPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "web", rawUrl);
            //判断文件是否存在
            if (File.Exists(physicalPath))
            {
                using (FileStream fs = new FileStream(physicalPath, FileMode.Open))
                {
                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    //准备发送响应报文
                    string ext = Path.GetExtension(rawUrl);
                    Response response = new Response(200, buffer, ext);
                    //发送响应报文，关闭当前socket连接，注意这里体现了http协议的无状态的基本原因
                    connSocket.Send(response.GetResponse());
                    SetLogText(connSocket.RemoteEndPoint + ":已经关闭连接");
                    connSocket.Close();
                }
            }
            else
            {
                //不存在
            }
        }
    }
}
