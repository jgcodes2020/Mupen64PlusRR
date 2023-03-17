using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using static Mupen64PlusRR.Models.Emulation.Mupen64Plus;

namespace Mupen64PlusRR.ViewModels
{
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
        }
    }
}