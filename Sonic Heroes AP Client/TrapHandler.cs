namespace Sonic_Heroes_AP_Client;

public enum FreezeType
{
    NoFreeze,
    StageFreeze,
    FullFreeze,
}


public class TrapHandler
{
    private static bool _runningStealth;
    private static byte _previousStealth;
    private int _stealthRemaining;
    public bool IsStealthRunning => _runningStealth;
    public void HandleStealthTrap()
    {
        int oldValue = Interlocked.Exchange(ref _stealthRemaining, 5);
        if (_runningStealth)
            return;
        _runningStealth = true;
        _previousStealth = GetStealth();
        var t = new Thread(() =>
        {
            SetStealth(1);
            SoundHandler.PlaySound((int)Mod.ModuleBase, 0xE00D);
            while (Interlocked.CompareExchange(ref _stealthRemaining, 0, 0) > 0) {
                Thread.Sleep(1000);
                Interlocked.Decrement(ref _stealthRemaining);
            }
            SetStealth(_previousStealth);
            SoundHandler.PlaySound((int)Mod.ModuleBase, 0xE00E);
            _runningStealth = false;
        });
        t.Start();
    }

    public void SetStealth(byte value)
    {
        unsafe
        {
            var baseAddr = *(int*)(Mod.ModuleBase + 0x6777E4);
            *(byte*)(baseAddr + 0x25) = value;
        }
    }

    public byte GetStealth()
    {
        unsafe
        {
            var baseAddr = *(int*)(Mod.ModuleBase + 0x6777E4);
            return *(byte*)(baseAddr + 0x25);
        }
    }

    private bool isFullFrozen;
    private bool isStageFrozen;
    public bool IsFreezeRunning => isFullFrozen || isStageFrozen;
    public void HandleFreezeTrap()
    {
        StartFreeze(FreezeType.FullFreeze, 10);
    }

    private void StartFreeze(FreezeType freezeType, int seconds)
    {
        if (isStageFrozen && freezeType == FreezeType.StageFreeze)
            return;
        if (isFullFrozen && freezeType == FreezeType.FullFreeze)
            return;
        if (freezeType == FreezeType.StageFreeze)
            isStageFrozen = true;
        if (freezeType == FreezeType.FullFreeze)
            isFullFrozen = true;
        SoundHandler.PlaySound((int)Mod.ModuleBase, 0xE014);
        SetFreeze(freezeType);
        var timer = new System.Timers.Timer(seconds * 1000);
        timer.Elapsed += (sender, e) =>
        {
            SetFreeze(FreezeType.NoFreeze);
            if (freezeType == FreezeType.StageFreeze)
                isStageFrozen = false;
            else if (freezeType == FreezeType.FullFreeze)
                isFullFrozen = false;
            timer.Stop();
            timer.Dispose();
        };
        timer.AutoReset = false;
        timer.Start();
    }

    private void SetFreeze(FreezeType freezeType)
    {
        unsafe
        {
            var baseAddr = *(int*)(Mod.ModuleBase + 0x6777E4);
            *(byte*)(baseAddr + 0x1F) = (byte)freezeType;
        }
    }

    public void HandleNoSwapTrap()
    {
        SoundHandler.PlaySound((int)Mod.ModuleBase, 0xE018);
        SetNoSwap(10);
    }

    public void SetNoSwap(int seconds)
    {
        unsafe
        {
            var duration = seconds * 1000 / 15;
            var baseAddr = *(int*)(Mod.ModuleBase + 0x64C268);
            *(short*)(baseAddr + 0x204) = (short)duration;
        }
    }

    public bool IsNoSwapRunning
    {
        get
        {
            unsafe
            {
                var baseAddr = *(int*)(Mod.ModuleBase + 0x64C268);
                return *(short*)(baseAddr + 0x204) > 0;
            }
        }
    }

    private readonly int[] _charmyLines = 
    {
        1446, 485, 1602, 1636, 1971, 485,
        2055, 2079, 2103, 2116, 2259, 2296, 485, 2309, 2350, 2490,
        2710, 2755, 2832, 2844, 2878, 2941, 3169, 485, 3204, 3215,
        3220, 3230, 3287, 3321, 3355, 3373, 3485, 3738, 3762,
        3772, 3791, 3802, 3804, 3810, 3878, 4273, 4282, 4291,
        3398, 4522, 4621, 485
    };
    
    private bool _runningCharmy;
    public bool IsCharmyRunning => _runningCharmy;
    private readonly Random _random = new();
    private int _remaining;
    public void HandleCharmyTrap()
    {
        Interlocked.Add(ref _remaining, 5);
        if (_runningCharmy)
            return;
        _runningCharmy = true;
        var t = new Thread(() =>
        {
            while (Interlocked.CompareExchange(ref _remaining, 0, 0) > 0) {
                SoundHandler.PlayAFSSound((int)Mod.ModuleBase, _charmyLines[_random.Next(_charmyLines.Length)]);
                Thread.Sleep(_random.Next(5000, 15000));
                Interlocked.Decrement(ref _remaining);
            }
            _runningCharmy = false;
        });
        t.Start();
    }

    public bool Any()
    {
        return IsCharmyRunning || IsFreezeRunning || IsStealthRunning || IsNoSwapRunning;
    }
}