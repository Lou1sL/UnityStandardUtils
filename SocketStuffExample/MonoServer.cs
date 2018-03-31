using UnityEngine;
using UnityStandardUtils.Web.SocketStuff;

public class MonoServer:MonoBehaviour
{
    private Server server;

    DataBuffer _databuffer = new DataBuffer();
    sSocketData _socketData = new sSocketData();

    void Start()
    {
        //创建服务器
        server = new Server(GameConst.IP, GameConst.Port, delegate (System.Net.Sockets.Socket myClientSocket,int receiveNumber,byte[] resultBuffer)
        {
            //把数据包给我发射回去！！！
            myClientSocket.Send(resultBuffer, receiveNumber, 0);

            //把数据包给我解码，该干嘛干嘛！
            //将收到的数据添加到缓存器中
            _databuffer.AddBuffer(resultBuffer, receiveNumber);

            //取出一条完整数据
            while (_databuffer.GetData(out _socketData))
            {
                if (_socketData._protocallType == (int)eProtocalCommand.sc_protobuf_login)
                {
                    gprotocol.CS_LOGINSERVER _tmpLoginServer = SocketManager.ProtoBuf_Deserialize<gprotocol.CS_LOGINSERVER>(_socketData._data);
                    Debug.Log(_tmpLoginServer.account);
                    Debug.Log(_tmpLoginServer.password);
                }
                else if(_socketData._protocallType == (int)eProtocalCommand.sc_binary_login)
                {
                    ByteStreamBuff _tmpbuff = new ByteStreamBuff(_socketData._data);
                    Debug.Log(_tmpbuff.Read_Int());
                    Debug.Log(_tmpbuff.Read_Float());
                    Debug.Log(_tmpbuff.Read_UniCodeString());
                    Debug.Log(_tmpbuff.Read_UniCodeString());
                    _tmpbuff.Close();
                    _tmpbuff = null;
                }
            }
        });
    }

    void OnApplicationQuit()
    {
        server.Stop();
    }


}