using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using com.esp.common;

namespace com.esp.nfclib
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct SCARD_IO_REQUEST
    {
        public uint dwProtocol;
        public uint cbPciLength;
    }

    /// <summary>
    /// winscard.dllラッパー
    /// </summary>
    internal class WinSCard
    {
        private const string dll_name = "winscard.dll";

        public const uint SCARD_SCOPE_USER = 0x00;
        public const uint SCARD_SCOPE_SYSTEM = 0x02;

        public const uint SCARD_PROTOCOL_UNDEFINED = 0x00;
        public const uint SCARD_PROTOCOL_T0 = 0x01;
        public const uint SCARD_PROTOCOL_T1 = 0x02;
        public const uint SCARD_PROTOCOL_RAW = 0x04;

        public const uint SCARD_SHARE_EXCLUSIVE = 0x0001;
        public const uint SCARD_SHARE_SHARED = 0x0002;
        public const uint SCARD_SHARE_DIRECT = 0x0003;

        public static readonly SCARD_IO_REQUEST SCARD_PCI_UNDEFINED, SCARD_PCI_T0, SCARD_PCI_T1, SCARD_PCI_RAW;

        private static WinSCardError lastError;
        private static byte[] lastRes;

        static WinSCard()
        {
            SCARD_PCI_UNDEFINED = new SCARD_IO_REQUEST();
            SCARD_PCI_UNDEFINED.dwProtocol = SCARD_PROTOCOL_UNDEFINED;
            SCARD_PCI_UNDEFINED.cbPciLength = (uint)Marshal.SizeOf(SCARD_PCI_UNDEFINED);

            SCARD_PCI_T0 = new SCARD_IO_REQUEST();
            SCARD_PCI_T0.dwProtocol = SCARD_PROTOCOL_T0;
            SCARD_PCI_T0.cbPciLength = (uint)Marshal.SizeOf(SCARD_PCI_T0);

            SCARD_PCI_T1 = new SCARD_IO_REQUEST();
            SCARD_PCI_T1.dwProtocol = SCARD_PROTOCOL_T1;
            SCARD_PCI_T1.cbPciLength = (uint)Marshal.SizeOf(SCARD_PCI_T1);

            SCARD_PCI_RAW = new SCARD_IO_REQUEST();
            SCARD_PCI_RAW.dwProtocol = SCARD_PROTOCOL_RAW;
            SCARD_PCI_RAW.cbPciLength = (uint)Marshal.SizeOf(SCARD_PCI_RAW);
            lastRes = new byte[2];
        }

        [DllImport(dll_name)]
        private static extern uint SCardEstablishContext(
              uint dwScope,
              IntPtr pvReserved1,
              IntPtr pvReserved2,
              ref IntPtr phContext
            );

        [DllImport(dll_name)]
        private static extern uint SCardListReaders(
          IntPtr hContext,
          String mszGroups,
          StringBuilder mszReaders,
          ref uint pcchReaders
        );

        [DllImport(dll_name)]
        private static extern uint SCardReleaseContext(IntPtr context);

        [DllImport(dll_name)]
        private static extern uint SCardConnect(
          IntPtr hContext,
          String szReader,
          uint dwShareMode,
          uint dwPreferredProtocols,
          ref IntPtr phCard,
          ref uint pdwActiveProtocol
        );

        [DllImport(dll_name)]
        private static extern uint SCardDisconnect(
            IntPtr card,
            uint disposition);


        [DllImport(dll_name)]
        private static extern uint SCardTransmit(
          IntPtr hCard,
          ref SCARD_IO_REQUEST pioSendPci,
          byte[] pbSendBuffer,
          uint cbSendLength,
          ref SCARD_IO_REQUEST pioRecvPci,
          byte[] pbRecvBuffer,
          ref uint pcbRecvLength
        );

        [DllImport(dll_name)]
        private static extern int SCardStatus(
          IntPtr hCard,
          StringBuilder szReaderName,
          ref uint pcchReaderLen,
          ref uint pdwState,
          ref uint pdwProtocol,
          byte[]   pbAtr,
          ref uint pcbAtrLen
        );

        /// <summary>
        /// リーダーライター接続
        /// </summary>
        /// <param name="rwContext">[OUT]RWコンテキスト</param>
        /// <returns></returns>
        public static bool EstablishContext(ref IntPtr rwContext)
        {
            lastError = (WinSCardError)SCardEstablishContext(SCARD_SCOPE_USER, IntPtr.Zero, IntPtr.Zero, ref rwContext);
            return lastError == WinSCardError.SCARD_S_SUCCESS;
        }

        /// <summary>
        /// リーダー名称取得
        /// </summary>
        /// <param name="rwContext">RWコンテキスト</param>
        /// <returns></returns>
        public static string ListReaders(IntPtr rwContext)
        {
            uint len = 0;
            lastError = (WinSCardError)SCardListReaders(rwContext, null, null, ref len);
            if (lastError != WinSCardError.SCARD_S_SUCCESS)
                return null;

            StringBuilder sb = new StringBuilder((int)len);
            lastError = (WinSCardError)SCardListReaders(rwContext, null, sb, ref len);
            if (lastError != WinSCardError.SCARD_S_SUCCESS)
                return null;

            return sb.ToString();
        }

        /// <summary>
        /// リーダーライターの解放
        /// </summary>
        /// <param name="rwContext">RWコンテキスト</param>
        /// <returns></returns>
        public static bool ReleaseContext(IntPtr rwContext)
        {
            return ((WinSCardError)SCardReleaseContext(rwContext) == WinSCardError.SCARD_S_SUCCESS);
        }

        /// <summary>
        /// カード接続
        /// </summary>
        /// <param name="rwContext">RWコンテキスト</param>
        /// <param name="reader">リーダー名</param>
        /// <param name="shareMode">共有指定</param>
        /// <param name="protocol">プロトコル指定</param>
        /// <param name="card">[OUT]カード</param>
        /// <param name="activeProtocol">[OUT]接続プロトコル</param>
        /// <returns></returns>
        public static bool Connect(IntPtr rwContext, String reader, 
            uint shareMode, uint protocol, ref IntPtr card, ref uint activeProtocol)
        {
            lastError = (WinSCardError)SCardConnect(rwContext, reader, shareMode, protocol, ref card, ref activeProtocol);
            Debug.WriteLine("WinSCard.Connect Res:" + lastError.ToString());
            return lastError == WinSCardError.SCARD_S_SUCCESS;
        }

        /// <summary>
        /// カードの切断
        /// </summary>
        /// <param name="card">カードコンテキスト</param>
        /// <param name="disposition">0を指定</param>
        /// <returns></returns>
        public static bool Disconnect(IntPtr card, uint disposition)
        {
            lastError = (WinSCardError)SCardDisconnect(card, disposition);
            Debug.WriteLine("WinSCard.Disconnect Res:" + lastError.ToString());
            return lastError == WinSCardError.SCARD_S_SUCCESS;
        }

        /// <summary>
        /// ATR取得用
        /// </summary>
        /// <param name="card">カードコンテキスト</param>
        /// <param name="reader">[OUT]リーダー名</param>
        /// <param name="state">[OUT]リーダー状態</param>
        /// <param name="protocol">[OUT]プロトコル</param>
        /// <param name="atr">[OUT]</param>
        /// <returns></returns>
        public static bool GetStatus(IntPtr card, out string reader, out uint state, out uint protocol, out byte[] atr)
        {
            uint readerLen=0, atrLen=0;
            state = 0;
            protocol = 0;
            reader = null;
            atr = null;

            lastError = (WinSCardError)SCardStatus(card, null, ref readerLen, ref state, ref protocol, null, ref atrLen);
            if (lastError != WinSCardError.SCARD_S_SUCCESS)
                return false;

            StringBuilder sb = new StringBuilder((int)readerLen);
            atr = new byte[atrLen];
            lastError = (WinSCardError)SCardStatus(card, sb, ref readerLen, ref state, ref protocol, atr, ref atrLen);
            if (lastError != WinSCardError.SCARD_S_SUCCESS)
                return false;

            reader = sb.ToString();
            return true;
        }

        /// <summary>
        /// カードコマンド送受信
        /// </summary>
        /// <param name="card">カードコンテキスト</param>
        /// <param name="sendPci">GetPciを使用して取得したPCI構造体</param>
        /// <param name="sendBuffer">送信データ</param>
        /// <param name="sendLen">送信データ長</param>
        /// <param name="recvPci">レスポンス格納用PCI構造体</param>
        /// <param name="recvBuffer">レスポンス格納バッファ</param>
        /// <param name="recvLen">[OUT]レスポンス長</param>
        /// <returns></returns>
        public static bool Transmit(IntPtr card, SCARD_IO_REQUEST sendPci,
            byte[] sendBuffer, int sendLen, ref SCARD_IO_REQUEST recvPci, byte[] recvBuffer, ref uint recvLen)
        {
            Debug.WriteLine("WinSCard.Transmit Send:" + Utility.ByteToHex(sendBuffer, 0, (int)sendLen));
            lastError = (WinSCardError)SCardTransmit(
                card, ref sendPci, sendBuffer, (uint)sendLen, ref recvPci, recvBuffer, ref recvLen);

            if (lastError == WinSCardError.SCARD_S_SUCCESS && recvBuffer[recvLen - 2] == 0x90 && recvBuffer[recvLen- 1] == 0x00)
            {
                Array.Copy(recvBuffer, recvLen - 2, lastRes, 0, lastRes.Length);
                Debug.WriteLine("WinSCard.Transmit OK:" + Utility.ByteToHex(recvBuffer, 0, (int)recvLen));
                return true;
            }
            else if (lastError == WinSCardError.SCARD_S_SUCCESS)
            {
                Array.Copy(recvBuffer, recvLen - 2, lastRes, 0, lastRes.Length);
                Debug.WriteLine("WinSCard.Transmit Error:" + Utility.ByteToHex(recvBuffer, 0, (int)recvLen));
                return false;
            }
            else
            {
                Debug.WriteLine("WinSCard.Transmit Error:" + lastError.ToString());
                return false;
            }
        }

        /// <summary>
        /// プロトコル毎のPCI構造体取得
        /// </summary>
        /// <param name="protocol"></param>
        /// <returns></returns>
        public static SCARD_IO_REQUEST GetPci(uint protocol)
        {
            if (protocol == SCARD_PROTOCOL_T0)
            {
                return SCARD_PCI_T0;
            }
            else if (protocol == SCARD_PROTOCOL_T1)
            {
                return SCARD_PCI_T1;
            }
            else if (protocol == SCARD_PROTOCOL_RAW)
            {
                return SCARD_PCI_RAW;
            }
            else
            {
                return SCARD_PCI_UNDEFINED;
            }
        }

        /// <summary>
        /// 書き込み時に指定する、レスポンス格納用構造体を取得
        /// </summary>
        /// <returns></returns>
        public static SCARD_IO_REQUEST GetDefPci()
        {
            SCARD_IO_REQUEST resPci = new SCARD_IO_REQUEST();
            resPci.dwProtocol = WinSCard.SCARD_PROTOCOL_UNDEFINED;
            resPci.cbPciLength = (uint)Marshal.SizeOf(resPci);
            return resPci;
        }

        /// <summary>
        /// メソッド失敗時のエラー取得
        /// </summary>
        /// <returns></returns>
        public static WinSCardError GetLastError()
        {
            return lastError;
        }

        /// <summary>
        /// メソッド成功かつ、カードによる拒否等のエラー取得
        /// </summary>
        /// <returns></returns>
        public static byte[] GetLastRes()
        {
            return lastRes;
        }
    }
}
