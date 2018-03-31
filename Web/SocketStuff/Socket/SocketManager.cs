using System.Net.Sockets;
using System.Threading;
using System.Net;
using System;

namespace UnityStandardUtils.Web.SocketStuff
{
    internal class SocketManager
    {
        private static SocketManager _instance;
        internal static SocketManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SocketManager();
                }
                return _instance;
            }
        }
        private string _currIP;
        private int _currPort;

        private bool _isConnected = false;
        internal bool IsConnceted { get { return _isConnected; } }
        private Socket clientSocket = null;
        private Thread receiveThread = null;

        private PkgStruct.DataBuffer _databuffer = new PkgStruct.DataBuffer();

        byte[] _tmpReceiveBuff = new byte[4096];
        private PkgStruct.SocketData _socketData = new PkgStruct.SocketData();

        
        /// <summary>
        /// 连接
        /// </summary>
        private void _onConnet()
        {
            try
            {
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//创建套接字
                IPAddress ipAddress = IPAddress.Parse(_currIP);//解析IP地址
                IPEndPoint ipEndpoint = new IPEndPoint(ipAddress, _currPort);
                IAsyncResult result = clientSocket.BeginConnect(ipEndpoint, new AsyncCallback(_onConnect_Sucess), clientSocket);//异步连接
                bool success = result.AsyncWaitHandle.WaitOne(5000, true);
                if (!success) //超时
                {
                    _onConnect_Outtime();
                }
            }
            catch (System.Exception _e)
            {
                _onConnect_Fail();
            }
        }

        private void _onConnect_Sucess(IAsyncResult iar)
        {
            try
            {
                Socket client = (Socket)iar.AsyncState;
                client.EndConnect(iar);

                receiveThread = new Thread(new ThreadStart(_onReceiveSocket));
                receiveThread.IsBackground = true;
                receiveThread.Start();
                _isConnected = true;
                Console.WriteLine("Connection Established!");
            }
            catch (Exception _e)
            {
                Close();
            }
        }

        private void _onConnect_Outtime()
        {
            Close();
        }

        private void _onConnect_Fail()
        {
            Close();
        }

        /// <summary>
        /// 发送消息结果回掉，可判断当前网络状态
        /// </summary>
        /// <param name="asyncSend"></param>
        private void _onSendMsg(IAsyncResult asyncSend)
        {
            try
            {
                Socket client = (Socket)asyncSend.AsyncState;
                client.EndSend(asyncSend);
            }
            catch (Exception e)
            {
                Console.WriteLine("send msg exception:" + e.StackTrace);
            }
        }

        /// <summary>
        /// 接受网络数据
        /// </summary>
        private void _onReceiveSocket()
        {
            while (true)
            {
                if (!clientSocket.Connected)
                {
                    _isConnected = false;
                    break;
                }
                try
                {
                    int receiveLength = clientSocket.Receive(_tmpReceiveBuff);
                    if (receiveLength > 0)
                    {
                        _databuffer.AddBuffer(_tmpReceiveBuff, receiveLength);//将收到的数据添加到缓存器中
                        while (_databuffer.GetData(out _socketData))//取出一条完整数据
                        {
                            Event_NetMessageData tmpNetMessageData = new Event_NetMessageData();
                            tmpNetMessageData._eventType = _socketData._protocalType;
                            tmpNetMessageData._eventData = _socketData._data;

                            //锁死消息中心消息队列，并添加数据
                            lock (MessageCenter.Instance._netMessageDataQueue)
                            {
                                Console.WriteLine(tmpNetMessageData._eventType);
                                MessageCenter.Instance._netMessageDataQueue.Enqueue(tmpNetMessageData);
                            }
                        }
                    }
                }
                catch (System.Exception e)
                {
                    clientSocket.Disconnect(true);
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                    break;
                }
            }
        }

        /// <summary>
        /// 连接服务器
        /// </summary>
        /// <param name="_currIP"></param>
        /// <param name="_currPort"></param>
        internal void Connect(string _currIP, int _currPort)
        {
            if (!IsConnceted)
            {
                this._currIP = _currIP;
                this._currPort = _currPort;
                _onConnet();
            }
        }

        /// <summary>
        /// 发送消息基本方法
        /// </summary>
        /// <param name="_protocalType"></param>
        /// <param name="_data"></param>
        internal void SendMsgBase(int _protocalType, byte[] _data)
        {
            if (clientSocket == null || !clientSocket.Connected)
            {
                return;
            }

            byte[] _msgdata = PkgStruct.SocketDataToBytes(PkgStruct.BytesToSocketData(_protocalType, _data));
            clientSocket.BeginSend(_msgdata, 0, _msgdata.Length, SocketFlags.None, new AsyncCallback(_onSendMsg), clientSocket);
        }

        /// <summary>
        /// 断开
        /// </summary>
        internal void Close()
        {
            if (!_isConnected)
                return;

            _isConnected = false;

            if (receiveThread != null)
            {
                receiveThread.Abort();
                receiveThread = null;
            }

            if (clientSocket != null && clientSocket.Connected)
            {
                clientSocket.Close();
                clientSocket = null;
            }
        }
    }
}