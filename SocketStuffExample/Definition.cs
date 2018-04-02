
/// <summary>
/// 网络配置
/// </summary>
public class GameConst
{
    public const string IP = "192.168.1.102";
    public const int Port = 9876;

    //服务器调用的配置
    public const int Tick = 60;
}


/// <summary>
/// 网络事件ID；
/// 在服务器，它的长度为UInt16，
/// 因此数值上不能大于0xFFFF。
/// 同时，0xF000-0xFFFF是保留报文格式，仅内部使用。
/// 因此，可用的事件ID范围为：
/// 0x0000-0xEFFF
/// </summary>
public enum ProtocalCommand
{
    sc_binary_login = 0x1000,
    sc_protobuf_login = 0x2000,


    player_position = 0x3000,
}