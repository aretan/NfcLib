using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.esp.falplib
{
    /// <summary>
    /// FALP 命令の実行中に発生した例外
    /// FeliCa,Falp例外の種別は仕様書を参考
    /// </summary>
    public class FelicaException:Exception
    {
        /// <summary>FeliCaエラー</summary>
        public FelicaErrorType FelicaError { get; private set; }

        /// <summary>FALPエラー</summary>
        public FalpErrorType FalpError { get; private set; }

        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="error">FelicaErrorType or FalpErrorType</param>
        public FelicaException(Object error)
            :base(FelicaHelper.GetErrorDescription(error))
        {
            if (error is FelicaErrorType)
            {
                FelicaError = (FelicaErrorType)error;
            }
            else if (error is FalpErrorType)
            {
                FalpError = (FalpErrorType)error;
            }
        }
    }
}
