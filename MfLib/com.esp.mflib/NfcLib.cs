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
    /// Pasori用Nfcライブラリ
    /// </summary>
    public class NfcLib
    {
        #region 定数
        /// <summary>MifareClassicは読み込み、書き込み16Byte単位</summary>
        public const int MF_BLOCK_LENGTH = 16;
        /// <summary>Ultralightは書き込み4Byte単位、読み込みは16Byte単位</summary>
        public const int MF_UL_PAGE_LENGTH = 4;
        /// <summary>Ultralightは0～15ページ。LockByte=2,OTP=3,ユーザー領域は4～15</summary>
        public const int MF_UL_PAGE_MAX = 15;
        /// <summary>Felicaは読み込み、書き込み16Byte単位</summary>
        public const int FC_BLOCK_LENGTH = 16;
        /// <summary>FelicaはIDMは8Byte</summary>
        public const int FC_IDM_LENGTH = 8;
        #endregion

        #region プロパティ
        /// <summary>接続モード</summary>
        public ConnetKind Connection{get; private set;}
        #endregion

        #region フィールド
        private IReader reader;
        private UseCard useCard;
        #endregion

        /// <summary>
        /// ライブラリの初期化
        /// </summary>
        public void InitializeLibrary(UseCard useCard)
        {
            //PC/SC接続を優先する
            if (PcscReader.CheckPcsc())
            {
                reader = new PcscReader(this);
                Connection = ConnetKind.Pcsc;
            }
            else
            {
                //PC/SCが使えない場合
                if (useCard == (UseCard.Felica | UseCard.Mifare))
                {
                    throw new PcscException(WinSCardError.SCARD_E_NOT_READY, null);
                }
                else if (useCard == UseCard.Mifare)
                {
                    reader = new NfcReader(this);
                    Connection = ConnetKind.Nfc;
                }
                else
                {
                    reader = new FelicaReader(this);
                    Connection = ConnetKind.Felica;
                }
            }
            reader.InitializeLibrary();
            this.useCard = useCard;
        }

        /// <summary>
        /// ライブラリの解放
        /// (DisposeLibraryを実行した場合、次の利用では再びInitializeLibraryを実行する)
        /// </summary>
        /// <returns></returns>
        public bool DisposeLibrary()
        {
            bool res = true;

            if (reader != null)
            {
                reader.DisposeLibrary();
                reader = null;
                Connection = ConnetKind.None;
            }
            
            return res;
        }

        /// <summary>
        /// ポーリング
        /// </summary>
        /// <returns>カード情報,NULL=未検出</returns>
        public NfcTag Polling(ushort sysCode)
        {
            NfcTag tag = reader.Polling();

            if ((useCard&UseCard.Felica)==0 && tag is Felica)
            {
                tag.Release();
                tag = null;
            }
            else if ((useCard&UseCard.Mifare)==0 && tag is Mifare)
            {
                tag.Release();
                tag = null;
            }

            if (tag is Felica)
            {
                try
                {
                    tag = ((IFelicaReader)reader).Select(sysCode);
                }
                catch (PcscException)
                {
                    tag.Release();
                    tag = null;
                }
            }
            return tag;
        }

        /// <summary>
        /// ポーリング(Mifare)
        /// </summary>
        /// <returns>カード情報,NULL=未検出</returns>
        public Mifare Polling()
        {
            NfcTag tag = reader.Polling();
            if (tag!=null && !(tag is Mifare))
            {
                tag.Release();
                tag = null;
            }
            return (Mifare)tag;
        }

        /// <summary>
        /// カードのアクセス権を解放する
        /// </summary>
        public void StopAccess()
        {
            reader.StopAccess();
        }

        #region Mifare用
        /// <summary>
        /// Mifare Authentication A/B
        /// </summary>
        /// <param name="useA">'A'/'B'</param>
        /// <param name="block">アクセス対象ブロック番号</param>
        /// <param name="key">認証鍵[6Byte]</param>
        public void Authentication(bool useA, byte block, byte[] key)
        {
            ((IMifareReader)reader).Authentication(useA, block, key);
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
        /// Mifare指定ブロックの読み込み
        /// Classic:16Byte,UL:4Byte
        /// </summary>
        /// <param name="blockOrPage">ブロック番号、ページ番号</param>
        /// <param name="buffer">バッファ</param>
        /// <param name="offset">バッファ内の保存開始位置</param>
        public void ReadBlockData(byte blockOrPage, byte[] buffer, int offset)
        {
            ((IMifareReader)reader).ReadBlockData(blockOrPage, buffer, offset);
        }

        /// <summary>
        /// Mifare指定ブロック(16Byte)の書き込み
        /// </summary>
        /// <param name="block">ブロック番号</param>
        /// <param name="buffer">データバッファ</param>
        /// <param name="offset">バッファ内のデータ開始位置</param>
        public void WriteBlockData(byte block, byte[] buffer, int offset)
        {
            ((IMifareReader)reader).WriteBlockData(block, buffer, offset);
        }

        /// <summary>
        /// Mifare UL 指定ページ(4Byte)の書き込み
        /// </summary>
        /// <param name="page">ページ番号</param>
        /// <param name="buffer">データバッファ</param>
        /// <param name="offset">バッファ内のデータ開始位置</param>
        public void WritePageData(byte page, byte[] buffer, int offset)
        {
            ((IMifareReader)reader).WritePageData(page, buffer, offset);
        }
        #endregion

        #region FeliCa用
        /// <summary>
        /// FeliCa指定ブロックの読み込み
        /// Classic:16Byte,UL:4Byte
        /// </summary>
        public void ReadBlockData(ushort svCode, int[] block, byte[] buffer, int offset)
        {
            ((IFelicaReader)reader).ReadBlockData(svCode, block, buffer, offset);
        }

        /// <summary>
        /// FeliCa指定ブロックの書き込み
        /// </summary>
        /// <param name="svCode">サービスコード</param>
        /// <param name="block">ブロックリスト</param>
        /// <param name="buffer">データバッファ</param>
        /// <param name="offset">バッファ内のデータ開始位置</param>
        public void WriteBlockData(ushort svCode, int[] block, byte[] buffer, int offset)
        {
            ((IFelicaReader)reader).WriteBlockData(svCode, block, buffer, offset);
        }
        #endregion

    }
}
