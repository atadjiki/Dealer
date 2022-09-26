using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class PrefabManager : Singleton<PrefabManager>
{
    [Header("Character Models")]
    public Dictionary<Enumerations.CharacterModelID, CharacterModel> CharacterModels;

    [Header("MarkerGroup")]
    public Dictionary<int, GameObject> MarkerGroups;
}
