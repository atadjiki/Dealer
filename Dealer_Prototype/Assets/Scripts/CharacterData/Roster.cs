using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class Roster : MonoBehaviour
{
    public Enumerations.Team Team;
    public CharacterInfo[] Characters;

    public int GetRosterSize()
    {
        return Characters.Length;
    }
}
