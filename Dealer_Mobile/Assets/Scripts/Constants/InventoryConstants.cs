using System.Collections.Generic;

namespace Constants
{
    public class Inventory
    {
        //who does this inventory belong to?
        public enum OwnerID
        {
            Player_Stash,
            Player_Bag
        }

        public enum ItemID
        {
            //DRUGS

            //None,
            //Heroin,
            Cocaine,
            //Ecstasy,
            //LSD,
            //Mushrooms,
            //Amphetamines
        }

        public static string GetIconResourcePathByID(ItemID ID)
        {
            string prefix = "Sprites/Icons/Drugs/Black/";
            switch (ID)
            {
                //case ID.Amphetamines:
                //    return prefix + "Icon_Black_Drugs_Crystals";
                case ItemID.Cocaine:
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

        public static List<ItemID> GetDrugIDs()
        {
            List<ItemID> result = new List<ItemID>()
            { 
                ItemID.Cocaine,
            };

            return result;
        }
    }
}
