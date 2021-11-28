using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

[System.Serializable]
public struct SpawnData
{
    public CharacterConstants.CharacterID ID;
    public CharacterConstants.Mode BehaviorMode;
    public List<CharacterConstants.Behavior> AllowedBehaviors;
    public List<InteractableConstants.InteractableID> AllowedInteractables;
}

[System.Serializable]
public struct SpawnGroup
{
    public SpawnData Data;
    public int Size;
    public float Initial_Delay;
    public float Delay_Between;
}
