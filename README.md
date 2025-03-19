![AP Banner.png](AP%20Banner.png)

## [APWorld](https://github.com/Ethicallogic-Archipelago/SonicHeroesArchipelago) | [![Game Banana]("https://gamebanana.com/mods/embeddables/582396?type=large")](https://gamebanana.com/mods/582396)

## Getting Started

You need to know which version of Sonic Heroes you have.
This implementation is only compatible with the PC version of the game.
There are two versions of the PC release. One of these requires the disc to be inserted for the game to run, the other does not.

If you know you have the NoCD release, you can skip the following section and go straight to [Setup](#setup)

### For the CD Release

The CD release of the game uses a digital rights management system called safedisc to ensure you have the disc on booting the game.
Unfortunately, the driver allowing the code to be re-injected into the game on launch is removed from windows due to a vulnerability.
To get the game running on PC, you'll need [SafeDiscShim](https://github.com/RibShark/SafeDiscShim/releases). This app runs in the background and runs the code to re-inject the code into the game on launch.

Once you've installed SafeDiscShim you should be able to launch the game with the disc inserted. Keep in mind that in the setup section you'll need to pay attention to the extra steps to run the mod

## Setup

You'll need

* A legally obtained copy of the PC Version of Sonic Heroes
* [SafeDsicShim](https://github.com/RibShark/SafeDiscShim/releases) \[CD RELEASE ONLY\]
* [Reloaded-II Mod Loader](https://github.com/Reloaded-Project/Reloaded-II)
* The [APWorld](https://github.com/Ethicallogic-Archipelago/SonicHeroesArchipelago) \[FOR GENERATION\] 

First, follow the setup for [Reloaded Mod Loader](https://github.com/Reloaded-Project/Reloaded-II)

Once you've set up the Sonic Heroes application, go to the add mods page in Reloaded-II, search for Sonic Heroes Archipelago Client, and install the mod.

After a world is hosted, in Reloaded, enable the Mod and click Configure. A UI will open up and you can set a server, port, slot name and password (if required).

Finally, launch the game through Reloaded and if the mod is enabled and the correct settings are set the Mod will connect to AP.

### FOR CD RELEASE

If you are running using the SafeDiscShim setup, you will not be able to load the mods directly. Instead, you'll need to launch first then inject after.

To do this, go to the application in Reloaded-II and select edit application. Expand the `Advanced Tools & Options` dropdown and select `Don't Inject Loader`.

When you launch the game, you'll see the game appear in the processes list in Reloaded-II. Click the listing and press `Inject` to load the mods.

## On Launch

Once you have the game booting, check the log that appears to ensure you're connected. If you're not, check your mod configuration and try again.

If you connect successfully, you should then create a save file. You'll have to manage your saves manually but you have 99 slots so it shouldn't be too hard.

At the start of an AP session, either start a new save or delete an old one and start a new one in its slot. You can reselect this slot without any issues if you need to relaunch.

The entire session is handled through the level select menu under `Challenge`. You will not be able to select any other options if connected to AP.

## Sonic Heroes

### Gates 

The sonic heroes implementation follows similar patterns to SA2. The world is split into gate as specified in the options.
Each gate has a random boss from the pool of Heroes bosses. Bosses are unlocked by collecting a set number of emblems.
When a boss is unlocked, it will be available for all active teams. On completion of the boss with any of the available teams, the checks for all available teams will be sent for that boss.

The number of levels in a gate is determined by the number of teams you have active and the emblem cost is determined by a percentage set in the yaml.

The final gate will always have the boss as `Metal Madness`. Upon completion, your client will send Release status.

### Levels

Each level in the game has some base checks. 
There is a mission 1 completion check (referred to as Act 1), 
a mission 2 completion check (referred to as Act 2), 
and, if the stage is the second in the zone, a check for completing the emerald stage.
The emerald stage completion check will always be a priority location.

### Emeralds

Emeralds can be turned on as a completion condition. When this is done, all seven chaos emeralds must be found throughout the multiworld for `Metal Madness` to open.
The emeralds that have been found are tracked through the level select.

### Teams

Each team plays slightly differently in archipelago based on the sanity options available.

#### Team Sonic

Team Sonic is the most basic team with no sanity options. They're a great pick for some standard gameplay with no fuss.

#### Team Dark

Team Dark adds enemysanity to mission 2 for each level. For every X enemies killed (specified in yaml), you'll send a check.

#### Team Rose

Team Rose brings us ringsanity. For every X rings (specified in yaml) collected in the second mission of each level, you'll send a check.
This scales the weight of ring items in the pool to avoid cascading checks finishing the mission immediately for low ring intervals.

#### Team Chaotix

I see you Shadow the Hedgehog players... and I respect you.
So this adds a check to every single mission objective in the Chaotix treasure hunt, enemy killing, and ring missions... all of them.

### Items

The following items can be obtained in AP
- Emblems
- Chaos Emeralds
- Extra Life
- 5 Rings
- 10 Rings
- 20 Rings
- Shield
- Speed Level Up
- Power Level Up
- Flying Level Up
- Team Level Up

### Traps

The following traps can be activated in AP
- Stealth Trap
- Freeze Trap
- No Swap Trap
- Ring Trap
- Charmy Trap
