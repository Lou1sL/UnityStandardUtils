using System.Net.Sockets;
using System.Net;
using System.Threading;
using System;
using System.Diagnostics;

namespace UnityStandardUtils.Web.SocketStuff
{
    public delegate void MessageHandler(Socket clientSocket, PkgStruct.SocketData socketData);

    public class Server
    {
        private static IPAddress ip;
        private static int port;
        private static event MessageHandler messageHandle;


        private static Socket serverSocket;
        private static Thread listenClientThread;
        
        /// <summary>
        /// 设置服务器参数
        /// </summary>
        /// <param name="IP"></param>
        /// <param name="Port"></param>
        /// <param name="messageHandler"></param>
        public Server(string IP, int Port, MessageHandler messageHandler)
        {
            Console.WriteLine("-----------------------------------------------------------------------------------");
            Console.WriteLine("-----------------------------SocketStuff Server Engine-----------------------------");
            Console.WriteLine("-----------------------------               By RyuBAI -----------------------------");
            Console.WriteLine("-----------------------------------------------------------------------------------");
            Console.WriteLine();


            ip = IPAddress.Parse(IP);
            port = Port;
            messageHandle = messageHandler;


        }

        /// <summary>  
        /// 监听客户端连接  
        /// </summary>  
        private void ListenClientConnect()
        {
            while (true)
            {
                Socket clientSocket = serverSocket.Accept();
                Thread receiveThread = new Thread(ReceiveMessage);
                receiveThread.Start(clientSocket);
                Console.WriteLine(">Client Connected From(" + clientSocket.RemoteEndPoint.ToString() + ")");
            }
        }


        private static byte[] resultBuffer = new byte[1024];
        private static PkgStruct.DataBuffer _databuffer = new PkgStruct.DataBuffer();
        private static PkgStruct.SocketData _socketData = new PkgStruct.SocketData();

        /// <summary>  
        /// 接收消息
        /// </summary>  
        /// <param name="clientSocket"></param>
        private static void ReceiveMessage(object clientSocket)
        {
            Socket myClientSocket = (Socket)clientSocket;
            string ClientDetail = myClientSocket.RemoteEndPoint.ToString();

            while (true)
            {
                try
                {
                    int receiveLength = myClientSocket.Receive(resultBuffer);
                    if (receiveLength > 0)
                    {
                        Console.WriteLine(">RcvMsg From(" + ClientDetail + ") & Len =(" + receiveLength + ")");


                        //将收到的数据添加到缓存器中
                        _databuffer.AddBuffer(resultBuffer, receiveLength);
                        //取出一条完整数据
                        while (_databuffer.GetData(out _socketData))
                        {
                            Console.WriteLine(">Calling Handler");
                            messageHandle?.Invoke(myClientSocket, _socketData);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(">Client Droped From(" + ClientDetail + ")" + " With Reason (" + ex.Message + ")");
                    myClientSocket.Shutdown(SocketShutdown.Both);
                    myClientSocket.Close();
                    break;
                }
            }


        }

        public void Start()
        {
            GentleStop();

            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(ip, port));  //绑定IP地址：端口  
            serverSocket.Listen(10);    //设定最多10个排队连接请求  
                                        //通过Clientsoket发送数据
            listenClientThread = new Thread(ListenClientConnect);
            listenClientThread.Start();

            Console.WriteLine(">Srv Start");
        }

        public void GentleStop()
        {
            if (listenClientThread != null)
            {
                Console.WriteLine(">Srv Trying To Gentlely Stopping...");

                listenClientThread.Abort();
                listenClientThread = null;

                serverSocket.Close();
            }
        }
    }
}