using System.Net.Sockets;
using System.Threading;
using System.Net;
using System;

namespace UnityStandardUtils.Web.SocketStuff
{
    internal class ClientSocketManager:Singleton<ClientSocketManager>
    {
        private bool _isConnected = false;
        internal bool IsConnceted => _isConnected;

        private Socket clientSocket = null;
        private Thread receiveThread = null;

        

        byte[] _tmpReceiveBuff = new byte[4096];
        private PkgStruct.DataBuffer _databuffer = new PkgStruct.DataBuffer();
        private PkgStruct.SocketData _socketData = new PkgStruct.SocketData();



        /// <summary>
        /// 连接服务器
        /// </summary>
        /// <param name="_currIP"></param>
        /// <param name="_currPort"></param>
        internal void Connect(string _currIP, int _currPort)
        {
            if (!IsConnceted)
            {
                try
                {
                    //创建套接字
                    clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    //解析IP地址
                    IPAddress ipAddress = IPAddress.Parse(_currIP);
                    IPEndPoint ipEndpoint = new IPEndPoint(ipAddress, _currPort);
                    //异步连接
                    IAsyncResult result = clientSocket.BeginConnect(ipEndpoint, new AsyncCallback(_onConnect_Sucess), clientSocket);
                    bool success = result.AsyncWaitHandle.WaitOne(5000, true);

                    if (!success)
                    {
                        //超时
                        Close();
                    }
                }
                catch (System.Exception _e)
                {
                    //失败
                    Close();
                }


            }
        }

        /// <summary>
        /// 连接成功，建立接受线程
        /// </summary>
        /// <param name="iar"></param>
        private void _onConnect_Sucess(IAsyncResult iar)
        {
            try
            {
                Socket client = (Socket)iar.AsyncState;
                client.EndConnect(iar);

                receiveThread = new Thread(new ThreadStart(_onReceiveSocket));
                receiveThread.IsBackground = true;
                receiveThread.Start();
                //_isConnected = true;
                //Console.WriteLine("Connection Established!");


                _requestServerTick();
            }
            catch (Exception _e)
            {
                Close();
            }
        }

        /// <summary>
        /// 连接成功第一时间获取Tick
        /// </summary>
        private void _requestServerTick()
        {
            SendMsgBase((int)PkgStruct.InternalProtocol.RequestServerTick, new byte[] { Convert.ToByte('A') });
        }

        /// <summary>
        /// 接受网络数据，将接受到的放入消息队列
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
                        //将收到的数据添加到缓存器中
                        _databuffer.AddBuffer(_tmpReceiveBuff, receiveLength);
                        //取出一条完整数据
                        while (_databuffer.GetData(out _socketData))
                        {

                            //如果数据属于内部协议
                            if(Enum.IsDefined(typeof(PkgStruct.InternalProtocol), _socketData._protocalType))
                            {
                                if (_socketData._protocalType == (int)PkgStruct.InternalProtocol.ServerTick)
                                {
                                    PkgStruct.ByteStreamBuff _tmpbuff = new PkgStruct.ByteStreamBuff(_socketData._data);
                                    Client.SetServerTick = _tmpbuff.Read_Int();
                                    _tmpbuff.Close();

                                    _isConnected = true;
                                }
                            }
                            else
                            {
                                //锁死消息中心消息队列，并添加数据
                                lock (ClientMessageCenter.Instance._netMessageDataQueue)
                                {
                                    ClientMessageCenter.Instance._netMessageDataQueue.Enqueue(_socketData);
                                }
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
        /// 发送消息结果回调，可判断当前网络状态
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
                //Console.WriteLine("send msg exception:" + e.StackTrace);
            }
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