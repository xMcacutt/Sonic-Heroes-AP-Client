#pragma once
#ifdef SHAPNATIVECALLER_EXPORTS
#define SHAPNATIVECALLER_API __declspec(dllexport)
#else
#define SHAPNATIVECALLER_API __declspec(dllimport)
#endif

extern "C" {
    SHAPNATIVECALLER_API void RestartLevel(int moduleBase);
    SHAPNATIVECALLER_API void ModifyLives(int moduleBase, int amount);
    SHAPNATIVECALLER_API void GiveShield(int moduleBase);
    SHAPNATIVECALLER_API void PlaySound(int moduleBase, int soundId);
    SHAPNATIVECALLER_API void PlayAFSSound(int moduleBase, int soundId);
}

class SHAPNative
{
};

