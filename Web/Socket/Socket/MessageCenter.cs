/// <summary>
/// 网络消息处理中心
/// 
/// create at 2014.8.26 by sun
/// </summary>


using System.Collections.Generic;

namespace UnityStandardUtils.Web.SocketStuff
{
    public struct sEvent_GameLogicData
    {
        public int _eventType;
        public object _eventData;
    }

    public struct sEvent_NetMessageData
    {
        public int _eventType;
        public byte[] _eventData;
    }

    public delegate void Callback_GameLogic_Handle(object _data);
    public delegate void Callback_NetMessage_Handle(byte[] _data);

    public class MessageCenter : SingletonMonoBehaviour<MessageCenter>
    {
        private Dictionary<int, Callback_NetMessage_Handle> _netMessage_EventList = new Dictionary<int, Callback_NetMessage_Handle>();
        public Queue<sEvent_NetMessageData> _netMessageDataQueue = new Queue<sEvent_NetMessageData>();

        private Dictionary<int, Callback_GameLogic_Handle> _gameLogic_EventList = new Dictionary<int, Callback_GameLogic_Handle>();
        public Queue<sEvent_GameLogicData> _gameLogicDataQueue = new Queue<sEvent_GameLogicData>();

        //添加网络事件观察者
        public void addObsever<T>(T protocalType, Callback_NetMessage_Handle callback)
        {
            if (!typeof(T).IsEnum) throw new System.ArgumentException("Please use Enum for protocalType!");
            int _protocalType = (int)(object)protocalType;


            if (_netMessage_EventList.ContainsKey(_protocalType))
            {
                _netMessage_EventList[_protocalType] += callback;
            }
            else
            {
                _netMessage_EventList.Add(_protocalType, callback);
            }
        }
        //删除网络事件观察者
        public void removeObserver<T>(T protocalType, Callback_NetMessage_Handle callback)
        {
            if (!typeof(T).IsEnum) throw new System.ArgumentException("Please use Enum for protocalType!");
            int _protocalType = (int)(object)protocalType;


            if (_netMessage_EventList.ContainsKey(_protocalType))
            {
                _netMessage_EventList[_protocalType] -= callback;
                if (_netMessage_EventList[_protocalType] == null)
                {
                    _netMessage_EventList.Remove(_protocalType);
                }
            }
        }


        //添加普通事件观察者
        public void AddEventListener<T>(T eventType, Callback_GameLogic_Handle callback)
        {
            if (!typeof(T).IsEnum) throw new System.ArgumentException("Please use Enum for eventType!");
            int _eventType = (int)(object)eventType;

            if (_gameLogic_EventList.ContainsKey(_eventType))
            {
                _gameLogic_EventList[_eventType] += callback;
            }
            else
            {
                _gameLogic_EventList.Add(_eventType, callback);
            }
        }
        //删除普通事件观察者
        public void RemoveEventListener<T>(T eventType, Callback_GameLogic_Handle callback)
        {
            if (!typeof(T).IsEnum) throw new System.ArgumentException("Please use Enum for eventType!");
            int _eventType = (int)(object)eventType;


            if (_gameLogic_EventList.ContainsKey(_eventType))
            {
                _gameLogic_EventList[_eventType] -= callback;
                if (_gameLogic_EventList[_eventType] == null)
                {
                    _gameLogic_EventList.Remove(_eventType);
                }
            }
        }
        //推送消息
        public void PostEvent<T>(T eventType, object data = null)
        {
            if (!typeof(T).IsEnum) throw new System.ArgumentException("Please use Enum for eventType!");
            int _eventType = (int)(object)eventType;


            if (_gameLogic_EventList.ContainsKey(_eventType))
            {
                _gameLogic_EventList[_eventType](data);
            }
        }



        void Update()
        {
            while (_gameLogicDataQueue.Count > 0)
            {
                sEvent_GameLogicData tmpGameLogicData = _gameLogicDataQueue.Dequeue();
                if (_gameLogic_EventList.ContainsKey(tmpGameLogicData._eventType))
                {
                    _gameLogic_EventList[tmpGameLogicData._eventType](tmpGameLogicData._eventData);
                }
            }

            while (_netMessageDataQueue.Count > 0)
            {
                lock (_netMessageDataQueue)
                {
                    sEvent_NetMessageData tmpNetMessageData = _netMessageDataQueue.Dequeue();
                    if (_netMessage_EventList.ContainsKey(tmpNetMessageData._eventType))
                    {
                        _netMessage_EventList[tmpNetMessageData._eventType](tmpNetMessageData._eventData);
                    }
                }
            }
        }
    }
}