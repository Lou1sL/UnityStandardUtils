using System.Net.Sockets;
using UnityEngine;
using UnityStandardUtils.Web.SocketStuff;

public class MonoServer:MonoBehaviour
{
    private Server server;

    void Start()
    {
        //创建服务器
        server = new Server(GameConst.IP, GameConst.Port, delegate (Socket myClientSocket, PkgStruct.SocketData socketData)
        {
            if (socketData._protocalType == (int)eProtocalCommand.sc_protobuf_login)
            {
                gprotocol.CS_LOGINSERVER _tmpLoginServer = PkgStruct.ProtoBuf_Deserialize<gprotocol.CS_LOGINSERVER>(socketData._data);
                Debug.Log(_tmpLoginServer.account);
                Debug.Log(_tmpLoginServer.password);
            }
            else if (socketData._protocalType == (int)eProtocalCommand.sc_binary_login)
            {
                PkgStruct.ByteStreamBuff _tmpbuff = new PkgStruct.ByteStreamBuff(socketData._data);
                Debug.Log(_tmpbuff.Read_Int());
                Debug.Log(_tmpbuff.Read_Float());
                Debug.Log(_tmpbuff.Read_UniCodeString());
                Debug.Log(_tmpbuff.Read_UniCodeString());
                _tmpbuff.Close();
                _tmpbuff = null;
            }

            //把数据包给我发射回去！！！
            byte[] repackage = PkgStruct.SocketDataToBytes(socketData);
            myClientSocket.Send(repackage, repackage.Length, 0);
        });

        server.Start();
    }

    void OnApplicationQuit()
    {
        server.GentleStop();
    }


}