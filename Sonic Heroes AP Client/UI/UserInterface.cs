﻿using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Archipelago.MultiClient.Net.Converters;
using DearImguiSharp;
using Reloaded.Hooks.ReloadedII.Interfaces;
using Reloaded.Imgui.Hook;
using Reloaded.Imgui.Hook.Direct3D11;
using Reloaded.Imgui.Hook.Implementations;
using SharpDX.Direct3D9;

namespace Sonic_Heroes_AP_Client;

public class UserInterface
{
    public Logger Logger;
    public EmeraldTracker EmeraldTracker;
    
    public UserInterface()
    {
        Task.Run(CreateGui);
    }

    private async void CreateGui()
    {
        Logger = new Logger();
        EmeraldTracker = new EmeraldTracker();
        await ImguiHook.Create(Render, new ImguiHookOptions()
        {
            Implementations = new List<IImguiHook>
            {
                new ImguiHookDx9(),
                new ImguiHookDx11()
            }
        }).ConfigureAwait(false);
    }
    
    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetWindowRect(IntPtr hWnd, out Logger.RECT lpRect);
    
    private unsafe void Render()
    {
        if (!GetWindowRect(ImguiHook.WndProcHook.WindowHandle, out var rect))
            return;
        var width = rect.Right - rect.Left;
        var height = rect.Bottom - rect.Top;
        const int baseWidth = 1920;
        const int baseHeight = 1080;
        var widthScale = (float)width / baseWidth;
        var heightScale = (float)height / baseHeight;
        var uiScale = widthScale < heightScale ? widthScale : heightScale;
        Logger.Draw(width, height, uiScale);
        EmeraldTracker.Draw(width, height, uiScale);
    }
}