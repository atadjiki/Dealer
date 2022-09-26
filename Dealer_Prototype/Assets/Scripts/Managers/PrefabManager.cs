using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

[System.Serializable]
public struct PrefabInfo
{
    public Enumerations.PrefabID ID;
    public GameObject Prefab;
}

public class PrefabManager : Singleton<PrefabManager>
{
    [Header("Character Models")]
    public PrefabInfo[] Models;

    [Header("MarkerGroup")]
    public PrefabInfo[] MarkerGroups;

    public GameObject GetCharacterModel(Enumerations.PrefabID ID)
    {
        return SearchPrefabArray(Models, ID);
    }

    public GameObject GetMarkerGroupBySize(int size)
    {
        switch(size)
        {
            case 1: return SearchPrefabArray(MarkerGroups, Enumerations.PrefabID.MarkerGroup_1);
            case 2: return SearchPrefabArray(MarkerGroups, Enumerations.PrefabID.MarkerGroup_2);
            case 3: return SearchPrefabArray(MarkerGroups, Enumerations.PrefabID.MarkerGroup_3);
            case 4: return SearchPrefabArray(MarkerGroups, Enumerations.PrefabID.MarkerGroup_4);
            case 5: return SearchPrefabArray(MarkerGroups, Enumerations.PrefabID.MarkerGroup_5);

            default:
            return SearchPrefabArray(MarkerGroups, Enumerations.PrefabID.MarkerGroup_5);
        }
        
    }

    private GameObject SearchPrefabArray(PrefabInfo[] Array, Enumerations.PrefabID ID)
    {
        foreach (PrefabInfo info in Array)
        {
            if (info.ID == ID)
            {
                return info.Prefab;
            }
        }

        return null;
    }
}
