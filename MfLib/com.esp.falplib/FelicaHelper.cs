using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;
using System.IO;

using com.esp.common;

namespace com.esp.falplib
{
    /// <summary>
    /// FeliCaAPI呼び出しクラス
    /// </summary>
    internal class FelicaHelper
    {
        public const int BLOCK_LENGTH = 16;
        public const int IDM_LENGTH = 8;

        /// <summary>
        /// Idmの読み出し
        /// </summary>
        /// <param name="sysCode">システムコード</param>
        /// <param name="idm">Idm(idm)</param>
        /// <returns>成功？</returns>
        public static bool ReadIdm(ushort sysCode, byte[] idm)
        {
            HandleContainer hc = new HandleContainer();
            try
            {
                //取得対象
                byte[] sysCodeBuffer = new byte[2];
                BigEndian.SetUIntToBytes(sysCode, sysCodeBuffer, 0);

                StructurePolling polling = new StructurePolling();
                polling.TimeSlot = 0x00;//同時に検出できる枚数＝1
                polling.SystemCode = hc.AddPinnedObject(sysCodeBuffer).AddrOfPinnedObject();

                //出力バッファ
                byte[] idmBuffer = new byte[8];
                byte[] pmmBuffer = new byte[8];
                StructureCardInformation card = new StructureCardInformation();
                card.CardIdm = hc.AddPinnedObject(idmBuffer).AddrOfPinnedObject();
                card.CardPmm = hc.AddPinnedObject(pmmBuffer).AddrOfPinnedObject();

                //メモリ固定
                byte cards = new Byte();
                hc.AddPinnedObject(cards);

                bool res = felica_dll.PollingAndGetCardInformation(ref polling, ref cards, ref card);
                if (res && cards > 0)
                {
                    //取得したIdmを返す
                    idmBuffer.CopyTo(idm, 0);
                    return true;
                }
                return false;
            }
            finally
            {
                //GC除外メモリの解放
                hc.FreeHandle();
            }
        }

        /// <summary>
        /// FalpConnet処理
        /// </summary>
        /// <param name="appId">APP_ID</param>
        /// <param name="buffer">接続後、最初に送るデータ</param>
        /// <returns>T:OK,F:NG</returns>
        public static bool ConnectFalp(byte[] appId, byte[] buffer)
        {
            //APPID
            uint dataLen = buffer!=null ? (uint)buffer.Length: 0;
            bool res = felica_dll.FalpConnect(
                1000,
                10000,
                appId,
                (byte)appId.Length,
                buffer,
                ref dataLen);
            return res;
        }

        /// <summary>
        /// FALPでのデータ送信
        /// </summary>
        /// <param name="buffer">データ</param>
        /// <param name="offset">位置</param>
        /// <param name="length">長さ</param>
        /// <returns>T:OK,F:NG</returns>
        public static bool SendFalp(byte[] buffer, int offset, int length)
        {
            bool finish = false;
            uint sendCount = 0;
            Thread th = new Thread(
                new ThreadStart(delegate (){
                    while (true)
                    {
                        if (sendCount >= length)
                        {
                            finish = true;
                            break;
                        }

                        byte[] buf = new byte[length - sendCount];
                        Array.Copy(buffer, offset+sendCount, buf, 0, buf.Length);

                        //送信
                        uint buffered =(uint) buf.Length;
                        if (!felica_dll.FalpSend(buf, ref buffered))
                        {
                            FalpErrorType error = FalpErrorType.FALP_UNKNOWN_ERROR;
                            felica_dll.FalpGetLastErrorType(out error);
                            Debug.WriteLine("FALP Error:" + GetErrorDescription(error));
                            break;
                        }
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
            return finish;
        }

        public static String GetErrorDescription(Object error)
        {
            String msg = "Unknown error";
            if (error is RwErrorType)
            {
                if (Enum.IsDefined(typeof(RwErrorType), error))
                    return ((RwErrorType)error).ToString();
            }
            else if (error is FelicaErrorType)
            {
                if (Enum.IsDefined(typeof(FelicaErrorType), error))
                    return ((FelicaErrorType)error).ToString();
            }
            else if (error is FalpErrorType)
            {
                if (Enum.IsDefined(typeof(FalpErrorType), error))
                    return ((FalpErrorType)error).ToString();
            }
            return msg;
        }
#if true
        /// <summary>
        /// 鍵無し領域への書込み
        /// </summary>
        /// <param name="svCode">サービスコード</param>
        /// <param name="idm">Idm</param>
        /// <param name="block">ブロックリスト</param>
        /// <param name="buffer">書込みデータ</param>
        /// <param name="offset">データ開始位置</param>
        /// <param name="length">データ長さ</param>
        /// <returns>T:成功 F:失敗</returns>
        public static bool WriteBlockData(int svCode, byte[] idm, int[] block, byte[] buffer, int offset, int length)
        {
            //"write without encryption"s source code is protected by SONY NDA
            //binary libray support this command
            //書き込みコマンドのソースコードは、NDA規約により開示不可となっています
            return false;
        }
#endif

#if true
        /// <summary>
        /// 鍵無し領域からの読み出し
        /// </summary>
        /// <param name="svCode">サービスコード</param>
        /// <param name="idm">Idm</param>
        /// <param name="block">ブロックリスト</param>
        /// <param name="buffer">取得データバッファ</param>
        /// <param name="offset">データ開始位置</param>
        /// <param name="readCount">読み取った長さ</param>
        /// <returns>T:成功 F:失敗</returns>
        public static bool ReadBlockData(int svCode, byte[] idm, int[] block, byte[] buffer, int offset, out int readCount)
        {
            //"read without encryption"s source code is protected by SONY NDA
            //binary libray support this command
            //読み込みコマンドのソースコードは、NDA規約により開示不可となっています
            readCount = 0;
            return false;
        }
#endif
        /// <summary>
        /// ブロックリスト配列の生成
        /// </summary>
        /// <param name="block">ブロック番号リスト</param>
        /// <returns></returns>
        public static byte[] makeBlockList(int[] block)
        {
            //ブロックリスト
            int len = (int)block.Sum((x) => x <= 255 ? 2 : 3);
            int index = 0;
            byte[] buffer = new byte[len];
            for (int i = 0; i < block.Length; i++)
            {
                if (block[i] <= 255)
                {
                    //block<=255? (0x80,block): (0x00,block,block)
                    buffer[index++] = 0x80;
                    buffer[index++] = (byte)block[i];
                }
                else
                {
                    //0x00,block,block[little endian]
                    buffer[index++] = 0x00;
                    buffer[index++] = (byte)(block[i] & 0xff);
                    buffer[index++] = (byte)(block[i] >> 8);
                }
            }
            return buffer;
        }
    }
}
