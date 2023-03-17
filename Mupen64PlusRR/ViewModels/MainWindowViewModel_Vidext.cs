
using System;
using System.Runtime.InteropServices;
using Mupen64PlusRR.Models.Emulation;
using Mupen64PlusRR.Models.Interfaces;

namespace Mupen64PlusRR.ViewModels;
using PluginType = Mupen64Plus.PluginType;
using LogSources = Mupen64Plus.LogSources;
using MessageLevel = Mupen64Plus.MessageLevel;
using Error = Mupen64Plus.Error;
public unsafe partial class MainWindowViewModel : IVideoExtensionService
{
    public Error VidextInit()
    {
        try
        {
            VidextSurfaceService.InitWindow();
            return Error.Success;
        }
        catch (Exception)
        {
            return Error.Internal;
        }
    }

    public Error VidextQuit()
    {
        try
        {
            VidextSurfaceService.QuitWindow();
            return Error.Success;
        }
        catch (Exception)
        {
            return Error.Internal;
        }
    }

    public Error VidextListFullscreenModes(Span<Mupen64Plus.Size2D> sizes, ref int len)
    {
        // FUTURE: support fullscreen
        return Error.Unsupported;
    }

    public Error VidextListFullscreenRates(Mupen64Plus.Size2D size, Span<int> output, ref int len)
    {
        // FUTURE: support fullscreen
        return Error.Unsupported;
    }

    public Error VidextSetVideoMode(int width, int height, int bpp, Mupen64Plus.VideoMode mode, Mupen64Plus.VideoFlags flags)
    {
        // FUTURE: support fullscreen and resizing
        try
        {
            if (mode != Mupen64Plus.VideoMode.Windowed || flags != 0)
                return Error.Unsupported;

            WindowWidth = width;
            WindowHeight = height + MenuHeight;

            VidextSurfaceService.CreateWindow(width, height, bpp);
            return Error.Success;
        }
        catch (Exception)
        {
            return Error.Internal;
        }
    }

    public Error VidextSetVideoModeWithRate(int width, int height, int refreshRate, int bpp, Mupen64Plus.VideoMode mode, Mupen64Plus.VideoFlags flags)
    {
        return Error.Unsupported;
    }

    public IntPtr VidextGLGetProcAddress(byte* symbol)
    {
        return VidextSurfaceService.GetProcAddress((IntPtr) symbol);
    }

    public Error VidextGLSetAttr(Mupen64Plus.GLAttribute attr, int value)
    {
        try
        {
            VidextSurfaceService.SetGLAttribute(attr, value);
            return Error.Success;
        }
        catch (Exception)
        {
            return Error.Internal;
        }
    }

    public Error VidextGLGetAttr(Mupen64Plus.GLAttribute attr, out int value)
    {
        try
        {
            value = VidextSurfaceService.GetGLAttribute(attr);
            return Error.Success;
        }
        catch (Exception)
        {
            value = 0;
            return Error.Internal;
        }
    }

    public Error VidextResizeWindow(int width, int height)
    {
        // FUTURE: support resizing/fullscreen
        return Error.Unsupported;
    }

    public Error VidextSetCaption(string str)
    {
        // This is absolutely useless!
        return Error.Success;
    }

    public Error VidextToggleFullscreen()
    {
        // FUTURE: support resizing/fullscreen
        return Error.Unsupported;
    }

    public Error VidextSwapBuffers()
    {
        try
        {
            VidextSurfaceService.SwapBuffers();
            return Error.Success;
        }
        catch (Exception)
        {
            return Error.Internal;
        }
    }

    public uint VidextGLGetDefaultFramebuffer()
    {
        return 0;
    }
}