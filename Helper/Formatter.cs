using System;
using System.Text;
using System.Text.Json;

namespace SharpSocket.Helper
{
    public static class Formatter
    {
        public static byte[] Serialize<T>(T data)
        {
            byte[] mByte = JsonSerializer.SerializeToUtf8Bytes(data);
            return mByte;
        }

        public static T Deserialize<T>(byte[] data, int count)
        {
            var stringData = Encoding.UTF8.GetString(data, 0, count);
            return JsonSerializer.Deserialize<T>(stringData)!;
        }

        public static T Deserialize<T>(string data)
        {            
            T? result = JsonSerializer.Deserialize<T>(data);

            if (result == null)
            {
                throw new InvalidOperationException("Null object after deserialization");
            }

            return result;
        }
    }
}