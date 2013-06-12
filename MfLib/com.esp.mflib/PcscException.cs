using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.esp.nfclib
{
    /// <summary>
    /// Pasoriエラー
    /// </summary>
    public class PcscException:Exception
    {
        private static readonly byte[] noDeail = new byte[2];

        /// <summary>Pcsc エラーコード</summary>
        public WinSCardError ApiError { get; private set; }

        /// <summary>Pasori ドライバエラーコード</summary>
        public byte[] Detail { get; private set; }

        /// <summary>
        /// PC/SCエラー
        /// </summary>
        /// <param name="apiError">APIエラーコード</param>
        /// <param name="detail">詳細</param>
        public PcscException(WinSCardError apiError, byte[] detail)
        {
            this.ApiError = apiError;
            this.Detail = detail;
        }

        /// <summary>
        /// エラー内容
        /// </summary>
        public override string Message
        {
            get
            {
                if (Detail == null) Detail = noDeail;

                if (Enum.IsDefined(typeof(WinSCardError), ApiError))
                {
                    return String.Format("API:{0}, DIRVER:{1:X} {2:X}", ApiError, Detail[0], Detail[1]);
                }
                else
                {
                    return String.Format("API:{0:X}, DIRVER:{1:X} {2:X}", ApiError, Detail[0], Detail[1]);
                }
            }
        }
    }

    /// <summary>
    /// winscrad.dllのエラーコード
    /// </summary>
    public enum WinSCardError : uint
    {
        /// <summary>SCARD_S_SUCCESS</summary>
        SCARD_S_SUCCESS = 0x00000000,
        /// <summary>SCARD_F_INTERNAL_ERROR</summary>
        SCARD_F_INTERNAL_ERROR = 0x80100001,
        /// <summary>SCARD_E_CANCELLED</summary>
        SCARD_E_CANCELLED = 0x80100002,
        /// <summary>SCARD_E_INVALID_HANDLE</summary>
        SCARD_E_INVALID_HANDLE = 0x80100003,
        /// <summary>SCARD_E_INVALID_PARAMETER</summary>
        SCARD_E_INVALID_PARAMETER = 0x80100004,
        /// <summary>SCARD_E_INVALID_TARGET</summary>
        SCARD_E_INVALID_TARGET = 0x80100005,
        /// <summary>SCARD_E_NO_MEMORY</summary>
        SCARD_E_NO_MEMORY = 0x80100006,
        /// <summary>SCARD_F_WAITED_TOO_LONG</summary>
        SCARD_F_WAITED_TOO_LONG = 0x80100007,
        /// <summary>SCARD_E_INSUFFICIENT_BUFFER</summary>
        SCARD_E_INSUFFICIENT_BUFFER = 0x80100008,
        /// <summary>SCARD_E_UNKNOWN_READER</summary>
        SCARD_E_UNKNOWN_READER = 0x80100009,
        /// <summary>SCARD_E_TIMEOUT</summary>
        SCARD_E_TIMEOUT = 0x8010000A,
        /// <summary>SCARD_E_SHARING_VIOLATION</summary>
        SCARD_E_SHARING_VIOLATION = 0x8010000B,
        /// <summary>SCARD_E_NO_SMARTCARD</summary>
        SCARD_E_NO_SMARTCARD = 0x8010000C,
        /// <summary>SCARD_E_UNKNOWN_CARD</summary>
        SCARD_E_UNKNOWN_CARD = 0x8010000D,
        /// <summary>SCARD_E_CANT_DISPOSE</summary>
        SCARD_E_CANT_DISPOSE = 0x8010000E,
        /// <summary>SCARD_E_PROTO_MISMATCH</summary>
        SCARD_E_PROTO_MISMATCH = 0x8010000F,
        /// <summary>SCARD_E_NOT_READY</summary>
        SCARD_E_NOT_READY = 0x80100010,
        /// <summary>SCARD_E_INVALID_VALUE</summary>
        SCARD_E_INVALID_VALUE = 0x80100011,
        /// <summary>SCARD_E_SYSTEM_CANCELLED</summary>
        SCARD_E_SYSTEM_CANCELLED = 0x80100012,
        /// <summary>SCARD_F_COMM_ERROR</summary>
        SCARD_F_COMM_ERROR = 0x80100013,
        /// <summary>SCARD_F_UNKNOWN_ERROR</summary>
        SCARD_F_UNKNOWN_ERROR = 0x80100014,
        /// <summary>SCARD_E_INVALID_ATR</summary>
        SCARD_E_INVALID_ATR = 0x80100015,
        /// <summary>SCARD_E_NOT_TRANSACTED</summary>
        SCARD_E_NOT_TRANSACTED = 0x80100016,
        /// <summary>SCARD_E_READER_UNAVAILABLE</summary>
        SCARD_E_READER_UNAVAILABLE = 0x80100017,
        /// <summary>SCARD_E_PCI_TOO_SMALL</summary>
        SCARD_E_PCI_TOO_SMALL = 0x80100019,
        /// <summary>SCARD_E_READER_UNSUPPORTED</summary>
        SCARD_E_READER_UNSUPPORTED = 0x8010001A,
        /// <summary>SCARD_E_DUPLICATE_READER</summary>
        SCARD_E_DUPLICATE_READER = 0x8010001B,
        /// <summary>SCARD_E_CARD_UNSUPPORTED</summary>
        SCARD_E_CARD_UNSUPPORTED = 0x8010001C,
        /// <summary>SCARD_E_NO_SERVICE</summary>
        SCARD_E_NO_SERVICE = 0x8010001D,
        /// <summary>SCARD_E_SERVICE_STOPPED</summary>
        SCARD_E_SERVICE_STOPPED = 0x8010001E,
        /// <summary>SCARD_E_UNEXPECTED</summary>
        SCARD_E_UNEXPECTED = 0x8010001F,
        /// <summary>SCARD_E_ICC_INSTALLATION</summary>
        SCARD_E_ICC_INSTALLATION = 0x80100020,
        /// <summary>SCARD_E_ICC_CREATEORDER</summary>
        SCARD_E_ICC_CREATEORDER = 0x80100021,
        /// <summary>SCARD_E_DIR_NOT_FOUND</summary>
        SCARD_E_DIR_NOT_FOUND = 0x80100023,
        /// <summary>SCARD_E_FILE_NOT_FOUND</summary>
        SCARD_E_FILE_NOT_FOUND = 0x80100024,
        /// <summary>SCARD_E_NO_DIR</summary>
        SCARD_E_NO_DIR = 0x80100025,
        /// <summary>SCARD_E_NO_FILE</summary>
        SCARD_E_NO_FILE = 0x80100026,
        /// <summary>SCARD_E_NO_ACCESS</summary>
        SCARD_E_NO_ACCESS = 0x80100027,
        /// <summary>SCARD_E_WRITE_TOO_MANY</summary>
        SCARD_E_WRITE_TOO_MANY = 0x80100028,
        /// <summary>SCARD_E_BAD_SEEK</summary>
        SCARD_E_BAD_SEEK = 0x80100029,
        /// <summary>SCARD_E_INVALID_CHV</summary>
        SCARD_E_INVALID_CHV = 0x8010002A,
        /// <summary>SCARD_E_UNKNOWN_RES_MNG</summary>
        SCARD_E_UNKNOWN_RES_MNG = 0x8010002B,
        /// <summary>SCARD_E_NO_SUCH_CERTIFICATE</summary>
        SCARD_E_NO_SUCH_CERTIFICATE = 0x8010002C,
        /// <summary>SCARD_E_CERTIFICATE_UNAVAILABLE</summary>
        SCARD_E_CERTIFICATE_UNAVAILABLE = 0x8010002D,
        /// <summary>SCARD_E_NO_READERS_AVAILABLE</summary>
        SCARD_E_NO_READERS_AVAILABLE = 0x8010002E,
        /// <summary>SCARD_E_COMM_DATA_LOST</summary>
        SCARD_E_COMM_DATA_LOST = 0x8010002F,
        /// <summary>SCARD_E_NO_KEY_CONTAINER</summary>
        SCARD_E_NO_KEY_CONTAINER = 0x80100030,
        /// <summary>SCARD_E_SERVER_TOO_BUSY</summary>
        SCARD_E_SERVER_TOO_BUSY = 0x80100031,
        /// <summary>SCARD_W_UNSUPPORTED_CARD</summary>
        SCARD_W_UNSUPPORTED_CARD = 0x80100065,
        /// <summary>SCARD_W_UNRESPONSIVE_CARD</summary>
        SCARD_W_UNRESPONSIVE_CARD = 0x80100066,
        /// <summary>SCARD_W_UNPOWERED_CARD</summary>
        SCARD_W_UNPOWERED_CARD = 0x80100067,
        /// <summary>SCARD_W_RESET_CARD</summary>
        SCARD_W_RESET_CARD = 0x80100068,
        /// <summary>SCARD_W_REMOVED_CARD</summary>
        SCARD_W_REMOVED_CARD = 0x80100069,
        /// <summary>SCARD_W_SECURITY_VIOLATION</summary>
        SCARD_W_SECURITY_VIOLATION = 0x8010006A,
        /// <summary>SCARD_W_WRONG_CHV</summary>
        SCARD_W_WRONG_CHV = 0x8010006B,
        /// <summary>SCARD_W_CHV_BLOCKED</summary>
        SCARD_W_CHV_BLOCKED = 0x8010006C,
        /// <summary>SCARD_W_EOF</summary>
        SCARD_W_EOF = 0x8010006D,
        /// <summary>SCARD_W_CANCELLED_BY_USER</summary>
        SCARD_W_CANCELLED_BY_USER = 0x8010006E,
        /// <summary>SCARD_W_CARD_NOT_AUTHENTICATED</summary>
        SCARD_W_CARD_NOT_AUTHENTICATED = 0x8010006F,
    };
}
