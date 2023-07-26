using System;
using System.Collections.Generic;
using Constants;
using UnityEngine;

[Serializable]
public struct EncounterSetupData
{
    public List<PlayerSpawnData> PlayerCharacters;
    public List<EnemySpawnGroupData> EnemyGroups;
}
