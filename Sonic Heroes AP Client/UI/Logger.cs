using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Archipelago.MultiClient.Net.Converters;
using DearImguiSharp;
using Reloaded.Imgui.Hook;

namespace Sonic_Heroes_AP_Client;

public partial class Logger
{
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
    
    private struct Word
    {
        public string Text;
        public uint Color;

        public Word(string text, uint color)
        {
            Text = text;
            Color = color;
        }
    }
    
    private static readonly Dictionary<string, uint> KeywordColors = new() {
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

    private bool _isOpen = true;
    private List<LogMessage> VisibleMessages = new();
    private Queue<LogMessage> CachedMessages = new();
    private int _messageLength = 5;
    private int _maxMessages = 10;
    private static Regex _keywordPattern;

    public Logger()
    {
        var patterns = KeywordColors.Keys.Select(Regex.Escape);
        var combinedPattern = string.Join("|", patterns);
        _keywordPattern = new Regex(combinedPattern, RegexOptions.IgnoreCase);
    }
    
    public unsafe void Draw(float outerWidth, float outerHeight, float uiScale)
    {
        UpdateVisibleMessages();
        
        var windowWidth = 900 * uiScale;
        var windowHeight = 800 * uiScale;
        var padding = 10 * uiScale;
        var logPos = new ImVec2.__Internal {
            x = padding, 
            y = outerHeight - windowHeight - padding 
        };
        var logPivot = new ImVec2.__Internal { x = 0, y = 0 };
        var logSize = new ImVec2.__Internal { x = windowWidth, y = windowHeight };
        ImGui.__Internal.SetNextWindowPos(logPos, (int)ImGuiCond.Always, logPivot);
        ImGui.__Internal.SetNextWindowSize(logSize, (int)ImGuiCond.Always);
        ImGui.__Internal.Begin("Log", null, 
            (int)(ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoBackground));
        ImGui.SetWindowFontScale(uiScale + 0.3f);
        
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
            
            var lines = message.Message.Split('\n');
            foreach (var line in lines)
            {
                var matches = _keywordPattern.Matches(line);
                var words = new List<Word>();
                var currentColor = 0xffffffff;
                var currentWord = "";
                var currentMatchLength = 0;
                var currentMatchIndex = 0;
                for (var cIndex = 0; cIndex < line.Length; cIndex++)
                {
                    var match = matches.FirstOrDefault(x => x.Index == cIndex);
                    if (match != null)
                    {
                        currentColor = KeywordColors[match.Value];
                        currentMatchLength = match.Length;
                        currentMatchIndex = cIndex;
                    }
                    currentWord += line[cIndex];
                    if (line[cIndex] != ' ' && cIndex != line.Length - 1) 
                        continue;
                    words.Add(new Word(currentWord.Trim(), currentColor));
                    if (cIndex >= currentMatchIndex + currentMatchLength)
                        currentColor = 0xffffffff;
                    currentWord = "";
                }

                var currentLine = "";
                var currentLineWords = new List<Word>();
                var wrappedLines = new List<List<Word>>();
                float maxLineWidth = 0;
                
                foreach (var word in words)
                {
                    var testLine = string.IsNullOrEmpty(currentLine) ? word.Text : currentLine + " " + word.Text;
                    var textSize = new ImVec2.__Internal();
                    ImGui.__Internal.CalcTextSize((IntPtr) (&textSize), testLine, null, false, -1.0f);
                    
                    if (textSize.x <= maxWidth || string.IsNullOrEmpty(currentLine))
                    {
                        currentLineWords.Add(word);
                        currentLine = testLine;
                    }
                    else
                    {
                        wrappedLines.Add(new List<Word>(currentLineWords));
                        var lineTextSize = new ImVec2.__Internal();
                        ImGui.__Internal.CalcTextSize((IntPtr) (&lineTextSize), GetWordString(currentLineWords), null, false, -1.0f);
                        maxLineWidth = float.Max(maxLineWidth, lineTextSize.x);
                        
                        currentLineWords.Clear();
                        currentLineWords.Add(word);
                        currentLine = word.Text;
                    }
                }

                if (currentLineWords.Count > 0)
                {
                    wrappedLines.Add(currentLineWords);
                    var textSize = new ImVec2();
                    ImGui.CalcTextSize(textSize, GetWordString(currentLineWords), null, false, -1.0f);
                    maxLineWidth = float.Max(maxLineWidth, textSize.X);
                }
                
                for (var lineIndex = wrappedLines.Count - 1; lineIndex >= 0; lineIndex--)
                {
                    var wrappedLine = wrappedLines[lineIndex];
                    var textSize = new ImVec2.__Internal();
                    ImGui.__Internal.CalcTextSize((IntPtr) (&textSize), GetWordString(wrappedLine), 
                        null, false, -1.0f);
                    
                    var boxMin = new ImVec2.__Internal { x = windowPos.X - 1, y = yPos - textSize.y - 1 };
                    var boxMax = new ImVec2.__Internal { x = maxLineWidth + windowPos.X + 1, y = yPos + 1 };
                    ImGui.__Internal.ImDrawListAddRectFilled(ImGui.__Internal.GetWindowDrawList(), boxMin, boxMax, 
                        0xB0000000, 0.0f, 0);
                    
                    yPos -= textSize.y + 2;
                    xPos = windowPos.X;
                    var renderPos = new ImVec2.__Internal() { x = xPos, y = yPos };
                    
                    foreach (var word in wrappedLine)
                        DrawText(word.Text + " ", ref xPos, renderPos, word.Color);
                }
            }
            yPos -= 5.0f;
        }
        ImGui.End();
    }

    private string GetWordString(IEnumerable<Word> words)
    {
        var line = "";
        foreach (var word in words)
            line += word.Text + " ";
        return line;
    }
    
    private unsafe void DrawText(string text, ref float xPos, ImVec2.__Internal renderPos, uint color)
    {
        var pos = new ImVec2.__Internal { x = xPos, y = renderPos.y };
        ImGui.__Internal.ImDrawListAddTextVec2(ImGui.__Internal.GetWindowDrawList(), pos, color, text, null);

        var textSize = new ImVec2.__Internal();
        ImGui.__Internal.CalcTextSize((IntPtr)(&textSize), text, null, false, -1.0f);
        xPos += textSize.x;
    }

    private void UpdateVisibleMessages()
    {
        var now = DateTime.Now.ToUnixTimeStamp();
        VisibleMessages.RemoveAll(msg => ((now - msg.Timestamp) > _messageLength));
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