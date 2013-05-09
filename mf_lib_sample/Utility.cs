using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mf_lib_sample
{
    class Utility
    {
        public static string ByteToHex(byte[] buffer, int start, int length)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                sb.Append(buffer[start + i].ToString("X2"));
                sb.Append(" ");
            }
            return sb.ToString();
        }

        public static byte[] HexToByte(string hex)
        {
            hex = hex.Replace(" ", "");
            
            byte[] ret = new byte[hex.Length / 2];
            for (int i = 0; i < hex.Length / 2; i++)
            {
                ret[i] = byte.Parse(hex.Substring(i * 2, 2),  System.Globalization.NumberStyles.HexNumber);
            }
            return ret;
        }
    }
}
