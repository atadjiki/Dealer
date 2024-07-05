using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CharacterDefinition", order = 1)]
public class CharacterDefinition : ScriptableObject
{
    public CharacterID ID;

    public List<GameObject> AllowedModels;

  //  public WeaponID[] AllowedWeapons;

    public int BaseHealth;

    public int BaseActionPoints;

    public int MovementRange;

    public float MovementSpeed;

    public MovementType AllowedMovements;

    public int CritChance;

    public int Aim;

    public int Defence;

    public GameObject GetRandomModelPrefab()
    {
        return AllowedModels[Random.Range(0, AllowedModels.Count)];
    }

    public bool RollCritChance()
    {
        float roll = Random.Range(0, 99);

        if (CritChance > roll)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
