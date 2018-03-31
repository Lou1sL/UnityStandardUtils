using System.Net.Sockets;
using System.Net;
using System.Threading;
using System;

namespace UnityStandardUtils.Web.SocketStuff
{
    public delegate void MessageHandler(Socket clientSocket, int receiveNumber, byte[] resultBuffer);

    public class Server
    {

        private static Socket serverSocket;
        private Thread myThread;
        private static event MessageHandler messageHandle;

        public Server(string IP, int Port, MessageHandler messageHandler)
        {


            Console.WriteLine("-----------------------------------------------------------------------------------");
            Console.WriteLine("-----------------------------SocketStuff Server Engine-----------------------------");
            Console.WriteLine("-----------------------------               By RyuBAI -----------------------------");
            Console.WriteLine("-----------------------------------------------------------------------------------");
            Console.WriteLine();


            messageHandle = messageHandler;
            //服务器IP地址  
            IPAddress ip = IPAddress.Parse(IP);
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(ip, Port));  //绑定IP地址：端口  
            serverSocket.Listen(10);    //设定最多10个排队连接请求  
                                        //通过Clientsoket发送数据
            myThread = new Thread(ListenClientConnect);
            myThread.Start();
            
            Console.WriteLine(">Srv Start");
            Console.ReadLine();
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
                Console.WriteLine(">Client Connected From("+clientSocket.RemoteEndPoint.ToString()+")");
            }
        }


        private static byte[] resultBuffer = new byte[1024];
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
                    int receiveNumber = myClientSocket.Receive(resultBuffer);
                    if (receiveNumber > 0)
                    {
                        Console.WriteLine(">RcvMsg From(" + ClientDetail + ") & Len =(" + receiveNumber + ")");
                        Console.WriteLine(">Calling Handler");
                        messageHandle?.Invoke(myClientSocket, receiveNumber, resultBuffer);
                    }

                }
                catch (System.Exception ex)
                {
                    Console.WriteLine(">Client Droped From(" + ClientDetail + ")" + " With Reason (" + ex.Message + ")");
                    myClientSocket.Shutdown(SocketShutdown.Both);
                    myClientSocket.Close();
                    break;
                }
            }


        }

        public void Stop()
        {

            if (myThread != null)
            {
                myThread.Abort();
                Console.WriteLine(">Srv Stop");
                myThread = null;
            }
        }



    }
}