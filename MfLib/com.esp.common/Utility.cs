using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.esp.common
{
    /// <summary>
    /// Utilityクラス
    /// </summary>
    public class Utility
    {
        /// <summary>
        /// Utilityクラス
        /// </summary>
        private Utility()
        {
        }

        /// <summary>
        /// Byte[]からHex文字列へ
        /// </summary>
        /// <param name="buffer">バッファ</param>
        /// <param name="offset">開始位置</param>
        /// <param name="length">長さ</param>
        /// <returns>00 01... FF形式の文字列</returns>
        public static string ByteToHex(byte[] buffer, int offset, int length)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                sb.Append(buffer[offset + i].ToString("X2"));
                sb.Append(" ");
            }
            return sb.ToString();
        }

        /// <summary>
        /// Hex文字列からByte[]へ
        /// </summary>
        /// <param name="hex">00 01...FF形式の文字列</param>
        /// <returns>Byte[]</returns>
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
