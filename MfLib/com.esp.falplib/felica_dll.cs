using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace com.esp.falplib
{
    internal class felica_dll
    {
        #region 定数
        protected const byte TRUE = 1;
        #endregion

        #region DLL定義
        //DllImport
        [DllImport("felica.dll")]
        protected static extern byte initialize_library();

        [DllImport("felica.dll")]
        protected static extern byte open_reader_writer_auto();

        [DllImport("felica.dll")]
        protected static extern byte reader_writer_is_open(ref bool isOpen);

        [DllImport("felica.dll")]
        protected static extern byte close_reader_writer();

        [DllImport("felica.dll")]
        protected static extern byte dispose_library();

        [DllImport("felica.dll")]
        protected static extern byte transaction_lock();

        [DllImport("felica.dll")]
        protected static extern byte transaction_unlock();

        [DllImport("felica.dll")]
        protected static extern byte polling_and_get_card_information(
            [In]ref StructurePolling polling,
            ref byte num_of_cards,
            [In,Out]ref StructureCardInformation card_information);

        [DllImport("felica.dll")]
        protected static extern byte write_block_without_encryption(
            ref InputStructureWriteBlockWithoutEncryption udtInputWriteBlockWithoutEncryption,
            ref OutputStructureWriteBlockWithoutEncryption udtOutputWriteBlockWithoutEncryption);

        [DllImport("felica.dll")]
        protected static extern byte read_block_without_encryption(
            ref InputStructureReadBlockWithoutEncryption udtInputReadBlockWithoutEncryption,
            ref OutputStructureReadBlockWithoutEncryption udtOutputReadBlockWithoutEncryption);
        
        [DllImport("felica.dll")]
        protected static extern byte falp_open();

        [DllImport("felica.dll")]
        protected static extern byte falp_close();

        [DllImport("felica.dll")]
        protected static extern byte falp_connect(
                ushort propose_time_out,
                ushort handshake_time_out,
                byte[] appid,
                byte appid_length,
                byte[] data,
                ref uint data_length
            );

        [DllImport("felica.dll")]
        protected static extern byte falp_shutdown(uint flag);

        [DllImport("felica.dll")]
        protected static extern byte falp_recv(
            byte[] data,
            ref uint data_length
        );

        [DllImport("felica.dll")]
        protected static extern byte falp_send(
            byte[] data,
            ref uint data_length
        );

        [DllImport("felica.dll")]
        protected static extern byte falp_wait_event(
            ushort time_out,
            ref uint detected,
            uint mask
        );

        [DllImport("felica.dll")]
        protected static extern byte falp_get_last_error_type(
            ref uint error_type
        );

        [DllImport("felica.dll")]
        protected static extern byte get_last_error_type(
            ref uint error_type
        );
        #endregion

        #region C#ラッパ
        //Wrapper functions
        /// <summary>
        /// ライブラリの初期化
        /// </summary>
        /// <returns>T:正常終了 F:異常終了</returns>
        public static bool InitializeLibrary()
        {
            return initialize_library() == TRUE;
        }

        /// <summary>
        /// リーダーライターを開く
        /// </summary>
        /// <returns>T:正常終了 F:異常終了</returns>
        public static bool OpenReaderWriterAuto()
        {
            return open_reader_writer_auto() == TRUE;
        }

        /// <summary>
        /// リーダー・ライターの解放
        /// </summary>
        /// <returns>T:正常終了 F:異常終了</returns>
        public static bool CloseReaderWriter()
        {
            return close_reader_writer() == TRUE;
        }

        /// <summary>
        /// ライブラリの解放
        /// </summary>
        /// <returns>T:正常終了 F:異常終了</returns>
        public static bool DisposeLibrary()
        {
            return dispose_library() == TRUE;
        }

        /// <summary>
        /// R/Wを専有する
        /// </summary>
        /// <returns></returns>
        public static bool TransactionLock()
        {
            return transaction_lock() == TRUE;
        }

        /// <summary>
        /// R/Wを専有する
        /// </summary>
        /// <returns></returns>
        public static bool TransactionUnlock()
        {
            return transaction_unlock() == TRUE;
        }

        /// <summary>
        /// Pollingを実行する
        /// </summary>
        /// <param name="polling"></param>
        /// <param name="numOfCard"></param>
        /// <param name="cardInformation"></param>
        /// <returns></returns>
        public static bool PollingAndGetCardInformation(
            ref StructurePolling polling,
            ref byte numOfCard,
            ref StructureCardInformation cardInformation)
        {
            return polling_and_get_card_information(
                ref polling, ref numOfCard, ref cardInformation) == TRUE;
        }

        /// <summary>
        /// 鍵無し領域への書込み
        /// </summary>
        /// <param name="udtInputWriteBlockWithoutEncryption"></param>
        /// <param name="udtOutputWriteBlockWithoutEncryption"></param>
        /// <returns>T:正常終了 F:異常終了</returns>
        public static bool WriteBlockWithoutEncryption(
            ref InputStructureWriteBlockWithoutEncryption udtInputWriteBlockWithoutEncryption,
            ref OutputStructureWriteBlockWithoutEncryption udtOutputWriteBlockWithoutEncryption)
        {
            return write_block_without_encryption(
                       ref udtInputWriteBlockWithoutEncryption,
                       ref udtOutputWriteBlockWithoutEncryption) == TRUE;
        }

        /// <summary>
        /// 鍵無し領域からの読込み
        /// </summary>
        /// <param name="udtInputReadBlockWithoutEncryption"></param>
        /// <param name="udtOutputReadBlockWithoutEncryption"></param>
        /// <returns>T:正常終了 F:異常終了</returns>
        public static bool ReadBlockWithoutEncryption(
            ref InputStructureReadBlockWithoutEncryption udtInputReadBlockWithoutEncryption,
            ref OutputStructureReadBlockWithoutEncryption udtOutputReadBlockWithoutEncryption)
        {
            return read_block_without_encryption(
                       ref udtInputReadBlockWithoutEncryption,
                       ref udtOutputReadBlockWithoutEncryption) == TRUE;
        }
    
        /// <summary>
        /// FALP Initiatorを開始する
        /// </summary>
        /// <returns></returns>
        public static bool FalpOpen()
        {
            return falp_open() == TRUE;
        }

        /// <summary>
        /// FALP Initiatorを終了する
        /// </summary>
        /// <returns></returns>
        public static bool FalpClose()
        {
            return falp_close() == TRUE;
        }

        /// <summary>
        /// Targetに接続する
        /// </summary>
        /// <returns></returns>
        public static bool FalpConnect(ushort propose_time_out,
                ushort handshake_time_out,
                byte[] appid,
                byte appid_length,
                byte[] data,
                ref uint data_length)
        {
            return falp_connect(propose_time_out,
                handshake_time_out,
                appid,
                appid_length,
                data,
                ref data_length) == TRUE;
        }

        /// <summary>
        /// Targetから切断する
        /// </summary>
        /// <returns></returns>
        public static bool FalpShutdown()
        {
            return falp_shutdown((uint)FalpShutdownFlag.BOTH) == TRUE;
        }

        /// <summary>
        /// データを送信する
        /// </summary>
        /// <param name="data">データ</param>
        /// <param name="buffered">データ長、成功時はバッファされたデータ長が格納される</param>
        /// <returns></returns>
        public static bool FalpSend(byte[] data, ref uint buffered)
        {
            return falp_send(data, ref buffered) == TRUE;
        }

        /// <summary>
        /// データを送信する
        /// </summary>
        /// <param name="data">データ</param>
        /// <param name="buffered">データ長、成功時はバッファされたデータ長が格納される</param>
        /// <returns></returns>
        public static bool FalpRecv(byte[] data, ref uint buffered)
        {
            return falp_recv(data, ref buffered) == TRUE;
        }

        public static bool FalpWaitEvent(ushort timeout, out FalpEvent detected, FalpEvent mask)
        {
            uint detectedEvent = 0;
            byte res = falp_wait_event(timeout, ref detectedEvent, (uint)mask);
            if (res == TRUE)
            {
                detected = (FalpEvent)detectedEvent;
                return true;
            }
            else
            {
                detected = 0;
                return false;
            }
        }

        public static bool GetLastErrorType(out FelicaErrorType type)
        {
            uint errorType = 0;
            byte res = get_last_error_type(ref errorType);
            if (res == TRUE)
            {
                type = (FelicaErrorType)errorType;
                return true;
            }
            else
            {
                type = 0;
                return false;
            }
        }

        public static bool FalpGetLastErrorType(out FalpErrorType type)
        {
            uint errorType = 0;
            byte res = falp_get_last_error_type(ref errorType);
            if (res == TRUE)
            {
                type = (FalpErrorType)errorType;
                return true;
            }
            else
            {
                type = 0;
                return false;
            }
        }
        #endregion
    }

    internal enum FalpShutdownFlag
    {
        // FALPシャットダウンフラグの定義
        SEND = 0x00000001,   // 送信シャットダウン
        RECV = 0x00000002,    // 受信シャットダウン
        BOTH = 0x00000003,    // 送受信シャットダウン
    }

    internal enum FalpEvent
    {
        SEND_READY = 0x00000001,    // 送信可能イベント
        RECV_READY = 0x00000002,    // 受信可能イベント
        SHUTDOWNED = 0x00000004,    // 切断イベント
        SEND_EMPTY = 0x00000008,	 // 送信データなしイベント
    }
}

#region 構造体定義
[StructLayout(LayoutKind.Sequential)]
internal struct StructurePolling
{
    public IntPtr SystemCode;//(ポインタ)(2byte)
    public byte TimeSlot;//複数毎のカードを読込む必要が無ければ、0x00を指定
}

[StructLayout(LayoutKind.Sequential)]
internal struct StructureCardInformation
{
    public IntPtr CardIdm;//ポインタ(8byte)
    public IntPtr CardPmm;//ポインタ(8byte)
}
[StructLayout(LayoutKind.Sequential)]
internal struct StructureInputRequestService
{
    public IntPtr CardIdm;
    public byte NumberOfServices;
    public IntPtr ServiceCcodeList;
}

[StructLayout(LayoutKind.Sequential)]
internal struct StructureOutputRequestService
{
    public IntPtr NumberOfServices;
    public IntPtr ServiceKeyVersionList;
}

[StructLayout(LayoutKind.Sequential)]
internal struct InputStructureWriteBlockWithoutEncryption
{
    /// <summary>
    /// Byte型へのポインタ(8byte) Idm
    /// </summary>
    public IntPtr CardIdm;

    /// <summary>
    /// サービス数 m(1～m～16)
    /// </summary>
    public byte NumberOfServices;

    /// <summary>
    /// Byte型へのポインタ(m*2byte、m*4byte)　サービスコードリスト 
    /// </summary>
    public IntPtr ServiceCodeList;

    /// <summary>
    /// ブロック数 n(1～n～255)
    /// </summary>
    public byte NumberOfBlocks;

    /// <summary>
    /// Byte型へのポインタ(n*2byte～n*3byte)　ブロック位置リスト
    /// </summary>
    public IntPtr BlockList;

    /// <summary>
    /// Byte型へのポインタ(n*16byte) ブロックデータ領域リスト
    /// </summary>
    public IntPtr BlockData;
}

[StructLayout(LayoutKind.Sequential)]
internal struct OutputStructureWriteBlockWithoutEncryption
{
    /// <summary>
    /// byte型へのポインタ 0=OK,それ以外=NG;
    /// </summary>
    public IntPtr StatusFlag1;

    /// <summary>
    /// byte型へのポインタ 0=OK,それ以外=NG;
    /// </summary>
    public IntPtr StatusFlag2;
}

[StructLayout(LayoutKind.Sequential)]
internal struct InputStructureReadBlockWithoutEncryption
{
    /// <summary>
    /// byte型へのポインタ(8byte) Idm
    /// </summary>
    public IntPtr CardIdm;

    /// <summary>
    /// サービス数 m(1～m～16)
    /// </summary>
    public byte NumberOfServices;

    /// <summary>
    /// Byte型へのポインタ(m*2byte、m*4byte)　サービスコードリスト 
    /// </summary>
    public IntPtr ServiceCodeList;

    /// <summary>
    /// ブロック数n n(1～n～255)
    /// </summary>
    public byte NumberOfBlocks;

    /// <summary>
    /// Byte型へのポインタ(n*2byte～n*3byte)　ブロック位置リスト
    /// </summary>
    public IntPtr BlockList;
}

[StructLayout(LayoutKind.Sequential)]
internal struct OutputStructureReadBlockWithoutEncryption
{
    /// <summary>
    /// Byte型(1byte)へのポインタ 0=OK,それ以外=NG;
    /// </summary>
    public IntPtr StatusFlag1;

    /// <summary>
    /// Byte型(1byte)へのポインタ 0=OK,それ以外=NG;
    /// </summary>
    public IntPtr StatusFlag2;

    /// <summary>
    /// Byte型(1byte)へのポインタ ブロック数n
    /// </summary>
    public IntPtr ResultNumberOfBlocks;

    /// <summary>
    /// Byte型(n*16byte) 読み取ったブロックデータ
    /// </summary>
    public IntPtr BlockData;
}

#endregion