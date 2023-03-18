using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using Mupen64PlusRR.Models.Emulation;
using Mupen64PlusRR.Models.Helpers;
using Mupen64PlusRR.ViewModels.Services;

namespace Mupen64PlusRR.ViewModels;

using PluginType = Mupen64Plus.PluginType;
using LogSources = Mupen64Plus.LogSources;
using MessageLevel = Mupen64Plus.MessageLevel;

public partial class MainWindowViewModel
{
    #region Emulator thread

    private static void EmulatorThreadRun(object? romPathObj)
    {
        string romPath = (string) romPathObj!;
        string bundlePath = Mupen64Plus.GetBundledLibraryPath();
        Mupen64Plus.OpenRom(romPath);

        Mupen64Plus.AttachPlugin(Path.Join(bundlePath, NativeLibHelper.AsDLL("mupen64plus-video-rice")));
        Mupen64Plus.AttachPlugin(Path.Join(bundlePath, NativeLibHelper.AsDLL("mupen64plus-audio-sdl")));
        Mupen64Plus.AttachPlugin(Path.Join(bundlePath, NativeLibHelper.AsDLL("mupen64plus-input-sdl")));
        Mupen64Plus.AttachPlugin(Path.Join(bundlePath, NativeLibHelper.AsDLL("mupen64plus-rsp-hle")));

        Mupen64Plus.Execute();

        Mupen64Plus.CloseRom();

        Mupen64Plus.DetachPlugin(PluginType.Graphics);
        Mupen64Plus.DetachPlugin(PluginType.Audio);
        Mupen64Plus.DetachPlugin(PluginType.Input);
        Mupen64Plus.DetachPlugin(PluginType.RSP);
    }

    private Thread? _emuThread;

    #endregion

    #region Emulator commands

    [RelayCommand(CanExecute = nameof(MupenIsStopped))]
    private async void OpenRom()
    {
        var paths = await SystemDialogService.ShowOpenDialog("Choose a ROM...", new List<FileFilter>
        {
            new(Name: "N64 ROMs", Patterns: new[] {"*.n64", "*.v64", "*.z64"}, AppleTypeIds: Array.Empty<string>())
        }, allowMulti: false);
        if (paths == null)
            return;

        _emuThread = new Thread(EmulatorThreadRun);
        _emuThread.Start(paths[0]);
    }

    [RelayCommand(CanExecute = nameof(MupenIsActive))]
    private void CloseRom()
    {
        Mupen64Plus.Log(LogSources.App, MessageLevel.Info, "Stopping M64+");
        Mupen64Plus.Stop();
    }

    [RelayCommand(CanExecute = nameof(MupenIsActive))]
    private void ResetRom()
    {
        Mupen64Plus.Reset();
    }

    [RelayCommand(CanExecute = nameof(MupenIsActive))]
    private void PauseOrResume()
    {
        if (MupenIsPaused)
            Mupen64Plus.Resume();
        else
            Mupen64Plus.Pause();
    }

    [RelayCommand(CanExecute = nameof(MupenIsActive))]
    private void FrameAdvance()
    {
        Mupen64Plus.AdvanceFrame();
    }

    [RelayCommand(CanExecute = nameof(MupenIsActive))]
    private async void LoadStateFromFile()
    {
        var paths = await SystemDialogService.ShowOpenDialog("Load state...", new List<FileFilter>
        {
            new(Name: "N64 Savestates", Patterns: new[] {".st", ".savestate"}, Array.Empty<string>())
        }, allowMulti: false);
        if (paths == null)
            return;
        
        Mupen64Plus.LoadStateFromFile(paths[0]);
    }

    [RelayCommand(CanExecute = nameof(MupenIsActive))]
    private async void SaveStateToFile()
    {
        var path = await SystemDialogService.ShowSaveDialog("Save state...", new List<FileFilter>
        {
            new(Name: "Mupen64Plus savestate", Patterns: new[] {".st", ".savestate"}, AppleTypeIds: Array.Empty<string>()),
            new(Name: "Project64 Savestate (compressed)", Patterns: new[] {".pj64c.st"}, AppleTypeIds: Array.Empty<string>()),
            new(Name: "Project64 Savestate (uncompressed)", Patterns: new[] {".pj64.st"}, AppleTypeIds: Array.Empty<string>()),
        });
        if (path == null)
            return;

        var nameParts = Path.GetFileName(path).Split('.');
        var saveType = nameParts[^1] switch
        {
            "savestate" => Mupen64Plus.SavestateType.Mupen64Plus,
            "st" => nameParts[^2] switch
            {
                "pj64c" => Mupen64Plus.SavestateType.Project64Compressed,
                "pj64" => Mupen64Plus.SavestateType.Project64Uncompressed,
                _  => Mupen64Plus.SavestateType.Mupen64Plus
            },
            _ => Mupen64Plus.SavestateType.Mupen64Plus
        };
        
        Mupen64Plus.SaveStateToFile(path, saveType);
    }

    #endregion

    #region View commands

    [RelayCommand]
    private void ShowSettings()
    {
        ViewDialogService.ShowSettingsDialog();
    }

    #endregion
}