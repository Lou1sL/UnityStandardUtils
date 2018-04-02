using System;
using System.Diagnostics;
using System.Net.Sockets;
using UnityStandardUtils.Web.SocketStuff;

namespace SocketStuffExample
{
    class IndependentServer
    {
        private static Server server = null;
        
        private static float AX = 0f;
        private static float AY = 0f;

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

                    return socketData;
                }
                else if (socketData._protocalType == (int)ProtocalCommand.sc_binary_login)
                {
                    PkgStruct.ByteStreamBuff _tmpbuff = new PkgStruct.ByteStreamBuff(socketData._data);
                    Console.WriteLine(_tmpbuff.Read_UniCodeString());
                    _tmpbuff.Close();
                    _tmpbuff = null;

                    return socketData;
                }else if(socketData._protocalType == (int)ProtocalCommand.player_position)
                {
                    PkgStruct.ByteStreamBuff RcvData = new PkgStruct.ByteStreamBuff(socketData._data);
                    try
                    {
                        AX = RcvData.Read_Float();
                        AY = RcvData.Read_Float();

                    }catch(Exception e) {
                        Console.WriteLine("FUCK"+e.StackTrace);
                    }
                    RcvData.Close();
                    RcvData = null;

                    PkgStruct.ByteStreamBuff SendData = new PkgStruct.ByteStreamBuff();
                    SendData.Write_Float(AX);
                    SendData.Write_Float(AY);

                    
                    return PkgStruct.BytesToSocketData(socketData._protocalType, SendData.ToArray());
                }
                else
                {
                    return socketData;
                }
            });



            server.Start();

            Console.ReadLine();

            server.GentleStop();

        }
    }
}
