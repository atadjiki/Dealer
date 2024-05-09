using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CharacterData", order = 1)]
public class CharacterData : ScriptableObject
{
    public CharacterID ID;

    public float MovementSpeed;

    public Vector3 CameraFollowOffset;

    public GameObject Model;
}
