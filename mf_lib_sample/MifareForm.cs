using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using com.esp.nfclib;
using com.esp.nfclib.trailer;
using com.esp.nfclib.card;
using com.esp.common;


namespace nfc_lib_sample
{
    public partial class MifareForm : Form
    {
        NfcLib nfclib;
        NfcTag card;

        public MifareForm()
        {
            InitializeComponent();
            nfclib = new NfcLib();
        }

        private void btAuth_Click(object sender, EventArgs e)
        {
            string keyStr = tbKey.Text;
            byte block = (byte)nudBlock.Value;

            byte[] key = chkAscii.Checked?
                Encoding.ASCII.GetBytes(keyStr):
                Utility.HexToByte(keyStr);

            bool isKeyA = rbKeyA.Checked;


            try
            {
                if (card is MifareCL)
                {
                    MifareCL mfcl = card as MifareCL;
                    mfcl.Authentication(isKeyA, block, key);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                nfclib.DisposeLibrary();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btPoll_Click(object sender, EventArgs e)
        {
            try
            {
                UInt16 sysCode = UInt16.Parse(tbSysCode.Text, NumberStyles.HexNumber);
                card = nfclib.Polling(sysCode);
                if (card != null)
                {
                    tbCard.Text = card.ToString() + ":"
                        + Utility.ByteToHex(card.Uid, 0, card.Uid.Length);
                }
                else
                {
                    tbCard.Text = "未検出";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btRelease_Click(object sender, EventArgs e)
        {
            try
            {
                card.Release();
                card = null;
                tbCard.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btRead_Click(object sender, EventArgs e)
        {
            byte block = (byte)nudBlock.Value;

            try
            {
                byte[] data = null;
                data = new byte[NfcLib.MF_BLOCK_LENGTH];
                if (card is Felica)
                {
                    UInt16 svCode = UInt16.Parse(tbSvCode.Text, NumberStyles.HexNumber);
                    ((Felica)card).Read(svCode, new int[]{block}, data, 0);
                }
                else if (card is Mifare)
                {
                    ((Mifare)card).Read(block, data, 0);
                }
                tbRead.Text = Utility.ByteToHex(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btWrite_Click(object sender, EventArgs e)
        {
            try
            {
                byte block = (byte)nudBlock.Value;
                byte[] data = Utility.HexToByte(tbWrite.Text);
                if (card is Felica)
                {
                    UInt16 svCode = UInt16.Parse(tbSvCode.Text, NumberStyles.HexNumber);
                    ((Felica)card).Write(svCode, new int[] { block }, data, 0);
                }
                else if (card is Mifare)
                {
                    ((Mifare)card).Write(block, data, 0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btFormat_Click(object sender, EventArgs e)
        {
            try
            {
                SectorTrailer st = new SectorTrailer();

                //KeyA
                st.ST.KeyA.Read = Grant.None;
                if (chkKeyaRA.Checked)
                    st.ST.KeyA.Read |= Grant.KeyA;
                if (chkKeyaRB.Checked)
                    st.ST.KeyA.Read |= Grant.KeyB;

                st.ST.KeyA.Write = Grant.None;
                if (chkKeyaWA.Checked)
                    st.ST.KeyA.Write |= Grant.KeyA;
                if (chkKeyaWB.Checked)
                    st.ST.KeyA.Write |= Grant.KeyB;

                //KeyB
                st.ST.KeyB.Read = Grant.None;
                if (chkKeybRA.Checked)
                    st.ST.KeyB.Read |= Grant.KeyA;
                if (chkKeybRB.Checked)
                    st.ST.KeyB.Read |= Grant.KeyB;

                st.ST.KeyB.Write = Grant.None;
                if (chkKeybWA.Checked)
                    st.ST.KeyB.Write |= Grant.KeyA;
                if (chkKeybWB.Checked)
                    st.ST.KeyB.Write |= Grant.KeyB;

                //Accessbis
                st.ST.AccessBits.Read = Grant.None;
                if (chkAbRA.Checked)
                    st.ST.AccessBits.Read |= Grant.KeyA;
                if (chkAbRB.Checked)
                    st.ST.AccessBits.Read |= Grant.KeyB;

                st.ST.AccessBits.Write = Grant.None;
                if (chkAbWA.Checked)
                    st.ST.AccessBits.Write |= Grant.KeyA;
                if (chkAbWB.Checked)
                    st.ST.AccessBits.Write |= Grant.KeyB;

                //D0
                st.DB[0].Block.Increment= Grant.None;
                if (chkD0IA.Checked)
                    st.DB[0].Block.Increment |= Grant.KeyA;
                if (chkD0IB.Checked)
                    st.DB[0].Block.Increment |= Grant.KeyB;

                st.DB[0].Block.Decrement = Grant.None;
                if (chkD0DA.Checked)
                    st.DB[0].Block.Decrement |= Grant.KeyA;
                if (chkD0DB.Checked)
                    st.DB[0].Block.Decrement |= Grant.KeyB;

                st.DB[0].Block.Read = Grant.None;
                if (chkD0RA.Checked)
                    st.DB[0].Block.Read |= Grant.KeyA;
                if (chkD0RB.Checked)
                    st.DB[0].Block.Read |= Grant.KeyB;

                st.DB[0].Block.Write = Grant.None;
                if (chkD0WA.Checked)
                    st.DB[0].Block.Write |= Grant.KeyA;
                if (chkD0WB.Checked)
                    st.DB[0].Block.Write |= Grant.KeyB;

                //D1
                st.DB[1].Block.Increment = Grant.None;
                if (chkD1IA.Checked)
                    st.DB[1].Block.Increment |= Grant.KeyA;
                if (chkD1IB.Checked)
                    st.DB[1].Block.Increment |= Grant.KeyB;

                st.DB[1].Block.Decrement = Grant.None;
                if (chkD1DA.Checked)
                    st.DB[1].Block.Decrement |= Grant.KeyA;
                if (chkD1DB.Checked)
                    st.DB[1].Block.Decrement |= Grant.KeyB;

                st.DB[1].Block.Read = Grant.None;
                if (chkD1RA.Checked)
                    st.DB[1].Block.Read |= Grant.KeyA;
                if (chkD1RB.Checked)
                    st.DB[1].Block.Read |= Grant.KeyB;

                st.DB[1].Block.Write = Grant.None;
                if (chkD1WA.Checked)
                    st.DB[1].Block.Write |= Grant.KeyA;
                if (chkD1WB.Checked)
                    st.DB[1].Block.Write |= Grant.KeyB;

                //D2
                st.DB[2].Block.Increment = Grant.None;
                if (chkD2IA.Checked)
                    st.DB[2].Block.Increment |= Grant.KeyA;
                if (chkD2IB.Checked)
                    st.DB[2].Block.Increment |= Grant.KeyB;

                st.DB[2].Block.Decrement = Grant.None;
                if (chkD2DA.Checked)
                    st.DB[2].Block.Decrement |= Grant.KeyA;
                if (chkD2DB.Checked)
                    st.DB[2].Block.Decrement |= Grant.KeyB;

                st.DB[2].Block.Read = Grant.None;
                if (chkD2RA.Checked)
                    st.DB[2].Block.Read |= Grant.KeyA;
                if (chkD2RB.Checked)
                    st.DB[2].Block.Read |= Grant.KeyB;

                st.DB[2].Block.Write = Grant.None;
                if (chkD2WA.Checked)
                    st.DB[2].Block.Write |= Grant.KeyA;
                if (chkD2WB.Checked)
                    st.DB[2].Block.Write |= Grant.KeyB;

                tbSt.Text = Utility.ByteToHex(st.GetBlockData(), 0, 16);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btSet_Click(object sender, EventArgs e)
        {
            tbWrite.Text = tbSt.Text;
        }

        private void btInit_Click(object sender, EventArgs e)
        {
            try
            {
                UseCard useCard = UseCard.None;
                if (chkFelica.Checked)
                    useCard |= UseCard.Felica;
                if (chkMifare.Checked)
                    useCard |= UseCard.Mifare;

                nfclib.DisposeLibrary();
                nfclib.InitializeLibrary(useCard);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
