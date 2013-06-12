using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.esp.nfclib
{
    /// <summary>
    /// Pasoriエラー
    /// </summary>
    public class NfcLibException:Exception
    {
        /// <summary>Pasori APIエラーコード</summary>
        public ApiErrorKind ApiError { get; set; }

        /// <summary>Pasori ドライバエラーコード</summary>
        public UInt32 DriverError { get; set; }

        /// <summary>
        /// Pasoriエラー
        /// </summary>
        /// <param name="apiError">APIエラーコード</param>
        /// <param name="driverError">ドライバエラーコード</param>
        public NfcLibException(ApiErrorKind apiError, UInt32 driverError)
        {
            ApiError = apiError;
            DriverError = driverError;
        }

        /// <summary>
        /// エラー内容
        /// </summary>
        public override string Message
        {
            get
            {
                if (Enum.IsDefined(typeof(ApiErrorKind), ApiError))
                {
                    return String.Format("API:{0}, DIRVER:0x{1:X}", ApiError, DriverError);
                }
                else
                {
                    return String.Format("API:0x{0:X}, DIRVER:0x{1:X}", ApiError, DriverError);
                }
            }
        }
    }

    /// <summary>
    /// Pasoriエラー定義
    /// </summary>
    public enum ApiErrorKind:uint
    {
        /// <summary>未定義</summary>
        APP_UNKNOWN = 0x10000000,
        /// <summary>Initializeに失敗</summary>
        APP_INITIALIZE_FALED = 0x10000001,
        /// <summary>Autenticationに失敗</summary>
        APP_MF_AUTH_FALED = 0x10000002,
        /// <summary>カード未検出</summary>
        APP_MF_NOT_DETECTED = 0x10000003,
        /// <summary>カード未解放</summary>
        APP_MF_NOT_RELEASE = 0x10000004,
        /// <summary>セクタトレーラーが不正</summary>
        APP_INVALID_ACCESS_CONDITION = 0x10000005,
        /// <summary>ドライバでエラーが発生しました※1</summary>
        FELICA_NFC_E_DRIVER_ERROR = 0x80080000,
        /// <summary>プラグインへの引数が異常です</summary>
        FELICA_NFCP_E_INVALID_ARGUMENT = 0x80A00001,
        /// <summary>ドライバの初期化に失敗しました</summary>
        FELICA_NFCP_E_INITIALIZE_DRIVER = 0x80A00002,
        /// <summary>メモリが確保できません</summary>
        FELICA_NFCP_E_MEMORY_ALLOCATE = 0x80A00003,
        /// <summary>ポート名が不正です</summary>
        FELICA_NFCP_E_INVALID_PORT_NAME = 0x80A00004,
        /// <summary>ポートがオープンされていません</summary>
        FELICA_NFCP_E_NOT_PORT_OPENED = 0x80A00005,
        /// <summary>すでにポートがオープンされています</summary>
        FELICA_NFCP_E_OPENED = 0x80A00006,
        /// <summary>ドライバからのレスポンスデータが不正です</summary>
        FELICA_NFCP_E_INVALID_RESPONSE = 0x80A00007,
        /// <summary>引数が異常です</summary>
        FELICA_NFC_E_INVALID_ARGUMENT = 0x80C00001,
        /// <summary>実装されていません</summary>
        FELICA_NFC_E_NOT_IMPLEMENT = 0x80C00002,
        /// <summary>FeliCaライブラリが動作中です</summary>
        FELICA_NFC_E_INVALID_MODE = 0x80C00003,
        /// <summary>メモリが確保できません</summary>
        FELICA_NFC_E_MEMORY_ALLOCATE = 0x80C00004,
        /// <summary>ライブラリが初期化されていません</summary>
        FELICA_NFC_E_NOT_INITIALIZE = 0x80C00005,
        /// <summary>ポートリスト名が不正です</summary>
        FELICA_NFC_E_INVALID_PORT_LIST = 0x80C00006,
        /// <summary>ポート名が不正です</summary>
        FELICA_NFC_E_INVALID_PORT_NAME = 0x80C00007,
        /// <summary>プラグインディレクトリが不正です</summary>
        FELICA_NFC_E_INVALID_PLUGINS_DIR = 0x80C00008,
        /// <summary>プラグインファイル名が不正です</summary>
        FELICA_NFC_E_INVALID_PLUGIN_NAME = 0x80C00009,
        /// <summary>プラグインファイルが不正です</summary>
        FELICA_NFC_E_INVALID_PLUGIN = 0x80C0000a,
        /// <summary>プラグインファイルが存在しません</summary>
        FELICA_NFC_E_NO_PLUGIN = 0x80C0000b,
        /// <summary>スレッドが生成できません</summary>
        FELICA_NFC_E_CREATE_THREAD = 0x80C0000c,
        /// <summary>ポートがオープンされていません</summary>
        FELICA_NFC_E_NOT_PORT_OPENED = 0x80C0000d,
        /// <summary>コールバックパラメータが設定されていません</summary>
        FELICA_NFC_E_NO_CALLBACK_PARAM = 0x80C0000e,
        /// <summary>コールバック用メッセージが生成できません</summary>
        FELICA_NFC_E_NOT_CREATE_MESSAGE = 0x80C0000f,
        /// <summary>NFCアクセスライブラリでタイムアウトが発生しました</summary>
        FELICA_NFC_E_TIME_OUT = 0x80C00010,
        /// <summary>コールバック呼び出しに失敗しました</summary>
        FELICA_NFC_E_CALLBACK = 0x80C00011,
        /// <summary>ライブラリはすでに初期化されています</summary>
        FELICA_NFC_E_ALREADY_INITIALIZED = 0x80C00012,
        /// <summary>すでにポートがオープンされています</summary>
        FELICA_NFC_E_PORT_OPENED = 0x80C00013,
        /// <summary>ドライバステータスが不正です</summary>
        FELICA_NFC_E_DRIVER_STATUS = 0x80C00014,
        /// <summary>捕捉したターゲットデバイスタイプが不明です</summary>
        FELICA_NFC_E_INVALID_TARGET_DEVICE_TYPE = 0x80C00015,
        /// <summary>同期非同期排他エラーが発生しました</summary>
        FELICA_NFC_E_FORBIDDEN_OPERARION = 0x80C00016,
        /// <summary>捕捉デバイスが見つかりませんでした</summary>
        FELICA_NFC_E_TARGET_DEVICE_NOT_FOUND = 0x80C00050,
        /// <summary>捕捉デバイス使用開始要求が時間内にありませんでした</summary>
        FELICA_NFC_E_TARGET_DEVICE_TIMEOUT = 0x80C00051,
        /// <summary>ターゲットデバイスアクセスでのエラーがドライバより通知されました</summary>
        FELICA_NFC_E_TARGET_DEVICE_ACCESS = 0x80C00052,
        /// <summary>ターゲットデバイスアクセスでの不明エラーがドライバより通知されました</summary>
        FELICA_NFC_E_TARGET_DEVICE = 0x80C00053,
        /// <summary>モード移行エラーがドライバ内で発生しています</summary>
        FELICA_NFC_E_RW_DEVICE_MODE = 0x80C00071,
        /// <summary>FeliCaポートデバイスの初期化処理エラーです</summary>
        FELICA_NFC_E_RW_DEVICE_INITIALIZE = 0x80C00072,
        /// <summary>FeliCaポートデバイスが温度異常状態です</summary>
        FELICA_NFC_E_RW_DEVICE_TEMPERATURE_ERROR = 0x80C00073,
        /// <summary>FeliCaポートデバイスが温度異常状態により、使用できない状態です</summary>
        FELICA_NFC_E_RW_DEVICE_TEMPERATURE_FATAL_ERROR = 0x80C00074,
        /// <summary>FeliCaポートデバイスが切断されました</summary>
        FELICA_NFC_E_RW_DEVICE_READER_WRITER_DISCONNECTED = 0x80C00075,
        /// <summary>FeliCaポートデバイスで不明なエラーが発生しています</summary>
        FELICA_NFC_E_RW_DEVICE_ERROR = 0x80C00076,
        /// <summary>レジストリアクセスに失敗しました</summary>
        ELICA_NFC_E_REGISTRY = 0x80C00101,
        /// <summary>フォルダが存在しないか不正です</summary>
        FELICA_NFC_E_INVALID_DIR = 0x80C00102,
        /// <summary>ファイルが存在しないか不正です</summary>
        FELICA_NFC_E_INVALID_FILE = 0x80C00103,
        /// <summary>ファイルアクセスに失敗しました</summary>
        FELICA_NFC_E_FILE_ACCESS = 0x80C00104,
    };
}
