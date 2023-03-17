using System.Threading;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;

namespace Mupen64PlusRR.ViewModels;

public partial class MainWindowViewModel
{
    [RelayCommand]
    public async void OpenRom(string path)
    {
    }

    [RelayCommand]
    public void CloseRom()
    {
    }

    private Thread? _emuThread;
}