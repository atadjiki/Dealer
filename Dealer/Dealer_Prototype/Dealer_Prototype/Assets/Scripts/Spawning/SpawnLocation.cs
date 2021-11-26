using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLocation : MonoBehaviour
{
    [SerializeField] private int uses = 0;

    public void IncrementUses() { uses++; }

    public int GetUses() { return uses; }
}
