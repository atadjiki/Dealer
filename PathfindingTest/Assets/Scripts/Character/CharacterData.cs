using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CharacterData", order = 1)]
public class CharacterData : ScriptableObject
{
    public CharacterID ID;

    public GameObject Navigator;

    public GameObject CameraFollow;

    public GameObject Model;
}
