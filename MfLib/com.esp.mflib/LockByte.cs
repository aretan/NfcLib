using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.esp.nfclib.trailer
{
    /// <summary>
    /// Mifare Ultralight LockByte
    /// LockByteにより、ページ単位の書き換え禁止設定を行います。
    /// 書き換え禁止ページの追加はできますが、禁止設定のページを修正可能には戻せません。
    /// </summary>
    public class LockByte
    {
        /// <summary>ページ4～15の書き換え禁止設定
        /// true=禁止,false=許可</summary>
        public bool[] Page { get; private set; }

        /// <summary>OTP部(ページ3)の書き換え禁止</summary>
        public bool Otp { get; set; }

        /// <summary>
        /// 全ページ書き換え可能状態
        /// </summary>
        public LockByte()
        {
            Page = new bool[NfcLib.MF_UL_PAGE_MAX];
        }

        /// <summary>
        /// LockByteに対応する4Byteを取得
        /// </summary>
        /// <returns>Byte[4]</returns>
        public byte[] GetPageData()
        {
            byte[] ret = new byte[NfcLib.MF_UL_PAGE_LENGTH];

            ret[2] = (byte)(ret[2] | (byte)(Otp ? 0x01 : 0x00));
            int offset = 4;
            ret[2] = (byte)(ret[2] | (byte)(Page[offset++] ? 0x10 : 0x00));
            ret[2] = (byte)(ret[2] | (byte)(Page[offset++] ? 0x20 : 0x00));
            ret[2] = (byte)(ret[2] | (byte)(Page[offset++] ? 0x40 : 0x00));
            ret[2] = (byte)(ret[2] | (byte)(Page[offset++] ? 0x80 : 0x00));

            ret[3] = (byte)(ret[3] | (byte)(Page[offset++] ? 0x01 : 0x00));
            ret[3] = (byte)(ret[3] | (byte)(Page[offset++] ? 0x02 : 0x00));
            ret[3] = (byte)(ret[3] | (byte)(Page[offset++] ? 0x04 : 0x00));
            ret[3] = (byte)(ret[3] | (byte)(Page[offset++] ? 0x08 : 0x00));
            ret[3] = (byte)(ret[3] | (byte)(Page[offset++] ? 0x10 : 0x00));
            ret[3] = (byte)(ret[3] | (byte)(Page[offset++] ? 0x20 : 0x00));
            ret[3] = (byte)(ret[3] | (byte)(Page[offset++] ? 0x40 : 0x00));
            ret[3] = (byte)(ret[3] | (byte)(Page[offset++] ? 0x80 : 0x00));
            return ret;
        }


    }
}
