using System;
using System.Runtime.InteropServices;

namespace Mupen64PlusRR.Models.Emulation;

public static partial class Mupen64Plus
{
    public enum Error
    {
        Success = 0,
        NotInit, /* Function is disallowed before InitMupen64Plus() is called */
        AlreadyInit, /* InitMupen64Plus() was called twice */
        Incompatible, /* API versions between components are incompatible */
        InputAssert, /* Invalid parameters for function call, such as ParamValue=NULL for GetCoreParameter() */
        InputInvalid, /* Invalid input data, such as ParamValue="maybe" for SetCoreParameter() to set a BOOL-type value */
        InputNotFound, /* The input parameter(s) specified a particular item which was not found */
        NoMemory, /* Memory allocation failed */
        Files, /* Error opening, creating, reading, or writing to a file */
        Internal, /* Internal error */
        InvalidState, /* Current program state does not allow operation */
        PluginFail, /* A plugin function returned a fatal error */
        SystemFail, /* A system function call, such as an SDL or file operation, failed */
        Unsupported, /* Function call is not supported (ie, core not built with debugger) */
        WrongType /* A given input type parameter cannot be used for desired operation */
    }

    

    public enum MessageLevel
    {
        Error = 1,
        Warning,
        Info,
        Status,
        Verbose
    }


    public enum PluginType
    {
        Null = 0,
        RSP = 1,
        Graphics,
        Audio,
        Input,
        Core
    }

    public enum Command
    {
        NoOp = 0,
        RomOpen,
        RomClose,
        RomGetHeader,
        RomGetSettings,
        Execute,
        Stop,
        Pause,
        Resume,
        CoreStateQuery,
        StateLoad,
        StateSave,
        StateSetSlot,
        SendSDLKeyDown,
        SendSDLKeyUp,
        SetFrameCallback,
        TakeNextScreenshot,
        CoreStateSet,
        ReadScreen,
        Reset,
        AdvanceFrame,
        SetMediaLoader,
        NetplayInit,
        NetplayControlPlayer,
        NetplayGetVersion,
        NetplayClose,
        PifOpen,
        RomSetSettings
    }

    public enum CoreParam
    {
        EmuState = 1,
        VideoMode,
        SavestateSlot,
        SpeedFactor,
        SpeedLimiter,
        VideoSize,
        AudioVolume,
        AudioMute,
        InputGameshark,
        StateLoadComplete,
        StateSaveComplete
    }

    public enum VideoMode
    {
        None = 1,
        Windowed,
        Fullscreen
    }


    public enum EmuState
    {
        Stopped = 1,
        Running,
        Paused
    }

    public enum Type
    {
        Int = 1,
        Float,
        Bool,
        String
    }

    public static Type GetType<T>()
    {
        if (typeof(T) == typeof(int))
            return Type.Int;
        if (typeof(T) == typeof(float))
            return Type.Float;
        if (typeof(T) == typeof(bool))
            return Type.Bool;
        if (typeof(T) == typeof(string))
            return Type.String;
        throw new ArgumentException("Invalid type");
    }

    public static System.Type MapToSystemType(Type t)
    {
        return t switch
        {
            Type.Int => typeof(int),
            Type.Float => typeof(float),
            Type.Bool => typeof(bool),
            Type.String => typeof(string),
            _ => throw new ArgumentException("Invalid type", nameof(t))
        };
    }

    public enum SystemType
    {
        NTSC = 0,
        PAL,
        MPAL
    }
    
    [Flags]
    public enum VideoFlags
    {
        SupportResizing = 1
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct RomHeader
    {
        public byte init_PI_BSB_DOM1_LAT_REG; /* 0x00 */
        public byte init_PI_BSB_DOM1_PGS_REG; /* 0x01 */
        public byte init_PI_BSB_DOM1_PWD_REG; /* 0x02 */
        public byte init_PI_BSB_DOM1_PGS_REG2; /* 0x03 */
        public uint ClockRate; /* 0x04 */
        public uint PC; /* 0x08 */
        public uint Release; /* 0x0C */
        public uint CRC1; /* 0x10 */
        public uint CRC2; /* 0x14 */

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public uint[] Unknown; /* 0x18 */

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public byte[] Name; /* 0x20 */

        public uint unknown; /* 0x34 */
        public uint Manufacturer_ID; /* 0x38 */
        public ushort Cartridge_ID; /* 0x3C - Game serial number  */
        public ushort Country_code; /* 0x3E */
    }


    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct RomSettings
    {
        [MarshalAs(UnmanagedType.LPStr, SizeConst = 256)]
        public string goodname;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
        public byte[] MD5;

        public byte savetype;
        public byte status; // Rom status on a scale from 0-5. 
        public byte players; // Local players 0-4, 2/3/4 way Netplay indicated by 5/6/7. 
        public byte rumble; // 0 - No, 1 - Yes boolean for rumble support. 
        public byte transferpak; // 0 - No, 1 - Yes boolean for transfer pak support. 
        public byte mempak; // 0 - No, 1 - Yes boolean for memory pak support. 
        public byte biopak; // 0 - No, 1 - Yes boolean for bio pak support. 
    }

    public enum GLAttribute
    {
        DoubleBuffer = 1,
        BufferSize,
        DepthSize,
        RedSize,
        GreenSize,
        BlueSize,
        AlphaSize,
        SwapControl,
        MultisampleBuffers,
        MultisampleSamples,
        ContextMajorVersion,
        ContextMinorVersion,
        ContextProfileMask
    }

    public enum GLContextType
    {
        Core = 0,
        Compatibilty,
        ES
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Size2D
    {
        public uint uiWidth;
        public uint uiHeight;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe class VideoExtensionFunctions
    {
        public uint Functions;
        public delegate*<Error> VidExtFuncInit;
        public delegate*<Error> VidExtFuncQuit;
        public delegate*<Error, Span<Size2D>, Span<int>> VidExtFuncListModes;
        public delegate*<Error, Size2D, Span<int>, Span<int>> VidExtFuncListRates;
        public delegate*<Error, int, int, int, int, int> VidExtFuncSetMode;
        public delegate*<Error, int, int, int, int, int, int> VidExtFuncSetModeWithRate;
        public delegate*<IntPtr, string> VidExtFuncGLGetProc;
        public delegate*<Error, GLAttribute, int> VidExtFuncGLSetAttr;
        public delegate*<Error, GLAttribute, Span<int>> VidExtFuncGLGetAttr;
        public delegate*<Error> VidExtFuncGLSwapBuf;
        public delegate*<Error, string> VidExtFuncSetCaption;
        public delegate*<Error> VidExtFuncToggleFS;
        public delegate*<Error, int, int> VidExtFuncResizeWindow;
        public delegate*<uint> VidExtFuncGLGetDefaultFramebuffer;
    }
    
    // Custom

    public enum PlayModes : int
    {
        None,
        Stopped,
        Running,
        Paused,
    }

    public enum SavestateType : int
    {
        Mupen64Plus,
        Project64Compressed,
        Project64Uncompressed,
    }

    public enum CoreTypes
    {
        PureInterpreter,
        CachedInterpreter,
        DynamicRecompiler,
    }

    public enum DisplayTypes
    {
        Windowed,
        ExclusiveFullscreen,
    }
}