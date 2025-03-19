using System.Reflection;
using System.Runtime.InteropServices;

namespace Sonic_Heroes_AP_Client;

public class NativeCallerHandler
{
    public static void Setup()
    {
        string extractedDllPath = ExtractEmbeddedDll("SHAP-NativeCaller.dll");
        Console.WriteLine(extractedDllPath);
        LoadDll(extractedDllPath);
    }

    static string ExtractEmbeddedDll(string dllName)
    {
        string tempPath = Path.Combine(Path.GetTempPath(), dllName);
        if (File.Exists(tempPath))
            File.Delete(tempPath);
        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Sonic_Heroes_AP_Client.SHAP-NativeCaller.dll");
        using (FileStream fs = new FileStream(tempPath, FileMode.Create, FileAccess.Write))
            stream?.CopyTo(fs);
        return tempPath;
    }

    static void LoadDll(string dllPath)
    {
        IntPtr handle = LoadLibrary(dllPath);
        if (handle == IntPtr.Zero)
        {
            throw new Exception($"Failed to load DLL: {dllPath}");
        }
    }

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr LoadLibrary(string dllToLoad);

}