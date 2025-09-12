namespace Sonic_Heroes_AP_Client;
using Reloaded.Memory;
using Reloaded.Memory.Interfaces;

public static class AbilityHandler
{

    private const byte LockedState = 0x27;
    
    public static unsafe void SetCharState(FormationChar formationChar, bool value, bool force)
    {
        Console.WriteLine($"Running SetCharState: {formationChar} with value {value} and force {force}");
        var baseAddress = *(int*)((int)Mod.ModuleBase + 0x64B1B0 + (4 * (int)formationChar));
        var ptr = (byte*)(baseAddress + 0xF4);
        var currentState = *ptr;

        if (!value && currentState != LockedState)
        {
            *ptr = LockedState;
        }
        
        if (value && currentState == LockedState && force)
            *ptr = 0x00;
        
        
        /*
        if (!value && currentState != 0x26)
        {
            *ptr = 0x26;
            //var xcoordptr = (float*)(baseAddress + 0x114);
            var ycoordptr = (float*)(baseAddress + 0x118);
            //var zcoordptr = (float*)(baseAddress + 0x11C);
            
            
            //var xsizeptr = (float*)(baseAddress + 0x12C);
            //var ysizeptr = (float*)(baseAddress + 0x130);
            //var zsizeptr = (float*)(baseAddress + 0x134);
            //not going to work

            Task.Run(() =>
            {
                Task.Delay(1000).Wait();
                (*ycoordptr) -= 100f;
            });
        }
        if (value && currentState == 0x26 && force)
            *ptr = 0x00;
        */
    }


    public static unsafe void SetCharLevel(FormationChar formationChar, byte level)
    {
        Console.WriteLine($"Running SetCharLevel: {formationChar} with value 0x{level:x}");
        
        var baseAddress = *(int*)((int)Mod.ModuleBase + 0x64C268);
        var charlevels = (byte*)(baseAddress + 0x208 + (byte)formationChar);
        *charlevels = level;
    }
    
    public static void SetAllAbilities(bool value)
    {
        SetAllSpeedAbilities(value);
        SetAllFlyingAbilities(value);
        SetAllPowerAbilities(value);
    }
        
    public static void SetAllSpeedAbilities(bool value)
    {
        SetHomingAttack(value);
        SetTornado(value);
        SetLightDash(value);
        SetTriangleJump(value);
        SetRocketAccel(value);
        SetLightAttack(value);
    }
    
    public static void SetAllFlyingAbilities(bool value)
    {
        SetFlying(value);
        SetThundershoot(value);
        SetFlowerSting(value);
    }
    
    public static void SetAllPowerAbilities(bool value)
    {
        SetTriangleDive(value);
        SetFireDunk(value);
        SetUltimateFireDunk(value);
        SetBellyFlop(value);
        SetComboFinisher(value);
    }


    public static void SetSpeedProg(int value)
    {
        SetAllSpeedAbilities(false);
        if (value >= 1)
            SetHomingAttack(true);
        if (value >= 2)
        {
            SetTornado(true);
            SetRocketAccel(true);
        }
        if (value >= 3)
            SetLightDash(true);
        if (value >= 4)
        {
            SetTriangleJump(true);
            SetLightAttack(true);
        }
    }
    
    public static void SetFlyingProg(int value)
    {
        SetAllFlyingAbilities(false);
        if (value >= 1)
            SetThundershoot(true);
        if (value >= 2)
            SetFlying(true);
        if (value >= 3)
            SetFlowerSting(true);
    }
    
    public static void SetPowerProg(int value)
    {
        SetAllPowerAbilities(false);
        if (value >= 1)
        {
            SetFireDunk(true);
            SetBellyFlop(true);
        }
        if (value >= 2)
            SetTriangleDive(true);
        if (value >= 3)
            SetComboFinisher(true);
    }
    
    public static void SetAmyHammerHover(bool value)
    {
        var bytes = value ? new byte[] { 0x74 } : new byte[] { 0xEB };
        Memory.Instance.SafeWrite(Mod.ModuleBase + 0x1CEFD4, bytes);
    }
    
    public static void SetHomingAttack(bool value)
    {
        var bytes = value ? new byte[] { 0x85 } : new byte[] { 0x84 };
        Memory.Instance.SafeWrite(Mod.ModuleBase + 0x1CEE71, bytes);
    }
    
    public static void SetTornado(bool value)
    {
        var bytes = value ? new byte[] { 0x75 } : new byte[] { 0xEB };
        Memory.Instance.SafeWrite(Mod.ModuleBase + 0x1AF093, bytes);
    }
    public static void SetLightDash(bool value)
    {
        var bytes = value ? new byte[] { 0x75 } : new byte[] { 0xEB };
        Memory.Instance.SafeWrite(Mod.ModuleBase + 0x1A67DD, bytes);
    }
    
    public static void SetTriangleJump(bool value)
    {
        var bytes = value ? new byte[] { 0x74, 0x07 } : new byte[] { 0x90, 0x90 };
        Memory.Instance.SafeWrite(Mod.ModuleBase + 0x1A68AD, bytes);
    }
    
    public static void SetRocketAccel(bool value)
    {
        var bytes = value ? new byte[] { 0x0F, 0x84, 0xBA, 0x01, 0x00, 0x00 } : new byte[] { 0xE9, 0xBB, 0x01, 0x00, 0x00, 0x90 };
        Memory.Instance.SafeWrite(Mod.ModuleBase + 0x1A5AD6, bytes);
    }
    
    public static void SetLightAttack(bool value)
    {
        var bytes = value ? new byte[] { 0x75 } : new byte[] { 0xEB };
        Memory.Instance.SafeWrite(Mod.ModuleBase + 0x1A6838, bytes);
    }
    
    public static void SetTriangleDive(bool value)
    {
        var bytes = value ? new byte[] { 0x4F } : new byte[] { 0xCB };
        Memory.Instance.SafeWrite(Mod.ModuleBase + 0x1AED5D, bytes);
    }
    
    public static void SetFireDunk(bool value)
    {
        //Sonic and Dark
        var bytes = value ? new byte[] { 0x74 } : new byte[] { 0xEB };
        Memory.Instance.SafeWrite(Mod.ModuleBase + 0x1AF2D0, bytes);
        Memory.Instance.SafeWrite(Mod.ModuleBase + 0x1AF2E3, bytes);
        
        //Big and Vector
        bytes = value ?  new byte[] { 0x3F } : new byte[] { 0x04 };
        Memory.Instance.SafeWrite(Mod.ModuleBase + 0x1C54F2, bytes);
        Memory.Instance.SafeWrite(Mod.ModuleBase + 0x1CF5ED, bytes);
    }
    
    public static void SetUltimateFireDunk(bool value)
    {
        var bytes = value ? new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90 } : new byte[] { 0xE8, 0x0D, 0x7F, 0xFF, 0xFF };
        Memory.Instance.SafeWrite(Mod.ModuleBase + 0x1AF2FE, bytes);
    }
    
    public static void SetBellyFlop(bool value)
    {
        var bytes = value ? new byte[] { 0x0F, 0x8A, 0x2A, 0x03, 0x00, 0x00 } : new byte[] { 0xEB, 0x2B, 0x03, 0x00, 0x00, 0x90 };
        Memory.Instance.SafeWrite(Mod.ModuleBase + 0x1AF335, bytes);
    }
    
    public static void SetComboFinisher(bool value)
    {
        var bytes = value ? new byte[] { 0x0F, 0x85, 0xDE, 0x03, 0x00, 0x00 } : new byte[] { 0xE9, 0xDF, 0x03, 0x00, 0x00, 0x90 };
        Memory.Instance.SafeWrite(Mod.ModuleBase + 0x1BFD3B, bytes);
    }
    
    public static void SetFlying(bool value)
    {
        var bytes = value ? new byte[] { 0x34 } : new byte[] { 0x35 };
        Memory.Instance.SafeWrite(Mod.ModuleBase + 0x1C9C7F, bytes);
        Memory.Instance.SafeWrite(Mod.ModuleBase + 0x1CA608, bytes);
    }
    
    public static void SetThundershoot(bool value)
    {
        //Air
        var bytes = value ? new byte[] { 0x7E } : new byte[] { 0xEB };
        Memory.Instance.SafeWrite(Mod.ModuleBase + 0x1AF5EE, bytes);
        //Ground
        bytes = value ? new byte[] { 0x0F, 0x84, 0xF4, 0x00, 0x00, 0x00 } : new byte[] { 0xE9, 0xF5, 0x00, 0x00, 0x00, 0x90 };
        Memory.Instance.SafeWrite(Mod.ModuleBase + 0x1AF56B, bytes);
    }
    
    public static void SetDummyRings(bool value)
    {
        //var bytes = value ? new byte[] { 0x66, 0xC7, 0x85, 0xF4, 0x00, 0x00, 0x00, 0x4B, 0x00 } : new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, };
        //Memory.Instance.SafeWrite(Mod.ModuleBase + 0x1C866B, bytes);
        
        var bytes = value ? new byte[] { 0xE8, 0xD7, 0x7A, 0xFF, 0xFF } : new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90 };
        Memory.Instance.SafeWrite(Mod.ModuleBase + 0x1AF644, bytes);
    }
    
    
    public static void SetFlowerSting(bool value)
    {
        //in stack
        var bytes = value ? new byte[] { 0x74 } : new byte[] { 0xEB };
        Memory.Instance.SafeWrite(Mod.ModuleBase + 0x1A62EA, bytes);
        //independent
        bytes = value ? new byte[] { 0xE8, 0xDB, 0x80, 0xFF, 0xFF } : new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90 };
        Memory.Instance.SafeWrite(Mod.ModuleBase + 0x1AF660, bytes);
    }
    
    public static void SetCheeseCannon(bool value)
    {
        var bytes = value ? new byte[] { 0xE8, 0x99, 0x7A, 0xFF, 0xFF } : new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90 };
        Memory.Instance.SafeWrite(Mod.ModuleBase + 0x1AF652, bytes);
    }
    
}