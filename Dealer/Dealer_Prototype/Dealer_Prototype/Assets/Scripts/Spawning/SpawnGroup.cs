using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

[System.Serializable]
public struct SpawnGroup
{
    public CharacterConstants.CharacterID ID;
    public CharacterConstants.Behavior BehaviorMode;
    public int Size;
    public float Initial_Delay;
    public float Delay_Between;
}
