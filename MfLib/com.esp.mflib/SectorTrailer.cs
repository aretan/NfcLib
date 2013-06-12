using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.esp.nfclib.trailer
{
    /// <summary>
    /// 権限 (KeyA,KeyBの両方に許可する場合はORして利用可能
    /// </summary>
    [Flags]
    public enum Grant
    {
        /// <summary>全て不可</summary>
        None = 0x00,
        /// <summary>KeyA許可</summary>
        KeyA = 0x01,
        /// <summary>KeyB許可</summary>
        KeyB = 0x02,
    }

    /// <summary>
    /// Access Condition
    /// </summary>
    public class Condition
    {
        /// <summary>read condition</summary>
        public Grant Read { get; set; }

        /// <summary>write condition</summary>
        public Grant Write { get; set; }

        /// <summary>increment condition</summary>
        public Grant Increment { get; set; }

        /// <summary>write condition</summary>
        public Grant Decrement { get; set; }
    }

    /// <summary>
    /// Mifare Classic Sector Trailer
    /// </summary>
    public class BlockCondition
    {
    }

    /// <summary>
    /// SetorTrailer用アクセス設定
    /// </summary>
    public class STCondition : BlockCondition
    {
        /// <summary>condition for KeyA</summary>
        public Condition KeyA { get; set; }

        /// <summary>condition for KeyB</summary>
        public Condition KeyB { get; set; }

        /// <summary>condition for AccessBits</summary>
        public Condition AccessBits { get; set; }

        /// <summary>
        /// Transport conditionで生成
        /// </summary>
        public STCondition()
        {
            KeyA = new Condition();
            KeyA.Read = Grant.None;
            KeyA.Write = Grant.KeyA;

            KeyB = new Condition();
            KeyB.Read = Grant.KeyA;
            KeyB.Write = Grant.KeyA;

            AccessBits = new Condition();
            AccessBits.Read = Grant.KeyA;
            AccessBits.Write = Grant.KeyA;
        }
    }

    /// <summary>
    /// データブロック用アクセス設定
    /// </summary>
    public class DBCondition : BlockCondition
    {
        /// <summary>condition for Value Block</summary>
        public Condition Block { get; set; }

        /// <summary>
        /// Transport conditionで生成
        /// </summary>
        public DBCondition()
        {
            Block = new Condition();
            Block.Read = (Grant.KeyA | Grant.KeyB);
            Block.Write = (Grant.KeyA | Grant.KeyB);
            Block.Increment = (Grant.KeyA | Grant.KeyB);
            Block.Decrement = (Grant.KeyA | Grant.KeyB);
        }
    }

    /// <summary>
    /// SectorTrailer生成補助
    /// </summary>
    public class SectorTrailer
    {
        private DBCondition[] dbAcList;
        private STCondition stAc;

        /// <summary>Mifare KeyA: byte[6]</summary>
        public byte[] KeyA { get; set; }

        /// <summary>Mifare KeyB: byte[6]</summary>
        public byte[] KeyB { get; set; }

        /// <summary>データブロック部(0～2)</summary>
        public DBCondition[] DB
        {
            get { return dbAcList;  }
        }

        /// <summary>SectorTrailer部</summary>
        public STCondition ST
        {
            get { return stAc; }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SectorTrailer()
        {
            dbAcList = new DBCondition[]{
                new DBCondition(),
                new DBCondition(),
                new DBCondition(),
            };

            stAc = new STCondition();

            KeyA = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, };
            KeyB = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, };
        }

        /// <summary>
        /// SectorTrailerに書き込むデータbyte[16]を取得
        /// </summary>
        /// <returns>byte[16]</returns>
        public byte[] GetBlockData()
        {
            byte[] ret = new byte[NfcLib.MF_BLOCK_LENGTH];

            Array.Copy(KeyA, 0, ret, 0, MfConst.KEY_LENGTH);
            Array.Copy(KeyB, 0, ret, 10, MfConst.KEY_LENGTH);
            SetAccessBits(ret);
            return ret;
        }

        /// <summary>
        /// アクセスBitを設定
        /// </summary>
        /// <param name="block"></param>
        private void SetAccessBits(byte[] block)
        {
            byte st = GetSTByte(stAc);
            byte b0 = GetDBByte(dbAcList[0]);
            byte b1 = GetDBByte(dbAcList[1]);
            byte b2 = GetDBByte(dbAcList[2]);

            int offset = 6;
            byte temp = (byte)
                ( (st & 0x02) << 6
                | (b2 & 0x02) << 5
                | (b1 & 0x02) << 4
                | (b0 & 0x02) << 3
                | (st & 0x01) << 3
                | (b2 & 0x01) << 2
                | (b1 & 0x01) << 1
                | (b0 & 0x01));
            temp = (byte)~temp;
            block[offset++] = temp;

            temp = (byte)
                ( (st & 0x01) << 7
                | (b2 & 0x01) << 6
                | (b1 & 0x01) << 5
                | (b0 & 0x01) << 4
                | (st & 0x04) << 1
                | (b2 & 0x04) 
                | (b1 & 0x04) >> 1
                | (b0 & 0x04) >> 2);
            temp = (byte)((temp & 0xf0) | ((~temp) & 0x0f));
            block[offset++] = temp;

            block[offset++] = (byte)
                ( (st & 0x04) << 5
                | (b2 & 0x04) << 4
                | (b1 & 0x04) << 3
                | (b0 & 0x04) << 2
                | (st & 0x02) << 2
                | (b2 & 0x02) << 1
                | (b1 & 0x02)
                | (b0 & 0x02) >> 1);
        }

        /// <summary>
        /// SectorTrailer用のConditionBitsを取得
        /// </summary>
        /// <param name="ac">Access Conditoin</param>
        /// <returns>下位3BitにC1,C2,C3を格納</returns>
        private byte GetSTByte(STCondition ac)
        {
            //C1 C2 C3(Bitの並びが逆になっているので注意)
            //0 0 0
            if (ac.KeyA.Read == Grant.None
                && ac.KeyA.Write == Grant.KeyA
                && ac.AccessBits.Read == Grant.KeyA
                && ac.AccessBits.Write == Grant.None
                && ac.KeyB.Read == Grant.KeyA
                && ac.KeyB.Write == Grant.KeyA)
            {
                return 0x00;
            }
            //0 1 0
            else if (ac.KeyA.Read == Grant.None
                && ac.KeyA.Write == Grant.None
                && ac.AccessBits.Read == Grant.KeyA
                && ac.AccessBits.Write == Grant.None
                && ac.KeyB.Read == Grant.KeyA
                && ac.KeyB.Write == Grant.None)
            {
                return 0x02;
            }
            //1 0 0
            else if (ac.KeyA.Read == Grant.None
                && ac.KeyA.Write == Grant.KeyB
                && ac.AccessBits.Read == (Grant.KeyA|Grant.KeyB)
                && ac.AccessBits.Write == Grant.None
                && ac.KeyB.Read == Grant.None
                && ac.KeyB.Write == Grant.KeyB)
            {
                return 0x01;
            }
            //1 1 0
            else if (ac.KeyA.Read == Grant.None
                && ac.KeyA.Write == Grant.None
                && ac.AccessBits.Read == (Grant.KeyA | Grant.KeyB)
                && ac.AccessBits.Write == Grant.None
                && ac.KeyB.Read == Grant.None
                && ac.KeyB.Write == Grant.None)
            {
                return 0x03;
            }
            //0 0 1(Transport)
            else if (ac.KeyA.Read == Grant.None
                && ac.KeyA.Write == Grant.KeyA
                && ac.AccessBits.Read == Grant.KeyA
                && ac.AccessBits.Write == Grant.KeyA
                && ac.KeyB.Read == Grant.KeyA
                && ac.KeyB.Write == Grant.KeyA)
            {
                return 0x04;
            }
            //0 1 1
            else if (ac.KeyA.Read == Grant.None
                && ac.KeyA.Write == Grant.KeyB
                && ac.AccessBits.Read == (Grant.KeyA | Grant.KeyB)
                && ac.AccessBits.Write == Grant.KeyB
                && ac.KeyB.Read == Grant.None
                && ac.KeyB.Write == Grant.KeyB)
            {
                return 0x06;
            }
            //1 0 1
            else if (ac.KeyA.Read == Grant.None
                && ac.KeyA.Write == Grant.None
                && ac.AccessBits.Read == (Grant.KeyA | Grant.KeyB)
                && ac.AccessBits.Write == Grant.KeyB
                && ac.KeyB.Read == Grant.None
                && ac.KeyB.Write == Grant.None)
            {
                return 0x05;
            }
            //1 1 1
            else if (ac.KeyA.Read == Grant.None
                && ac.KeyA.Write == Grant.None
                && ac.AccessBits.Read == (Grant.KeyA | Grant.KeyB)
                && ac.AccessBits.Write == Grant.None
                && ac.KeyB.Read == Grant.None
                && ac.KeyB.Write == Grant.None)
            {
                return 0x07;
            }
            throw new NfcLibException(ApiErrorKind.APP_INVALID_ACCESS_CONDITION, 0);
        }

        /// <summary>
        /// データブロックのConditionBitを取得
        /// </summary>
        /// <param name="ac">Access Condition</param>
        /// <returns>下位3BitにC1,C2,C3を格納</returns>
        private byte GetDBByte(DBCondition ac)
        {
            //C1 C2 C3(Bitの並びが逆になっているので注意)
            //0 0 0
            if (ac.Block.Read == (Grant.KeyA | Grant.KeyB)
                && ac.Block.Write == (Grant.KeyA | Grant.KeyB)
                && ac.Block.Increment == (Grant.KeyA | Grant.KeyB)
                && ac.Block.Decrement == (Grant.KeyA | Grant.KeyB))
            {
                return 0x00;
            }
            //0 1 0
            else if (ac.Block.Read == (Grant.KeyA | Grant.KeyB)
                && ac.Block.Write == Grant.None
                && ac.Block.Increment == Grant.None
                && ac.Block.Decrement == Grant.None)
            {
                return 0x02;
            }
            //1 0 0
            else if (ac.Block.Read == (Grant.KeyA | Grant.KeyB)
                && ac.Block.Write == Grant.KeyB
                && ac.Block.Increment == Grant.None
                && ac.Block.Decrement == Grant.None)
            {
                return 0x01;

            }
            //1 1 0
            else if (ac.Block.Read == (Grant.KeyA | Grant.KeyB)
                && ac.Block.Write == Grant.KeyB
                && ac.Block.Increment == Grant.KeyB
                && ac.Block.Decrement == (Grant.KeyA | Grant.KeyB))
            {
                return 0x03;
            }
            //0 0 1
            else if (ac.Block.Read == (Grant.KeyA | Grant.KeyB)
                && ac.Block.Write == Grant.None
                && ac.Block.Increment == Grant.None
                && ac.Block.Decrement == (Grant.KeyA | Grant.KeyB))
            {
                return 0x04;
            }
            //0 1 1
            else if (ac.Block.Read == Grant.KeyB
                && ac.Block.Write == Grant.KeyB
                && ac.Block.Increment == Grant.None
                && ac.Block.Decrement == Grant.None)
            {
                return 0x06;
            }
            //1 0 1
            else if (ac.Block.Read == Grant.KeyB
                && ac.Block.Write == Grant.None
                && ac.Block.Increment == Grant.None
                && ac.Block.Decrement == Grant.None)
            {
                return 0x05;
            }
            //1 1 1
            else if (ac.Block.Read == Grant.None
                && ac.Block.Write == Grant.None
                && ac.Block.Increment == Grant.None
                && ac.Block.Decrement == Grant.None)
            {
                return 0x07;
            }
            throw new NfcLibException(ApiErrorKind.APP_INVALID_ACCESS_CONDITION, 0);
        }
    }
}
