using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CharacterData", order = 1)]
public class CharacterData : ScriptableObject
{
    [Header("ID")]
    public CharacterID ID;

    //Stats
    [Header("Stats")]
    public int Health;
    public int ActionPoints;

    [Header("Setup Info")]
    public float MovementSpeed;
    public Vector3 CameraFollowOffset;

    [Header("Model")]
    public GameObject Model;
}
