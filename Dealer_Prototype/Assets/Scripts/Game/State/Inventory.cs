using System;
using System.Collections.Generic;
using Constants;

public class Inventory : Dictionary<Enumerations.InventoryID, int>
{
    public static Inventory DefaultPlayerInventory()
    {
        Inventory inventory = MakeInventory();

        inventory[Enumerations.InventoryID.MONEY] = 0;
        inventory[Enumerations.InventoryID.DRUGS_TYPE_A] = 0;

        return inventory;
    }

    public static Inventory DefaultSafehouseInventory()
    {
        Inventory inventory = MakeInventory();

        inventory[Enumerations.InventoryID.MONEY] = 1000;
        inventory[Enumerations.InventoryID.DRUGS_TYPE_A] = 100;

        return inventory;
    }

    private static Inventory MakeInventory()
    {
        Inventory inventory = new Inventory();

        foreach (Enumerations.InventoryID ID in Enum.GetValues(typeof(Enumerations.InventoryID)))
        {
            inventory.Add(ID, 0);
        }

        return inventory;
    }
}
