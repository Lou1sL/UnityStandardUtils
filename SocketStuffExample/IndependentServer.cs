using System;
using System.Diagnostics;
using System.Net.Sockets;
using UnityStandardUtils.Web.SocketStuff;

namespace SocketStuffExample
{
    class IndependentServer
    {
        private static Server server = null;

        
        static void Main(string[] args)
        {
            //一个最简单的服务器，只需要一行
            //它所唯一做的是监听连接
            //server = new Server(GameConst.IP, GameConst.Port, GameConst.Tick, null);


            //创建服务器
            server = new Server(GameConst.IP, GameConst.Port, GameConst.Tick, delegate (Socket myClientSocket, PkgStruct.SocketData socketData)
            {

                if (socketData._protocalType == (int)ProtocalCommand.sc_protobuf_login)
                {
                    gprotocol.CS_LOGINSERVER _tmpLoginServer = PkgStruct.ProtoBuf_Deserialize<gprotocol.CS_LOGINSERVER>(socketData._data);
                    Console.WriteLine(_tmpLoginServer.account);
                    Console.WriteLine(_tmpLoginServer.password);
                }
                else if (socketData._protocalType == (int)ProtocalCommand.sc_binary_login)
                {
                    PkgStruct.ByteStreamBuff _tmpbuff = new PkgStruct.ByteStreamBuff(socketData._data);
                    Console.WriteLine(_tmpbuff.Read_UniCodeString());
                    _tmpbuff.Close();
                    _tmpbuff = null;
                }
                //把数据包给我发射回去！！！
                return socketData;
            });



            server.Start();

            Console.ReadLine();

            server.GentleStop();

        }
    }
}
