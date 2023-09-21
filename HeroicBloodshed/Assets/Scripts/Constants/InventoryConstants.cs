using System.Collections.Generic;

public static partial class Constants
{
    //who does this inventory belong to?

    public static List<InventoryItemID> GetDrugIDs()
    {
        List<InventoryItemID> result = new List<InventoryItemID>()
            {
                InventoryItemID.Cocaine,
            };

        return result;
    }
}
