using System.Collections.Generic;

public static partial class Constants
{
    //who does this inventory belong to?

    public static string GetIconResourcePathByID(InventoryItemID ID)
    {
        string prefix = "Sprites/Icons/Drugs/Black/";
        switch (ID)
        {
            //case ID.Amphetamines:
            //    return prefix + "Icon_Black_Drugs_Crystals";
            case InventoryItemID.Cocaine:
                return prefix + "Icon_Black_Drugs_Powder";
            //case ID.Ecstasy:
            //    return prefix + "Icon_Black_Drugs_Pill_Plural";
            //case ID.Heroin:
            //    return prefix + "Icon_Black_Drugs_Baggie";
            //case ID.LSD:
            //    return prefix + "Icon_Black_Drugs_Blister_Pack";
            //case ID.Mushrooms:
            //    return prefix + "Icon_Black_Drugs_Mushrooms";
            default:
                return prefix + "Icon_Black_Drugs_Narcotic";
        }
    }

    public static List<InventoryItemID> GetDrugIDs()
    {
        List<InventoryItemID> result = new List<InventoryItemID>()
            {
                InventoryItemID.Cocaine,
            };

        return result;
    }
}
