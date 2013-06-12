using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;
using com.esp.nfclib.card;
using com.esp.common;

namespace com.esp.nfclib
{
    /// <summary>
    /// Pasori用Mifareライブラリ
    /// </summary>
    internal class NfcReader:IReader, IMifareReader, IDisposable
    {
        #region 定数
        private const int BUFFER_LENGTH = 512;
        private const int RETRY_POLLING = 10;
        private const int WAIT_API_TIME = 100;
        private const int POLLING_TIMEOUT = 2000;
        #endregion

        #region プロパティ
        /// <summary>ポーリング時間[ms]</summary>
        public uint PollingTimeout { get; set; }
        #endregion

        #region フィールド
        private static felica_nfc_dll_wrapper FeliCaNfcDllWrapperClass = new felica_nfc_dll_wrapper();
        private bool detected;
        private DEVICE_DATA_NFC_14443A_18092_106K device;
        private byte[] cmd,res;
        private NfcLib lib;
        private bool initialized, opend;
        #endregion

        /// <summary>
        /// Mifareライブラリを指定する
        /// </summary>
        /// <param name="lib"></param>
        public NfcReader(NfcLib lib)
        {
            this.lib = lib;
        }

        ~NfcReader()
        {
            this.DisposeLibrary();
        }

        /// <summary>
        /// ライブラリの初期化
        /// </summary>
        public void InitializeLibrary()
        {
            bool bRet = false;
            bRet = FeliCaNfcDllWrapperClass.FeliCaLibNfcInitialize();
            if (bRet == false)
            {
                Debug.Write("Failed: FeliCaLibNfcInitialize\n");
                Exception ex = GetMfException();
                if (!(ex is NfcLibException) ||
                    ((NfcLibException)ex).ApiError!=ApiErrorKind.FELICA_NFC_E_ALREADY_INITIALIZED)
                {
                    ErrorRoutine();
                    throw ex;
                }
            }
            initialized = true;

            StringBuilder port = new StringBuilder(20);
            port.Append("");
            bRet = FeliCaNfcDllWrapperClass.FeliCaLibNfcOpen(ref port);
            if (bRet == false)
            {
                Debug.Write("Failed: FeliCaLibNfcOpen\n");
                Exception ex = GetMfException();
                if (!(ex is NfcLibException) ||
                    ((NfcLibException)ex).ApiError != ApiErrorKind.FELICA_NFC_E_PORT_OPENED)
                {
                    ErrorRoutine();
                    throw ex;
                }
            }
            opend = true;

            cmd = new byte[BUFFER_LENGTH];
            res = new byte[BUFFER_LENGTH];
            PollingTimeout = POLLING_TIMEOUT;
            Console.Write("Initialized MfLib\n");
        }

        /// <summary>
        /// ライブラリの解放
        /// (DisposeLibraryを実行した場合、次の利用では再びInitializeLibraryを実行する)
        /// </summary>
        /// <returns></returns>
        public bool DisposeLibrary()
        {
            bool bRet = false;
            if (detected)
            {
                UInt32 RE_NOTIFICATION_SAME_DEVICE = 0x00;
                UInt32 stop_mode = RE_NOTIFICATION_SAME_DEVICE;
                bRet = FeliCaNfcDllWrapperClass.FeliCaLibNfcStopDevAccess(stop_mode);
                if (bRet == false)
                {
                    Debug.Write("Failed: FeliCaLibNfcStopDevAccess\n");
                    ErrorRoutine();
                    return false;
                }
                detected = false;
            }

            if (opend)
            {
                bRet = FeliCaNfcDllWrapperClass.FeliCaLibNfcClose();
                if (bRet == false)
                {
                    Debug.Write("Failed: FeliCaLibNfcClose\n");
                    ErrorRoutine();
                    return false;
                }
                opend = false;
            }

            if (initialized)
            {
                bRet = FeliCaNfcDllWrapperClass.FeliCaLibNfcUninitialize();
                if (bRet == false)
                {
                    Debug.Write("Failed: FeliCaLibNfcUninitialize\n");
                    ErrorRoutine();
                    return false;
                }
                initialized = false;
            }

            cmd = null;
            res = null;
            Debug.Write("Success Dispose Library\n");
            return true;
        }

        public void Dispose()
        {
            DisposeLibrary();
        }

        /// <summary>
        /// ポーリング
        /// </summary>
        /// <returns>カード情報,NULL=未検出</returns>
        public NfcTag Polling()
        {
            if(detected)
                StopAccess();

            if (FeliCaNfcDllWrapperClass.FeliCaLibNfcPollAndStartDevAccess(PollingTimeout, out device))
            {
                detected = true;
                return CreateCard();
            }
            else
            {
                Debug.WriteLine(GetMfException().Message);
                return null;
            }
        }

        /// <summary>
        /// カードのアクセス権を解放する
        /// </summary>
        public void StopAccess()
        {
            bool bRet = FeliCaNfcDllWrapperClass.FeliCaLibNfcStopDevAccess(felica_nfc_dll_wrapper.RE_NOTIFICATION_SAME_DEVICE);
            if (bRet == false)
            {
                Debug.Write("Failed: FeliCaLibNfcStopDevAccess\n");
                throw GetMfException();
            }
            Thread.Sleep(WAIT_API_TIME*3);
            detected = false;
            Debug.Write("Success stop device accesss\n");
        }

        /// <summary>
        /// Mifare Authentication A/B
        /// </summary>
        /// <param name="useA">'A'/'B'</param>
        /// <param name="block">アクセス対象ブロック番号</param>
        /// <param name="key">認証鍵[6Byte]</param>
        public void Authentication(bool useA, byte block, byte[] key)
        {
            if (!detected)
                throw new NfcLibException(ApiErrorKind.APP_MF_NOT_DETECTED, 0);

            //Authentication:CMD,ADDRESS,KEY[6],UID[4-7]
            int index = 0;
            cmd[index++] = (byte)(useA ? MfCmdCode.CMD_AUTH_A: MfCmdCode.CMD_AUTH_B);
            
            //ADDRESS
            cmd[index++] = block;

            //KEY
            Array.Copy(key, 0, cmd, 2, MfConst.KEY_LENGTH);
            index += MfConst.KEY_LENGTH;

            //UID
            Array.Copy(device.NFCID1, 0, cmd, index, device.NFCID1_size);
            index += device.NFCID1_size;

            UInt16 outLen = 0x00;
            NfcCommand(cmd, (ushort)index, out outLen);
            Debug.Write("Success Authentication\n");
        }

        /// <summary>
        /// Mifare Authentication A/B
        /// </summary>
        /// <param name="useA">'A'/'B'</param>
        /// <param name="key">認証鍵[6Byte]</param>
        /// <param name="block">アクセス対象ブロック番号</param>
        /// <returns>T:成功,F:失敗</returns>
        public bool TryAuthentication(bool useA, byte[] key, byte block)
        {
            try
            {
                Authentication(useA, block, key);
                return true;
            }
            catch (NfcLibException ex)
            {
                Debug.WriteLine("Failed: TryAutentication" + ex.ApiError.ToString());
                return false;
            }
        }

        /// <summary>
        /// 指定ブロックの読み込み
        /// Classic:16Byte,UL:4Byte
        /// </summary>
        /// <param name="blockOrPage">ブロック番号、ページ番号</param>
        /// <param name="buffer">バッファ</param>
        /// <param name="offset">バッファ内の保存開始位置</param>
        public void ReadBlockData(byte blockOrPage, byte[] buffer, int offset)
        {
            if (!detected)
                throw new NfcLibException(ApiErrorKind.APP_MF_NOT_DETECTED, 0);

            //Authentication:CMD,ADDRESS
            int index = 0;
            cmd[index++] = (byte)MfCmdCode.CMD_READ;

            //ADDRESS
            cmd[index++] = blockOrPage;

            UInt16 outLen = 0x00;
            NfcCommand(cmd, (ushort)index, out outLen);
            Array.Copy(res, 0, buffer, offset, NfcLib.MF_BLOCK_LENGTH);
        }

        /// <summary>
        /// 指定ブロック(16Byte)の書き込み
        /// </summary>
        /// <param name="block">ブロック番号</param>
        /// <param name="buffer">データバッファ</param>
        /// <param name="offset">バッファ内のデータ開始位置</param>
        public void WriteBlockData(byte block, byte[] buffer, int offset)
        {
            if (!detected)
                throw new NfcLibException(ApiErrorKind.APP_MF_NOT_DETECTED, 0);

            //Authentication:CMD,ADDRESS,DATA[16]
            int index = 0;
            cmd[index++] = (byte)MfCmdCode.CMD_WRITE;

            //ADDRESS
            cmd[index++] = block;

            //DATA[16]
            Array.Copy(buffer, offset, cmd, index, NfcLib.MF_BLOCK_LENGTH);
            index += NfcLib.MF_BLOCK_LENGTH;

            UInt16 outLen = 0x00;
            NfcCommand(cmd, (ushort)index, out outLen);
        }

        /// <summary>
        /// 指定ページ(4Byte)の書き込み
        /// </summary>
        /// <param name="page">ページ番号</param>
        /// <param name="buffer">データバッファ</param>
        /// <param name="offset">バッファ内のデータ開始位置</param>
        public void WritePageData(byte page, byte[] buffer, int offset)
        {
            if (!detected)
                throw new NfcLibException(ApiErrorKind.APP_MF_NOT_DETECTED, 0);

            //Authentication:CMD,ADDRESS,DATA[4]
            int index = 0;
            cmd[index++] = (byte)MfCmdCode.CMD_UL_WRITE;

            //ADDRESS
            cmd[index++] = page;

            //DATA
            Array.Copy(buffer, offset, cmd, index, NfcLib.MF_UL_PAGE_LENGTH);
            index += NfcLib.MF_UL_PAGE_LENGTH;

            UInt16 outLen = 0x00;
            NfcCommand(cmd, (ushort)index, out outLen);
        }

        /// <summary>
        /// NFCコマンドの送受信
        /// </summary>
        /// <param name="data">コマンドパケット</param>
        /// <param name="length">データ長</param>
        /// <param name="outLen">受信データ長</param>
        private void NfcCommand(byte[] data, ushort length, out UInt16 outLen)
        {
            outLen = 0;
            bool bRet = FeliCaNfcDllWrapperClass.FeliCaLibNfcThru(
                data,
                length,
                res,
                ref outLen);

            if (bRet == false)
            {
                StopAccess();
                Debug.Write("Failed: FeliCaLibNfcThru\n");
                throw GetMfException();
            }

            Debug.Write("Success: FeliCaLibNfcThru\n");
        }


        /// <summary>
        /// ライブラリを強制解放する
        /// </summary>
        private void ErrorRoutine()
        {
            try
            {
                UInt32[] error_info = new UInt32[2] { 0, 0 };
                FeliCaNfcDllWrapperClass.FeliCaLibNfcGetLastError(error_info);
                Console.Write("error_info[0]: 0x{0:X8}\nerror_info[1]: 0x{1:X8}\n", error_info[0], error_info[1]);
            }
            catch (Exception)
            {
            }

            try
            {
                FeliCaNfcDllWrapperClass.FeliCaLibNfcClose();
            }
            catch (Exception)
            {
            }
            opend = false;

            try
            {
                FeliCaNfcDllWrapperClass.FeliCaLibNfcUninitialize();
            }
            catch (Exception)
            {
            }
            initialized = false;
            return;
        }

        /// <summary>
        /// Pasoriで検出された例外を取得する
        /// </summary>
        private Exception GetMfException()
        {
            UInt32[] error_info = new UInt32[2] { 0, 0 };
            if (FeliCaNfcDllWrapperClass.FeliCaLibNfcGetLastError(error_info))
            {
                ApiErrorKind api = (ApiErrorKind)error_info[0];
                UInt32 driver = error_info[1];
                return new NfcLibException(api, driver);
            }
            else
            {
                return new NfcLibException(ApiErrorKind.APP_UNKNOWN, 0);
            }
        }

        /// <summary>
        /// Mifareオブジェクトの生成
        /// </summary>
        private NfcTag CreateCard()
        {
            ushort atqa = (ushort)BigEndian.SwapEndian((ushort)device.sens_res);
            byte[] uid = new byte[device.NFCID1_size];
            Array.Copy(device.NFCID1, 0, uid, 0, uid.Length);

            Mifare card = null;
            if (atqa == MfConst.ATQA_MFCL1K)
            {
                card = new MifareCL(lib, uid);
            }
            else if (atqa == MfConst.ATQA_MFUL)
            {
                card = new MifareUL(lib, uid);
            }
            else if (atqa == MfConst.ATQA_MFCL4K)
            {
                card = new MifareCL4K(lib, uid);
            }
            Debug.WriteLine("create_card:" + Utility.ByteToHex(card.Uid, 0, card.Uid.Length));
            return card;
        }
    }
}
