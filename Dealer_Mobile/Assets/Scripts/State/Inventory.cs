using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants.Inventory;

[Serializable]
public class DrugContainer : Dictionary<Drugs.ID, int>
{
    public DrugContainer()
    {
        foreach(Drugs.ID ID in Enum.GetValues(typeof(Drugs.ID)))
        {
            if(ID != Drugs.ID.None)
            {
                this.Add(ID, 0);
            }
        }
    }
}

public class Inventory : MonoBehaviour
{
    private DrugContainer PlayerStash;

    private void Awake()
    {
        PlayerStash = new DrugContainer();
    }

    public DrugContainer GetDrugStash()
    {
        return PlayerStash;
    }
}
