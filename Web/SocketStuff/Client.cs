
using System.Threading;

namespace UnityStandardUtils.Web.SocketStuff
{
    public delegate Q TickMsgPasser<Q>();


    public static class Client
    {
        public static bool IsConnected => ClientSocketManager.Instance.IsConnceted;

        private static int _serverTick = 0;
        public static int ServerTick => _serverTick;
        internal static int SetServerTick { set => _serverTick = value; }

        private static float _serverLatency = 0f;
        public static float ServerLatency => _serverLatency;
        internal static float UpdateServerLatency { set => _serverLatency = value; }




        public static void Connect(string IP, int Port)
        {
            ClientSocketManager.Instance.Connect(IP, Port);
        }
        public static void Disconnect()
        {
            ClientSocketManager.Instance.Close();
        }


        /// <summary>
        /// 发送
        /// </summary>
        /// <param name="_protocalType"></param>
        /// <param name="data"></param>
        public static void SendMsg<T,Q>(T cmd, Q data)
        {
            if (!IsConnected) return;

            CheckEnum<T>();
            CheckData(data);
            byte[] rawData = (data is ProtoBuf.IExtensible) ? PkgStruct.ProtoBuf_Serializer((ProtoBuf.IExtensible)data):
                ((PkgStruct.ByteStreamBuff)(object)data).ToArray();

            ClientSocketManager.Instance.SendMsgBase((int)(object)cmd, rawData);
        }



        private static Thread _tickMsgThread;
        public static bool IsTickingMsgSending => (_tickMsgThread != null);
        /// <summary>
        /// 二进制Tick发送
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cmd"></param>
        /// <param name="data"></param>
        public static void SendTickMsg<T,Q>(T cmd, TickMsgPasser<Q> tickMsgPasser)
        {
            if (IsConnected && ServerTick <= 0) throw new System.Exception("Server Isn't Ticked!!");
            _tickMsgThread = new Thread(delegate()
            {
                while (true)
                {
                    if (tickMsgPasser != null)
                    {
                        SendMsg(cmd, tickMsgPasser());
                    }
                    Thread.Sleep((int)(1f / (ServerTick) * 1000));
                }
            });
            _tickMsgThread.Start();
        }

        public static void StopTickMsg()
        {
            if (_tickMsgThread != null)
            {
                _tickMsgThread.Abort();
                _tickMsgThread = null;
            }
            
        }




        public static void AddCallBackObserver<T>(T cmd, Callback_NetMessage_Handle callBack)
        {
            CheckEnum<T>();
            if (ClientMessageCenter.Instance) ClientMessageCenter.Instance.addObserver((int)(object)cmd, callBack);
        }
        public static void RemoveCallBackObserver<T>(T cmd, Callback_NetMessage_Handle callBack)
        {
            CheckEnum<T>();
            if (ClientMessageCenter.Instance) ClientMessageCenter.Instance.removeObserver((int)(object)cmd, callBack);
        }

        private static void CheckEnum<T>()
        {
            if (!typeof(T).IsEnum)
                throw new System.ArgumentException("Please use Enum for Command !");
        }
        private static void CheckData<Q>(Q data)
        {
            if(!(data is PkgStruct.ByteStreamBuff) && !(data is ProtoBuf.IExtensible))
                throw new System.ArgumentException("Please use ProtoBuf.IExtensible or PkgStruct.ByteStreamBuff for Data Type!");
        }



    }
}