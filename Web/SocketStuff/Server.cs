using System.Net.Sockets;
using System.Net;
using System.Threading;
using System;
using System.Diagnostics;

namespace UnityStandardUtils.Web.SocketStuff
{
    public delegate PkgStruct.SocketData MessageHandler(Socket clientSocket, PkgStruct.SocketData socketData);
    
    public class Server
    {
        private IPAddress ip;
        private int port;
        private int tick;
        private event MessageHandler messageHandle;


        private Socket serverSocket;
        private Thread listenClientThread;

        /// <summary>
        /// 设置服务器参数
        /// </summary>
        /// <param name="IP">服务器绑定的IP</param>
        /// <param name="Port">绑定端口</param>
        /// <param name="Tick">时钟频率，小于等于0时</param>
        /// <param name="messageHandler"></param>
        public Server(string IP, int Port,int Tick, MessageHandler messageHandler)
        {
            Console.WriteLine("-----------------------------------------------------------------------------------");
            Console.WriteLine("-----------------------------SocketStuff Server Engine-----------------------------");
            Console.WriteLine("-----------------------------               By RyuBAI -----------------------------");
            Console.WriteLine("-----------------------------------------------------------------------------------");
            Console.WriteLine();


            ip = IPAddress.Parse(IP);
            port = Port;
            tick = Tick;
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
                _writeConsole(clientSocket.RemoteEndPoint.ToString(), "Connected");
            }
        }


        private byte[] resultBuffer = new byte[1024];
        private PkgStruct.DataBuffer _databuffer = new PkgStruct.DataBuffer();
        private PkgStruct.SocketData _socketData = new PkgStruct.SocketData();

        /// <summary>  
        /// 接收消息
        /// </summary>  
        /// <param name="clientSocket"></param>
        private void ReceiveMessage(object clientSocket)
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

                        //将收到的数据添加到缓存器中
                        _databuffer.AddBuffer(resultBuffer, receiveLength);
                        //取出一条完整数据
                        while (_databuffer.GetData(out _socketData))
                        {
                            //如果数据属于内部协议
                            if (Enum.IsDefined(typeof(PkgStruct.InternalProtocol), _socketData._protocalType))
                            {
                                if(_socketData._protocalType == (int)PkgStruct.InternalProtocol.RequestServerTick)
                                {
                                    _writeConsole(ClientDetail, "Requesting Server Tick Rate,Which is (" + tick + ")");
                                    PkgStruct.ByteStreamBuff _tmpbuff = new PkgStruct.ByteStreamBuff();
                                    _tmpbuff.Write_Int(tick);
                                    byte[] repackage = PkgStruct.SocketDataToBytes(PkgStruct.BytesToSocketData((int)PkgStruct.InternalProtocol.ServerTick, _tmpbuff.ToArray()));
                                    myClientSocket.Send(repackage, repackage.Length, 0);
                                }


                            }
                            else
                            {
                                //_writeConsole(ClientDetail, "Calling Handler");
                                if (messageHandle != null)
                                {
                                    byte[] repackage = PkgStruct.SocketDataToBytes(messageHandle(myClientSocket, _socketData));
                                    myClientSocket.Send(repackage, repackage.Length, 0);
                                }
                                else
                                {
                                    //_writeConsole(ClientDetail, "No Handler,Doing Nothing Now...");
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _writeConsole(ClientDetail, "Client Droped With Reason (" + ex.Message + ")");
                    myClientSocket.Shutdown(SocketShutdown.Both);
                    myClientSocket.Close();

                    throw ex;

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

            _writeConsole("Srv", "Srv Start");
        }

        public void GentleStop()
        {
            if (listenClientThread != null)
            {
                _writeConsole("Srv", "Trying To Gentlely Stopping...");

                listenClientThread.Abort();
                listenClientThread = null;

                serverSocket.Close();
            }
        }




        /// <summary>
        /// 输出控制台
        /// </summary>
        /// <param name="clientDetail"></param>
        /// <param name="Msg"></param>
        private static void _writeConsole(string clientDetail,string Msg)
        {
            Console.WriteLine(">" + clientDetail + " => " + Msg);
        }
    }
}