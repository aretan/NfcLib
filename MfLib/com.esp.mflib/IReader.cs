using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.esp.nfclib.card;

namespace com.esp.nfclib
{
    /// <summary>
    /// リーダライター基底インターフェイス
    /// </summary>
    internal interface IReader
    {
        void InitializeLibrary();

        bool DisposeLibrary();

        NfcTag Polling();

        void StopAccess();
    }

    internal interface IMifareReader
    {
        void Authentication(bool useA, byte block, byte[] key);

        void ReadBlockData(byte blockOrPage, byte[] buffer, int offset);

        void WriteBlockData(byte block, byte[] buffer, int offset);

        void WritePageData(byte page, byte[] buffer, int offset);
    }

    internal interface IFelicaReader
    {
        Felica Select(ushort sysCode);

        void WriteBlockData(ushort svCode, int[] block, byte[] buffer, int offset);

        void ReadBlockData(ushort svCode, int[] block, byte[] buffer, int offset);
    }
}
