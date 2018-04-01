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

            //创建服务器
            server = new Server(GameConst.IP, GameConst.Port, delegate (Socket myClientSocket, PkgStruct.SocketData socketData)
            {

                if (socketData._protocalType == (int)eProtocalCommand.sc_protobuf_login)
                {
                    gprotocol.CS_LOGINSERVER _tmpLoginServer = PkgStruct.ProtoBuf_Deserialize<gprotocol.CS_LOGINSERVER>(socketData._data);
                    Console.WriteLine(_tmpLoginServer.account);
                    Console.WriteLine(_tmpLoginServer.password);
                }
                else if (socketData._protocalType == (int)eProtocalCommand.sc_binary_login)
                {
                    PkgStruct.ByteStreamBuff _tmpbuff = new PkgStruct.ByteStreamBuff(socketData._data);
                    Console.WriteLine(_tmpbuff.Read_Int());
                    Console.WriteLine(_tmpbuff.Read_Float());
                    Console.WriteLine(_tmpbuff.Read_UniCodeString());
                    Console.WriteLine(_tmpbuff.Read_UniCodeString());
                    _tmpbuff.Close();
                    _tmpbuff = null;
                }
                //把数据包给我发射回去！！！
                byte[] repackage = PkgStruct.SocketDataToBytes(socketData);
                myClientSocket.Send(repackage, repackage.Length, 0);

            });

            server.Start();

            Console.ReadLine();

            server.GentleStop();

        }
    }
}
