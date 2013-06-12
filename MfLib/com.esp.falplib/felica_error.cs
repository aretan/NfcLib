namespace com.esp.falplib
{
    /// <summary>
    /// フェリカドライバの例外
    /// </summary>
    public enum FelicaErrorType : uint
    {
        ///<summary>FELICA_ERROR_NOT_OCCURRED</summary>
        FELICA_ERROR_NOT_OCCURRED = 1000,
        ///<summary>FELICA_UNKNOWN_ERROR</summary>
        FELICA_UNKNOWN_ERROR = 1001,
        ///<summary>FELICA_ILLEGAL_ARGUMENT</summary>
        FELICA_ILLEGAL_ARGUMENT = 1002,
        ///<summary>FELICA_MEMORY_ALLOCATION_ERROR</summary>
        FELICA_MEMORY_ALLOCATION_ERROR = 1003,
        ///<summary>FELICA_THREAD_CREATION_ERROR</summary>
        FELICA_THREAD_CREATION_ERROR = 1004,
        ///<summary>FELICA_LIBRARY_NOT_INITIALIZED</summary>
        FELICA_LIBRARY_NOT_INITIALIZED = 1005,
        ///<summary>FELICA_LIBRARY_ALREADY_INITIALIZED</summary>
        FELICA_LIBRARY_ALREADY_INITIALIZED = 1006,
        ///<summary>FELICA_INVALID_FILE_NAME</summary>
        FELICA_INVALID_FILE_NAME = 1007,
        ///<summary>FELICA_FILE_NOT_FOUND</summary>
        FELICA_FILE_NOT_FOUND = 1008,
        ///<summary>FELICA_FILE_OPEN_ERROR</summary>
        FELICA_FILE_OPEN_ERROR = 1009,
        ///<summary>FELICA_FILE_NOT_OPENED</summary>
        FELICA_FILE_NOT_OPENED = 1010,
        ///<summary>FELICA_FILE_ALREADY_OPENED</summary>
        FELICA_FILE_ALREADY_OPENED = 1011,
        ///<summary>FELICA_INVALID_DIRECTORY_NAME</summary>
        FELICA_INVALID_DIRECTORY_NAME = 1012,
        ///<summary>FELICA_DIRECTORY_NOT_FOUND</summary>
        FELICA_DIRECTORY_NOT_FOUND = 1013,
        ///<summary>FELICA_DIRECTORY_OPEN_ERROR</summary>
        FELICA_DIRECTORY_OPEN_ERROR = 1014,
        ///<summary>FELICA_DIRECTORY_NOT_OPENED</summary>
        FELICA_DIRECTORY_NOT_OPENED = 1015,
        ///<summary>FELICA_DIRECTORY_ALREADY_OPENED</summary>
        FELICA_DIRECTORY_ALREADY_OPENED = 1016,
        ///<summary>FELICA_INVALID_COMMUNICATIONS_PORT_NAME</summary>
        FELICA_INVALID_COMMUNICATIONS_PORT_NAME = 1017,
        ///<summary>FELICA_COMMUNICATIONS_PORT_NOT_FOUND</summary>
        FELICA_COMMUNICATIONS_PORT_NOT_FOUND = 1018,
        ///<summary>FELICA_COMMUNICATIONS_PORT_OPEN_ERROR</summary>
        FELICA_COMMUNICATIONS_PORT_OPEN_ERROR = 1019,
        ///<summary>FELICA_COMMUNICATIONS_PORT_NOT_OPENED</summary>
        FELICA_COMMUNICATIONS_PORT_NOT_OPENED = 1020,
        ///<summary>FELICA_COMMUNICATIONS_PORT_ALREADY_OPENED</summary>
        FELICA_COMMUNICATIONS_PORT_ALREADY_OPENED = 1021,
        ///<summary>FELICA_INVALID_TIME_OUT</summary>
        FELICA_INVALID_TIME_OUT = 1022,
        ///<summary>FELICA_INVALID_BAUD_RATE</summary>
        FELICA_INVALID_BAUD_RATE = 1023,
        ///<summary>FELICA_INVALID_RETRY_COUNT</summary>
        FELICA_INVALID_RETRY_COUNT = 1024,
        ///<summary>FELICA_READER_WRITER_CONTROL_LIBRARY_NOT_FOUND</summary>
        FELICA_READER_WRITER_CONTROL_LIBRARY_NOT_FOUND = 1025,
        ///<summary>FELICA_READER_WRITER_CONTROL_LIBRARY_LOAD_ERROR</summary>
        FELICA_READER_WRITER_CONTROL_LIBRARY_LOAD_ERROR = 1026,
        ///<summary>FELICA_READER_WRITER_OPEN_ERROR</summary>
        FELICA_READER_WRITER_OPEN_ERROR = 1027,
        ///<summary>FELICA_READER_WRITER_OPEN_AUTO_ERROR</summary>
        FELICA_READER_WRITER_OPEN_AUTO_ERROR = 1028,
        ///<summary>FELICA_READER_WRITER_NOT_OPENED</summary>
        FELICA_READER_WRITER_NOT_OPENED = 1029,
        ///<summary>FELICA_READER_WRITER_ALREADY_OPENED</summary>
        FELICA_READER_WRITER_ALREADY_OPENED = 1030,
        ///<summary>FELICA_READER_WRITER_RECONNECT_ERROR</summary>
        FELICA_READER_WRITER_RECONNECT_ERROR = 1031,
        ///<summary>FELICA_MESSAGE_OF_CARD_FIND_REGISTRATION_ERROR</summary>
        FELICA_MESSAGE_OF_CARD_FIND_REGISTRATION_ERROR = 1032,
        ///<summary>FELICA_MESSAGE_OF_CARD_LOSS_REGISTRATION_ERROR</summary>
        FELICA_MESSAGE_OF_CARD_LOSS_REGISTRATION_ERROR = 1033,
        ///<summary>FELICA_CALL_BACK_PARAMETERS_NOT_SETTED</summary>
        FELICA_CALL_BACK_PARAMETERS_NOT_SETTED = 1034,
        ///<summary>FELICA_SUCCESSIVE_POLLING_FOR_CALL_BACK_NOT_STARTED</summary>
        FELICA_SUCCESSIVE_POLLING_FOR_CALL_BACK_NOT_STARTED = 1035,
        ///<summary>FELICA_POLLING_ERROR</summary>
        FELICA_POLLING_ERROR = 1036,
        ///<summary>FELICA_REQUEST_RESPONSE_ERROR</summary>
        FELICA_REQUEST_RESPONSE_ERROR = 1037,
        ///<summary>FELICA_REQUEST_SERVICE_ERROR</summary>
        FELICA_REQUEST_SERVICE_ERROR = 1038,
        ///<summary>FELICA_MUTUAL_AUTHENTICATION_ERROR</summary>
        FELICA_MUTUAL_AUTHENTICATION_ERROR = 1039,
        ///<summary>FELICA_READ_BLOCK_ERROR</summary>
        FELICA_READ_BLOCK_ERROR = 1040,
        ///<summary>FELICA_WRITE_BLOCK_ERROR</summary>
        FELICA_WRITE_BLOCK_ERROR = 1041,
        ///<summary>FELICA_READ_BLOCK_WITHOUT_ENCRYPTION_ERROR</summary>
        FELICA_READ_BLOCK_WITHOUT_ENCRYPTION_ERROR = 1042,
        ///<summary>FELICA_WRITE_BLOCK_WIHTOUT_ENCRYPTION_ERROR</summary>
        FELICA_WRITE_BLOCK_WIHTOUT_ENCRYPTION_ERROR = 1043,
        ///<summary>FELICA_DUMB_ERROR</summary>
        FELICA_DUMB_ERROR = 1044,
        ///<summary>FELICA_MAKE_ACCESS_KEYS_ERROR</summary>
        FELICA_MAKE_ACCESS_KEYS_ERROR = 1045,
        ///<summary>FELICA_CARD_INFORMATION_NOT_FOUND</summary>
        FELICA_CARD_INFORMATION_NOT_FOUND = 1046,
        ///<summary>FELICA_CARD_INFORMATION_ACCESS_ERROR</summary>
        FELICA_CARD_INFORMATION_ACCESS_ERROR = 1047,
        ///<summary>FELICA_INVALID_CARD_INDEX</summary>
        FELICA_INVALID_CARD_INDEX = 1048,
        ///<summary>FELICA_CARD_IDM_UNMATCHED</summary>
        FELICA_CARD_IDM_UNMATCHED = 1049,
        ///<summary>FELICA_MUTUAL_AUTHENTICATION_NOT_DONE</summary>
        FELICA_MUTUAL_AUTHENTICATION_NOT_DONE = 1050,
        ///<summary>FELICA_ILLEGAL_SERVICE_SETTED</summary>
        FELICA_ILLEGAL_SERVICE_SETTED = 1051,
        ///<summary>FELICA_BLOCK_LIST_GENERATION_ERROR</summary>
        FELICA_BLOCK_LIST_GENERATION_ERROR = 1052,
        ///<summary>FELICA_ACCESS_LIBRARY_FUNCTION_ERROR</summary>
        FELICA_ACCESS_LIBRARY_FUNCTION_ERROR = 1053,
        ///<summary>FELICA_EXCHANGE_KEY_ERROR</summary>
        FELICA_EXCHANGE_KEY_ERROR = 1054,
        ///<summary>FELICA_SUCCESSIVE_POLLING_FOR_CALL_BACK_ALREADY_STARTED</summary>
        FELICA_SUCCESSIVE_POLLING_FOR_CALL_BACK_ALREADY_STARTED = 1055,
        ///<summary>FELICA_MESSAGE_OF_READER_WRITER_CONNECT_REGISTRATION_ERROR</summary>
        FELICA_MESSAGE_OF_READER_WRITER_CONNECT_REGISTRATION_ERROR = 1056,
        ///<summary>FELICA_MESSAGE_OF_READER_WRITER_PULLOUT_REGISTRATION_ERROR</summary>
        FELICA_MESSAGE_OF_READER_WRITER_PULLOUT_REGISTRATION_ERROR = 1057,
        ///<summary>FELICA_PLUG_AND_PLAY_WATCH_FOR_CALL_BACK_NOT_STARTED</summary>
        FELICA_PLUG_AND_PLAY_WATCH_FOR_CALL_BACK_NOT_STARTED = 1058,
        ///<summary>FELICA_PLUG_AND_PLAY_WATCH_FOR_CALL_BACK_ALREADY_STARTED</summary>
        FELICA_PLUG_AND_PLAY_WATCH_FOR_CALL_BACK_ALREADY_STARTED = 1059,
        ///<summary>FELICA_API_NOT_IMPLEMENTED_FOR_THIS_PLATHOME</summary>
        FELICA_API_NOT_IMPLEMENTED_FOR_THIS_PLATHOME = 1060,
        ///<summary>FELICA_REQUEST_SYSTEM_CODE_ERROR</summary>
        FELICA_REQUEST_SYSTEM_CODE_ERROR = 1061,
        ///<summary>FELICA_SEARCH_SERVICE_CODE_ERROR</summary>
        FELICA_SEARCH_SERVICE_CODE_ERROR = 1062,
        ///<summary>FELICA_APPEND_KEY_LIST_TO_ACCESS_KEYS_ERROR</summary>
        FELICA_APPEND_KEY_LIST_TO_ACCESS_KEYS_ERROR = 1063,
        ///<summary>FELICA_SET_SHARED_OPEN_MODE_ERROR</summary>
        FELICA_SET_SHARED_OPEN_MODE_ERROR = 1064,
        ///<summary>FELICA_TRANSACTION_LOCK_ERROR</summary>
        FELICA_TRANSACTION_LOCK_ERROR = 1065,
        ///<summary>FELICA_TRANSACTION_UNLOCK_ERROR</summary>
        FELICA_TRANSACTION_UNLOCK_ERROR = 1066,
        ///<summary>FELICA_SET_LOCK_TIMEOUT_ERROR</summary>
        FELICA_SET_LOCK_TIMEOUT_ERROR = 1067,
        ///<summary>FELICA_GET_LOCK_TIMEOUT_ERROR</summary>
        FELICA_GET_LOCK_TIMEOUT_ERROR = 1068,
        ///<summary>FELICA_GET_DEVICE_INFO_FAILED</summary>
        FELICA_GET_DEVICE_INFO_FAILED = 1069,
    }

    /// <summary>
    /// FALP命令の例外
    /// </summary>
    public enum FalpErrorType
    {
        ///<summary> エラーは発生していません (0x7d0)</summary>
        FALP_ERROR_NOT_OCCURRED = 2000,
        ///<summary> タイムアウトしました (0x7d1)</summary>
        FALP_TIMEOUT = 2001,
        ///<summary> シャットダウンされています (0x7d2)</summary>
        FALP_SHUTDOWN = 2002,
        ///<summary> ハンドシェイクに失敗しました (0x7d3)</summary>
        FALP_HANDSHAKE_FAIL = 2003,
        ///<summary> パラメータが不正です (0x7d4)</summary>
        FALP_PARAM_ERROR = 2004,
        ///<summary> メモリの確保ができません (0x7d5)</summary>
        FALP_MEMORY_ALLOCATION_ERROR = 2005,
        ///<summary> 範囲外の値が指定されました (0x7d6)</summary>
        FALP_OUT_OF_RANGE = 2006,
        ///<summary> 状態が不正です (0x7d7)</summary>
        FALP_ILLEGAL_STATE_ERROR = 2007,
        ///<summary> FeliCaコマンドがタイムアウトしました (0x7d8)</summary>
        FALP_FELICA_COMMAND_TIMEOUT = 2008,
        ///<summary> パケットフォーマットが不正です (0x7d9)</summary>
        FALP_PACKET_FORMAT_ERROR = 2009,
        ///<summary> ポートビジーが発生しました (0x7da)</summary>
        FALP_PORT_BUSY = 2010,
        ///<summary> FeliCaリーダ／ライタコントロールライブラリからエラーが返されました (0x7db)</summary>
        FALP_RW_LIBRARY_ERROR = 2011,
        ///<summary> 受信したデータの長さが異常です (0x7dc)</summary>
        FALP_RW_RECEIVE_DATA_LENGTH_ERROR = 2012,
        ///<summary> 受信したコードが異常です (0x7dd)</summary>
        FALP_RW_RECEIVE_DATA_CODE_ERROR = 2013,
        ///<summary> リーダ／ライタから無線通信速度が正しく取得できませんでした (0x7de)</summary>
        FALP_RW_GET_ATTRIBUE_RATE_ERROR = 2014,
        ///<summary> FeliCaライブラリが初期化されていません (0x7df)</summary>
        FALP_FELICA_LIBRARY_NOT_INITIALIZED = 2015,
        ///<summary> FALPライブラリの初期化に失敗しました (0x7e0)</summary>
        FALP_LIBRARY_INITIALIZE_ERROR = 2016,
        ///<summary> リーダ／ライタがオープンされていません (0x7e1)</summary>
        FALP_READER_WRITER_NOT_OPENED = 2017,
        ///<summary> START POLLING が稼働しているためFALPを開始できません (0x7e2)</summary>
        FALP_START_POLLING_STARTED = 2018,
        ///<summary> FALPモードに既に移行しています (0x7e3)</summary>
        FALP_ALREADY_OPENED = 2019,
        ///<summary> FALPモードではありません (0x7e4)</summary>
        FALP_NOT_OPEND = 2020,
        ///<summary> USB通信エラーが発生しているか、デバイスが存在しません(0x7e5)</summary>
        FALP_DEVICE_COMMUNICATION_ERROR = 2021,
        ///<summary> デバイスが他で使用されているか、アクセス権が獲得できません(0x7e6)</summary>
        FALP_DEVICE_BUSY = 2022,
        ///<summary> デバイスアクセス時のパラメータが不正です(0x7e7)</summary>
        FALP_DEVICE_COMMUNICATION_PARAMETER_ERROR = 2023,
        ///<summary> FeliCaコマンドレスポンスでエラーが通知されました(0x7e8)</summary>
        FALP_FELICA_COMMAND_ERROR = 2024,
        ///<summary> Communicate Thruでエラーが通知されました(0x7e9)</summary>
        FALP_FELICA_THRU_ERROR = 2025,
        ///<summary> シンタックスエラーが発生しました(0x7ea)</summary>
        FALP_SYNTAX_ERROR = 2026,
        ///<summary> コマンドチェックサム(LCS,DCS)エラーが発生しました(0x7eb)</summary>
        FALP_COMMAND_CHECKSUM_ERROR = 2027,
        ///<summary> 不正なトランザクションIDが検出されました(0x7ec)</summary>
        FALP_COMMAND_IDTR_ERROR = 2028,
        ///<summary> リーダ／ライタのモード状態によりコマンドが実行されませんでした(0x7ed)</summary>
        FALP_READER_WRITER_MODE_ERROR = 2029,
        ///<summary> コマンドのヘッダ・フッタ部が不正です(0x7ee)</summary>
        FALP_ILLEGAL_COMMAND_PACKET = 2030,
        ///<summary> ステータスコードが正常・タイムアウト以外です(0x7ef)</summary>
        FALP_STATUS_FLAG_ERROR = 2031,
        ///<summary> FALPログファイルがオープンできません(0x7f0) </summary>
        FALP_LOG_FILE_OPEN_ERROR = 2032,
        ///<summary> FALPログの取得開始に失敗しました(0x7f1)</summary>
        FALP_LOG_START_ERROR = 2033,
        ///<summary> 原因不明のエラーが発生しました(0x7f2)</summary>
        FALP_UNKNOWN_ERROR = 2034,
        ///<summary> ドライバ内部エラーが発生しました(0x7f3)</summary>
        FALP_USB_DRIVER_INTERNAL_ERROR = 2035,
        ///<summary> ターゲットを捕捉することができませんでした(0x7f4)</summary>
        FALP_TARGET_NOT_FOUND = 2036,
        ///<summary> FALP IDLEモードです(0x7f5)</summary>
        FALP_INITIATOR_IDLE_MODE = 2037,
        ///<summary> FALP イニシエーター転送モードです(0x7f6)</summary>
        FALP_INITIATOR_TRANSMIT_MODE = 2038,
        ///<summary> FALP イニシエーター転送終了モードです(0x7f7)</summary>
        FALP_INITIATOR_TRANSMIT_END_MODE = 2039,
        ///<summary> FALP ターゲット転送モードです(0x7f8)</summary>
        FALP_TARGET_TRANSMIT_MODE = 2040,
        ///<summary> FALP ターゲット転送終了モードです(0x7f9)</summary>
        FALP_TARGET_TRANSMIT_END_MODE = 2041,
        ///<summary> FALP ターゲット用コールバックパラメータが設定されていません(0x7fa)</summary>
        FALP_CALL_BACK_PARAMETERS_NOT_SET = 2042,
        ///<summary> すでにFALP ターゲット接続待ち要求が発行されています(0x7fb)</summary>
        FALP_TARGET_WAIT_MODE = 2043,
        ///<summary> FALP ターゲット接続待ち要求を発行していません(0x7fc)</summary>
        FALP_NOT_TARGET_WAIT_MODE = 2044,
        ///<summary> FALP ターゲット接続通知メッセージの登録に失敗しました(0x7fd)</summary>
        FALP_MESSAGE_REGISTRATION_ERROR = 2045,
        ///<summary> FALP ターゲットモードではありません(0x7fe)</summary>
        FALP_NOT_TARGET_MODE = 2046,
        ///<summary> FALP ターゲット接続待ちを開始できません(07ff)</summary>
        FALP_CANNOT_START_TARGET_WAIT = 2047,
        ///<summary> Propose Ad-hocの応答待ちでタイムアウトしました(0x800)</summary>
        FALP_PROPOSE_TIMEOUT = 2048,
        ///<summary> モード移行のためのデバイス設定処理でエラーが発生しました(0x801)</summary>
        FALP_MODE_TRANSMIT_ERROR = 2049,
        ///<summary> デバイスの初期化でエラーが発生しました(0x802)</summary>
        FALP_DEVICE_INITIALIZE_ERROR = 2050,
        ///<summary> デバイスが温度異常です(0x803)</summary>
        FALP_TEMPERATURE_ERROR = 2051,
        ///<summary> 温度異常によりデバイスが使用できない状態です(0x804)</summary>
        FALP_TEMPERATURE_FATAL_ERROR = 2052,
        ///<summary> システムビジーが発生しました(0x805)</summary>
        FALP_SYSTEM_BUSY = 2053,
    }

    /// <summary>
    /// RWの例外
    /// </summary>
    public enum RwErrorType : uint
    {
        ///<summary>RW_ERROR_NOT_OCCURRED</summary>
        RW_ERROR_NOT_OCCURRED = 100,
        ///<summary>RW_UNKNOWN_ERROR</summary>
        RW_UNKNOWN_ERROR = 101,
        ///<summary>RW_ILLEGAL_ARGUMENT</summary>
        RW_ILLEGAL_ARGUMENT = 102,
        ///<summary>RW_MEMORY_ALLOCATION_ERROR</summary>
        RW_MEMORY_ALLOCATION_ERROR = 103,
        ///<summary>RW_THREAD_CREATION_ERROR</summary>
        RW_THREAD_CREATION_ERROR = 104,
        ///<summary>RW_LIBRARY_NOT_INITIALIZED</summary>
        RW_LIBRARY_NOT_INITIALIZED = 105,
        ///<summary>RW_LIBRARY_ALREADY_INITIALIZED</summary>
        RW_LIBRARY_ALREADY_INITIALIZED = 106,
        ///<summary>RW_INVALID_FILE_NAME</summary>
        RW_INVALID_FILE_NAME = 107,
        ///<summary>RW_FILE_NOT_FOUND</summary>
        RW_FILE_NOT_FOUND = 108,
        ///<summary>RW_FILE_OPEN_ERROR</summary>
        RW_FILE_OPEN_ERROR = 109,
        ///<summary>RW_FILE_NOT_OPENED</summary>
        RW_FILE_NOT_OPENED = 110,
        ///<summary>RW_FILE_ALREADY_OPENED</summary>
        RW_FILE_ALREADY_OPENED = 111,
        ///<summary>RW_INVALID_DIRECTORY_NAME</summary>
        RW_INVALID_DIRECTORY_NAME = 112,
        ///<summary>RW_DIRECTORY_NOT_FOUND</summary>
        RW_DIRECTORY_NOT_FOUND = 113,
        ///<summary>RW_DIRECTORY_OPEN_ERROR</summary>
        RW_DIRECTORY_OPEN_ERROR = 114,
        ///<summary>RW_DIRECTORY_NOT_OPENED</summary>
        RW_DIRECTORY_NOT_OPENED = 115,
        ///<summary>RW_DIRECTORY_ALREADY_OPENED</summary>
        RW_DIRECTORY_ALREADY_OPENED = 116,
        ///<summary>RW_INVALID_COMMUNICATIONS_PORT_NAME</summary>
        RW_INVALID_COMMUNICATIONS_PORT_NAME = 117,
        ///<summary>RW_COMMUNICATIONS_PORT_NOT_FOUND</summary>
        RW_COMMUNICATIONS_PORT_NOT_FOUND = 118,
        ///<summary>RW_COMMUNICATIONS_PORT_OPEN_ERROR</summary>
        RW_COMMUNICATIONS_PORT_OPEN_ERROR = 119,
        ///<summary>RW_COMMUNICATIONS_PORT_NOT_OPENED</summary>
        RW_COMMUNICATIONS_PORT_NOT_OPENED = 120,
        ///<summary>RW_COMMUNICATIONS_PORT_ALREADY_OPENED</summary>
        RW_COMMUNICATIONS_PORT_ALREADY_OPENED = 121,
        ///<summary>RW_INVALID_TIME_OUT</summary>
        RW_INVALID_TIME_OUT = 122,
        ///<summary>RW_INVALID_BAUD_RATE</summary>
        RW_INVALID_BAUD_RATE = 123,
        ///<summary>RW_PLUGINS_HOME_DIRECTORY_NOT_SETTED</summary>
        RW_PLUGINS_HOME_DIRECTORY_NOT_SETTED = 124,
        ///<summary>RW_PLUGINS_HOME_DIRECTORY_NOT_FOUND</summary>
        RW_PLUGINS_HOME_DIRECTORY_NOT_FOUND = 125,
        ///<summary>RW_PLUGINS_INITIALIZE_ERROR</summary>
        RW_PLUGINS_INITIALIZE_ERROR = 126,
        ///<summary>RW_PLUGIN_NOT_FOUND</summary>
        RW_PLUGIN_NOT_FOUND = 127,
        ///<summary>RW_COMMAND_PLUGIN_NOT_FOUND</summary>
        RW_COMMAND_PLUGIN_NOT_FOUND = 128,
        ///<summary>RW_DEVICE_PLUGIN_NOT_FOUND</summary>
        RW_DEVICE_PLUGIN_NOT_FOUND = 129,
        ///<summary>RW_SECURITY_PLUGIN_NOT_FOUND</summary>
        RW_SECURITY_PLUGIN_NOT_FOUND = 130,
        ///<summary>RW_TABLE_ACCESS_ERROR</summary>
        RW_TABLE_ACCESS_ERROR = 131,
        ///<summary>RW_COMMAND_NOT_FOUND</summary>
        RW_COMMAND_NOT_FOUND = 132,
        ///<summary>RW_SCRIPT_EXECUTION_ERROR</summary>
        RW_SCRIPT_EXECUTION_ERROR = 133,
        ///<summary>RW_COMMUNICATIONS_PORT_INITIALIZE_ERROR</summary>
        RW_COMMUNICATIONS_PORT_INITIALIZE_ERROR = 134,
        ///<summary>RW_COMMUNICATIONAS_PORT_SET_TIME_OUT_ERROR</summary>
        RW_COMMUNICATIONAS_PORT_SET_TIME_OUT_ERROR = 135,
        ///<summary>RW_COMMUNICATIONS_PORT_SET_TIME_OUT_ERROR</summary>
        RW_COMMUNICATIONS_PORT_SET_TIME_OUT_ERROR = 135,
        ///<summary>RW_ILLEGAL_COMMUNICATIONS_PORT_STATUS</summary>
        RW_ILLEGAL_COMMUNICATIONS_PORT_STATUS = 136,
        ///<summary>RW_COMMUNICATIONS_PORT_MASKING_ERROR</summary>
        RW_COMMUNICATIONS_PORT_MASKING_ERROR = 137,
        ///<summary>RW_COMMUNICATIONAS_PORT_PURGING_ERROR</summary>
        RW_COMMUNICATIONAS_PORT_PURGING_ERROR = 138,
        ///<summary>RW_COMMUNICATIONS_PORT_PURGING_ERROR</summary>
        RW_COMMUNICATIONS_PORT_PURGING_ERROR = 138,
        ///<summary>RW_PLAIN_DATA_LOAD_ERROR</summary>
        RW_PLAIN_DATA_LOAD_ERROR = 139,
        ///<summary>RW_PLAIN_DATA_SAVE_ERROR</summary>
        RW_PLAIN_DATA_SAVE_ERROR = 140,
        ///<summary>RW_CIPHER_DATA_LOAD_ERROR</summary>
        RW_CIPHER_DATA_LOAD_ERROR = 141,
        ///<summary>RW_CIPHER_DATA_SAVE_ERROR</summary>
        RW_CIPHER_DATA_SAVE_ERROR = 142,
        ///<summary>RW_RANDOM_NUMBER_NOT_SETTED</summary>
        RW_RANDOM_NUMBER_NOT_SETTED = 143,
        ///<summary>RW_TRANSACTION_ID_NOT_SETTED</summary>
        RW_TRANSACTION_ID_NOT_SETTED = 144,
        ///<summary>RW_TRANSACTION_KEY_ACCESS_ERROR</summary>
        RW_TRANSACTION_KEY_ACCESS_ERROR = 145,
        ///<summary>RW_ILLEGAL_TRANSACTION_KEY</summary>
        RW_ILLEGAL_TRANSACTION_KEY = 146,
        ///<summary>RW_COMMUNICATIONS_PARITY_ERROR</summary>
        RW_COMMUNICATIONS_PARITY_ERROR = 147,
        ///<summary>RW_NAK_RECEIVED</summary>
        RW_NAK_RECEIVED = 148,
        ///<summary>RW_SYNTAX_ERROR</summary>
        RW_SYNTAX_ERROR = 149,
        ///<summary>RW_ILLEGAL_PACKET_RECEIVED</summary>
        RW_ILLEGAL_PACKET_RECEIVED = 150,
        ///<summary>RW_SESSION_COUNT_OVER</summary>
        RW_SESSION_COUNT_OVER = 151,
        ///<summary>RW_RESPONSE_PACKET_NOT_RECEIVED</summary>
        RW_RESPONSE_PACKET_NOT_RECEIVED = 152,
        ///<summary>RW_DATA_SENDING_ERROR</summary>
        RW_DATA_SENDING_ERROR = 153,
        ///<summary>RW_DATA_SENDING_TIME_OUT</summary>
        RW_DATA_SENDING_TIME_OUT = 154,
        ///<summary>RW_DATA_RECEIVING_ERROR</summary>
        RW_DATA_RECEIVING_ERROR = 155,
        ///<summary>RW_DATA_RECEIVING_TIME_OUT</summary>
        RW_DATA_RECEIVING_TIME_OUT = 156,
        ///<summary>RW_CARD_NOT_FOUND</summary>
        RW_CARD_NOT_FOUND = 157,
        ///<summary>RW_INVALID_CARD_INDEX</summary>
        RW_INVALID_CARD_INDEX = 158,
        ///<summary>RW_CARD_STATUS_FLAG_ERROR</summary>
        RW_CARD_STATUS_FLAG_ERROR = 159,
        ///<summary>RW_DUMB_RESPONSE_PACKET_NOT_RECEIVED</summary>
        RW_DUMB_RESPONSE_PACKET_NOT_RECEIVED = 160,
        ///<summary>RW_USER_KEY_ERROR</summary>
        RW_USER_KEY_ERROR = 161,
        ///<summary>RW_USER_KEY_EXPIRATION</summary>
        RW_USER_KEY_EXPIRATION = 162,
        ///<summary>RW_CALL_BACK_PARAMETERS_NOT_SETTED</summary>
        RW_CALL_BACK_PARAMETERS_NOT_SETTED = 163,
        ///<summary>RW_MESSAGE_OF_READER_WRITER_CONNECT_REGISTRATION_ERROR</summary>
        RW_MESSAGE_OF_READER_WRITER_CONNECT_REGISTRATION_ERROR = 164,
        ///<summary>RW_MESSAGE_OF_READER_WRITER_PULLOUT_REGISTRATION_ERROR</summary>
        RW_MESSAGE_OF_READER_WRITER_PULLOUT_REGISTRATION_ERROR = 165,
        ///<summary>RW_PLUG_AND_PLAY_WATCH_FOR_CALL_BACK_NOT_STARTED</summary>
        RW_PLUG_AND_PLAY_WATCH_FOR_CALL_BACK_NOT_STARTED = 166,
        ///<summary>RW_PLUG_AND_PLAY_WATCH_FOR_CALL_BACK_ALREADY_STARTED</summary>
        RW_PLUG_AND_PLAY_WATCH_FOR_CALL_BACK_ALREADY_STARTED = 167,
        ///<summary>RW_WINDOW_CLASS_OF_PLUG_AND_PLAY_REGISTRATION_ERROR</summary>
        RW_WINDOW_CLASS_OF_PLUG_AND_PLAY_REGISTRATION_ERROR = 168,
        ///<summary>RW_WINDOW_OF_PLUG_AND_PLAY_CREATE_ERROR</summary>
        RW_WINDOW_OF_PLUG_AND_PLAY_CREATE_ERROR = 169,
        ///<summary>RW_GUID_DEVICE_INFORMATION_SET_ERROR</summary>
        RW_GUID_DEVICE_INFORMATION_SET_ERROR = 170,
        ///<summary>RW_GUID_INTERFACE_DATA_ERROR</summary>
        RW_GUID_INTERFACE_DATA_ERROR = 171,
        ///<summary>RW_GUID_SYMBOLIC_DATA_ERROR</summary>
        RW_GUID_SYMBOLIC_DATA_ERROR = 172,
        ///<summary>RW_API_NOT_IMPLEMENTED_FOR_THIS_PLATHOME</summary>
        RW_API_NOT_IMPLEMENTED_FOR_THIS_PLATHOME = 173,
        ///<summary>RW_REQUEST_SYSTEM_CODE_ERROR</summary>
        RW_REQUEST_SYSTEM_CODE_ERROR = 174,
        ///<summary>RW_SEARCH_SERVICE_CODE_ERROR</summary>
        RW_SEARCH_SERVICE_CODE_ERROR = 175,
        ///<summary>RW_USB_COMMUNICATION_ERROR</summary>
        RW_USB_COMMUNICATION_ERROR = 176,
        ///<summary>RW_READER_WRITER_DISCONNECTED</summary>
        RW_READER_WRITER_DISCONNECTED = 177,
        ///<summary>RW_NO_SYSTEM_RESOURCES_ERROR</summary>
        RW_NO_SYSTEM_RESOURCES_ERROR = 178,
        ///<summary>RW_COMMAND_CHECKSUM_ERROR</summary>
        RW_COMMAND_CHECKSUM_ERROR = 179,
        ///<summary>RW_COMMAND_IDTR_ERROR</summary>
        RW_COMMAND_IDTR_ERROR = 180,
        ///<summary>RW_READER_WRITER_MODE_ERROR</summary>
        RW_READER_WRITER_MODE_ERROR = 181,
        ///<summary>RW_INVALID_COMMAND_PACKET_PARAMETER</summary>
        RW_INVALID_COMMAND_PACKET_PARAMETER = 182,
        ///<summary>RW_INVALID_COMMAND_CODE</summary>
        RW_INVALID_COMMAND_CODE = 183,
        ///<summary>RW_READER_WRITER_VERSION_MISMATCH</summary>
        RW_READER_WRITER_VERSION_MISMATCH = 184,
        ///<summary>RW_ILLEGAL_COMMAND_PACKET</summary>
        RW_ILLEGAL_COMMAND_PACKET = 185,
        ///<summary>RW_READER_WRITER_UNIT_NUMBER_ERROR</summary>
        RW_READER_WRITER_UNIT_NUMBER_ERROR = 186,
        ///<summary>RW_LOCK_TIMEOUT</summary>
        RW_LOCK_TIMEOUT = 187,
        ///<summary>RW_GET_ACCESS_AUTHORITY_ERROR</summary>
        RW_GET_ACCESS_AUTHORITY_ERROR = 188,
        ///<summary>RW_READER_WRITER_VERSION_UNSUPPORTED</summary>
        RW_READER_WRITER_VERSION_UNSUPPORTED = 189
    }
}