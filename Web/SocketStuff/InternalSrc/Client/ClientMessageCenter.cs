/// <summary>
/// 网络消息处理中心
/// 
/// create at 2014.8.26 by sun
/// </summary>


using System.Collections.Generic;

namespace UnityStandardUtils.Web.SocketStuff
{

    public delegate void Callback_NetMessage_Handle(byte[] _data);

    public class ClientMessageCenter : SingletonMonoBehaviour<ClientMessageCenter>
    {
        private Dictionary<int, Callback_NetMessage_Handle> _netMessage_EventList = new Dictionary<int, Callback_NetMessage_Handle>();
        internal Queue<PkgStruct.SocketData> _netMessageDataQueue = new Queue<PkgStruct.SocketData>();

        //添加网络事件观察者
        internal void addObserver(int protocalType, Callback_NetMessage_Handle callback)
        {
            if (_netMessage_EventList.ContainsKey(protocalType))
            {
                _netMessage_EventList[protocalType] += callback;
            }
            else
            {
                _netMessage_EventList.Add(protocalType, callback);
            }
        }
        //删除网络事件观察者
        internal void removeObserver(int protocalType, Callback_NetMessage_Handle callback)
        {
            if (_netMessage_EventList.ContainsKey(protocalType))
            {
                _netMessage_EventList[protocalType] -= callback;
                if (_netMessage_EventList[protocalType] == null)
                {
                    _netMessage_EventList.Remove(protocalType);
                }
            }
        }
        
        void Update()
        {
            while (_netMessageDataQueue.Count > 0)
            {
                lock (_netMessageDataQueue)
                {
                    PkgStruct.SocketData tmpNetMessageData = _netMessageDataQueue.Dequeue();
                    if (_netMessage_EventList.ContainsKey(tmpNetMessageData._protocalType))
                    {
                        _netMessage_EventList[tmpNetMessageData._protocalType](tmpNetMessageData._data);
                    }
                }
            }
        }

        
    }
}