#include "SHAPNative.h"

extern "C" __declspec(dllexport) void __cdecl RestartLevel(int moduleBase) {
    auto esiPtrAddr = moduleBase + 0x4D66E8;
    auto restartFuncAddr = moduleBase + 0x4520;
    _asm {
        mov esi, esiPtrAddr
        call restartFuncAddr
    }
}

extern "C" __declspec(dllexport) void __cdecl ModifyLives(int moduleBase, int amount) {
    auto lifeSetFunc = moduleBase + 0x23B60;
    _asm {
        mov ecx, 0
        mov edx, amount
        call lifeSetFunc
    }
}

extern "C" __declspec(dllexport) void __cdecl GiveShield(int moduleBase) {
    auto giveShieldFunction = moduleBase + 0x1821C0;
    _asm {
        mov ebx, 0
        call giveShieldFunction
    }
}

extern "C" __declspec(dllexport) void __cdecl PlaySound(int moduleBase, int soundId)
{
    auto playSoundFunction = moduleBase + 0x40720;
    int soundStreamAddr = *(int*)(moduleBase + 0x62F8B0);
    __asm
    {
        mov esi,soundStreamAddr
        push 00
        push 00
        mov ebx,soundId
        call playSoundFunction
    }
}

extern "C" __declspec(dllexport) void __cdecl PlayAFSSound(int moduleBase, int soundId)
{
    auto playSoundFunction = moduleBase + 0x3EC20;
    int soundStreamAddr = *(int*)(moduleBase + 0x62F8B0);
    __asm
    {
        pushad
        pushfd
        push 07
        push soundId
        mov ebx,403
        call playSoundFunction
        add esp, 8
        popfd
        popad
    }
}
