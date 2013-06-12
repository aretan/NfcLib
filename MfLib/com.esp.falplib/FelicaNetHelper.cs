using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace com.esp.falplib
{
    /// <summary>
    /// FALPライブラリ C#ラッパー
    /// </summary>
    public class FelicaNetHelper
    {
        /// <summary>
        /// 完全Staticクラス
        /// </summary>
        private FelicaNetHelper()
        {
        }

        /// <summary>ブロックのバイト数</summary>
        public const int BLOCK_LENGTH = FelicaHelper.BLOCK_LENGTH;

        /// <summary>IDMのバイト数</summary>
        public const int IDM_LENGTH = FelicaHelper.IDM_LENGTH;

        //Wrapper functions
        /// <summary>
        /// ライブラリの初期化
        /// </summary>
        /// <returns>T:正常終了 F:異常終了</returns>
        public static void InitializeLibrary()
        {
            if (!felica_dll.InitializeLibrary())
            {
                Debug.WriteLine("FelicaNetHelper Initialize Error");
                FelicaErrorType error;
                if (felica_dll.GetLastErrorType(out error))
                {
                    if (error != FelicaErrorType.FELICA_LIBRARY_ALREADY_INITIALIZED)
                        throw new FelicaException(error);
                }
                else
                {
                    throw new FelicaException(FelicaErrorType.FELICA_UNKNOWN_ERROR);
                }
            }
            Debug.WriteLine("FelicaNetHelper Initialize OK!");
        }

        /// <summary>
        /// リーダーライターを開く
        /// </summary>
        /// <returns>T:正常終了 F:異常終了</returns>
        public static void OpenReaderWriterAuto()
        {
            if (!felica_dll.OpenReaderWriterAuto())
            {
                FelicaErrorType error;
                if (felica_dll.GetLastErrorType(out error))
                {
                    if (error != FelicaErrorType.FELICA_READER_WRITER_ALREADY_OPENED)
                        throw new FelicaException(error);
                }
                else
                {
                    throw new FelicaException(FelicaErrorType.FELICA_UNKNOWN_ERROR);
                }
            }
        }

        /// <summary>
        /// リーダー・ライターの解放
        /// </summary>
        /// <returns>T:正常終了 F:異常終了</returns>
        public static bool CloseReaderWriter()
        {
            return felica_dll.CloseReaderWriter();
        }

        /// <summary>
        /// ライブラリの解放
        /// </summary>
        /// <returns>T:正常終了 F:異常終了</returns>
        public static bool DisposeLibrary()
        {
            bool res = felica_dll.DisposeLibrary();
            Debug.WriteLine("FelicaNetHelper Dispose:" + res.ToString());
            return res;
        }

        /// <summary>
        /// R/Wを専有する
        /// </summary>
        /// <returns></returns>
        public static void TransactionLock()
        {
            if (!felica_dll.TransactionLock())
                RaiseException(typeof(FelicaErrorType));
        }

        /// <summary>
        /// R/Wを専有する
        /// </summary>
        /// <returns></returns>
        public static bool TransactionUnlock()
        {
            return felica_dll.TransactionUnlock();
        }

        /// <summary>
        /// FALP Initiatorを開始する
        /// </summary>
        /// <returns></returns>
        public static void FalpOpen()
        {
            if (!felica_dll.FalpOpen())
                RaiseException(typeof(FalpErrorType));

        }

        /// <summary>
        /// FALP Initiatorを終了する
        /// </summary>
        /// <returns></returns>
        public static bool FalpClose()
        {
            return felica_dll.FalpClose();
        }

        /// <summary>
        /// Targetに接続する
        /// </summary>
        /// <returns></returns>
        public static void ConnectFalp(byte[] appId, ushort timeout, ushort handshakeTimeout, byte[] data)
        {
            uint dataLen = data != null ? (uint)data.Length : 0;
            bool res = felica_dll.FalpConnect(
                timeout,
                handshakeTimeout,
                appId,
                (byte)appId.Length,
                data,
                ref dataLen);
            if (!res)
            {
                FalpErrorType error;
                if (felica_dll.FalpGetLastErrorType(out error))
                {
                    if (error != FalpErrorType.FALP_SHUTDOWN)
                        throw new FelicaException(error);
                }
                else
                {
                    throw new FelicaException(FelicaErrorType.FELICA_UNKNOWN_ERROR);
                }
            }
        }

        /// <summary>
        /// Targetから切断する
        /// </summary>
        /// <returns></returns>
        public static void FalpShutdown()
        {
            if (!felica_dll.FalpShutdown())
                RaiseException(typeof(FalpErrorType));

        }

        /// <summary>
        /// データを送信する
        /// </summary>
        /// <param name="data">データ</param>
        /// <param name="offset">バッファ内の位置</param>
        /// <param name="length">データの長さ</param>
        public static void SendByFalp(byte[] data, int offset, int length)
        {
            bool finish = false;
            uint sendCount = 0;

            Thread th = new Thread(
                new ThreadStart(delegate()
            {
                while (true)
                {
                    if (sendCount >= length)
                    {
                        finish = true;
                        break;
                    }

                    byte[] buf = new byte[length - sendCount];
                    Array.Copy(data, sendCount, buf, 0, buf.Length);

                    //送信
                    uint buffered = (uint)buf.Length;
                    if (!felica_dll.FalpSend(buf, ref buffered))
                        break;

                    Debug.WriteLine("FALP Send Prepare:" + sendCount + "," + buffered);
                    sendCount += buffered;

                    //待つ
                    FalpEvent detected;
                    if (!felica_dll.FalpWaitEvent(1000, out detected
                        , FalpEvent.SEND_READY | FalpEvent.SEND_EMPTY))
                        break;
                }
                return;
            }));
            th.Start();
            th.Join();

            if (!finish)
                RaiseException(typeof(FalpErrorType));
        }

        /// <summary>
        /// FALPによるデータの受信
        /// </summary>
        /// <param name="buffer">受信データ格納先</param>
        /// <param name="offset">位置</param>
        /// <param name="timeout">タイムアウト時間[ms]</param>
        /// <returns>受信したデータの長さ</returns>
        public static int RecvByFalp(byte[] buffer, int offset, ushort timeout)
        {
            FalpEvent detected;
            if (!felica_dll.FalpWaitEvent(timeout, out detected, FalpEvent.RECV_READY))
                throw new TimeoutException();

            byte[] temp = new byte[256];
            uint length = (uint)temp.Length;
            if (!felica_dll.FalpRecv(temp, ref length))
                RaiseException(typeof(FalpErrorType));

            Array.Copy(temp, 0, buffer, offset, length);
            return (int)length;
        }

        /// <summary>
        /// Pollingの実行
        /// </summary>
        /// <param name="sysCode">システムコード</param>
        /// <param name="idm">Idm</param>
        /// <returns>T:捕捉, F:捕捉なし</returns>
        public static bool Polling(ushort sysCode, byte[] idm)
        {
            return FelicaHelper.ReadIdm(sysCode, idm);
        }

                /// <summary>
        /// 鍵無し領域からの読み出し
        /// </summary>
        /// <param name="svCode">サービスコード</param>
        /// <param name="idm">Idm</param>
        /// <param name="block">ブロックリスト</param>
        /// <param name="buffer">取得データバッファ</param>
        /// <param name="offset">保存開始位置</param>
        public static void ReadBlockData(ushort svCode, byte[] idm, int[] block, byte[] buffer, int offset)
        {
            int len;
            if(!FelicaHelper.ReadBlockData(svCode, idm, block, buffer, offset, out len))
                RaiseException(typeof(FelicaErrorType));

            //読み取りサイズ異常
            if (len != BLOCK_LENGTH * block.Length)
                throw new FelicaException(FelicaErrorType.FELICA_READ_BLOCK_WITHOUT_ENCRYPTION_ERROR);
        }

        /// <summary>
        /// 鍵無し領域への書込み
        /// </summary>
        /// <param name="svCode">サービスコード</param>
        /// <param name="idm">Idm</param>
        /// <param name="block">ブロックリスト</param>
        /// <param name="buffer">書込みデータバッファ</param>
        /// <param name="offset">バッファ内のデータ開始位置</param>
        /// <param name="length">データの長さ</param>
        public static void WriteBlockData(ushort svCode, byte[] idm, int[] block, byte[] buffer, int offset, int length)
        {
            if(!FelicaHelper.WriteBlockData(svCode, idm, block, buffer, offset, length))
                RaiseException(typeof(FelicaErrorType));
        }

        /// <summary>
        /// FelicaExceptionをthrowする
        /// </summary>
        /// <param name="type">FelicaErorrTyper</param>
        private static void RaiseException(Type type)
        {
            if (type == typeof(FelicaErrorType))
            {
                FelicaErrorType error = FelicaErrorType.FELICA_UNKNOWN_ERROR;
                felica_dll.GetLastErrorType(out error);
                throw new FelicaException(error);
            }
            else if (type == typeof(FalpErrorType))
            {
                FalpErrorType error = FalpErrorType.FALP_UNKNOWN_ERROR;
                felica_dll.FalpGetLastErrorType(out error);
                throw new FelicaException(error);
            }
        }
    }
}
