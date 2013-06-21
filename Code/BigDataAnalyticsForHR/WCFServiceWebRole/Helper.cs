using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WCFServiceWebRole
{
    public class Helper
    {
        public static MemoryStream GenerateSampleStream(string strSerializedText)
        {
            //// Generate sample data.
            //byte[] subData = new byte[strSerializedText.Length];
            //for (int i = 0; i < subData.Length; i++)
            //{
            //    //if(Convert.ToByte(strSerializedText[i]) > 251)
            //    //    subData[i] = Convert.ToByte(strSerializedText[i]);
            //    subData[i] = Convert.ToByte(strSerializedText[i]);
            //}

            byte[] subData = GetBytes(strSerializedText);
            return new MemoryStream(subData);
        }

        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        public static String GetUTF8String(String InputString)
        {
            System.Text.Encoding utf_8 = System.Text.Encoding.UTF8;
            byte[] utf8Bytes = utf_8.GetBytes(InputString);
            String strutf8 = utf_8.GetString(utf8Bytes, 0, utf8Bytes.Length);
            return strutf8;
        }
    }
}