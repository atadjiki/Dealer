using System.Collections.Generic;
using Constants;

[System.Serializable]
public class PlayerSpawnData : CharacterSpawnData
{
    PlayerSpawnData()
    {
        ModelID = Enumerations.CharacterModelID.Model_Male_Player;
    }
}

[System.Serializable]
public class NPCSpawnData : CharacterSpawnData
{
}

[System.Serializable]
public class CharacterSpawnData
{
    public Enumerations.CharacterModelID ModelID;
}
