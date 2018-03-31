
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



        public static void SendMsg<T>(T cmd, ProtoBuf.IExtensible data)
        {
            if (!typeof(T).IsEnum) throw new System.ArgumentException("Please use Enum for command Base!");
            SocketManager.Instance.SendMsg((int)(object)cmd, data);
        }
        public static void SendMsg<T>(T cmd, ByteStreamBuff data)
        {
            if (!typeof(T).IsEnum) throw new System.ArgumentException("Please use Enum for command Base!");
            SocketManager.Instance.SendMsg((int)(object)cmd, data);
        }


        public static void AddCallBackObserver<T>(T cmd, Callback_NetMessage_Handle callBack)
        {
            if (!typeof(T).IsEnum) throw new System.ArgumentException("Please use Enum for command Base!");
            MessageCenter.Instance.addObserver((int)(object)cmd, callBack);
        }
        public static void RemoveCallBackObserver<T>(T cmd, Callback_NetMessage_Handle callBack)
        {
            if (!typeof(T).IsEnum) throw new System.ArgumentException("Please use Enum for command Base!");
            MessageCenter.Instance.removeObserver((int)(object)cmd, callBack);
        }
    }
}