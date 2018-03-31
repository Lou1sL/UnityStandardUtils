using UnityEngine;
using UnityStandardUtils.Web.SocketStuff;

public class MonoClient : MonoBehaviour
{

    void OnEnable()
    {
        //绑定数据包发送后的服务器回调处理函数
        Client.AddCallBackObserver(eProtocalCommand.sc_protobuf_login, CallBack_ProtoBuff_LoginServer);
        Client.AddCallBackObserver(eProtocalCommand.sc_binary_login, CallBack_Binary_LoginServer);
    }

    void OnDisable()
    {
        //解绑
        Client.RemoveCallBackObserver(eProtocalCommand.sc_protobuf_login, CallBack_ProtoBuff_LoginServer);
        Client.RemoveCallBackObserver(eProtocalCommand.sc_binary_login, CallBack_Binary_LoginServer);
    }

    void OnApplicationQuit()
    {
        OnButton_DisConnect();
    }

    /// <summary>
    /// 连接服务器
    /// </summary>
    public void OnButton_Connect()
    {
        Client.Connect(GameConst.IP, GameConst.Port);
    }
    /// <summary>
    /// 断开服务器
    /// </summary>
    public void OnButton_DisConnect()
    {
        Client.Disconnect();
    }


    /// <summary>
    /// 发送经过ProtoBuf编码的数据包
    /// </summary>
    public void OnButton_ProtoBuff_SendMsg()
    {
        gprotocol.CS_LOGINSERVER _cs_loginServer = new gprotocol.CS_LOGINSERVER();
        _cs_loginServer.account = "留白";
        _cs_loginServer.password = "真机智";
        Client.SendMsg(eProtocalCommand.sc_protobuf_login, _cs_loginServer);
    }
    /// <summary>
    /// 发送二进制数据包
    /// </summary>
    public void OnButton_Binary_SendMsg()
    {
        PkgStruct.ByteStreamBuff _tmpbuff = new PkgStruct.ByteStreamBuff();
        _tmpbuff.Write_Int(1234);
        _tmpbuff.Write_Float(56.78f);
        _tmpbuff.Write_UniCodeString("留白");
        _tmpbuff.Write_UniCodeString("真机智");
        Client.SendMsg(eProtocalCommand.sc_binary_login, _tmpbuff);
    }


    /// <summary>
    /// 发送后的回调
    /// </summary>
    /// <param name="_msgData"></param>
    private void CallBack_ProtoBuff_LoginServer(byte[] _msgData)
    {
        gprotocol.CS_LOGINSERVER _tmpLoginServer = PkgStruct.ProtoBuf_Deserialize<gprotocol.CS_LOGINSERVER>(_msgData);
        Debug.Log(_tmpLoginServer.account);
        Debug.Log(_tmpLoginServer.password);
    }
    /// <summary>
    /// 还是发送后的回调
    /// </summary>
    /// <param name="_msgData"></param>
    private void CallBack_Binary_LoginServer(byte[] _msgData)
    {
        PkgStruct.ByteStreamBuff _tmpbuff = new PkgStruct.ByteStreamBuff(_msgData);
        Debug.Log(_tmpbuff.Read_Int());
        Debug.Log(_tmpbuff.Read_Float());
        Debug.Log(_tmpbuff.Read_UniCodeString());
        Debug.Log(_tmpbuff.Read_UniCodeString());
        _tmpbuff.Close();
        _tmpbuff = null;
    }
}
