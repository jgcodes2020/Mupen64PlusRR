using System.Collections.Generic;
using System.IO;
using System.Threading;
using Avalonia.Controls;
using Avalonia.Metadata;
using CommunityToolkit.Mvvm.Input;
using Mupen64PlusRR.Models.Emulation;
using Mupen64PlusRR.Models.Helpers;
using static Mupen64PlusRR.Models.Emulation.Mupen64Plus;
namespace Mupen64PlusRR.ViewModels;

public partial class MainWindowViewModel
{
    [RelayCommand(CanExecute = "MupenIsStopped")]
    public async void OpenRom()
    {
        var paths = await IODialogService.ShowOpenDialog("Choose a ROM...", new List<FileDialogFilter>
        {
            new()
            {
                Name = "N64 ROMs",
                Extensions = { "n64", "z64", "v64" }
            }
        });
        if (paths == null)
            return;

        _emuThread = new Thread(EmulatorThreadRun);
        _emuThread.Start(paths[0]);
    }

    [RelayCommand(CanExecute = "MupenIsActive")]
    public void CloseRom()
    {
        Log(LogSources.App, MessageLevel.Info, "Stopping M64+");
        Stop();
    }

    private void EmulatorThreadRun(object? romPathObj)
    {
        string romPath = (string) romPathObj!;
        string bundlePath = GetBundledLibraryPath();
        Mupen64Plus.OpenRom(romPath);

        AttachPlugin(Path.Join(bundlePath, NativeLibHelper.AsDLL("mupen64plus-video-rice")));
        AttachPlugin(Path.Join(bundlePath, NativeLibHelper.AsDLL("mupen64plus-audio-sdl")));
        AttachPlugin(Path.Join(bundlePath, NativeLibHelper.AsDLL("mupen64plus-input-sdl")));
        AttachPlugin(Path.Join(bundlePath, NativeLibHelper.AsDLL("mupen64plus-rsp-hle")));

        Execute();

        Mupen64Plus.CloseRom();

        DetachPlugin(PluginType.Graphics);
        DetachPlugin(PluginType.Audio);
        DetachPlugin(PluginType.Input);
        DetachPlugin(PluginType.RSP);
    }

    private Thread? _emuThread;
}