using System.Diagnostics;

namespace Korn.Utils.WinForms;
public static class IExplorerService
{
    public static void OpenUrl(string url) => global::System.Diagnostics.Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
}