using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class Roster : MonoBehaviour
{
    public Enumerations.Team Team;
    public CharacterInfo[] Characters;

    public int GetSize() { return Characters.Length; }

    [HideInInspector]
    public Enumerations.ArenaSide ArenaSide;
}
