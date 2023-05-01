using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants.Inventory;

[Serializable]
public class DrugItem
{
    public Drugs.ID ID = Drugs.ID.None;
    public Drugs.Grade Grade = Drugs.Grade.None;
    public int Quantity = 0;
}

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<DrugItem> DrugItems;

    public List<DrugItem> GetDrugStash()
    {
        return DrugItems;
    }
}
