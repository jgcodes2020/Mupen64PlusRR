using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Dialogs;
using Avalonia.Interactivity;
using Mupen64PlusRR.Controls;
using Mupen64PlusRR.ViewModels;
using Mupen64PlusRR.ViewModels.Interfaces;

namespace Mupen64PlusRR.Views;

public partial class MainWindow : Window, IIODialogService
{
    public MainWindow()
    {
        InitializeComponent();
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);
        if (DataContext == null)
            return;
        // Dependency injection for view model
        var vm = (DataContext as MainWindowViewModel)!;
        vm.IODialogService = this;
        vm.VidextSurfaceService = this.Find<VidextControl>("EmulatorWindow");
    }

    public Task<string[]?> ShowOpenDialog(string title, List<FileDialogFilter> filters, bool allowMulti)
    {
        OpenFileDialog ofd = new()
        {
            Title = title,
            Filters = filters,
            AllowMultiple = allowMulti
        };
        return ofd.ShowAsync(this);
    }

    public Task<string?> ShowSaveDialog(string title, List<FileDialogFilter> filters)
    {
        SaveFileDialog sfd = new()
        {
            Title = title,
            Filters = filters
        };
        return sfd.ShowAsync(this);
    }
}