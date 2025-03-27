using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DearImguiSharp;
using Reloaded.Hooks.Definitions;
using Reloaded.Hooks.Definitions.Structs;
using Reloaded.Hooks.Definitions.X86;
using Reloaded.Imgui.Hook;
using Reloaded.Imgui.Hook.Direct3D9;
using Reloaded.Imgui.Hook.Direct3D9.Definitions;
using Reloaded.Imgui.Hook.Implementations;
using Reloaded.Imgui.Hook.Misc;
using SharpDX;
using SharpDX.Direct3D9;
using CallingConventions = Reloaded.Hooks.Definitions.X86.CallingConventions;
using IDirect3DDevice9 = DearImguiSharp.IDirect3DDevice9;
using IReloadedHooks = Reloaded.Hooks.ReloadedII.Interfaces.IReloadedHooks;

namespace Sonic_Heroes_AP_Client;

public class EmeraldTracker
{
    public unsafe void Draw(float outerWidth, float outerHeight, float uiScale)
    {
        var menuScreenIndex = *(int*)(Mod.ModuleBase + 0x4D69A4);
        //if (menuScreenIndex != 6)
           // return;

        var windowWidth = 700 * uiScale;
        var windowHeight = 400 * uiScale;
        var padding = 10 * uiScale;
        var trackerPos = new ImVec2.__Internal {
            x = 30,//outerWidth - windowWidth - padding, 
            y = 30//outerHeight - windowHeight - padding 
        };
        var trackerPivot = new ImVec2.__Internal { x = 0, y = 0 };
        var trackerSize = new ImVec2.__Internal { x = windowWidth, y = windowHeight };
        ImGui.__Internal.SetNextWindowPos(trackerPos, (int)ImGuiCond.Always, trackerPivot);
        ImGui.__Internal.SetNextWindowSize(trackerSize, (int)ImGuiCond.Always);
        ImGui.__Internal.PushStyleColorU32((int)ImGuiCol.WindowBg, 0xC0000000);
        ImGui.__Internal.Begin("Emerald Tracker", null, 
            (int)(ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoTitleBar));
        
        var windowPos = new ImVec2();
        ImGui.GetCursorScreenPos(windowPos);
        var windowSize = new ImVec2();
        ImGui.GetContentRegionAvail(windowSize);
        
        var imgSize = new ImVec2.__Internal { x = 64.0f, y = 64.0f };
        var imgUv0 = new ImVec2.__Internal { x = 0.0f, y = 0.0f };
        var imgUv1 = new ImVec2.__Internal { x = 1.0f, y = 1.0f };
        var tint = new ImVec4.__Internal { x = 1.0f, y = 1.0f, z = 1.0f, w = 1.0f };
        var border = new ImVec4.__Internal { x = 0, y = 0, z = 0, w = 0 };
        if (DXHook.Textures.Count != 0)
            ImGui.__Internal.Image(DXHook.Textures[0].NativePointer.ToInt32(), imgSize, imgUv0, imgUv1, tint, border);
        
        ImGui.End();
        ImGui.__Internal.PopStyleColor(1);
    }
}