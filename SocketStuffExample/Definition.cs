
/// <summary>
/// 网络配置
/// </summary>
public class GameConst
{
    public const string IP = "127.0.0.1";
    public const int Port = 9876;
}


/// <summary>
/// 网络事件ID
/// </summary>
public enum eProtocalCommand
{
    sc_binary_login = 0x1000,
    sc_protobuf_login = 0x2000,
}