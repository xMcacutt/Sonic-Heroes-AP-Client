using System.Globalization;
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

internal class ChaotixSanityData
{
    public LevelId Level;
    public string Type;
    public int Act1Max;
    public int Act2Max;
    public long Act1Offset;
    public long Act2Offset;

    public ChaotixSanityData(LevelId level, string type, int act1Max, int act2Max, long act1Offset, long act2Offset)
    {
        Level = level;
        Type = type;
        Act1Max = act1Max;
        Act2Max = act2Max;
        Act1Offset = act1Offset;
        Act2Offset = act2Offset;
    }
}

public class LevelTracker
{
    private Dictionary<int, int> _levelMapping = new()
    {
        {0, (int)LevelId.SeasideHill},
        {1, (int)LevelId.OceanPalace},
        {2, (int)LevelId.EggHawk},
        {3, (int)LevelId.GrandMetropolis},
        {4, (int)LevelId.PowerPlant},
        {5, (int)LevelId.TeamFight1},
        {6, (int)LevelId.CasinoPark},
        {7, (int)LevelId.BingoHighway},
        {8, (int)LevelId.RobotCarnival},
        {9, (int)LevelId.RailCanyon},
        {10, (int)LevelId.BulletStation},
        {11, (int)LevelId.EggAlbatross},
        {12, (int)LevelId.FrogForest},
        {13, (int)LevelId.LostJungle},
        {14, (int)LevelId.TeamFight2},
        {15, (int)LevelId.HangCastle},
        {16, (int)LevelId.MysticMansion},
        {17, (int)LevelId.RobotStorm},
        {18, (int)LevelId.EggFleet},
        {19, (int)LevelId.FinalFortress},
        {20, (int)LevelId.EggEmperor},
        {21, (int)LevelId.MetalMadness}
    };
    
    private Dictionary<int, ChaotixSanityData> _chaotixsanityData = new()
    {
        {
            (int)LevelId.SeasideHill, 
            new ChaotixSanityData(LevelId.SeasideHill, "Crabs", 10, 20, 0x11B8, 0x11C2)
        },
        {
            (int)LevelId.OceanPalace, 
            new ChaotixSanityData(LevelId.OceanPalace, "N/A", 0, 0, 0, 0)
        },
        {
            (int)LevelId.GrandMetropolis, 
            new ChaotixSanityData(LevelId.GrandMetropolis, "Enemies", 85, 85, 0x11D6, 0x122B)
        },
        {
            (int)LevelId.PowerPlant, 
            new ChaotixSanityData(LevelId.PowerPlant, "Turtles", 3, 5, 0x1280, 0x1283)
        },
        {
            (int)LevelId.CasinoPark, 
            new ChaotixSanityData(LevelId.CasinoPark, "Rings", 200, 500, 0x1288, 0x1350)
        },
        {
            (int)LevelId.BingoHighway, 
            new ChaotixSanityData(LevelId.BingoHighway, "Chips", 10, 20, 0x1544, 0x154E)
        },
        {
            (int)LevelId.RailCanyon, 
            new ChaotixSanityData(LevelId.RailCanyon, "N/A", 0, 0, 0, 0)
        },
        {
            (int)LevelId.BulletStation, 
            new ChaotixSanityData(LevelId.BulletStation, "Capsules", 30, 50, 0x1562, 0x1580)
        },
        {
            (int)LevelId.FrogForest, 
            new ChaotixSanityData(LevelId.FrogForest, "N/A", 0, 0, 0, 0)
        },
        {
            (int)LevelId.LostJungle, 
            new ChaotixSanityData(LevelId.LostJungle, "Chao", 10, 20, 0x15B2, 0x15BC)
        },
        {
            (int)LevelId.HangCastle, 
            new ChaotixSanityData(LevelId.HangCastle, "Keys", 10, 10, 0x15d0, 0x15da)
        },
        {
            (int)LevelId.MysticMansion, 
            new ChaotixSanityData(LevelId.MysticMansion, "Torches", 60, 46, 0x15e4, 0x1620)
        },
        {
            (int)LevelId.EggFleet, 
            new ChaotixSanityData(LevelId.EggFleet, "N/A", 0, 0, 0, 0)
        },
        {
            (int)LevelId.FinalFortress, 
            new ChaotixSanityData(LevelId.FinalFortress, "Keys", 5, 10, 0x164e, 0x1653)
        },
    };

    private IntPtr _drawList;
    private float _col1Centre;
    private float _col2Centre;
    private float _circRadius;
    private float _outerWidth;
    private float _outerHeight;
    private float _uiScale;
    private float _windowWidth;
    private float _windowHeight;
    private float _windowPosX;
    private float _windowPosY;
    
    public unsafe void Draw(float outerWidth, float outerHeight, float uiScale)
    {
        var menuScreenIndex = *(int*)(Mod.ModuleBase + 0x4D69A4);
        if (Mod.ArchipelagoHandler?.SlotData == null || Mod.SanityHandler == null)
            return;
        if (menuScreenIndex != 6)
            return;

        _outerHeight = outerHeight;
        _outerWidth = outerWidth;
        _uiScale = uiScale;
        _windowPosX = _outerWidth - _windowWidth;
        _windowPosY = _outerHeight - _windowHeight;
        _windowWidth = 0.3f * _outerWidth;
        _windowHeight = 0.145f * _outerHeight;
        var trackerPos = new ImVec2.__Internal { x = _windowPosX, y = _windowPosY };
        var trackerSize = new ImVec2.__Internal { x = _windowWidth, y = _windowHeight };
        var trackerPivot = new ImVec2.__Internal { x = 0, y = 0 };
        ImGui.__Internal.SetNextWindowPos(trackerPos, (int)ImGuiCond.Always, trackerPivot);
        ImGui.__Internal.SetNextWindowSize(trackerSize, (int)ImGuiCond.Always);
        ImGui.__Internal.PushStyleColorU32((int)ImGuiCol.WindowBg, 0xC0000000);
        ImGui.__Internal.Begin("Tracker", null,
            (int)(ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoResize));
        ImGui.SetWindowFontScale(uiScale + 0.3f);
        
        _drawList = ImGui.__Internal.GetWindowDrawList();
        _col1Centre = _windowWidth / 3;
        _col2Centre = 2 * _col1Centre;
        _circRadius = 5 * uiScale;
        var textSize = new ImVec2.__Internal();
        
        var levelSelectPtr = *(IntPtr*)(Mod.ModuleBase + 0x6777B4);
        var levelIndex = *(int*)(levelSelectPtr + 0x194);
        if (levelIndex is < 0 or > 21)
        {
            ImGui.End();
            ImGui.__Internal.PopStyleColor(1);
            return;
        }
        levelIndex = _levelMapping[levelIndex];
        var storyIndex = *(int*)(levelSelectPtr + 0x194 + 0x8C);

        var gate = Mod.ArchipelagoHandler.SlotData.FindGateForLevel((LevelId)levelIndex, (Team)storyIndex);
        var gateStr = gate == null ? "?" : gate.ToString();
        ImGui.SetWindowFontScale(uiScale + 0.5f);
        ImGui.__Internal.CalcTextSize((IntPtr) (&textSize), $"{(LevelId)levelIndex} - Gate {gateStr}", null, false, -1.0f);
        ImGui.SetCursorPosX(_windowWidth / 2 - textSize.x / 2);
        ImGui.Text($"{(LevelId)levelIndex} - Gate {gateStr}");
        ImGui.SetWindowFontScale(uiScale + 0.3f);

        switch (levelIndex)
        {
            case < 2 or > 23:
                ImGui.End();
                ImGui.__Internal.PopStyleColor(1);
                return;
            case < 16:
            {
                var act1CompleteId = 0xA0 + storyIndex * 42 + (levelIndex - 2) * 2 + 0;
                var act2CompleteId = 0xA0 + storyIndex * 42 + (levelIndex - 2) * 2 + 1;
                var isAct1Complete = Mod.ArchipelagoHandler.IsLocationChecked(act1CompleteId);
                var isAct2Complete = Mod.ArchipelagoHandler.IsLocationChecked(act2CompleteId);
        
                var cursorPos = new ImVec2();
                ImGui.GetCursorScreenPos(cursorPos);

                if (Mod.ArchipelagoHandler.SlotData.Act1Enabled)
                {
                    ImGui.__Internal.CalcTextSize((IntPtr) (&textSize), "Act 1", null, false, -1.0f);
                    ImGui.SetCursorPosX(_col1Centre - textSize.x / 2);
                    ImGui.Text("Act 1");
                    DrawCircle(_drawList, _windowPosX + _col1Centre - textSize.x / 2 - _windowWidth * 0.05f, cursorPos.Y + 2 * _circRadius, _circRadius, isAct1Complete); 
                }

                if (Mod.ArchipelagoHandler.SlotData.Act2Enabled)
                {
                    if (Mod.ArchipelagoHandler.SlotData.Act1Enabled) 
                        ImGui.__Internal.SameLine(0, 0);
                    ImGui.__Internal.CalcTextSize((IntPtr) (&textSize), "Act 2", null, false, -1.0f);
                    ImGui.SetCursorPosX(_col2Centre - textSize.x / 2);
                    ImGui.Text("Act 2");
                    DrawCircle(_drawList, _windowPosX + _col2Centre + textSize.x / 2 + _windowWidth * 0.05f, cursorPos.Y + 2 * _circRadius, _circRadius, isAct2Complete); 
                }

                ImGui.GetCursorScreenPos(cursorPos);
                
                switch (storyIndex)
                {
                    case (int)Team.Sonic:
                        HandleSonicLayout((LevelId)levelIndex);
                        break;
                    case (int)Team.Dark:
                        HandleDarkLayout((LevelId)levelIndex);
                        break;
                    case (int)Team.Rose:
                        HandleRoseLayout((LevelId)levelIndex);
                        break;
                    case (int)Team.Chaotix:
                        HandleChaotixLayout((LevelId)levelIndex);
                        break;
                }

                break;
            }
            default:
                HandleBossLayout((LevelId)levelIndex);
                break;
        }
        
        if (levelIndex % 2 == 1 && levelIndex < 16)
        {
            var emeraldCompleteId = 0x148 + ((int)levelIndex - 2) / 2;
            var emeraldStageComplete = Mod.ArchipelagoHandler.IsLocationChecked(emeraldCompleteId);
            ImGui.__Internal.CalcTextSize((IntPtr) (&textSize), "Emerald Stage", null, false, -1.0f);
            ImGui.SetCursorPosX(_windowWidth / 2 - textSize.x / 2);
            ImGui.Text("Emerald Stage");
            var cursorPos = new ImVec2();
            ImGui.GetCursorScreenPos(cursorPos);
            DrawCircle(_drawList, _outerWidth - _windowWidth / 2, cursorPos.Y + _circRadius, _circRadius, emeraldStageComplete);
        }
        
        ImGui.End();
        ImGui.__Internal.PopStyleColor(1);
    }

    private unsafe void HandleSonicLayout(LevelId level)
    {

    }

    private unsafe void HandleDarkLayout(LevelId level)
    {
        if (!Mod.ArchipelagoHandler.SlotData.Act2Enabled || !Mod.ArchipelagoHandler.SlotData.Darksanity)
            return;
        
        var sanityLevelOffset = 0x150 + ((int)level - 2) * 100;
        var sanityMax = 100 / Mod.ArchipelagoHandler.SlotData.DarksanityCheckSize;
        var sanityChecked = Mod.ArchipelagoHandler.CountLocationsCheckedInRange(sanityLevelOffset, sanityLevelOffset + 100);
        HandleSanityLayout("Enemies", sanityChecked, sanityMax, _windowWidth / 2);
    }
    
    private unsafe void HandleRoseLayout(LevelId level)
    {
        if (!Mod.ArchipelagoHandler.SlotData.Act2Enabled || !Mod.ArchipelagoHandler.SlotData.Rosesanity)
            return;
        
        var sanityLevelOffset = 0x6C8 + ((int)level - 2) * 200;
        var sanityMax = 200 / Mod.ArchipelagoHandler.SlotData.RosesanityCheckSize;
        var sanityChecked = Mod.ArchipelagoHandler.CountLocationsCheckedInRange(sanityLevelOffset, sanityLevelOffset + 200);
        HandleSanityLayout("Rings", sanityChecked, sanityMax, _windowWidth / 2);
    }
    
    private unsafe void HandleChaotixLayout(LevelId level)
    {
        var chaotixData = _chaotixsanityData[(int)level];
        if (!Mod.ArchipelagoHandler.SlotData.Chaotixsanity)
            return;
        if (Mod.ArchipelagoHandler.SlotData.Act1Enabled)
        {
            var sanityMax = chaotixData.Act1Max;
            var sanityChecked = Mod.ArchipelagoHandler.CountLocationsCheckedInRange(chaotixData.Act1Offset, chaotixData.Act1Offset + chaotixData.Act1Max);
            if (level == LevelId.CasinoPark)
                sanityMax /= Mod.ArchipelagoHandler.SlotData.ChaotixsanityRingCheckSize;
            HandleSanityLayout(chaotixData.Type, sanityChecked, sanityMax, _col1Centre - 0.05f * _windowWidth);
        }
        if (Mod.ArchipelagoHandler.SlotData.Act2Enabled)
        {
            if (Mod.ArchipelagoHandler.SlotData.Act1Enabled)
                ImGui.__Internal.SameLine(0, 0);
            var sanityMax = chaotixData.Act2Max;
            var sanityChecked = Mod.ArchipelagoHandler.CountLocationsCheckedInRange(chaotixData.Act2Offset, chaotixData.Act2Offset + chaotixData.Act2Max);
            if (level == LevelId.CasinoPark)
                sanityMax /= Mod.ArchipelagoHandler.SlotData.ChaotixsanityRingCheckSize;
            HandleSanityLayout(chaotixData.Type, sanityChecked, sanityMax, _col2Centre + 0.05f * _windowWidth);
        }
    }

    private unsafe void HandleSanityLayout(string sanityText, int sanityChecked, int sanityMax, float posX)
    {
        var textSize = new ImVec2.__Internal();
        ImGui.__Internal.CalcTextSize((IntPtr) (&textSize), $"{sanityText} {sanityChecked}/{sanityMax}", null, false, -1.0f);
        ImGui.SetCursorPosX(posX - textSize.x / 2);
        ImGui.Text($"{sanityText} {sanityChecked}/{sanityMax}");
    }

    private unsafe void HandleBossLayout(LevelId level)
    {
        var cursorPos = new ImVec2();
        ImGui.GetCursorScreenPos(cursorPos);
        
        var textSize = new ImVec2.__Internal();
        
        var bossCompleteId = 0xA0 + 0 * 42 + 28 + ((int)level - 16) + 0;
        var isBossComplete = Mod.ArchipelagoHandler.IsLocationChecked(bossCompleteId);
        
        ImGui.__Internal.CalcTextSize((IntPtr) (&textSize), "Boss", null, false, -1.0f);
        ImGui.SetCursorPosX(_windowWidth / 2 - textSize.x / 2);
        ImGui.Text("Boss");
        
        ImGui.GetCursorScreenPos(cursorPos);
        DrawCircle(_drawList, _outerWidth - _windowWidth / 2, cursorPos.Y + _circRadius, _circRadius, isBossComplete); 
        
        ImGui.NewLine();
    }
    

    private void DrawCircle(IntPtr drawList, float x, float y, float radius, bool complete)
    {
        var center = new ImVec2.__Internal { x = x, y = y };
        if (complete)
            ImGui.__Internal.ImDrawListAddCircleFilled(drawList, center, radius, 0xffffffff, 16);
        else
            ImGui.__Internal.ImDrawListAddCircle(drawList, center, radius, 0xffffffff, 16, radius * 0.25f);
    }
}