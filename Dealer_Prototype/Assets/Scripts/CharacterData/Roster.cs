using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class Roster : MonoBehaviour
{
    public Enumerations.Team Team;
    public CharacterInfo[] Characters;

    public int GetSize() { return Characters.Length; }

    private Enumerations.ArenaSide ArenaSide;
    public Enumerations.ArenaSide GetSide() { return ArenaSide; }
    public void SetSide(Enumerations.ArenaSide side) { ArenaSide = side; }
}
