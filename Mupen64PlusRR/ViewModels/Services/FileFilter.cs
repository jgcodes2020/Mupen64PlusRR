using System.Collections.Generic;

namespace Mupen64PlusRR.ViewModels.Services;

public record FileFilter(string Name, IReadOnlyList<string> Patterns, IReadOnlyList<string> AppleTypeIds);