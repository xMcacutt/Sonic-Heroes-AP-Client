using System.Numerics;

namespace Sonic_Heroes_AP_Client;

public class LevelData
{

    public class BonusKey(Team team, LevelId levelId, float x, float y, float z, int index, int offset, bool secret = false)
    {
        public Team Team = team;
        public LevelId LevelId = levelId;
        public Vector3 SpawnPos = new Vector3(x, y, z);
        public int Index = index; //in level
        public int IdOffset = offset; //for Ids
        public bool IsSecret = secret;
    }

    public class Checkpoint(Team team, LevelId levelId, float x, float y, float z, int index, int offset, bool secret = false)
    {
        public Team Team = team;
        public LevelId LevelId = levelId;
        public Vector3 SpawnPos = new Vector3(x, y, z);
        public int Index = index; //in level
        public int IdOffset = offset; //for Ids
        public bool IsSecret = secret;
    }


    public class LevelSpawn(float x, float y, float z, ushort pitch = 0x0080, SpawnMode mode = SpawnMode.Normal, ushort runningtime = 0, bool unlocked = false, bool secret = false, bool isdefault = false)
    {
        public Vector3 Pos = new Vector3(x, y, z);
        public ushort Pitch = pitch;
        public SpawnMode Mode = mode;
        public ushort RunningTime = runningtime;
        public bool Unlocked =  unlocked;
        public bool Secret = secret;
        public bool IsDefault = isdefault;

        public override string ToString()
        {
            return $"Pos: {this.Pos}, Pitch: {this.Pitch}, Mode: {this.Mode},  RunningTime: {this.RunningTime}, Unlocked: {this.Unlocked},  Secret: {this.Secret}, IsDefault: {this.IsDefault}";
        }

        public override bool Equals(object? obj)
        {
            if (obj is not LevelSpawnEntry entry)
                return false;

            return Vector3.Distance(this.Pos, entry.Pos) < 100;
        }
    }



    public class Mission
    {
        public Team Team;
        public LevelId LevelId;
        public Region Region;
        public Act Act;
        public LevelSpawn[] SpawnPositions = [];
        public int SpawnIndex;
        public bool IsBoss;
        public bool IsMultiplayer;
        public bool IsSpecial;
        public bool IsStealth;
        
        
        //Ocean Palace 2
        //Frog Forest 1 2
        //Hang Castle 2
        //Egg Fleet 1 2
    }
    
    
    //ram ID (for pointers)
    //spawn index (for spawn data)
    
    
    //id
    //index for spawn pos
    //multiplayer
    //boss
    //stealth
    //region
    //key stuff
    //checkpoint stuff
    //other sanity stuff
}