using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Mupen64PlusRR.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        [ObservableProperty] private int _windowWidth;
        [ObservableProperty] private int _windowHeight;
        [ObservableProperty] private bool _resizable;
        
        public MainWindowViewModel()
        {
            
        }
        
        [RelayCommand]
        public void OpenRom()
        {
            
        }
    }
}