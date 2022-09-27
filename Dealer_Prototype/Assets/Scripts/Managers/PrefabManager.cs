using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

[System.Serializable]
public struct PrefabInfo
{
    public Enumerations.AssetID ID;
    public GameObject Prefab;
}

public class PrefabManager : Singleton<PrefabManager>
{
    [Header("UI Elements")]
    public PrefabInfo[] UIElements;

    [Header("Weapon Models")]
    public PrefabInfo[] Weapons;

    [Header("Character Models")]
    public PrefabInfo[] Characters;

    [Header("MarkerGroup")]
    public PrefabInfo[] MarkerGroups;

    public GameObject GetUIElement(Enumerations.UIID ID)
    {
        return SearchPrefabArray(ID);
    }

    public GameObject GetWeaponModel(Enumerations.WeaponID ID)
    {
        return SearchPrefabArray(ID);
    }

    public GameObject GetCharacterModel(Enumerations.CharacterModelID ID)
    {
        return SearchPrefabArray(ID);
    }

    public GameObject GetMarkerGroupBySize(int size)
    {
        switch(size)
        {
            case 1: return SearchPrefabArray(Enumerations.MarkerGroupID.Group_1);
            case 2: return SearchPrefabArray(Enumerations.MarkerGroupID.Group_2);
            case 3: return SearchPrefabArray(Enumerations.MarkerGroupID.Group_3);
            case 4: return SearchPrefabArray(Enumerations.MarkerGroupID.Group_4);
            case 5: return SearchPrefabArray(Enumerations.MarkerGroupID.Group_5);

            default:
            return SearchPrefabArray(Enumerations.MarkerGroupID.Group_5);
        }
        
    }

    private GameObject SearchPrefabArray(Enumerations.UIID ID)
    {
        return SearchPrefabArray(UIElements, ID.ToString());
    }

    private GameObject SearchPrefabArray(Enumerations.WeaponID ID)
    {
        return SearchPrefabArray(Weapons, ID.ToString());
    }

    private GameObject SearchPrefabArray(Enumerations.CharacterModelID ID)
    {
        return SearchPrefabArray(Characters, ID.ToString());
    }

    private GameObject SearchPrefabArray(Enumerations.MarkerGroupID ID)
    {
        return SearchPrefabArray(MarkerGroups, ID.ToString());
    }

    private GameObject SearchPrefabArray(PrefabInfo[] array, string ID)
    {
        foreach(PrefabInfo info in array)
        {
            if(info.ID.ToString() == ID)
            {
                return info.Prefab;
            }
        }

        return null;
    }
}
