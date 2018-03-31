
namespace UnityStandardUtils.Web.SocketStuff
{
    public static class Client
    {

        public static void Connect(string IP, int Port)
        {
            SocketManager.Instance.Connect(IP, Port);
        }
        public static void Disconnect()
        {
            SocketManager.Instance.Close();
        }


        /// <summary>
        /// 以ProtoBuf方式发送
        /// </summary>
        /// <param name="_protocalType"></param>
        /// <param name="data"></param>
        public static void SendMsg<T>(T cmd, ProtoBuf.IExtensible data)
        {
            if (!typeof(T).IsEnum) throw new System.ArgumentException("Please use Enum for command Base!");
            SocketManager.Instance.SendMsgBase((int)(object)cmd, PkgStruct.ProtoBuf_Serializer(data));
        }

        /// <summary>
        /// 以二进制方式发送
        /// </summary>
        /// <param name="_protocalType"></param>
        /// <param name="_byteStreamBuff"></param>
        public static void SendMsg<T>(T cmd, PkgStruct.ByteStreamBuff data)
        {
            if (!typeof(T).IsEnum) throw new System.ArgumentException("Please use Enum for command Base!");
            SocketManager.Instance.SendMsgBase((int)(object)cmd, data.ToArray());
        }


        public static void AddCallBackObserver<T>(T cmd, Callback_NetMessage_Handle callBack)
        {
            if (!typeof(T).IsEnum) throw new System.ArgumentException("Please use Enum for command Base!");
            if (MessageCenter.Instance) MessageCenter.Instance.addObserver((int)(object)cmd, callBack);
        }
        public static void RemoveCallBackObserver<T>(T cmd, Callback_NetMessage_Handle callBack)
        {
            if (!typeof(T).IsEnum) throw new System.ArgumentException("Please use Enum for command Base!");
            if(MessageCenter.Instance) MessageCenter.Instance.removeObserver((int)(object)cmd, callBack);
        }
    }
}