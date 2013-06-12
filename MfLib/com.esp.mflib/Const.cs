using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.esp.nfclib
{
    /// <summary>
    /// Mifareコマンドコード
    /// </summary>
    internal enum MfCmdCode:byte
    {
        /// <summary>READ</summary>
        CMD_READ = 0x30,
        /// <summary>1K,4K Write</summary>
        CMD_WRITE = 0xA0,
        /// <summary>UL Write</summary>
        CMD_UL_WRITE = 0xA2,
        /// <summary>Auth by KyeA</summary>
        CMD_AUTH_A = 0x60,
        /// <summary>Auth by KyeB</summary>
        CMD_AUTH_B = 0x61,
    };

    /// <summary>
    /// Mifare定数
    /// </summary>
    internal class MfConst
    {
        public const int CMD_HEADER_LENGTH = 2;
        public const int KEY_LENGTH = 6;
        public const ushort ATQA_MFCL1K = 0x04;
        public const ushort ATQA_MFCL4K = 0x02;
        public const ushort ATQA_MFUL = 0x44; 
    }

    /// <summary>
    /// 使用するカード
    /// </summary>
    public enum UseCard
    {
        /// <summary>なし</summary>
        None=0,
        /// <summary>Felicaのみ</summary>
        Felica = 1,
        /// <summary>Mifareのみ</summary>
        Mifare = 2
    }

    /// <summary>
    /// 接続モード
    /// </summary>
    public enum ConnetKind
    {
        /// <summary>未接続</summary>
        None=0,
        /// <summary>Felica API</summary>
        Felica = 1,
        /// <summary>Nfc API</summary>
        Nfc = 2,
        /// <summary>PC/SC API</summary>
        Pcsc = 3,
    }
}
