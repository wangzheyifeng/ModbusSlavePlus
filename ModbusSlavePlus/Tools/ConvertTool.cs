using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusSlavePlus.Tools
{
    public class ConvertTool
    {
        // 转换成float ABCD data1对应input[2],data2对应input[1]
        public static float GetFloatFormUshortArray(ushort data1, ushort data2)
        {
            List<byte> result = new List<byte>();
            result.AddRange(BitConverter.GetBytes(data2));
            result.AddRange(BitConverter.GetBytes(data1));
            return BitConverter.ToSingle(result.ToArray(), 0);
        }

        public static long GetLongFormUshort(ushort data1, ushort data2)
        {
            return (Int32)(((UInt32)data1 << 16) | (UInt32)data2);
        }

        public static ulong GetULongFormUshort(ushort data1, ushort data2)
        {
            return (UInt32)(((UInt32)data1 << 16) | (UInt32)data2);
        }

        public static int GetIntFormUshort(ushort data1, ushort data2)
        {
            return (Int32)(((UInt32)data1 << 16) | (UInt32)data2);
        }

        public static uint GetUIntFormUshort(ushort data1, ushort data2)
        {
            return (UInt32)(((UInt32)data1 << 16) | (UInt32)data2);
        }

        public static ushort[] FloatToUshort(float value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            ushort high = BitConverter.ToUInt16(bytes, 2);
            ushort low = BitConverter.ToUInt16(bytes, 0);
            return new ushort[] { high, low };
        }

        public static ushort[] UInt32ToUshort(UInt32 value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            ushort high = BitConverter.ToUInt16(bytes, 2);
            ushort low = BitConverter.ToUInt16(bytes, 0);
            return new ushort[] { high, low };
        }

        public static ushort[] Int32ToUshort(Int32 value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            ushort high = BitConverter.ToUInt16(bytes, 2);
            ushort low = BitConverter.ToUInt16(bytes, 0);
            return new ushort[] { high, low };
        }

        public static float Uint32ToFloat(UInt32 value)
        {
            ushort[] ushorts = UInt32ToUshort(value);
            return GetFloatFormUshortArray(ushorts[1], ushorts[0]);
        }
    }
}
