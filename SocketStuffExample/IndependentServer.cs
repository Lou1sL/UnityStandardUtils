using System;
using UnityStandardUtils.Web.SocketStuff;

namespace SocketStuffExample
{
    class IndependentServer
    {
        private static Server server = null;

        private static DataBuffer _databuffer = new DataBuffer();
        private static sSocketData _socketData = new sSocketData();

        static void Main(string[] args)
        {

            //创建服务器
            server = new Server(GameConst.IP, GameConst.Port, delegate (System.Net.Sockets.Socket myClientSocket, int receiveNumber, byte[] resultBuffer)
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
                        Console.WriteLine(_tmpLoginServer.account);
                        Console.WriteLine(_tmpLoginServer.password);
                    }
                    else if (_socketData._protocallType == (int)eProtocalCommand.sc_binary_login)
                    {
                        ByteStreamBuff _tmpbuff = new ByteStreamBuff(_socketData._data);
                        Console.WriteLine(_tmpbuff.Read_Int());
                        Console.WriteLine(_tmpbuff.Read_Float());
                        Console.WriteLine(_tmpbuff.Read_UniCodeString());
                        Console.WriteLine(_tmpbuff.Read_UniCodeString());
                        _tmpbuff.Close();
                        _tmpbuff = null;
                    }
                }
            });

            server.Stop();
        }
    }
}
