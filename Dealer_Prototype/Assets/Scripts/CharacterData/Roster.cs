using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class Roster : MonoBehaviour
{
    public Enumerations.Team Team;
    public CharacterInfo[] Characters = new CharacterInfo[CombatConstants.RosterSize];
}
