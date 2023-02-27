using System;
using System.Collections.Generic;
using Constants;

public class Inventory : Dictionary<Enumerations.InventoryID, int>
{
    private int _capacity;

    public Inventory(int size)
    {
        _capacity = size;
    }

    public int GetCapacity()
    {
        return _capacity;
    }

    public int GetCount()
    {
        int count = 0;

        foreach(Enumerations.InventoryID ID in this.Keys)
        {
            count += this[ID];
        }

        return count;
    }

    public bool AtMaxCapacity()
    {
        return GetCount() < GetCapacity();
    }

    public static Inventory DefaultPlayerInventory()
    {
        Inventory inventory = MakeInventory(100);

        inventory[Enumerations.InventoryID.DRUGS_TYPE_A] = 0;

        return inventory;
    }

    public static Inventory DefaultSafehouseInventory()
    {
        Inventory inventory = MakeInventory(1000);

        inventory[Enumerations.InventoryID.DRUGS_TYPE_A] = 100;

        return inventory;
    }

    private static Inventory MakeInventory(int capacity)
    {
        Inventory inventory = new Inventory(capacity);

        foreach (Enumerations.InventoryID ID in Enum.GetValues(typeof(Enumerations.InventoryID)))
        {
            inventory.Add(ID, 0);
        }

        return inventory;
    }
}
