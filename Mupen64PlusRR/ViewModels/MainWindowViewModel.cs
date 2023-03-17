using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Mupen64PlusRR.Models.Emulation;
using Mupen64PlusRR.ViewModels.Interfaces;
using static Mupen64PlusRR.Models.Emulation.Mupen64Plus;

namespace Mupen64PlusRR.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty] private int _windowWidth = 640;
    [ObservableProperty] private int _windowHeight = 480;
    [ObservableProperty] private bool _resizable = true;
        
    public MainWindowViewModel()
    {
        var version = GetVersionInfo();
        Log(LogSources.App, MessageLevel.Info, 
            $"Loaded M64+ v{version.VersionMajor}.{version.VersionMinor}.{version.VersionPatch}");

        Mupen64Plus.StateChanged += OnMupenStateChange;
    }

    private void OnMupenStateChange(object? sender, StateChangeEventArgs args)
    {
        switch (args.Param)
        {
            case CoreParam.EmuState:
                OpenRomCommand.NotifyCanExecuteChanged();
                CloseRomCommand.NotifyCanExecuteChanged();
                break;
        }
    }

    #region Tracker properties and events

    private EmuState MupenEmuState => (EmuState) CoreStateQuery(CoreParam.EmuState);

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
        private get => _ioDialogService;
    }

    #endregion
}