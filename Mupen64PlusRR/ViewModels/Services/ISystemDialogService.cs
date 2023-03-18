using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls;

namespace Mupen64PlusRR.ViewModels.Services;

public interface ISystemDialogService
{
    Task<string[]?> ShowOpenDialog(string title, List<FileFilter> filters, bool allowMulti = true);
    Task<string?> ShowSaveDialog(string title, List<FileFilter> filters);
}