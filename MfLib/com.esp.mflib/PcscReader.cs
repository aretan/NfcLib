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
using System.Text.RegularExpressions;
using System.IO;

namespace com.esp.nfclib
{
    /// <summary>
    /// Pasori用Mifareライブラリ
    /// </summary>
    internal class PcscReader:IReader, IMifareReader, IFelicaReader, IDisposable
    {
        #region 定数
        private const int BUFFER_LENGTH = 512;
        #endregion

        #region フィールド
        private byte[] cmd,res;

        //RC-S380用
        private IntPtr rwContext;
        private IntPtr cardContext;
        private String readerName;
        private uint protocol;
        private byte[] authKey;
        private NfcLib lib;
        #endregion

        /// <summary>
        /// Pasori PC/SCの利用可否
        /// </summary>
        /// <returns>T:利用可能</returns>
        public static bool CheckPcsc()
        {
            //RC-S380以降
            IntPtr context = IntPtr.Zero;
            if (WinSCard.EstablishContext(ref context))
            {
                String readerName = WinSCard.ListReaders(context);
                WinSCard.ReleaseContext(context);

                if (readerName != null)
                {
                    Regex ver = new Regex("PaSoRi\\s+([0-9]+(\\.[0-9]+)*)", RegexOptions.IgnoreCase);
                    Match m = ver.Match(readerName);
                    if (m.Success)
                    {
                        String verStr = m.Groups[1].Value;
                        if (float.Parse(verStr) >= 3.0f)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        #region 共通メソッド
        /// <summary>
        /// mifareライブラリを指定する
        /// </summary>
        /// <param name="lib"></param>
        public PcscReader(NfcLib lib)
        {
            this.lib = lib;
        }

        /// <summary>
        /// ライブラリを開放する
        /// </summary>
        ~PcscReader()
        {
            this.Dispose();
        }

        /// <summary>
        /// ライブラリの初期化
        /// </summary>
        public void InitializeLibrary()
        {
            //RC-S380以降
            if (WinSCard.EstablishContext(ref rwContext))
            {
                readerName = WinSCard.ListReaders(rwContext);
                if (readerName != null)
                {
                    Regex ver = new Regex("PaSoRi\\s+([0-9]+(\\.[0-9]+)*)", RegexOptions.IgnoreCase);
                    Match m = ver.Match(readerName);
                    if (m.Success)
                    {
                        String verStr = m.Groups[1].Value;
                        if (float.Parse(verStr) >= 3.0f)
                        {
                            cmd = new byte[BUFFER_LENGTH];
                            res = new byte[BUFFER_LENGTH];
                            authKey = null;
                            Debug.WriteLine("PcscReader Initialize OK!");
                            return;
                        }
                    }
                }
            }
            Debug.WriteLine("PcscReader Error!");
            DisposeLibrary();
            throw new PcscException(WinSCardError.SCARD_E_NOT_READY, null);
        }

        /// <summary>
        /// ライブラリの解放
        /// (DisposeLibraryを実行した場合、次の利用では再びInitializeLibraryを実行する)
        /// </summary>
        /// <returns></returns>
        public bool DisposeLibrary()
        {
            if (cardContext != IntPtr.Zero)
            {
                WinSCard.Disconnect(cardContext, 0);
                cardContext = IntPtr.Zero;
            }

            if (rwContext != IntPtr.Zero)
            {
                WinSCard.ReleaseContext(rwContext);
                rwContext = IntPtr.Zero;
            }
            res = cmd = authKey = null;
            Debug.WriteLine("PcscReader Disposed!");
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
            cardContext = IntPtr.Zero;
            if (!WinSCard.Connect(rwContext, readerName, WinSCard.SCARD_SHARE_SHARED,
                WinSCard.SCARD_PROTOCOL_T0 | WinSCard.SCARD_PROTOCOL_T1, ref cardContext, ref protocol))
                return null;
            try
            {
                Debug.WriteLine("PcscReader:CardConnected!");
                return CreateCard();
            }
            catch (PcscException)
            {
                StopAccess();
                return null;
            }
        }

        /// <summary>
        /// カードのアクセス権を解放する
        /// </summary>
        public void StopAccess()
        {
            if (cardContext != IntPtr.Zero)
            {
                WinSCard.Disconnect(cardContext, 0);
                cardContext = IntPtr.Zero;
            }
        }

        /// <summary>
        /// Pasoriで検出された例外を取得する
        /// </summary>
        private Exception GetException()
        {
            if (rwContext == IntPtr.Zero)
                return new PcscException(WinSCardError.SCARD_E_NO_READERS_AVAILABLE, null);

            if (cardContext == IntPtr.Zero)
                return new PcscException(WinSCardError.SCARD_W_REMOVED_CARD, null);

            if (WinSCard.GetLastError() != WinSCardError.SCARD_S_SUCCESS)
            {
                return new PcscException(WinSCard.GetLastError(), null);
            }
            else
            {
                return new PcscException(WinSCard.GetLastError(), WinSCard.GetLastRes());
            }
        }

        /// <summary>
        /// カードオブジェクトの生成
        /// </summary>
        private NfcTag CreateCard()
        {
            uint status;
            byte[] atr;
            if (!WinSCard.GetStatus(cardContext, out readerName, out status, out protocol, out atr))
                return null;

            int index = 0;
            uint recvLen = 0;

            index += SetHeader(0xCA, 0);

            cmd[index++] = (byte)res.Length;

            PcscCommand(cmd, index, out recvLen);

            byte[] uid = new byte[recvLen - 2];
            Array.Copy(res, 0, uid, 0, uid.Length);

            if (atr[13] == 0x00 && atr[14] == 0x01)
            {
                return new MifareCL(lib, uid);
            }
            else if (atr[13] == 0x00 && atr[14] == 0x02)
            {
                return new MifareCL4K(lib, uid);
            }
            else if (atr[13] == 0x00 && atr[14] == 0x03)
            {
                return new MifareUL(lib, uid);
            }
            else if (atr[13] == 0x00 && atr[14] == 0x3b)
            {
                return new Felica(lib, uid);
            }
            else if (atr[13] == 0x00 && atr[14] == 0x3a)
            {
                return new NTAG203(lib, uid);
            }
            return null;
        }
        #endregion

        #region FeliCa用
        /// <summary>
        /// FeliCaシステムコード指定
        /// </summary>
        /// <param name="sysCode">システムコード</param>
        public Felica Select(ushort sysCode)
        {
            int index = 0;
            cmd[index++] = 0xff;
            cmd[index++] = 0xfe;
            cmd[index++] = 0x00;//thru
            cmd[index++] = 0x00;//timeout
            cmd[index++] = (byte)(1 + 2 + 2);//CMD[1] + SYSCODE[2] + PAD[2]

            cmd[index++] = 0x00;//CMD
 
            cmd[index++] = (byte)(sysCode >> 8);
            cmd[index++] = (byte)(sysCode & 0xff);
            cmd[index++] = 0x00;
            cmd[index++] = 0x00;

            uint recvLen;
            Debug.WriteLine("PcscReader:FeliCa Select.");
            PcscCommand(cmd, index, out recvLen);

            byte[] idm = new byte[NfcLib.FC_IDM_LENGTH];
            Array.Copy(res, 1, idm, 0, idm.Length);
            return new Felica(lib, idm);
        }

        /// <summary>
        /// FeliCaブロック読込み
        /// </summary>
        /// <param name="svCode">サービスコード</param>
        /// <param name="block">ブロックリスト</param>
        /// <param name="buffer">データバッファ</param>
        /// <param name="offset">バッファ内のデータ開始位置</param>
        public void ReadBlockData(ushort svCode, int[] block, byte[] buffer, int offset)
        {
            byte[] blockList = com.esp.falplib.FelicaHelper.makeBlockList(block);

            int index = 0;
            cmd[index++] = 0xff;
            cmd[index++] = 0xfe;
            cmd[index++] = 0x00;//thru
            cmd[index++] = 0x00;//timeout
            //CMD[1] + SV_COUNT[1] + SV_LIST[2*count] + BLOCK_COUNT + BLOCK_LIST[可変長]
            cmd[index++] = (byte)(1 + 1 + 2 + 1 + blockList.Length);

            cmd[index++] = 0x06;//CMD

            cmd[index++] = 1;//SV_COUNT

            cmd[index++] = (byte)(svCode & 0xff);
            cmd[index++] = (byte)(svCode >> 8);

            cmd[index++] = (byte)block.Length;//BLOCK_COUNT
            Array.Copy(blockList, 0, cmd, index, blockList.Length);
            index += blockList.Length;

            uint recvLen;
            Debug.WriteLine("PcscReader:ReadBlockData(FeliCa)");
            PcscCommand(cmd, index, out recvLen);

            if (res[1] != 0 || res[2] != 0)
                throw new PcscException(WinSCardError.SCARD_F_COMM_ERROR, new byte[] { res[1], res[2] });

            //読込み結果
            Array.Copy(res, 4, buffer, offset, block.Length * NfcLib.FC_BLOCK_LENGTH);
        }

        /// <summary>
        /// FeliCa指定ブロック(16Byte)の書き込み
        /// </summary>
        /// <param name="svCode">サービスコード</param>
        /// <param name="block">ブロック番号</param>
        /// <param name="buffer">データバッファ</param>
        /// <param name="offset">バッファ内のデータ開始位置</param>
        public void WriteBlockData(ushort svCode, int[] block, byte[] buffer, int offset)
        {
            byte[] blockList = com.esp.falplib.FelicaHelper.makeBlockList(block);

            int index = 0;
            cmd[index++] = 0xff;
            cmd[index++] = 0xfe;
            cmd[index++] = 0x00;//thru
            cmd[index++] = 0x00;//timeout
            //CMD[1] + IDM[8] + SV_COUNT[1] + SV_LIST[2*count] + BLOCK_COUNT + BLOCK_LIST[可変] + DATA
            cmd[index++] = (byte)(1 + 1 + 2 + 1 + blockList.Length + block.Length * NfcLib.FC_BLOCK_LENGTH);
            cmd[index++] = 0x08;//CMD

            cmd[index++] = 1;//SV_COUNT
            cmd[index++] = (byte)(svCode & 0xff);
            cmd[index++] = (byte)(svCode >> 8);

            cmd[index++] = (byte)block.Length;//BLOCK_COUNT
            Array.Copy(blockList, 0, cmd, index, blockList.Length);
            index += blockList.Length;

            Array.Copy(buffer, offset, cmd, index, NfcLib.FC_BLOCK_LENGTH * block.Length);
            index += NfcLib.FC_BLOCK_LENGTH * block.Length;

            uint recvLen;
            Debug.WriteLine("PcscReader:WriteBlockData(FeliCa)");
            PcscCommand(cmd, index, out recvLen);

            if (res[1] != 0 || res[2] != 0)
                throw new PcscException(WinSCardError.SCARD_F_COMM_ERROR, new byte[] { res[1], res[2] });
        }
        #endregion

        #region Mifare用
        /// <summary>
        /// Mifare Authentication A/B
        /// </summary>
        /// <param name="useA">'A'/'B'</param>
        /// <param name="block">アクセス対象ブロック番号</param>
        /// <param name="key">認証鍵[6Byte]</param>
        public void Authentication(bool useA, byte block, byte[] key)
        {
            if (rwContext == IntPtr.Zero || cardContext == IntPtr.Zero)
                throw GetException();

            uint recvLen;
            int index = 0;

            //鍵の変更がなければ省略
            if (!IsSameKey(key))
            {
                index += SetHeader(0x82, 0);//block部にKeyNumberが入る

                cmd[index++] = (byte)key.Length;

                Array.Copy(key, 0, cmd, index, key.Length);
                index += key.Length;

                Debug.WriteLine("PcscReader:AuthKey load");
                PcscCommand(cmd, index, out recvLen);
                authKey = (byte[])key.Clone();
            }

            //認証
            index = 0;
            index += SetHeader(0x86, 0);

            cmd[index++] = 0x05;

            cmd[index++] = 0x01;//Version
            cmd[index++] = 0x00;

            cmd[index++] = block;
                
            cmd[index++] = (byte)(useA?0x60:0x61);
            cmd[index++] = 0;//Key kind

            Debug.WriteLine("PcscReader:Authentication.");
            PcscCommand(cmd, index, out recvLen);
        }

        private bool IsSameKey(byte[] newKey)
        {
            if (authKey == null) return false;
            for (int i = 0; i < newKey.Length; i++)
            {
                if (authKey[i] != newKey[i])
                    return false;
            }
            return true;
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
            int index = 0;

            index += SetHeader(0xb0, blockOrPage);

            cmd[index++] = NfcLib.MF_BLOCK_LENGTH;

            uint recvLen;
            Debug.WriteLine("PcscReader:ReadBlockData");
            PcscCommand(cmd, index, out recvLen);

            //読込み結果
            Array.Copy(res, 0, buffer, offset, recvLen - 2);
        }

        /// <summary>
        /// 指定ブロック(16Byte)の書き込み
        /// </summary>
        /// <param name="block">ブロック番号</param>
        /// <param name="buffer">データバッファ</param>
        /// <param name="offset">バッファ内のデータ開始位置</param>
        public void WriteBlockData(byte block, byte[] buffer, int offset)
        {
            int index = 0;

            index += SetHeader(0xd6, block);

            cmd[index++] = NfcLib.MF_BLOCK_LENGTH;

            Array.Copy(buffer, offset, cmd, index, NfcLib.MF_BLOCK_LENGTH);
            index += NfcLib.MF_BLOCK_LENGTH;

            uint recvLen;
            Debug.WriteLine("PcscReader:WriteBlockData");
            PcscCommand(cmd, index, out recvLen);
        }

        /// <summary>
        /// 指定ページ(4Byte)の書き込み
        /// </summary>
        /// <param name="page">ページ番号</param>
        /// <param name="buffer">データバッファ</param>
        /// <param name="offset">バッファ内のデータ開始位置</param>
        public void WritePageData(byte page, byte[] buffer, int offset)
        {
            int index = 0;

            index += SetHeader(0xd6, page);

            cmd[index++] = NfcLib.MF_UL_PAGE_LENGTH;

            Array.Copy(buffer, offset, cmd, index, NfcLib.MF_UL_PAGE_LENGTH);
            index += NfcLib.MF_UL_PAGE_LENGTH;

            uint recvLen;
            Debug.WriteLine("PcscReader:WritePageData");
            PcscCommand(cmd, index, out recvLen);
        }
        #endregion

        #region PC/SC API
        private int SetHeader(byte code, byte blockOrPage)
        {
            int index = 0;
            cmd[index++] = 0xff;
            cmd[index++] = code;
            cmd[index++] = 0;
            cmd[index++] = blockOrPage;
            return index;
        }

        private void PcscCommand(byte[] data, int len, out uint recvLen)
        {
            SCARD_IO_REQUEST reqCpi = WinSCard.GetPci(protocol);
            SCARD_IO_REQUEST resCpi = WinSCard.GetDefPci();

            recvLen = (uint)res.Length;
            if (!WinSCard.Transmit(cardContext, reqCpi, data, len, ref resCpi, res, ref recvLen))
            {
                throw GetException();
            }

            if (res[recvLen-2] != 0x90 || res[recvLen-1] != 0x00)
                throw new PcscException(WinSCardError.SCARD_F_UNKNOWN_ERROR, new byte[] { res[recvLen-2], res[recvLen-1] });
        }
        #endregion
    }
}
