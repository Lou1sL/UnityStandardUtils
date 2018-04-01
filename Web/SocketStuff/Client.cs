
using System.Threading;

namespace UnityStandardUtils.Web.SocketStuff
{
    public static class Client
    {

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
        /// 以ProtoBuf方式发送
        /// </summary>
        /// <param name="_protocalType"></param>
        /// <param name="data"></param>
        public static void SendMsg<T>(T cmd, ProtoBuf.IExtensible data)
        {
            CheckEnum<T>();
            ClientSocketManager.Instance.SendMsgBase((int)(object)cmd, PkgStruct.ProtoBuf_Serializer(data));
        }

        /// <summary>
        /// 以二进制方式发送
        /// </summary>
        /// <param name="_protocalType"></param>
        /// <param name="_byteStreamBuff"></param>
        public static void SendMsg<T>(T cmd, PkgStruct.ByteStreamBuff data)
        {
            CheckEnum<T>();
            ClientSocketManager.Instance.SendMsgBase((int)(object)cmd, data.ToArray());
        }



        private static Thread _tickMsgThread;
        /// <summary>
        /// 二进制Tick发送
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cmd"></param>
        /// <param name="data"></param>
        public static void SendTickMsg<T>(T cmd, ref PkgStruct.ByteStreamBuff data)
        {
            PkgStruct.ByteStreamBuff _data = data;
            if (ServerTick <= 0) return;
            _tickMsgThread = new Thread(delegate()
            {
                while (true)
                {
                    CheckEnum<T>();
                    ClientSocketManager.Instance.SendMsgBase((int)(object)cmd, _data.ToArray());
                    Thread.Sleep((int)(1f / (ServerTick) * 1000));
                }
            });
            _tickMsgThread.Start();
        }

        public static void StopTickMsg()
        {
            _tickMsgThread.Abort();
            _tickMsgThread = null;
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
            if (!typeof(T).IsEnum) throw new System.ArgumentException("Please use Enum for command Base!");
        }




    }
}