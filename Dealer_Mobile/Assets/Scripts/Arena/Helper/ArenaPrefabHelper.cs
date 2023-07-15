using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public static class ArenaPrefabHelper
{
    public static GameObject GetCharacterDecal()
    {
        return Resources.Load<GameObject>("Prefabs/Arena/Character/Marker/Decal_Character");
    }

    public static GameObject GetCharacterModelByTeam(CharacterConstants.Team team, CharacterConstants.CharacterType type)
    {
        switch(team)
        {
            case CharacterConstants.Team.Mafia:
                return GetMafiaCharacterPrefab(type);
            case CharacterConstants.Team.DEA:
                return GetDEACharacterPrefab(type);
            default:
                return null;

        }
    }

    public static GameObject GetMafiaCharacterPrefab(CharacterConstants.CharacterType type)
    {
        switch (type)
        {
            case CharacterConstants.CharacterType.Alternate:
                return Resources.Load<GameObject>("Prefabs/Arena/Character/Mafia/Model_Character_Mafia_Alternate");
            default:
                return Resources.Load<GameObject>("Prefabs/Arena/Character/Mafia/Model_Character_Mafia_Default");
        }
    }

    public static GameObject GetDEACharacterPrefab(CharacterConstants.CharacterType type)
    {
        switch (type)
        {
            case CharacterConstants.CharacterType.Alternate:
                return Resources.Load<GameObject>("Prefabs/Arena/Character/DEA/Model_Character_DEA_Alternate");
            default:
                return Resources.Load<GameObject>("Prefabs/Arena/Character/DEA/Model_Character_DEA_Default");
        }
    }
}
