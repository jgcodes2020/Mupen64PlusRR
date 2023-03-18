using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Dialogs;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using Mupen64PlusRR.Controls;
using Mupen64PlusRR.ViewModels;
using Mupen64PlusRR.ViewModels.Services;

namespace Mupen64PlusRR.Views;

public partial class MainWindow : Window, ISystemDialogService, IViewDialogService
{
    public MainWindow()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private MainWindowViewModel ViewModel => (MainWindowViewModel) DataContext!;

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);
        if (DataContext == null)
            return;
        // Dependency injection for view model
        var vm = (DataContext as MainWindowViewModel)!;
        vm.SystemDialogService = this;
        vm.VidextSurfaceService = this.Find<VidextControl>("EmulatorWindow")!;
        vm.ViewDialogService = this;
    }

    private static FilePickerFileType ToFilePickerType(FileFilter filter)
    {
        return new FilePickerFileType(filter.Name)
        {
            Patterns = filter.Patterns,
            AppleUniformTypeIdentifiers = filter.AppleTypeIds
        };
    }

    public async Task<string[]?> ShowOpenDialog(string title, List<FileFilter> filters, bool allowMulti = true)
    {
        var storagePaths = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = title,
            AllowMultiple = allowMulti,
            FileTypeFilter = filters.Select(ToFilePickerType).ToArray()
        });
        return storagePaths.Count == 0? null : storagePaths.Select(path => path.Path.LocalPath).ToArray();
    }

    public async Task<string?> ShowSaveDialog(string title, List<FileFilter> filters)
    {
        var storagePath = await StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = title,
            FileTypeChoices = filters.Select(ToFilePickerType).ToArray()
        });
        return storagePath?.Path.LocalPath;
    }

    public Task ShowSettingsDialog()
    {
        var dialog = new SettingsDialog();
        return dialog.ShowDialog(this);
    }
}