using System;
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Platform;
using Mupen64PlusRR.Models.Emulation;
using Silk.NET.OpenGL;
using Silk.NET.SDL;
using static Mupen64PlusRR.Controls.Helpers.SilkGlobals;
using Mutex = System.Threading.Mutex;
using SDL_Window = Silk.NET.SDL.Window;

namespace Mupen64PlusRR.Controls;

public unsafe class VidextControl : NativeControlHost
{
    public VidextControl()
    {
        _mutex = new Mutex();
    }
    protected override IPlatformHandle CreateNativeControlCore(IPlatformHandle parent)
    {
        var platHandle = base.CreateNativeControlCore(parent);
        _winHandle = platHandle.Handle;
        return platHandle;
    }
    
    

    private IntPtr _winHandle;
    private SDL_Window* _sdlWin;
    private void* _sdlGL;
    private Mutex _mutex;
}