using System;
using System.IO;
using System.Text;

namespace UnityStandardUtils.Web.SocketStuff
{
    public class PkgStruct
    {
        /// <summary>
        /// 计算用常量
        /// </summary>
        private class Constants
        {
            //消息：数据总长度(4byte) + 数据类型(2byte) + 数据(N byte)
            public static int HEAD_DATA_LEN = 4;
            public static int HEAD_TYPE_LEN = 2;
            public static int HEAD_LEN//6byte
            {
                get { return HEAD_DATA_LEN + HEAD_TYPE_LEN; }
            }
        }

        /// <summary>
        /// 二进制数据流
        /// </summary>
        public class ByteStreamBuff
        {
            private MemoryStream stream = null;
            private BinaryWriter writer = null;
            private BinaryReader reader = null;

            public ByteStreamBuff()
            {
                stream = new MemoryStream();
                writer = new BinaryWriter(stream);
            }

            public ByteStreamBuff(byte[] _data)
            {
                stream = new MemoryStream(_data);
                reader = new BinaryReader(stream);
            }

            public void Write_Byte(byte _data)
            {
                writer.Write(_data);
            }
            public void Write_Bytes(byte[] _data)
            {
                writer.Write(_data.Length);
                writer.Write(_data);
            }
            public void Write_Int(int _data)
            {
                writer.Write(_data);
            }
            public void Write_uInt(uint _data)
            {
                writer.Write(_data);
            }
            public void Write_Short(short _data)
            {
                writer.Write(_data);
            }
            public void Write_uShort(ushort _data)
            {
                writer.Write(_data);
            }
            public void Write_Long(long _data)
            {
                writer.Write(_data);
            }
            public void Write_uLong(ulong _data)
            {
                writer.Write(_data);
            }
            public void Write_Bool(bool _data)
            {
                writer.Write(_data);
            }
            public void Write_Float(float _data)
            {
                byte[] temp = flip(BitConverter.GetBytes(_data));
                writer.Write(temp.Length);
                writer.Write(BitConverter.ToSingle(temp, 0));
            }
            public void Write_Double(double _data)
            {
                byte[] temp = flip(BitConverter.GetBytes(_data));
                writer.Write(temp.Length);
                writer.Write(BitConverter.ToDouble(temp, 0));
            }
            public void Write_UTF8String(string _data)
            {
                byte[] temp = Encoding.UTF8.GetBytes(_data);
                writer.Write(temp.Length);
                writer.Write(temp);
            }
            public void Write_UniCodeString(string _data)
            {
                byte[] temp = Encoding.Unicode.GetBytes(_data);
                writer.Write(temp.Length);
                writer.Write(temp);
            }

            public byte Read_Byte()
            {
                return reader.ReadByte();
            }
            public byte[] Read_Bytes()
            {
                int len = Read_Int();
                return reader.ReadBytes(len);
            }
            public int Read_Int()
            {
                return reader.ReadInt32();
            }
            public uint Read_uInt()
            {
                return reader.ReadUInt32();
            }
            public short Read_Short()
            {
                return reader.ReadInt16();
            }
            public ushort Read_uShort()
            {
                return reader.ReadUInt16();
            }
            public long Read_Long()
            {
                return reader.ReadInt64();
            }
            public ulong Read_uLong()
            {
                return reader.ReadUInt64();
            }
            public bool Read_Bool()
            {
                return reader.ReadBoolean();
            }
            public float Read_Float()
            {
                return BitConverter.ToSingle(flip(Read_Bytes()), 0);
            }
            public double Read_Double()
            {
                return BitConverter.ToDouble(flip(Read_Bytes()), 0);
            }
            public string Read_UTF8String()
            {
                return Encoding.UTF8.GetString(Read_Bytes());
            }
            public string Read_UniCodeString()
            {
                return Encoding.Unicode.GetString(Read_Bytes());
            }

            private byte[] flip(byte[] _data)
            {
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(_data);
                return _data;
            }
            public byte[] ToArray()
            {
                stream.Flush();
                return stream.ToArray();
            }

            public void Close()
            {
                if (stream != null)
                {
                    stream.Close();
                    stream = null;
                }
                if (writer != null)
                {
                    writer.Close();
                    writer = null;
                }
                if (reader != null)
                {
                    reader.Close();
                    reader = null;
                }
            }
        }

        
        /// <summary>
        /// 网络数据结构
        /// </summary>
        [System.Serializable]
        public struct SocketData
        {
            public byte[] _data;
            public int _protocalType;
            public int _buffLength;
            public int _dataLength;
        }

        /// <summary>
        /// 网络数据缓存器，
        /// </summary>
        [System.Serializable]
        internal class DataBuffer
        {
            //自动大小数据缓存器
            private int _minBuffLen;
            private byte[] _buff;
            private int _curBuffPosition;
            private int _buffLength = 0;
            private int _dataLength;
            private UInt16 _protocalType;

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="_minBuffLen">最小缓冲区大小</param>
            public DataBuffer(int _minBuffLen = 1024)
            {
                if (_minBuffLen <= 0)
                {
                    this._minBuffLen = 1024;
                }
                else
                {
                    this._minBuffLen = _minBuffLen;
                }
                _buff = new byte[this._minBuffLen];
            }

            /// <summary>
            /// 添加缓存数据
            /// </summary>
            /// <param name="_data"></param>
            /// <param name="_dataLen"></param>
            public void AddBuffer(byte[] _data, int _dataLen)
            {
                if (_dataLen > _buff.Length - _curBuffPosition)//超过当前缓存
                {
                    byte[] _tmpBuff = new byte[_curBuffPosition + _dataLen];
                    Array.Copy(_buff, 0, _tmpBuff, 0, _curBuffPosition);
                    Array.Copy(_data, 0, _tmpBuff, _curBuffPosition, _dataLen);
                    _buff = _tmpBuff;
                    _tmpBuff = null;
                }
                else
                {
                    Array.Copy(_data, 0, _buff, _curBuffPosition, _dataLen);
                }
                _curBuffPosition += _dataLen;//修改当前数据标记
            }

            /// <summary>
            /// 更新数据长度
            /// </summary>
            public void UpdateDataLength()
            {
                if (_dataLength == 0 && _curBuffPosition >= Constants.HEAD_LEN)
                {
                    byte[] tmpDataLen = new byte[Constants.HEAD_DATA_LEN];
                    Array.Copy(_buff, 0, tmpDataLen, 0, Constants.HEAD_DATA_LEN);
                    _buffLength = BitConverter.ToInt32(tmpDataLen, 0);

                    byte[] tmpProtocalType = new byte[Constants.HEAD_TYPE_LEN];
                    Array.Copy(_buff, Constants.HEAD_DATA_LEN, tmpProtocalType, 0, Constants.HEAD_TYPE_LEN);
                    _protocalType = BitConverter.ToUInt16(tmpProtocalType, 0);

                    _dataLength = _buffLength - Constants.HEAD_LEN;
                }
            }

            /// <summary>
            /// 获取一条可用数据，返回值标记是否有数据
            /// </summary>
            /// <param name="_tmpSocketData"></param>
            /// <returns></returns>
            public bool GetData(out SocketData _tmpSocketData)
            {
                _tmpSocketData = new SocketData();

                if (_buffLength <= 0)
                {
                    UpdateDataLength();
                }

                if (_buffLength > 0 && _curBuffPosition >= _buffLength)
                {
                    _tmpSocketData._buffLength = _buffLength;
                    _tmpSocketData._dataLength = _dataLength;
                    _tmpSocketData._protocalType = _protocalType;
                    _tmpSocketData._data = new byte[_dataLength];
                    Array.Copy(_buff, Constants.HEAD_LEN, _tmpSocketData._data, 0, _dataLength);
                    _curBuffPosition -= _buffLength;
                    byte[] _tmpBuff = new byte[_curBuffPosition < _minBuffLen ? _minBuffLen : _curBuffPosition];
                    Array.Copy(_buff, _buffLength, _tmpBuff, 0, _curBuffPosition);
                    _buff = _tmpBuff;


                    _buffLength = 0;
                    _dataLength = 0;
                    _protocalType = 0;
                    return true;
                }
                return false;
            }

        }

        
        /// <summary>
        /// 数据转网络结构
        /// </summary>
        /// <param name="_protocalType"></param>
        /// <param name="_data"></param>
        /// <returns></returns>
        public static SocketData BytesToSocketData(int _protocalType, byte[] _data)
        {
            SocketData tmpSocketData = new SocketData();
            tmpSocketData._buffLength = Constants.HEAD_LEN + _data.Length;
            tmpSocketData._dataLength = _data.Length;
            tmpSocketData._protocalType = _protocalType;
            tmpSocketData._data = _data;
            return tmpSocketData;
        }

        /// <summary>
        /// 网络结构转数据
        /// </summary>
        /// <param name="tmpSocketData"></param>
        /// <returns></returns>
        public static byte[] SocketDataToBytes(SocketData tmpSocketData)
        {
            byte[] _tmpBuff = new byte[tmpSocketData._buffLength];
            byte[] _tmpBuffLength = BitConverter.GetBytes(tmpSocketData._buffLength);
            byte[] _tmpDataLenght = BitConverter.GetBytes((UInt16)tmpSocketData._protocalType);

            Array.Copy(_tmpBuffLength, 0, _tmpBuff, 0, Constants.HEAD_DATA_LEN);//缓存总长度
            Array.Copy(_tmpDataLenght, 0, _tmpBuff, Constants.HEAD_DATA_LEN, Constants.HEAD_TYPE_LEN);//协议类型
            Array.Copy(tmpSocketData._data, 0, _tmpBuff, Constants.HEAD_LEN, tmpSocketData._dataLength);//协议数据

            return _tmpBuff;
        }



        /// <summary>
        /// ProtoBuf序列化
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] ProtoBuf_Serializer(ProtoBuf.IExtensible data)
        {
            using (MemoryStream m = new MemoryStream())
            {
                byte[] buffer = null;
                ProtoBuf.Serializer.Serialize(m, data);
                m.Position = 0;
                int length = (int)m.Length;
                buffer = new byte[length];
                m.Read(buffer, 0, length);
                return buffer;
            }
        }

        /// <summary>
        /// ProtoBuf反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_data"></param>
        /// <returns></returns>
        public static T ProtoBuf_Deserialize<T>(byte[] _data)
        {
            using (MemoryStream m = new MemoryStream(_data))
            {
                return ProtoBuf.Serializer.Deserialize<T>(m);
            }
        }

    }

    

}
