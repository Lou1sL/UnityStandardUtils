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
            messageHandle = messageHandler;
            //服务器IP地址  
            IPAddress ip = IPAddress.Parse(IP);
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(ip, Port));  //绑定IP地址：端口  
            serverSocket.Listen(10);    //设定最多10个排队连接请求  
                                        //通过Clientsoket发送数据
            myThread = new Thread(ListenClientConnect);
            myThread.Start();
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
                Console.WriteLine(clientSocket.RemoteEndPoint.ToString());
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

            while (true)
            {
                try
                {
                    int receiveNumber = myClientSocket.Receive(resultBuffer);
                    if (receiveNumber > 0)
                    {
                        messageHandle?.Invoke(myClientSocket, receiveNumber, resultBuffer);
                    }

                }
                catch (System.Exception ex)
                {
                    System.Console.WriteLine(ex.Message);
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
                myThread = null;
            }
        }



    }
}