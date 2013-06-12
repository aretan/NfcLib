using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using com.esp.falplib;
using com.esp.nfclib.card;

namespace com.esp.nfclib
{
    /// <summary>
    /// Felicaライブラリを利用して読み書きする
    /// </summary>
    internal class FelicaReader:IReader,IFelicaReader,IDisposable
    {
        private NfcLib lib;
        private byte[] idm;
        private bool initialized;
        private bool opend;

        public FelicaReader(NfcLib lib)
        {
            this.lib = lib;
        }

        ~FelicaReader()
        {
            DisposeLibrary();
        }

        public void Dispose()
        {
            DisposeLibrary();
        }

        public void InitializeLibrary()
        {
            FelicaNetHelper.InitializeLibrary();
            initialized = true;
        }

        public bool DisposeLibrary()
        {
            if (initialized)
            {
                bool res = FelicaNetHelper.DisposeLibrary();
                initialized = false;
                return res;
            }
            return true;
        }

        public NfcTag Polling()
        {
            FelicaNetHelper.OpenReaderWriterAuto();
            opend = true;
            return Select(0xffff);
        }

        public void StopAccess()
        {
            if (opend)
            {
                FelicaNetHelper.CloseReaderWriter();
                opend = false;
            }
        }

        public Felica Select(ushort sysCode)
        {
            idm = new byte[FelicaNetHelper.IDM_LENGTH];
            if (FelicaNetHelper.Polling(sysCode, idm))
            {
                Felica card = new Felica(lib, idm);
                return card;
            }
            else
            {
                StopAccess();
                return null;
            }
        }

        public void WriteBlockData(ushort svCode, int[] block, byte[] buffer, int offset)
        {
            FelicaNetHelper.WriteBlockData(svCode, idm, block, buffer, offset, block.Length * FelicaNetHelper.BLOCK_LENGTH);
        }

        public void ReadBlockData(ushort svCode, int[] block, byte[] buffer, int offset)
        {
            FelicaNetHelper.ReadBlockData(svCode, idm, block, buffer, offset);
        }
    }
}
