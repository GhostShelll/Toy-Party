using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

using UnityEngine;

using com.jbg.core;

// The OpenAL 1.1 type spec:

/** 8-bit boolean */
using mina_bool = System.Byte;

/** character */
using mina_char = System.SByte;

/** signed 8-bit 2's complement integer */
using mina_byte = System.SByte;

/** unsigned 8-bit integer */
using mina_ubyte = System.SByte;

/** signed 16-bit 2's complement integer */
using mina_short = System.Int16;

/** unsigned 16-bit integer */
using mina_ushort = System.UInt16;

/** signed 32-bit 2's complement integer */
using mina_int = System.Int32;

/** unsigned 32-bit integer */
using mina_uint = System.UInt32;

/** non-negative 32-bit binary integer size */
using mina_sizei = System.Int32;

/** enumerated 32-bit value */
using mina_enum = System.Int32;

/** 32-bit IEEE754 floating-point */
using mina_float = System.Single;

/** 64-bit IEEE754 floating-point */
using mina_double = System.Double;

/** void type (for opaque pointers only) */
using mina_voidptr = System.IntPtr;

/** signed 64-bit integer */
using mina_int64 = System.Int64;

/** unsigned 64-bit integer */
using mina_uint64 = System.UInt64;

public sealed class MiniAudioCS
{

    // ----------------------------------------------------------------------------------------------------



    #region MINA_NATIVE_IDS

    public const mina_int MINA_OK = 0;
    public const mina_int MINA_INVALID_ID = -1; // 0xFFFFFFFF; uint.MaxValue; (unsigned int)-1
    public const mina_int MINA_FINISHED = -2;
    public static bool IsSuccess(mina_int ret) { return MiniAudioCS.MINA_OK <= ret; }
    public static bool IsError(mina_int ret) { return MiniAudioCS.MINA_OK > ret; }

    public const mina_int MINA_STREAMING_WAIT = -11;
    public const mina_int MINA_STREAMING_MAX_NUM = 32;

    public const mina_int MINA_ERR_INIT_ALREADY = -10001;
    public const mina_int MINA_ERR_INIT_INVALID_DEVICE_ID = -10002;
    //public const mina_int MINA_ERR_INIT_FAILED_AL = -10003; // DEPRECATED:
    public const mina_int MINA_ERR_INIT_EMPTY = -10004;
    public const mina_int MINA_ERR_INIT_FAILED_ALC_OPEN_DEVICE = -10005;
    public const mina_int MINA_ERR_INIT_FAILED_ALC_CREATE_CONTEXT = -10006;

    public const mina_int MINA_ERR_LOAD_BUFFER_FULL = -10101;
    public const mina_int MINA_ERR_LOAD_INVALID_BUFFER = -10102;
    public const mina_int MINA_ERR_LOAD_ALREADY_RELEASED = -10103;
    public const mina_int MINA_ERR_LOAD_AL_NOT_INIT = -10104;

    public const mina_int MINA_ERR_PLAY_FULL = -10201;
    public const mina_int MINA_ERR_PLAY_INVALID_BUFFER = -10202;
    public const mina_int MINA_ERR_PLAY_INVALID_PLAY = -10203;
    public const mina_int MINA_ERR_PLAY_BAD_BUFFER = -10204;
    public const mina_int MINA_ERR_PLAY_NOT_PLAYING = -10205;
    public const mina_int MINA_ERR_PLAY_ALREADY_STOPPED = -10206;
    public const mina_int MINA_ERR_PLAY_TYPE_MISSMATCH = -10207;
    public const mina_int MINA_ERR_PLAY_BAD_CONTEXT = -10208;   // NOTE: try mina_terminate() and mina_init()... (internal error)

    //public const mina_int MINA_ERR_STREAMING_NO_SUPPORTED_SET_NOMALIZED_TIME = -10301;
    public const mina_int MINA_ERR_STREAMING_NO_SUPPORTED_TELL_CALLBACK = -10302;
    public const mina_int MINA_ERR_STREAMING_NO_SUPPORTED_SEEK_CALLBACK = -10303;
    public const mina_int MINA_ERR_STREAMING_NOT_FOUND_BUFFER = -10304;
    public const mina_int MINA_ERR_STREAMING_INVALID_CALLBACK = -10305;
    public const mina_int MINA_ERR_STREAMING_INVALID_COUNT = -10306;
    public const mina_int MINA_ERR_STREAMING_TOO_MANY_COUNT = -10307;
    public const mina_int MINA_ERR_STREAMING_NOT_STREAMING = -10308;
    public const mina_int MINA_ERR_STREAMING_ALREADY_PLAY = -10309;
    public const mina_int MINA_ERR_STREAMING_NOT_PLAYING = -10310;
    public const mina_int MINA_ERR_STREAMING_LOAD_FAILED = -10311;

    //public const mina_int MINA_ERR_ASYNC_BAD_ID = -10401;                    // NOTE: async worker 를 구현하다가 말았다. (언제 살릴지 몰라서 주석으로 남김)
    //public const mina_int MINA_ERR_ASYNC_CREATE_THREAD_FAILED = -10402;
    //public const mina_int MINA_ERR_ASYNC_TO_MANY_TASK = - 10403;
    //public const mina_int MINA_ERR_ASYNC_ALREADY_STOPEED = - 10404;

    //public const mina_int MINA_ERR_WORK_BAD_ID = -10501;
    //public const mina_int MINA_ERR_WORK_NOT_WORKING = -10502;

    public const mina_int MINA_ERR_IO_ALREADY_INIT = -10601;
    public const mina_int MINA_ERR_IO_NO_INIT = -10602;
    public const mina_int MINA_ERR_IO_TOO_MANY_BUFFERS = -10603;
    public const mina_int MINA_ERR_IO_BUFFER_IS_NULL = -10604;
    public const mina_int MINA_ERR_IO_BUFFER_ALREADY_DESTROYED = -10605;
    public const mina_int MINA_ERR_IO_NOT_FOUND = -10606;
    //public const mina_int MINA_ERR_IO_UNKNOWN_LOCATION = -10607;
    public const mina_int MINA_ERR_IO_BAD_LOCATION = -10608;
    public const mina_int MINA_ERR_IO_INTERNAL_ERROR = -10609;
    public const mina_int MINA_ERR_IO_NOT_SUPPORT = -10610;

    public const mina_int MINA_ERR_IO_OS_FAILED_START = -10650;

    public const mina_int MINA_ERR_OV_FAIL = -10701;




    public const mina_int MINA_TYPE_NONE = 0;
    public const mina_int MINA_TYPE_PLAY = 1;
    public const mina_int MINA_TYPE_STREAMING = 2;

    public const mina_int MINA_STATE_PLAYING = 101;
    public const mina_int MINA_STATE_PAUSED = 102;
    public const mina_int MINA_STATE_STOPPED = 103;
    public const mina_int MINA_STATE_STREAMING_BUFFER_EMPTY = 104;
    public const mina_int MINA_STATE_STREAMING_PREPARE = 105;



#if UNITY_ANDROID
    public const mina_int MINA_ERR_ANDROID_FILE_JNI_IS_NULL = -10650;
    public const mina_int MINA_ERR_ANDROID_FILE_JNI_IS_BAD = -10651;
    public const mina_int MINA_ERR_ANDROID_FILE_CLASS_IS_NULL = -10652;
    public const mina_int MINA_ERR_ANDROID_FILE_METHOD_IS_NULL = -10653;
    public const mina_int MINA_ERR_ANDROID_FILE_AMOBJ_IS_NULL = -10654;
    public const mina_int MINA_ERR_ANDROID_FILE_AM_IS_NULL = -10655;
    public const mina_int MINA_ERR_ANDROID_FILE_FAILED_CONVERT_STR = -10656;
    public const mina_int MINA_ERR_ANDROID_FILE_FAILED_GETLENGTH = -10657;
    public const mina_int MINA_ERR_ANDROID_FILE_FAILED_READ = -10658;
#endif// UNITY_ANDROID

    #endregion MINA_NATIVE_IDS



    // ----------------------------------------------------------------------------------------------------



    #region MINA_NATIVE_FUNCTIONS

    // ----------------------------------------------------------------------------------------------------

#if UNITY_EDITOR_WIN && DEBUG
    private const string PluginName = "miniaudio_d";// windows editor
#elif !UNITY_EDITOR && (UNITY_IPHONE || UNITY_IOS)
private const string PluginName = "__Internal"; // iOS
#else
private const string PluginName = "miniaudio";    // android & osx & windows
#endif

#if DEBUG
    private const CallingConvention PlugCall = CallingConvention.Cdecl;
#else// DEBUG
private const CallingConvention PlugCall = CallingConvention.Cdecl;
#endif// DEBUG

    // ----------------------------------------------------------------------------------------------------

    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static mina_bool mina_is_activated();
    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static mina_int mina_init(mina_int bufferMax, mina_int playMax, mina_float frameRate);
    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static mina_int mina_init_advanced(mina_int deviceIdx, mina_int bufferMax, mina_int playMax, mina_float frameRate);
    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static void mina_terminate();
    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static mina_int mina_get_al_error();
    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static mina_int mina_reset();
    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static mina_int mina_restore();
    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static void mina_set_framerate(mina_float frameRate);
    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static mina_int mina_update(mina_float deltaTime);
    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static mina_int mina_update_async(mina_float frameRate);
    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static mina_int mina_update_async_stop();
    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static void mina_update_lock(mina_bool lock_); // for thread
    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static mina_bool mina_is_updating(); // for thread
    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static mina_bool mina_is_updating_async(); // for thread
    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static mina_int mina_update_async_start_waiting(); // for thread
    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static mina_float mina_get_async_framerate(); // for thread
    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static mina_uint64 mina_time_ms();
    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static void mina_sleep(mina_uint ms);

    // ----------------------------------------------------------------------------------------------------

    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static void mina_device_info_load();
    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static void mina_device_info_close();
    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static mina_int mina_device_get_count();
    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static mina_int mina_device_get_default();
    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static IntPtr mina_device_get_name(mina_int deviceIdx);
    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static IntPtr mina_device_selected_name();
    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static mina_int mina_device_selected_play_limit_mono();
    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static mina_int mina_device_selected_play_limit_stereo();
    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static mina_int mina_device_get_alc_error();

    // ----------------------------------------------------------------------------------------------------

    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static mina_int mina_buffer_load([MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_I1)] mina_byte[] pcmBuffer, mina_sizei pcmSize, mina_enum format, mina_int frequency);
    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static mina_int mina_buffer_release(mina_int bufferId);
    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static mina_int mina_buffer_release_all();
    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static mina_int mina_buffer_unique_key(mina_int bufferId);
    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static mina_int mina_buffer_is_valid(mina_int bufferId, mina_int uniqueKey);

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MINIAUDIO_BUFFER_INFO
    {
        public mina_int pcmSize;
        public mina_int frequency;
        public mina_int channelCount;
        public mina_int bitrate;
        public mina_float secTimeLength;
        public mina_int pcmSizePerSec;
    }

    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static IntPtr mina_buffer_info(mina_int bufferId);

    // ----------------------------------------------------------------------------------------------------

    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static mina_int mina_play(mina_int bufferId, mina_bool loop, mina_float volume);
    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static mina_int mina_play_advanced(mina_int bufferId, mina_bool loop, mina_float volume, mina_float secOffset, mina_float left_right);
    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static mina_int mina_stop(mina_int playId);
    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static mina_int mina_pause(mina_int playId);
    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static mina_int mina_resume(mina_int playId);
    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static mina_int mina_get_state(mina_int playId);
    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static mina_int mina_get_play_type(mina_int playId);
    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static mina_int mina_get_unique_key(mina_int playId);
    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static mina_int mina_is_valid(mina_int playId, mina_int uniqueKey);
    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static mina_int mina_set_tag(mina_int playId, IntPtr tag, mina_int tagSize);
    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static IntPtr mina_get_tag(mina_int playId);
    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static mina_int mina_get_tag_size(mina_int playId);
    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static mina_int mina_get_play_count();
    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static mina_int mina_get_play_max();
    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static mina_int mina_query_idle_count();
    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static mina_int mina_pcm_playid_to_bufferid(mina_int playId);
    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static void mina_volume_master(mina_float masterVolume);

    // ----------------------------------------------------------------------------------------------------

    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static mina_int mina_stop_all();
    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static mina_int mina_stop_pcm_all();
    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static void mina_pause_all(mina_bool pause);

    // ----------------------------------------------------------------------------------------------------

    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static mina_float mina_get_volume(mina_int playId);
    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static mina_int mina_volume(mina_int playId, mina_float volume);
    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static mina_float mina_get_panning(mina_int playId);    // TODO: 마저 구현. param value 및 return value 범위 룰 확실히.
    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static mina_int mina_panning(mina_int playId, mina_float left_right);
    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static mina_float mina_get_normalized_time(mina_int playId);
    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static mina_int mina_set_normalized_time(mina_int playId, mina_float nt);
    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static mina_float mina_get_sec_time_total(mina_int playId);
    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static mina_float mina_get_sec_time(mina_int playId);
    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static mina_int mina_set_sec_time(mina_int playId, mina_float st);
    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static mina_int mina_set_loop(mina_int playId, mina_bool loop);
    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static mina_int mina_is_loop(mina_int playId);

    // ----------------------------------------------------------------------------------------------------

    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static mina_int mina_streaming_play(mina_int playId, mina_bool loop, mina_float volume, mina_float secOffset);
    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static IntPtr mina_streaming_info(mina_int playId);


    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MINIAUDIO_STREAMING_ADVANCED_INFO
    {
        [MarshalAs(UnmanagedType.I4, SizeConst = MINA_STREAMING_MAX_NUM)]
        public mina_int[] useBufferIds;

        public mina_ubyte streamingBufferCountFixed;
        public mina_ubyte streamingBufferCount;
        public mina_ubyte streamingBufferForceLoadCount;
        public mina_ubyte streamingBufferNeedFillCount;
        public mina_ubyte streamingBufferLimit;

        public mina_ubyte streamingBufferCountDebug;
    }

    /*
    MINA_OK
    MINA_ERR_PLAY_INVALID_PLAY
    */
    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static mina_int mina_streaming_debug_dump(mina_int playId, IntPtr info);

    // ----------------------------------------------------------------------------------------------------

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate void MINIAUDIO_OV_STREAMING_RELEASE_OVBUFFER_CALLBACK(mina_int playId, IntPtr tag);

    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static mina_int mina_buffer_load_from_ov_oneshot([MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_I1)] mina_byte[] ovBuffer, mina_sizei ovBufferSize);
    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static mina_int mina_buffer_load_from_ov_oneshot_from_uri(IntPtr uri, mina_sizei uriLen);
    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static mina_int mina_buffer_load_from_ov_streaming([MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_I1)] mina_byte[] ovBuffer, mina_sizei ovBufferSize, [MarshalAs(UnmanagedType.FunctionPtr)] MINIAUDIO_OV_STREAMING_RELEASE_OVBUFFER_CALLBACK releaseOvBufferCallback, IntPtr releaseOvBufferTag, mina_int streamingBufferCount, mina_int streamingBufferForceLoadCount, mina_float streamingSecUnitTime);
    [DllImport(PluginName, CallingConvention = PlugCall)] public extern static mina_int mina_buffer_load_from_ov_streaming_from_uri(IntPtr uri, mina_sizei uriLen, mina_int streamingBufferCount, mina_int streamingBufferForceLoadCount, mina_float streamingSecUnitTime);

    // ----------------------------------------------------------------------------------------------------

    #endregion MINA_NATIVE_FUNCTIONS



    // ----------------------------------------------------------------------------------------------------

    static int deviceIdx;
    static int bufferMax;
    static int playMax;
    static float frameRate;

    public static void StartUp(LogFunction logFunc, LogFunction logWarnFunc, LogFunction logErrFunc)
    {
        ++MiniAudioCS.GenerationCurrent;

        MiniAudioCS.Verbose = logFunc;
        MiniAudioCS.Warning = logWarnFunc;
        MiniAudioCS.Error = logErrFunc;

#if UNITY_ANDROID && !UNITY_EDITOR
        MiniAudioCS.OpenJni("com.unity3d.player.UnityPlayer");
#endif

        try
        {
            string[] dlist = MiniAudioCS.GetDeviceList(out int defaultIdx);
#if LOG_MINA || LOG_MINA_OPEN
            for (int n = 0, nMax = dlist.Length; n < nMax; ++n)
            {
                if (n == defaultIdx)
                    MiniAudioCS.Log(MiniAudioCS.Verbose, string.Format("MINA_ALCDEVICE:[{0}] = {1} - default", n, dlist[n]));
                else
                    MiniAudioCS.Log(MiniAudioCS.Verbose, string.Format("MINA_ALCDEVICE:[{0}] = {1}", n, dlist[n]));
            }
#endif  // LOG_MINA || LOG_MINA_OPEN

#if UNITY_ANDROID && !UNITY_EDITOR
            int audioTrackIdx = -1;
            int openslIdx = -1;
            for (int n = 0, nMax = dlist.Length; n < nMax; ++n)
            {
                if (-1 == openslIdx)
                {
                    if (0 == "OpenSL".CompareTo(dlist[n]))
                        openslIdx = n;
                }
                if (-1 == audioTrackIdx)
                {
                    if (0 == "Android Legacy".CompareTo(dlist[n]))
                        audioTrackIdx = n;
                }
            }
            int selectIdx = (-1 != openslIdx) ? openslIdx : (-1 != audioTrackIdx) ? audioTrackIdx : 0;
#if LOG_MINA || LOG_MINA_OPEN
            MiniAudioCS.Log(MiniAudioCS.Verbose, "MINA_ALCDEVICE_SELECT:" + selectIdx + "(" + dlist[selectIdx] + ")"); 
#endif  // LOG_MINA || LOG_MINA_OPEN

            int sourceMax = 32;

            MiniAudioCS.InitAdvanced(selectIdx, 512, sourceMax, 1f / 25f); // android HAL = OpenSL ES
#else
            MiniAudioCS.Init(512, 32, 1f / 40f);
#endif
#if LOG_MINA || LOG_MINA_OPEN
            if (MiniAudioCS.IsActivate())
                MiniAudioCS.Log(MiniAudioCS.Verbose, "MINA_AL_PLAY_MAX:" + MiniAudioCS.GetPlayMax() + ", LIMIT:" + MiniAudioCS.GetPlayLimit());
#endif  // LOG_MINA || LOG_MINA_OPEN


            MiniAudioCS.UpdateAsync(1f / 40f);

        }
        catch (System.Exception e)
        {
            MiniAudioCS.Log(MiniAudioCS.Error, "MINA_ERROR(StartUp):" + e.ToString());
        }
    }

    public static bool IsActivate()
    {
        return (1 == MiniAudioCS.mina_is_activated());
    }

    private static void Init(int bufferMax, int playMax, float frameRate)
    {
        MiniAudioCS.deviceIdx = -1;
        MiniAudioCS.bufferMax = bufferMax;
        MiniAudioCS.playMax = playMax;
        MiniAudioCS.frameRate = frameRate;

        int ret = MiniAudioCS.mina_init(bufferMax, playMax, frameRate);
        if (MINA_OK != ret)
        {
#if UNITY_EDITOR
            if (MINA_ERR_INIT_ALREADY == ret)
            {
                MiniAudioCS.CloseAll();
                MiniAudioCS.mina_terminate();

                ret = MiniAudioCS.mina_init(bufferMax, playMax, frameRate);

                if (MINA_OK != ret)
                    MiniAudioCS.Log(MiniAudioCS.Error, "MINA_ERROR(mina_init):" + ret);

                return;
            }
#endif  // UNITY_EDITOR
            MiniAudioCS.Log(MiniAudioCS.Error, "MINA_ERROR(mina_init):" + ret + ", ALC_ERROR:" + MiniAudioCS.mina_device_get_alc_error() + ", AL_ERROR:" + MiniAudioCS.mina_get_al_error());
        }
    }

#if UNITY_ANDROID && !UNITY_EDITOR
    private static void InitAdvanced(int deviceIdx, int bufferMax, int playMax, float frameRate)
    {
        MiniAudioCS.deviceIdx = deviceIdx;
        MiniAudioCS.bufferMax = bufferMax;
        MiniAudioCS.playMax = playMax;
        MiniAudioCS.frameRate = frameRate;

        int ret = MiniAudioCS.mina_init_advanced(deviceIdx, bufferMax, playMax, frameRate);
        if (MINA_OK != ret)
            MiniAudioCS.Log(MiniAudioCS.Error, "MINA_ERROR(mina_init_advanced)_RET:" + ret);
    }
#endif  // UNITY_ANDROID && !UNITY_EDITOR

    public static string[] GetDeviceList(out int defaultIdx)
    {
        MiniAudioCS.mina_device_info_load();

        int count = MiniAudioCS.mina_device_get_count();
        string[] list = new string[count];

        for (int n = 0; n < count; ++n)
            list[n] = MiniAudioCS.ToStrSafe(MiniAudioCS.mina_device_get_name(n));

        defaultIdx = MiniAudioCS.mina_device_get_default();
        return list;
    }

    public static void CleanUp()
    {
        MiniAudioCS.CloseAll();

        if (1 == MiniAudioCS.mina_is_updating_async())
            MiniAudioCS.UpdateAsyncStop();

#if UNITY_ANDROID && ! UNITY_EDITOR
        MiniAudioCS.CloseJni();
#endif

        try
        {
            MiniAudioCS.mina_terminate();
        }
        catch (System.Exception e)
        {
            MiniAudioCS.Log(MiniAudioCS.Error, e.ToString());
        }

        Verbose = default;
        Warning = default;
        Error = default;
    }

    public static bool Reset()
    {
#if LOG_DEBUG
        Debug.Log("MiniAudioCS.Reset");
#endif

        MiniAudioCS.AutoDestroyList.Clear();

        int ret = MiniAudioCS.mina_reset();
        return MINA_OK <= ret;
    }

    public static void TerminateMina()
    {
#if LOG_DEBUG
        Debug.Log("MiniAudioCS.TerminateMina");
#endif

        MiniAudioCS.AutoDestroyList.Clear();
        MiniAudioCS.mina_terminate();
    }

    public static int InitMina()
    {
#if LOG_DEBUG
        Debug.Log("MiniAudioCS.InitMina");
#endif

        if (MiniAudioCS.deviceIdx != -1)
            return mina_init_advanced(MiniAudioCS.deviceIdx, MiniAudioCS.bufferMax, MiniAudioCS.playMax, MiniAudioCS.frameRate);
        else
            return mina_init(MiniAudioCS.bufferMax, MiniAudioCS.playMax, MiniAudioCS.frameRate);
    }

    public static void Restore()
    {
        int restoreRet = MiniAudioCS.mina_restore();

#if LOG_DEBUG
        Debug.Log("MiniAudioCS.mina_restore: ret - " + restoreRet);
#endif  // LOG_DEBUG
    }

    public static void SetFrameRate(float frameRate)
    {
        MiniAudioCS.mina_set_framerate(frameRate);
    }

    public static int Update(float deltaTime)
    {
        int ret = 0;

        if (1 != MiniAudioCS.mina_is_updating_async())
            ret = MiniAudioCS.mina_update(deltaTime);

        MiniAudioCS.UpdateOneshotPlayings();
        return ret;
    }

    public static int UpdateAsync(float frameRate)
    {
        return MiniAudioCS.mina_update_async(frameRate);
    }

    public static int UpdateAsyncStop()
    {
        return MiniAudioCS.mina_update_async_stop();
    }

    public static void UpdateLock(bool lock_) // for thread
    {
        MiniAudioCS.mina_update_lock((mina_bool)(lock_ ? 1 : 0));
    }


#if UNITY_ANDROID && !UNITY_EDITOR
    public static void OpenJni(string activityClassPath)
    {
#if LOG_OPENJNI
        MiniAudioCS.Log(MiniAudioCS.Verbose, "OpenJni(miniaudio)");
#endif  // LOG_OPENJNI
    
        AndroidJavaClass activityClass = new AndroidJavaClass(activityClassPath);
#if LOG_OPENJNI
        MiniAudioCS.Log(MiniAudioCS.Verbose, "activityClass = " + activityClass);
#endif  // LOG_OPENJNI

        AndroidJavaObject activity = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
#if LOG_OPENJNI
        MiniAudioCS.Log(MiniAudioCS.Verbose, "activity = " + activity);
#endif  // LOG_OPENJNI

        AndroidJavaClass envClass = new AndroidJavaClass("com.miniaudio.env");
#if LOG_OPENJNI
        MiniAudioCS.Log(MiniAudioCS.Verbose, "envClass = " + envClass);
#endif  // LOG_OPENJNI

        envClass.CallStatic("loadAssetManager", activity.Call<AndroidJavaObject>("getAssets"));

#if LOG_OPENJNI
        MiniAudioCS.Log(MiniAudioCS.Verbose, "OpenJni(miniaudio) - end");
#endif  // LOG_OPENJNI
    }

    public static void CloseJni()
    {
#if LOG_OPENJNI
        MiniAudioCS.Log(MiniAudioCS.Verbose, "CloseJni(miniaudio)");
#endif  // LOG_OPENJNI

        AndroidJavaClass envClass = new AndroidJavaClass("com.miniaudio.env");
#if LOG_OPENJNI
        MiniAudioCS.Log(MiniAudioCS.Verbose, "envClass = " + envClass);
#endif  // LOG_OPENJNI

        envClass.CallStatic("unloadAssetManager");
    
#if LOG_OPENJNI
        MiniAudioCS.Log(MiniAudioCS.Verbose, "CloseJni(miniaudio) - end");
#endif  // LOG_OPENJNI
    }
#endif

    public class BufferInfo
    {
        public mina_int pcmSize;
        public mina_int frequency;
        public mina_int channelCount;
        public mina_int bitrate;
        public mina_float secTimeLength;
        public mina_int pcmSizePerSec;

        public unsafe void Setup(MINIAUDIO_BUFFER_INFO* p)
        {
            this.pcmSize = p->pcmSize;
            this.frequency = p->frequency;
            this.channelCount = p->channelCount;
            this.bitrate = p->bitrate;
            this.secTimeLength = p->secTimeLength;
            this.pcmSizePerSec = p->pcmSizePerSec;
#if LOG_MINA || LOG_MINA_OPEN
            MiniAudioCS.Log(MiniAudioCS.Verbose, "MINA_OPEN_BUFFER:" + this.ToString());
#endif  // LOG_MINA || LOG_MINA_OPEN
        }

        public override string ToString()
        {
            return string.Format("{{pcmSize:{0}, frequency:{1}, chCnt:{2}, br:{3}, sec:{4}, pps:{5}}}",
                                 this.pcmSize,
                                 this.frequency,
                                 this.channelCount,
                                 this.bitrate,
                                 this.secTimeLength,
                                 this.pcmSizePerSec);
        }
    }


    public static string ToStr(IntPtr p)
    {
        return Marshal.PtrToStringAnsi(p);
    }
    public static string ToStrSafe(IntPtr p)
    {
        string s = Marshal.PtrToStringAnsi(p);
        return s ?? "";
    }

    public static string ToStr(IntPtr p, int len)
    {
        return Marshal.PtrToStringAnsi(p, len);
    }
    public static string ToStrSafe(IntPtr p, int len)
    {
        string s = Marshal.PtrToStringAnsi(p, len);
        return s ?? "";
    }








#if UNITY_ANDROID
    public const int MINA_STREAMING_BUFFER_COUNT = 4;
    public const int MINA_STREAMING_BUFFER_FORCE_LOAD_COUNT = 2;
    public const float MINA_STREAMING_UNIT_TIME = 0.25f;

    public const string MINA_JAR_TYPE = "jar:";
#else
    public const int MINA_STREAMING_BUFFER_COUNT = 4;
    public const int MINA_STREAMING_BUFFER_FORCE_LOAD_COUNT = 2;
    public const float MINA_STREAMING_UNIT_TIME = 0.25f;
#endif

    public int Handle { private set; get; }
    public int UniqueKey { private set; get; }

    public string Uri { private set; get; }

    public int Generation { private set; get; } // for internal
    public static int GenerationCurrent { private set; get; }

    public bool IsPlayNoExcept { set; get; }

#if LOG_MINA || LOG_MINA_OPEN || LOG_MINA_PLAY || LOG_MINA_ACTION
    private const int DEFAULT_STRINGBUILDER_SIZE = 256;
    private static StringBuilder m_myStrBuilder = null;
    private static StringBuilder SB
    {
        get
        {
            if (m_myStrBuilder == null)
            {
                m_myStrBuilder = new StringBuilder(DEFAULT_STRINGBUILDER_SIZE);
            }
            return m_myStrBuilder;
        }
    }
#endif  // LOG_MINA || LOG_MINA_OPEN

    public enum PLAYTYPE
    {
        None = 0,
        PCMFull,
        PCMStreaming,
    }
    public PLAYTYPE PlayType { private set; get; }
    public MiniAudioCS.BufferInfo Info { private set; get; }


    public delegate void LogFunction(string log);
    public static LogFunction Verbose { private set; get; }
    public static LogFunction Warning { private set; get; }
    public static LogFunction Error { private set; get; }

    private static void Log(MiniAudioCS.LogFunction l, string msg)
    {
        l?.Invoke(msg);
    }


    public static bool UseErrLogDataList { set; get; }

    public class ErrLogData
    {
        public enum State
        {
            Open = 0,
            Play,
        }
        public State state;

        public int ret;
        public PLAYTYPE playType;
        public int handle;
        public string uri;
        public DateTime time;
        public string msg;
    }
    public static List<ErrLogData> ErrLogBuffer = new(64);

    private void AppendErrLog(ErrLogData.State state, int ret)
    {
        ErrLogData errLogData = new();
        errLogData.state = state;
        errLogData.ret = ret;
        errLogData.playType = this.PlayType;
        errLogData.handle = this.Handle;
        errLogData.uri = this.Uri;
        errLogData.time = System.DateTime.Now;
        MiniAudioCS.ErrLogBuffer.Add(errLogData);
    }

    public MiniAudioCS()
    {
        this.Handle = MiniAudioCS.MINA_INVALID_ID;
        this.PlayType = PLAYTYPE.None;
    }

    ~MiniAudioCS()
    {
        this.Close();
    }

    public static MiniAudioCS OpenOneshot(string uri, bool streaming)
    {
        MiniAudioCS audio = new();
        if (audio.Open(uri, streaming))
            return audio;

        return null;
    }


    private static readonly FastArray<MiniAudioCS> AutoDestroyList = new(32);
    public static int OpenAndPlayOneshotStreaming(string uri, float vol, float pan, float secOffset)
    {
        MiniAudioCS audio = MiniAudioCS.OpenOneshot(uri, true);
        if (null != audio)
        {
            audio.Play(vol, pan, secOffset, false);
            MiniAudioCS.AutoDestroyList.Add(audio);
            return audio.Handle;
        }

        return MiniAudioCS.MINA_INVALID_ID;
    }

    public static int StopOneshotPlayings()
    {
        int oneshotPlayCount = MiniAudioCS.AutoDestroyList.Count;

#if LOG_CLEANUP
        MiniAudioCS.Log(MiniAudioCS.Verbose, "MiniAudio.StopOneshotPlayings #1:" + oneshotPlayCount);
#endif  // LOG_CLEANUP

        for (int n = oneshotPlayCount - 1; 0 <= n; --n)
            MiniAudioCS.AutoDestroyList[n].Close();

#if LOG_CLEANUP
        MiniAudioCS.Log(MiniAudioCS.Verbose, "MiniAudio.StopOneshotPlayings #2");
#endif  // LOG_CLEANUP

        MiniAudioCS.AutoDestroyList.Clear();

#if LOG_CLEANUP
        MiniAudioCS.Log(MiniAudioCS.Verbose, "MiniAudio.StopOneshotPlayings #3");
#endif  // LOG_CLEANUP
        return oneshotPlayCount;
    }

    private static void UpdateOneshotPlayings()
    {
        for (int n = MiniAudioCS.AutoDestroyList.Count - 1; 0 <= n; --n)
        {
            MiniAudioCS a = MiniAudioCS.AutoDestroyList[n];
            if (MiniAudioCS.MINA_INVALID_ID == MiniAudioCS.GetState(a.Handle))
            {
                a.Close();

                MiniAudioCS.AutoDestroyList[n] = null;
                MiniAudioCS.AutoDestroyList.RemoveAt(n);
            }
        }
    }




    public bool Open(string uri, bool streaming)
    {
        if (PLAYTYPE.None != this.PlayType)
            this.Close();

        this.Generation = MiniAudioCS.GenerationCurrent;

        try
        {
            this.Uri = uri;

            if (!streaming)
            {
                this.PlayType = PLAYTYPE.PCMFull;

#if LOG_MINA || LOG_MINA_OPEN
                MiniAudioCS.Log(MiniAudioCS.Verbose, "MINA_OPEN_PCM_URI:" + uri);
#endif  // LOG_MINA || LOG_MINA_OPEN

                IntPtr nativeStr = uri.ToNativeMBCS(out int nativeLen);
                int bufferId = MiniAudioCS.mina_buffer_load_from_ov_oneshot_from_uri(nativeStr, nativeLen);
                nativeStr.FreeNativeStr();

                if (MiniAudioCS.MINA_OK > bufferId)
                {
                    if (MiniAudioCS.MINA_ERR_LOAD_BUFFER_FULL != bufferId)
                    {
                        MiniAudioCS.Log(MiniAudioCS.Error, "MINA_ERROR(mina_buffer_load_from_ov_oneshot_from_uri)_URI:" + uri + ", RET:" + bufferId);
                        return false;
                    }
                }

#if LOG_MINA || LOG_MINA_OPEN
                MiniAudioCS.Log(MiniAudioCS.Verbose, "MINA_OPEN_PCM_BUFFERID:" + bufferId);
#endif  // LOG_MINA || LOG_MINA_OPEN

                if (MiniAudioCS.MINA_OK <= (this.Handle = bufferId))
                {
                    this.UniqueKey = MiniAudioCS.mina_buffer_unique_key(bufferId);
                    if (MINA_OK > this.UniqueKey)
                    {
                        MiniAudioCS.Log(MiniAudioCS.Warning, "MINA_INVALID_UNIQUE_KEY_ERROR:" + this.UniqueKey);
                        this.UniqueKey = 0;
                    }

#if LOG_MINA || LOG_MINA_OPEN
                    MiniAudioCS.Log(MiniAudioCS.Verbose, "MINA_OPEN_PCM_UKEY:" + UniqueKey);
#endif  // LOG_MINA || LOG_MINA_OPEN

                    IntPtr ptr = MiniAudioCS.mina_buffer_info(this.Handle);

#if LOG_MINA || LOG_MINA_OPEN
                    MiniAudioCS.Log(MiniAudioCS.Verbose, "MINA_OPEN_PCM_BI_PTR:" + ptr);
#endif  // LOG_MINA || LOG_MINA_OPEN

                    if (IntPtr.Zero == ptr)
                    {
                        this.Info = null;
                    }
                    else
                    {
                        this.Info = new BufferInfo();
                        unsafe
                        {
                            this.Info.Setup((MINIAUDIO_BUFFER_INFO*)ptr);
                        }
                    }

#if LOG_MINA || LOG_MINA_OPEN
                    MiniAudioCS.Log(MiniAudioCS.Verbose, "MINA_OPEN_PCM_FREQ:" + this.Info.frequency);
#endif  // LOG_MINA || LOG_MINA_OPEN

                    return true;
                }
                else
                {
                    this.OpenError(streaming, this.Handle);
                    return false;
                }
            }
            else
            {
                this.PlayType = PLAYTYPE.PCMStreaming;

#if LOG_MINA || LOG_MINA_OPEN
                MiniAudioCS.Log(MiniAudioCS.Verbose, "MINA_OPEN_STREAMING_URI:" + uri);
#endif  // LOG_MINA || LOG_MINA_OPEN

                IntPtr nativeStr = uri.ToNativeMBCS(out int nativeLen);
                int playId = MiniAudioCS.mina_buffer_load_from_ov_streaming_from_uri(nativeStr, nativeLen, MINA_STREAMING_BUFFER_COUNT, MINA_STREAMING_BUFFER_FORCE_LOAD_COUNT, MINA_STREAMING_UNIT_TIME);
                nativeStr.FreeNativeStr();

                if (MiniAudioCS.MINA_OK > playId)
                {
                    if (MiniAudioCS.MINA_ERR_PLAY_FULL != playId && MiniAudioCS.MINA_ERR_PLAY_BAD_CONTEXT != playId)
                    {
                        MiniAudioCS.Log(MiniAudioCS.Error, "MINA_ERROR(mina_buffer_load_from_ov_streaming_from_uri)_URI:" + uri + ", RET:" + playId);
                        return false;
                    }
                }

#if LOG_MINA || LOG_MINA_OPEN
                MiniAudioCS.Log(MiniAudioCS.Verbose, "MINA_OPEN_STREAMING_PLAYID:" + playId);
#endif  // LOG_MINA || LOG_MINA_OPEN

                if (MiniAudioCS.MINA_OK <= (this.Handle = playId))
                {
                    IntPtr ptr = MiniAudioCS.mina_streaming_info(this.Handle);

#if LOG_MINA || LOG_MINA_OPEN
                    MiniAudioCS.Log(MiniAudioCS.Verbose, "MINA_OPEN_STREAMING_BI_PTR:" + ptr);
#endif  // LOG_MINA || LOG_MINA_OPEN

                    if (IntPtr.Zero == ptr)
                    {
                        this.Info = null;
                    }
                    else
                    {
                        this.Info = new BufferInfo();
                        unsafe
                        {
                            this.Info.Setup((MINIAUDIO_BUFFER_INFO*)ptr);
                        }
                    }

#if LOG_MINA || LOG_MINA_OPEN
                    MiniAudioCS.Log(MiniAudioCS.Verbose, "MINA_OPEN_STREAMING_FREQ:" + this.Info.frequency);
#endif  // LOG_MINA || LOG_MINA_OPEN

                    this.UniqueKey = MiniAudioCS.GetUniqueKey(this.Handle);

#if LOG_MINA || LOG_MINA_OPEN
                    MiniAudioCS.Log(MiniAudioCS.Verbose, "MINA_OPEN_STREAMING_UKEY:" + this.UniqueKey);
#endif  // LOG_MINA || LOG_MINA_OPEN

                    return true;
                }
                else
                {
                    this.OpenError(streaming, this.Handle);
                    return false;
                }
            }
        }
        catch (System.Exception e)
        {
            MiniAudioCS.Log(MiniAudioCS.Warning, "MINA_WARNING_EXCEPTION:" + e.ToString());
            return false;
        }
    }

    private void OpenError(bool streaming, int ret)
    {
#if LOG_MINA || LOG_MINA_OPEN

        SB.Length = 0;

        SB.Append("MINA_OPEN_ERROR_");
        
        if (streaming)
            SB.Append("STREAMING_");

        SB.Append("URI:")
          .Append(this.Uri)
          .Append(", PLAYTYPE:")
          .Append(this.PlayType.ToString())
          .Append(", HANDLE:")
          .Append(this.Handle);

        MiniAudioCS.Log(MiniAudioCS.Warning, SB.ToString());

#endif  // LOG_MINA || LOG_MINA_OPEN

        if (MiniAudioCS.UseErrLogDataList)
            this.AppendErrLog(ErrLogData.State.Open, ret);

        this.Handle = MiniAudioCS.MINA_INVALID_ID;
        this.Uri = null;
        this.PlayType = PLAYTYPE.None;
        this.Info = null;
    }


    public bool Close()
    {
#if LOG_MINA || LOG_MINA_OPEN
        MiniAudioCS.Log(MiniAudioCS.Verbose, "MINA_CLOSE_CURGEN:" + this.Generation + ", STATICGEN:" + MiniAudioCS.GenerationCurrent + ", PLAYTYPE:" + this.PlayType);
#endif  // LOG_MINA || LOG_MINA_OPEN

        int ret = MiniAudioCS.MINA_OK;
        bool retb = false;
        if (this.Generation == MiniAudioCS.GenerationCurrent)
        {
            if (PLAYTYPE.PCMFull == this.PlayType)
            {
                if (MiniAudioCS.MINA_OK == MiniAudioCS.mina_buffer_is_valid(this.Handle, this.UniqueKey))
                {
                    retb = MiniAudioCS.MINA_OK == (ret = MiniAudioCS.mina_buffer_release(this.Handle));
#if LOG_MINA || LOG_MINA_OPEN
                    MiniAudioCS.Log(MiniAudioCS.Verbose, "MINA_ACTION_PCM_CLOSE_HANDLE:" + this.Handle + ", RET:" + ret);
#endif  // LOG_MINA || LOG_MINA_OPEN
                }
                else
                {
                    retb = true; // NOTE: already closed
                }
            }
            else if (PLAYTYPE.PCMStreaming == this.PlayType)
            {
#if LOG_MINA || LOG_MINA_OPEN
                MiniAudioCS.Log(MiniAudioCS.Verbose, "MINA_BUGFIX_HANDLE:" + this.Handle + ", UKEY:" + this.UniqueKey);
#endif  // LOG_MINA || LOG_MINA_OPEN
                if (MiniAudioCS.MINA_OK == MiniAudioCS.mina_is_valid(this.Handle, this.UniqueKey))
                {
                    int state = MiniAudioCS.GetState(this.Handle);
#if LOG_MINA || LOG_MINA_OPEN
                    MiniAudioCS.Log(MiniAudioCS.Verbose, "MINA_BUGFIX_STATE:" + state);
#endif  // LOG_MINA || LOG_MINA_OPEN
                    if (MiniAudioCS.MINA_STATE_STOPPED != state &&
                        MiniAudioCS.MINA_OK <= state)
                    {
                        retb = MiniAudioCS.MINA_OK == (ret = MiniAudioCS.mina_stop(this.Handle));
#if LOG_MINA || LOG_MINA_OPEN
                        MiniAudioCS.Log(MiniAudioCS.Verbose, "MINA_ACTION_STREAMING_CLOSE_HANDLE:" + this.Handle + ", RET:" + ret);
#endif  // LOG_MINA || LOG_MINA_OPEN
                    }
                }
                else
                {
                    retb = true; // NOTE: already closed
                }
            }
        }

#if LOG_MINA || LOG_MINA_OPEN
        MiniAudioCS.Log(MiniAudioCS.Verbose, "MINA_CLOSE_URI:" + this.Uri + ", RET:" + ret);
#endif  // LOG_MINA || LOG_MINA_OPEN
        this.Handle = MiniAudioCS.MINA_INVALID_ID;
        this.UniqueKey = MiniAudioCS.MINA_INVALID_ID;
        this.Uri = null;
        this.PlayType = PLAYTYPE.None;
        this.Info = null;
        return retb;
    }

    public static int CloseAll()
    {
        MiniAudioCS.StopOneshotPlayings();

        ++MiniAudioCS.GenerationCurrent;

        int ret = MiniAudioCS.mina_buffer_release_all();
#if LOG_MINA || LOG_MINA_OPEN
        MiniAudioCS.Log(MiniAudioCS.Verbose, "MINA_ACTION_CLOSE_ALL:" + ret);
#endif  // LOG_MINA || LOG_MINA_OPEN
        return ret;
    }

    public int Play(float vol, float pan, float secOffset, bool looping)
    {
        if (this.Generation != MiniAudioCS.GenerationCurrent)
            return MiniAudioCS.MINA_INVALID_ID;// NOTE: 이미 지워진 것.

        if (MiniAudioCS.MINA_INVALID_ID == this.Handle)
        {
#if LOG_MINA || LOG_MINA_PLAY || LOG_MINA_ACTION
            SB.Length = 0;
            MiniAudioCS.Log(MiniAudioCS.Warning, SB.Append("MINA_PLAY_WARNING_INVALID_AUDIO:").Append(this.Uri).Append(", PLAYTYPE:").Append(this.PlayType.ToString()).ToString());
#endif  // LOG_MINA || LOG_MINA_PLAY || LOG_MINA_ACTION

            return MiniAudioCS.MINA_INVALID_ID;
        }


        if (PLAYTYPE.PCMFull == this.PlayType)
        {
            if (MiniAudioCS.MINA_OK == MiniAudioCS.mina_buffer_is_valid(this.Handle, this.UniqueKey))
            {
                int playId = MiniAudioCS.MINA_INVALID_ID;
                if (this.IsPlayNoExcept)
                {
                    playId = MiniAudioCS.mina_play_advanced(this.Handle, (mina_bool)(looping ? 1 : 0), vol, secOffset, pan);
#if LOG_MINA || LOG_MINA_ACTION
                    MiniAudioCS.Log(MiniAudioCS.Verbose, "MINA_ACTION_PLAY_PCM_NOEXCEPT_BUFFER:" + this.Handle + ", PLAYID:" + playId + ", LOOP:" + looping + ", SEEK:" + secOffset + ", VOL:" + vol);
#endif  // LOG_MINA || LOG_MINA_ACTION
                }
                else
                {
                    playId = MiniAudioCS.mina_play_advanced(this.Handle, (mina_bool)(looping ? 1 : 0), vol, secOffset, pan);
#if LOG_MINA || LOG_MINA_ACTION
                    MiniAudioCS.Log(MiniAudioCS.Verbose, "MINA_ACTION_PLAY_PCM_BUFFER:" + this.Handle + ", PLAYID:" + playId + ", LOOP:" + looping + ", SEEK:" + secOffset + ", VOL:" + vol);
#endif  // LOG_MINA || LOG_MINA_ACTION
                    if (MiniAudioCS.MINA_OK > playId)
                    {
                        switch (playId)
                        {
                            case MiniAudioCS.MINA_ERR_PLAY_FULL:
                            case MiniAudioCS.MINA_ERR_PLAY_BAD_CONTEXT:
                                break;

                            default:
                                MiniAudioCS.Log(MiniAudioCS.Error, "MINA_ERROR(mina_play_advanced)_HANDLE:" + this.Handle + ", RET:" + playId + ", LOOP:" + looping);
                                break;
                        }
                    }
                }

                if (MiniAudioCS.MINA_OK <= playId)
                {
#if LOG_MINA || LOG_MINA_PLAY
                    MiniAudioCS.SetTextTag(playId, this.Uri);
                    SB.Length = 0;
                    MiniAudioCS.Log(MiniAudioCS.Verbose, SB.Append("MINA_PLAY_PCM_SUCCEED:").Append(playId).Append(", URI:").Append(this.Uri).Append(", REMAIN:").Append(MiniAudioCS.QueryIdleCount()).Append(", PLAYMAX:").Append(MiniAudioCS.GetPlayMax()).ToString());
#endif  // LOG_MINA || LOG_MINA_PLAY
                    return playId;
                }
                else
                {
                    if (MiniAudioCS.UseErrLogDataList)
                        this.AppendErrLog(ErrLogData.State.Play, playId);

#if LOG_MINA || LOG_MINA_PLAY
                    SB.Length = 0;
                    MiniAudioCS.Log(MiniAudioCS.Warning, SB.Append("MINA_WARNING_PLAY_PCM_FAILED:").Append(playId).Append(", URI:").Append(this.Uri).ToString());

                    if (MiniAudioCS.MINA_ERR_PLAY_FULL == playId || MiniAudioCS.MINA_ERR_PLAY_BAD_CONTEXT == playId)
                    {
                        int playMax = MiniAudioCS.GetPlayMax();
                        SB.Length = 0;
                        string msg = SB.Append("MINA_WARNING_PLAY_PCM_FAILED_REMAIN:").Append(MiniAudioCS.QueryIdleCount()).Append(", PLAYMAX:").Append(playMax).ToString();
                        MiniAudioCS.Log(MiniAudioCS.Warning, msg);
                        if (MiniAudioCS.UseErrLogDataList)
                            this.AppendErrLogCustom(msg);

                        MiniAudioCS.Log(MiniAudioCS.Warning, "MINA_WARNING_PLAY_LIST");
                        for (int n = 0; n < playMax; ++n)
                        {
                            try 
                            {
                                int playType = MiniAudioCS.GetPlayType(n);
                                switch (playType)
                                {
                                case MiniAudioCS.MINA_TYPE_PLAY:
                                    {
                                        int state = MiniAudioCS.GetState(n);
                                        int bufferId = MiniAudioCS.GetPlayIdToBufferId(n);
                                        string tag = MiniAudioCS.GetTextTag(n);
                                        
                                        SB.Length = 0;
                                        msg = SB.Append("MINA_WARNING_PLAY_AT(PCM):").Append(n).Append(", STATE:").Append(state).Append(", BUFFER:").Append(bufferId).Append(", TAG:").Append(tag).ToString();
                                        MiniAudioCS.Log(MiniAudioCS.Warning, msg);
                                        if (MiniAudioCS.UseErrLogDataList)
                                            this.AppendErrLogCustom(msg);
                                    
                                        break;
                                    }
                                    
                                case MiniAudioCS.MINA_TYPE_STREAMING:
                                    {
                                        int state = MiniAudioCS.GetState(n);
                                        string tag = MiniAudioCS.GetTextTag(n);
                                    
                                        SB.Length = 0;
                                        msg = SB.Append("MINA_WARNING_PLAY_AT(STREAMING):").Append(n).Append(", STATE:").Append(state).Append(", TAG:").Append(tag).ToString();
                                        MiniAudioCS.Log(MiniAudioCS.Warning, msg);
                                        if (MiniAudioCS.UseErrLogDataList)
                                            this.AppendErrLogCustom(msg);
                                        
                                        break;
                                    }

                                case MiniAudioCS.MINA_TYPE_NONE:
                                default:
                                    {
                                        SB.Length = 0;
                                        msg = SB.Append("MINA_WARNING_PLAY_AT(NONE):").Append(n).ToString();
                                        MiniAudioCS.Log(MiniAudioCS.Warning, msg);
                                        if (MiniAudioCS.UseErrLogDataList)
                                            this.AppendErrLogCustom(msg);
                                    
                                        break;
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                MiniAudioCS.Log(MiniAudioCS.Warning, e.ToString());
                            }
                        }
                    }
#endif  // LOG_MINA || LOG_MINA_PLAY

                    return MiniAudioCS.MINA_INVALID_ID;
                }
            }
            else
            {
                // NOTE: 누군가가 내 버퍼를 지워버렸킬⑸ 내부에서 문제 발생.
                MiniAudioCS.Log(MiniAudioCS.Warning, "MINA_WARNING_PLAY_PCM_INVALID_OBJECT_URI:" + this.Uri + ", BUFFER:" + this.Handle + ", UKEY:" + this.UniqueKey + ", REAL_UKEY:" + MiniAudioCS.GetUniqueKey(this.Handle));
                this.Close();
                return MiniAudioCS.MINA_INVALID_ID;
            }
        }
        else if (PLAYTYPE.PCMStreaming == this.PlayType)
        {
            if (MiniAudioCS.MINA_OK == MiniAudioCS.mina_is_valid(this.Handle, this.UniqueKey))
            {
                // NOTE: 이미 플레이 중일 경우는?
                int state = MiniAudioCS.GetState(this.Handle);
                if (MiniAudioCS.MINA_STATE_STOPPED != state)
                {
                    if (MiniAudioCS.MINA_STATE_PAUSED == state ||
                        REWIND_STREAMING_RESULT.Succeed_Seek == this.RewindStreaming(true)) // NOTE: 다시 오픈을 하지 않아도 되는 상황이면 (seeking) 바로 정보만 재설정 해준다.
                    {

                        MiniAudioCS.SetVolume(this.Handle, vol);
                        MiniAudioCS.SetLoop(this.Handle, looping);
                        MiniAudioCS.SetSecTime(this.Handle, secOffset);
                        MiniAudioCS.Resume(this.Handle);
#if LOG_MINA || LOG_MINA_ACTION
                        MiniAudioCS.Log(MiniAudioCS.Verbose, "MINA_PLAY_STREAMING_REWIND_HANDLE:" + this.Handle);
#endif  // LOG_MINA || LOG_MINA_ACTION
                        return this.Handle;
                    }
                }

                int ret = MiniAudioCS.MINA_INVALID_ID;
                if (this.IsPlayNoExcept)
                {
                    ret = MiniAudioCS.mina_streaming_play(this.Handle, (mina_bool)(looping ? 1 : 0), vol, secOffset);
#if LOG_MINA || LOG_MINA_ACTION
                    MiniAudioCS.Log(MiniAudioCS.Verbose, "MINA_PLAY_STREAMING_NOEXCEPT_HANDLE:" + this.Handle + ", RET:" + ret + ", LOOP:" + looping + ", SEEK:" + secOffset + ", VOL:" + vol);
#endif  // LOG_MINA || LOG_MINA_ACTION
                }
                else
                {
                    ret = MiniAudioCS.mina_streaming_play(this.Handle, (mina_bool)(looping ? 1 : 0), vol, secOffset);
#if LOG_MINA || LOG_MINA_ACTION
                    MiniAudioCS.Log(MiniAudioCS.Verbose, "MINA_PLAY_STREAMING_HANDLE:" + this.Handle + ", RET:" + ret + ", LOOP:" + looping + ", SEEK:" + secOffset + ", VOL:" + vol);
#endif  // LOG_MINA || LOG_MINA_ACTION
                    if (ret != this.Handle)
                    {
                        switch (ret)
                        {
                            case MiniAudioCS.MINA_ERR_PLAY_FULL:
                            case MiniAudioCS.MINA_ERR_PLAY_BAD_CONTEXT:
                                break;

                            case MiniAudioCS.MINA_ERR_STREAMING_ALREADY_PLAY:
                            case MiniAudioCS.MINA_ERR_STREAMING_LOAD_FAILED:
                                MiniAudioCS.Log(MiniAudioCS.Warning, "MINA_WARNING_PLAY_STREAMING:" + ret);
                                break;

                            default:
                                MiniAudioCS.Log(MiniAudioCS.Error, "MINA_ERROR(mina_streaming_play)_HANDLE:" + this.Handle + ", RET:" + ret);
                                break;
                        }
                    }
                }

                if (MiniAudioCS.MINA_OK <= ret)
                {
#if LOG_MINA || LOG_MINA_PLAY
                    MiniAudioCS.SetTextTag(this.Handle, this.Uri);
                    SB.Length = 0;
                    MiniAudioCS.Log(MiniAudioCS.Verbose, SB.Append("MINA_PLAY_STREAMING_HANDLE:").Append(this.Handle).Append(", URI:").Append(this.Uri).Append(", REMAIN:").Append(MiniAudioCS.QueryIdleCount()).Append(", PLAYMAX:").Append(MiniAudioCS.GetPlayMax()).ToString());
#endif  // LOG_MINA || LOG_MINA_PLAY

                    return this.Handle;
                }
                else
                {
                    if (MiniAudioCS.UseErrLogDataList)
                        this.AppendErrLog(ErrLogData.State.Play, ret);

#if LOG_MINA || LOG_MINA_PLAY
                        SB.Length = 0;
                        MiniAudioCS.Log(MiniAudioCS.Warning, SB.Append("MINA_WARNING_PLAY_STREAMING_RET:").Append(ret).Append(", URI:").Append(this.Uri).ToString());
#endif  // LOG_MINA || LOG_MINA_PLAY

                    return MiniAudioCS.MINA_INVALID_ID;
                }
            }
            else
            {
                // NOTE: 누군가가 내 플레이 아이디를 스탑시켰거나, 내부에서 문제 발생.
                MiniAudioCS.Log(MiniAudioCS.Warning, "MINA_WARNING_PLAY_STREAMING_INVALID_OBJECT_URI:" + this.Uri + ", HANDLE:" + this.Handle + ", UKEY:" + this.UniqueKey + ", REAL_UKEY:" + MiniAudioCS.GetUniqueKey(this.Handle));
                this.Close();
                return MiniAudioCS.MINA_INVALID_ID;
            }
        }

        return MiniAudioCS.MINA_INVALID_ID;
    }

    public static int GetPlayType(int playId)
    {
        int playType = MiniAudioCS.mina_get_play_type(playId);
        if (MINA_OK > playType)
            MiniAudioCS.Log(MiniAudioCS.Error, "MINA_ERROR(mina_get_play_type)_PLAYID:" + playId + ", RET:" + playType);

        return playType;
    }

    public static int GetUniqueKey(int playId)
    {
        int uniqueKey = MiniAudioCS.mina_get_unique_key(playId);
        if (MINA_OK > uniqueKey)
            MiniAudioCS.Log(MiniAudioCS.Error, "MINA_ERROR(mina_get_unique_key)_PLAYID:" + playId + ", RET:" + uniqueKey);

        return uniqueKey;
    }

    public static int GetPlayCount()
    {
        return MiniAudioCS.mina_get_play_count();
    }

    public static int GetPlayMax()
    {
        return MiniAudioCS.mina_get_play_max();
    }

    public static int QueryIdleCount()
    {
        return MiniAudioCS.mina_query_idle_count();
    }

    public static int GetPlayIdToBufferId(int playId)
    {
        int ret = MiniAudioCS.mina_pcm_playid_to_bufferid(playId);
        if (MINA_OK > ret)
            MiniAudioCS.Log(MiniAudioCS.Error, "MINA_ERROR(mina_pcm_playid_to_bufferid)_PLAYID:" + playId + ", RET:" + ret);

        return ret;
    }

    public static int GetPlayLimit()
    {
        int monoCnt = MiniAudioCS.mina_device_selected_play_limit_mono();
        if (MINA_OK > monoCnt)
            return 0;

        return monoCnt;
    }


    public static bool SetTextTag(int playId, string tag)
    {
        IntPtr nativeStr = tag.ToNativeMBCS(out int nativeLen);
        int ret = MiniAudioCS.mina_set_tag(playId, nativeStr, nativeLen);
        nativeStr.FreeNativeStr();

        return MINA_OK == ret;
    }

    public static string GetTextTag(int playId)
    {
        IntPtr ptr = MiniAudioCS.mina_get_tag(playId);
        int size = MiniAudioCS.mina_get_tag_size(playId);

        if (IntPtr.Zero == ptr)
            return null;

        return MiniAudioCS.ToStr(ptr, size);
    }


    public static bool SetPanning(int playId, float left_right)
    {
        int ret = MiniAudioCS.mina_panning(playId, left_right);
        if (MINA_OK > ret)
        {
            if (MINA_ERR_PLAY_NOT_PLAYING == ret)
                return false;
            else
                MiniAudioCS.Log(MiniAudioCS.Error, "MINA_ERROR(mina_panning)_PLAYID:" + playId + ", RET:" + ret);
        }

        return (MINA_OK == ret);
    }

    public static float GetPanning(int playId)
    {
        float left_right = MiniAudioCS.mina_get_panning(playId);
        if (MINA_OK > left_right)
        {
            int ret = (int)left_right;
            if (MINA_ERR_PLAY_NOT_PLAYING == ret)
                return (int)left_right;
            else
                MiniAudioCS.Log(MiniAudioCS.Error, "MINA_ERROR(mina_volume)_PLAYID:" + playId + ", RET:" + ret);
        }

        return left_right;
    }


    public static bool Pause(int playId)
    {
        int ret = MiniAudioCS.mina_pause(playId);
#if LOG_MINA || LOG_MINA_ACTION
        MiniAudioCS.Log(MiniAudioCS.Verbose, "MINA_ACTION_PAUSE_PLAYID:" + playId + ", RET:" + ret);
#endif  // LOG_MINA || LOG_MINA_ACTION
        if (MINA_OK > ret)
        {
            if (MINA_ERR_PLAY_NOT_PLAYING == ret || MINA_ERR_STREAMING_NOT_PLAYING == ret)
                return false;
            else
                MiniAudioCS.Log(MiniAudioCS.Error, "MINA_ERROR(mina_pause)_PLAYID:" + playId + ", RET:" + ret);
        }

        return (MINA_OK == ret);
    }

    public static bool Resume(int playId)
    {
        int ret = MiniAudioCS.mina_resume(playId);
#if LOG_MINA || LOG_MINA_ACTION
        MiniAudioCS.Log(MiniAudioCS.Verbose, "MINA_ACTION_RESUME_PLAYID:" + playId + ", RET:" + ret);
#endif// LOG_MINA || LOG_MINA_ACTION
        if (MINA_OK > ret)
        {
            if (MINA_ERR_PLAY_NOT_PLAYING == ret || MINA_ERR_STREAMING_NOT_PLAYING == ret)
                return false;
            else
                MiniAudioCS.Log(MiniAudioCS.Error, "MINA_ERROR(mina_resume)_PLAYID:" + playId + ", RET:" + ret);
        }

        return (MiniAudioCS.MINA_OK == ret);
    }

    public static void PauseAll(bool pause)
    {
        MiniAudioCS.mina_pause_all((mina_bool)(pause ? 1 : 0));
    }

    public static bool Stop(int playId)
    {
        return (MiniAudioCS.MINA_OK >= MiniAudioCS.mina_stop(playId));
    }

    public static int StopAll()
    {
#if LOG_CLEANUP
        MiniAudioCS.Log(MiniAudioCS.Verbose, "MiniAudio.StopAll #1");
#endif  // LOG_CLEANUP

        MiniAudioCS.StopOneshotPlayings();

#if LOG_CLEANUP
        MiniAudioCS.Log(MiniAudioCS.Verbose, "MiniAudio.StopAll #2");
#endif  // LOG_CLEANUP

        return MiniAudioCS.mina_stop_all();
    }

    public static int StopPCMAll()
    {
        return MiniAudioCS.mina_stop_pcm_all();
    }

    public static int GetState(int playId)
    {
        int state = MiniAudioCS.mina_get_state(playId);
        if (MINA_OK > state)
        {
            if (MINA_ERR_PLAY_NOT_PLAYING == state || MINA_ERR_PLAY_INVALID_PLAY == state || MINA_ERR_STREAMING_NOT_PLAYING == state)
                return state;
            else
                MiniAudioCS.Log(MiniAudioCS.Error, "MINA_ERROR(mina_get_state)_PLAYID:" + playId + ", RET:" + state);
        }

        if (0 > state)
        {
#if LOG_MINA || LOG_MINA_ACTION
            MiniAudioCS.Log(MiniAudioCS.Verbose, "MINA_ACTION_GETSTATE_INVALID_RET:" + state);
#endif// LOG_MINA || LOG_MINA_ACTION
            state = MiniAudioCS.MINA_INVALID_ID;
        }
        else if (MiniAudioCS.MINA_STATE_STREAMING_BUFFER_EMPTY == state)
        {
#if LOG_MINA || LOG_MINA_ACTION
            MiniAudioCS.Log(MiniAudioCS.Verbose, "MINA_ACTION_GETSTATE:EMPTY_BUFFER");
#endif// LOG_MINA || LOG_MINA_ACTION
            state = MiniAudioCS.MINA_STATE_PLAYING;
        }

        return state;
    }


    public static float GetNormalizedTime(int playId)
    {
        float nt = MiniAudioCS.mina_get_normalized_time(playId);
        if (MiniAudioCS.MINA_OK > nt)
        {
            int ret = (int)(nt + 0.5f);
            if (MINA_ERR_PLAY_NOT_PLAYING == ret)
                return 0;
            else
                MiniAudioCS.Log(MiniAudioCS.Error, "MINA_ERROR(mina_get_normalized_time)_PLAYID:" + playId + ", RET:" + ret);
        }

        return nt;
    }

    public static bool SetNormalizedTime(int playId, float nt)
    {
        int ret = MiniAudioCS.mina_set_normalized_time(playId, nt);
        if (MiniAudioCS.MINA_OK > ret)
        {
            if (MINA_ERR_PLAY_NOT_PLAYING == ret)
                return false;
            else
                MiniAudioCS.Log(MiniAudioCS.Error, "MINA_ERROR(mina_set_normalized_time)_PLAYID:" + playId + ", RET:" + ret);
        }

        return (MiniAudioCS.MINA_OK == ret);
    }


    public static float GetSecTimeTotal(int playId)
    {
        float st = MiniAudioCS.mina_get_sec_time_total(playId);
        if (MiniAudioCS.MINA_OK > st)
        {
            int ret = (int)st;
            if (MINA_ERR_PLAY_NOT_PLAYING == ret)
                return 0;
            else
                MiniAudioCS.Log(MiniAudioCS.Error, "MINA_ERROR(mina_get_sec_time_total)_PLAYID:" + playId + ", RET:" + ret);
        }

        return st;
    }

    public float SetTimeTotal
    {
        get
        {
            if (null != this.Info)
                return this.Info.secTimeLength;

            return 0;
        }
    }

    public static float GetSecTime(int playId)
    {
        float st = MiniAudioCS.mina_get_sec_time(playId);
        if (MiniAudioCS.MINA_OK > st)
        {
            int ret = (int)st;
            if (MINA_ERR_PLAY_NOT_PLAYING == ret)
                return 0;
            else
                MiniAudioCS.Log(MiniAudioCS.Error, "MINA_ERROR(mina_get_sec_time)_PLAYID:" + playId + ", RET:" + ret);
        }

        return st;
    }

    public static bool SetSecTime(int playId, float secOffset)
    {
        int ret = MiniAudioCS.mina_set_sec_time(playId, secOffset);
        if (MiniAudioCS.MINA_OK > ret)
        {
            if (MINA_ERR_PLAY_NOT_PLAYING == ret)
                return false;
            else
                MiniAudioCS.Log(MiniAudioCS.Error, "MINA_ERROR(mina_set_sec_time)_PLAYID:" + playId + ", RET:" + ret);
        }

        return (MiniAudioCS.MINA_OK == ret);
    }



    public static bool SetVolume(int playId, float vol)
    {
        int ret = MiniAudioCS.mina_volume(playId, vol);
        if (MiniAudioCS.MINA_OK > ret)
        {
            if (MiniAudioCS.MINA_ERR_PLAY_NOT_PLAYING != ret)
                MiniAudioCS.Log(MiniAudioCS.Error, "MINA_ERROR(mina_volume)_PLAYID:" + playId + ", RET:" + ret);
        }

        return (MiniAudioCS.MINA_OK == ret);
    }

    public static float GetVolume(int playId)
    {
        float volume = MiniAudioCS.mina_get_volume(playId);
        if (MiniAudioCS.MINA_OK > volume)
        {
            int ret = (int)volume;
            if (MINA_ERR_PLAY_NOT_PLAYING == ret)
                return (int)volume;
            else
                MiniAudioCS.Log(MiniAudioCS.Error, "MINA_ERROR(mina_get_volume)_PLAYID:" + playId + ", RET:" + ret);
        }

        return volume;
    }



    public static bool SetLoop(int playId, bool loop)
    {
        int ret = MiniAudioCS.mina_set_loop(playId, (mina_bool)(loop ? 1 : 0));
        if (MINA_OK > ret)
        {
            if (MINA_ERR_PLAY_NOT_PLAYING == ret || MINA_ERR_PLAY_ALREADY_STOPPED == ret)
                return false;
            else
                MiniAudioCS.Log(MiniAudioCS.Error, "MINA_ERROR(mina_set_loop)_PLAYID:" + playId + ", RET:" + ret);
        }

        return (MiniAudioCS.MINA_OK == ret);
    }

    public static bool IsLoop(int playId)
    {
        int ret = MiniAudioCS.mina_is_loop(playId);
        if (MINA_OK > ret)
        {
            if (MINA_ERR_PLAY_NOT_PLAYING == ret)
                return false;
            else
                MiniAudioCS.Log(MiniAudioCS.Error, "MINA_ERROR(mina_is_loop)_PLAYID:" + playId + ", RET:" + ret);
        }

        return (MiniAudioCS.MINA_OK == ret);
    }


    public enum REWIND_STREAMING_RESULT
    {
        Succeed_Initial = 0,
        Succeed_Seek,
        Succeed_Open,
        Failed,
    }
    public REWIND_STREAMING_RESULT RewindStreaming(bool reused)
    {
        if (this.Generation != MiniAudioCS.GenerationCurrent)
            return REWIND_STREAMING_RESULT.Failed;// NOTE: 이미 지워진 것.

        if (PLAYTYPE.PCMStreaming != this.PlayType)
            return REWIND_STREAMING_RESULT.Failed;

        try
        {
            if (reused && MiniAudioCS.MINA_INVALID_ID != this.Handle && MiniAudioCS.MINA_OK == MiniAudioCS.mina_is_valid(this.Handle, this.UniqueKey))
            {
                int state = MiniAudioCS.GetState(this.Handle);
                if (MiniAudioCS.MINA_ERR_STREAMING_NOT_PLAYING == state)
                    return REWIND_STREAMING_RESULT.Succeed_Initial;

                if (MiniAudioCS.MINA_OK <= state && MiniAudioCS.MINA_STATE_STOPPED != state)
                {
                    if (MiniAudioCS.MINA_STATE_PAUSED == state || MiniAudioCS.Pause(this.Handle))
                    {
                        if (MiniAudioCS.MINA_STATE_PAUSED != state)
                            state = MiniAudioCS.GetState(this.Handle);

                        //UnityEngine.MiniAudioCS.Log(MiniAudioCS.Verbose, "s = " + state);
                        if (MiniAudioCS.MINA_STATE_PAUSED == state && MiniAudioCS.SetSecTime(this.Handle, 0))
                        {
                            //UnityEngine.MiniAudioCS.Log(MiniAudioCS.Verbose, "seek");
                            //UnityEngine.MiniAudioCS.Log(MiniAudioCS.Verbose, "s2 = " + MiniAudioCS.GetState(this.Handle));

                            return REWIND_STREAMING_RESULT.Succeed_Seek;
                        }
                    }
                }
            }

            // NOTE: 재 오픈을 해야 한다.
            if (this.Open(this.Uri, true))
                return REWIND_STREAMING_RESULT.Succeed_Open;
        }
        catch (System.Exception e)
        {
            MiniAudioCS.Log(MiniAudioCS.Warning, e.ToString());
        }

        return REWIND_STREAMING_RESULT.Failed;
    }
}
