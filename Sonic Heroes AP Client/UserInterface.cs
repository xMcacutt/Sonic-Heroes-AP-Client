using System.Runtime.InteropServices;
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
    
    public UserInterface(IReloadedHooks hooks)
    {
        SDK.Init(hooks);
        Task.Run(CreateGui);
    }

    private async void CreateGui()
    {
        Logger = new Logger();
        await ImguiHook.Create(Render, new ImguiHookOptions()
        {
            Implementations = new List<IImguiHook>
            {
                new ImguiHookDx9(),
                new ImguiHookDx11()
            }
        }).ConfigureAwait(false);
    }
    
    private unsafe void Render()
    {
        Logger.Draw();
    }


}

public partial class Logger
{
    // Import the GetWindowRect function from user32.dll
    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
    
    // Define the RECT structure to hold the window dimensions
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    struct LogMessage {
        public string Message;
        public double Timestamp;

        public LogMessage(string message, double timestamp)
        {
            Message = message;
            Timestamp = timestamp;
        }
    };
    
    private struct ColoredSegment
    {
        public string Text;
        public uint Color;

        public ColoredSegment(string text, uint color)
        {
            Text = text;
            Color = color;
        }
    }
    
    private bool _isOpen = true;
    private List<LogMessage> VisibleMessages = new();
    private Queue<LogMessage> CachedMessages = new();
    private int _messageLength = 5;
    private int _maxMessages = 10;
    
    public unsafe void Draw()
    {
        if (!GetWindowRect(ImguiHook.WndProcHook.WindowHandle, out var rect))
            return;
        var width = rect.Right - rect.Left;
        var height = rect.Bottom - rect.Top;

        UpdateVisibleMessages();
        
        var logPos = new ImVec2.__Internal { x = 10, y = height - 500 - 10 };
        var logPivot = new ImVec2.__Internal { x = 0, y = 0 };
        var logSize = new ImVec2.__Internal { x = 800, y = 500 };
        ImGui.__Internal.SetNextWindowPos(logPos, (int)ImGuiCond.Always, logPivot);
        ImGui.__Internal.SetNextWindowSize(logSize, (int)ImGuiCond.Always);
        ImGui.__Internal.Begin("Log", null, 
            (int)(ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoBackground));
        
        var windowPos = new ImVec2();
        ImGui.GetCursorScreenPos(windowPos);
        var windowSize = new ImVec2();
        ImGui.GetContentRegionAvail(windowSize);

        var yPos = windowPos.Y + windowSize.Y;

        for (var i = VisibleMessages.Count - 1; i >= 0; --i)
        {
            var message = VisibleMessages[i];
            var xPos = windowPos.X;
            var maxWidth = windowSize.X;
            
            if (string.IsNullOrEmpty(message.Message))
                continue;
            
            var rawMessage = RemoveColorTags(message.Message);
            
            var wrappedLines = new List<string>();
            var currentLine = "";
            var totalTextHeight = 0f;
            var maxLineWidth = 0f;
            
            var words = rawMessage.Split(' ');
            foreach (var word in words)
            {
                var testLine = string.IsNullOrEmpty(currentLine) ? word : currentLine + " " + word;
                var textSize = new ImVec2.__Internal();
                ImGui.__Internal.CalcTextSize((IntPtr)(&textSize), currentLine, 
                    null, false, -1.0f);
                if (textSize.x > maxWidth && !string.IsNullOrEmpty(currentLine))
                {
                    wrappedLines.Add(currentLine);
                    maxLineWidth = float.Max(maxLineWidth, textSize.x);
                    totalTextHeight += textSize.y;
                    currentLine = word;
                }
                else
                    currentLine = testLine;
            }
            
            if (!string.IsNullOrEmpty(currentLine)) {
                wrappedLines.Add(currentLine);
                var textSize = new ImVec2.__Internal();
                ImGui.__Internal.CalcTextSize((IntPtr)(&textSize), currentLine, 
                    null, false, -1.0f);
                maxLineWidth = float.Max(maxLineWidth, textSize.x);
                totalTextHeight += textSize.y;
            }

            maxLineWidth += 5.0f;

            var messageStartY = yPos - totalTextHeight - 2.0f * wrappedLines.Count;
            
            if (wrappedLines.Count != 0)
            {
                var boxMin = new ImVec2.__Internal { x = windowPos.X, y = messageStartY };
                var boxMax = new ImVec2.__Internal { x = windowPos.X + maxLineWidth, y = yPos };
                ImGui.__Internal.ImDrawListAddRectFilled(ImGui.__Internal.GetWindowDrawList(), boxMin, boxMax, 
                    0x90000000, 0.0f, 0);
            }

            wrappedLines.Reverse();
            foreach (var line in wrappedLines) {
                var textSize = new ImVec2.__Internal();
                ImGui.__Internal.CalcTextSize((IntPtr)(&textSize), line, 
                    null, false, -1.0f);
                yPos -= textSize.y + 2.0f;
                var renderPos = new ImVec2.__Internal() { x = xPos, y = yPos };
                RenderFormattedText(line, renderPos);
            }

            yPos -= 5.0f;
        }
        
        ImGui.End();
    }

    private unsafe void RenderFormattedText(string line, ImVec2.__Internal renderPos)
    {
        var keywordColors = new Dictionary<string, uint> {
            {"Emblem", 0xff1ec9f6},
            {"Green Chaos Emerald", 0xff3dcc1c},
            {"Blue Chaos Emerald", 0xffd63b3a}, 
            {"Yellow Chaos Emerald", 0xff2fd4e2},
            {"White Chaos Emerald", 0xffe3e3e3},
            {"Cyan Chaos Emerald", 0xffcfc231},
            {"Purple Chaos Emerald", 0xffcf31aa},
            {"Red Chaos Emerald", 0xff3131cf},
            {"Extra Life", 0xff329eef},
            {"5 Rings", 0xff00e1f0},
            {"10 Rings", 0xff00e1f0},
            {"20 Rings", 0xff00e1f0},
            {"Speed Level Up", 0xffff5900},
            {"Power Level Up", 0xff1527e3},
            {"Flying Level Up", 0xff07d4ef},
            {"Shield", 0xff18a52b},
            {"Ring Trap", 0xff0000ff},
            {"Charmy Trap", 0xff0000ff},
            {"No Swap Trap", 0xff0000ff},
            {"Freeze Trap", 0xff0000ff},
            {"Stealth Trap", 0xff0000ff}
        };

        var defaultColor = 0xffffffff;
        var currentColor = defaultColor;
        var xPos = renderPos.x;
        
        var segments = ColorTagRegex().Split(line);

        foreach (var segment in segments)
        {
            if (segment.StartsWith("[color=") && segment.EndsWith("]"))
            {
                if (uint.TryParse(segment.AsSpan(7, segment.Length - 8), out var parsedColor))
                    currentColor = parsedColor;
                continue;
            }

            var lastIndex = 0;
            var matches = keywordColors.Keys
                .Select(k => new { Keyword = k, Index = segment.IndexOf(k, StringComparison.OrdinalIgnoreCase) })
                .Where(m => m.Index != -1)
                .OrderBy(m => m.Index)
                .ToList();

            foreach (var match in matches.Where(match => match.Index >= lastIndex))
            {
                if (match.Index > lastIndex)
                    DrawText(segment.Substring(lastIndex, match.Index - lastIndex), ref xPos, renderPos, currentColor);
                DrawText(match.Keyword, ref xPos, renderPos, keywordColors[match.Keyword]);
                lastIndex = match.Index + match.Keyword.Length;
            }
            
            if (lastIndex < segment.Length)
                DrawText(segment[lastIndex..], ref xPos, renderPos, currentColor);
        }
    }
    
    private unsafe void DrawText(string text, ref float xPos, ImVec2.__Internal renderPos, uint color)
    {
        var pos = new ImVec2.__Internal { x = xPos, y = renderPos.y };
        ImGui.__Internal.ImDrawListAddTextVec2(ImGui.__Internal.GetWindowDrawList(), pos, color, text, null);

        var textSize = new ImVec2.__Internal();
        ImGui.__Internal.CalcTextSize((IntPtr)(&textSize), text, null, false, -1.0f);
        xPos += textSize.x;
    }

    private string RemoveColorTags(string message)
    {
        return ColorTagRegex().Replace(message, " ");
    }

    private void UpdateVisibleMessages()
    {
        var now = DateTime.Now.ToUnixTimeStamp();

        VisibleMessages.RemoveAll(msg => (now - msg.Timestamp) > _messageLength);
        while (CachedMessages.Count != 0 && VisibleMessages.Count < _maxMessages)
            VisibleMessages.Add(CachedMessages.Dequeue());
    }

    public static void Log(string text)
    {
        var message = new LogMessage(text, DateTime.Now.ToUnixTimeStamp());
        Mod.UserInterface.Logger.CachedMessages.Enqueue(message);
    }

    [GeneratedRegex(@"(\[color=#[0-9A-Fa-f]{6,8}\])")]
    private static partial Regex ColorTagRegex();
}