using System.Runtime.InteropServices;

namespace Sonic_Heroes_AP_Client;

public class SoundHandler
{
    [DllImport("SHAP-NativeCaller.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern int PlaySound(int moduleBase, int soundId);
    
    [DllImport("SHAP-NativeCaller.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern int PlayAFSSound(int moduleBase, int soundId);



    /*
    public static int PlaySound(int moduleBase, int soundID)
    {
        return 0;
    }
    
    public static int PlayAFSSound(int moduleBase, int soundID)
    {
        return 0;
    }
    */
}