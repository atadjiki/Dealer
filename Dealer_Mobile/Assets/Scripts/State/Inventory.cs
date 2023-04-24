using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

[Serializable]
public class InventoryItem
{
    public Constants.Inventory.ID ID = Constants.Inventory.ID.None;
    public int Quantity = 0;
}

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<InventoryItem> Items;

    public List<InventoryItem> GetItems()
    {
        return Items;
    }
}
