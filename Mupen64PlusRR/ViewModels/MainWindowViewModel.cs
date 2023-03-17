using System;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using Mupen64PlusRR.Models.Emulation;
using Mupen64PlusRR.Models.Interfaces;
using Mupen64PlusRR.ViewModels.Interfaces;

namespace Mupen64PlusRR.ViewModels;

using LogSources = Mupen64Plus.LogSources;
using MessageLevel = Mupen64Plus.MessageLevel;
using EmuState = Mupen64Plus.EmuState;
using CoreParam = Mupen64Plus.CoreParam;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty] private double _windowWidth = 640;
    [ObservableProperty] private double _windowHeight = 480;
    [ObservableProperty] private double _menuHeight;
    [ObservableProperty] private bool _resizable = true;

    public MainWindowViewModel()
    {
        var version = Mupen64Plus.GetVersionInfo();
        Mupen64Plus.Log(LogSources.App, MessageLevel.Info,
            $"Loaded M64+ v{version.VersionMajor}.{version.VersionMinor}.{version.VersionPatch}");

        Mupen64Plus.StateChanged += OnMupenStateChange;
        Mupen64Plus.OverrideVidExt(this.ToVidextStruct());
    }

    private void OnMupenStateChange(object? sender, Mupen64Plus.StateChangeEventArgs args)
    {
        switch (args.Param)
        {
            case CoreParam.EmuState:
                Dispatcher.UIThread.Post(() =>
                {
                    OpenRomCommand.NotifyCanExecuteChanged();
                    CloseRomCommand.NotifyCanExecuteChanged();
                });
                break;
        }
    }

    #region Tracker properties and events

    private EmuState MupenEmuState => (EmuState) Mupen64Plus.CoreStateQuery(CoreParam.EmuState);

    public bool MupenIsStopped => MupenEmuState is EmuState.Stopped;
    public bool MupenIsRunning => MupenEmuState is EmuState.Running;
    public bool MupenIsPaused => MupenEmuState is EmuState.Paused;
    public bool MupenIsActive => MupenEmuState is EmuState.Running or EmuState.Paused;

    #endregion

    #region Service properties

    private IIODialogService? _ioDialogService;

    public IIODialogService IODialogService
    {
        set => _ioDialogService ??= value;
        private get => _ioDialogService ?? throw new NullReferenceException("No registered IIODialogService");
    }

    private IVidextSurfaceService? _vidextSurfaceService;

    public IVidextSurfaceService VidextSurfaceService
    {
        set => _vidextSurfaceService ??= value;
        private get => _vidextSurfaceService ?? throw new NullReferenceException("No registered IVidextSurfaceService");
    }

    #endregion
}