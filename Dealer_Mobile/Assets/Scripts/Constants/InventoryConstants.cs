namespace Constants
{
    public class Inventory
    {
        public enum ID
        {
            Player_Stash,
            Player_Bag
        }

        public class Drugs
        {
            public enum ID
            {
                None,
                Heroin,
                Cocaine,
                Ecstasy,
                LSD,
                Mushrooms,
                Amphetamines
            }

            public static string GetIconResourcePathByID(Drugs.ID ID)
            {
                string prefix = "Sprites/Icons/Drugs/Black/";
                switch (ID)
                {
                    case ID.Amphetamines:
                        return prefix + "Icon_Black_Drugs_Crystals";
                    case ID.Cocaine:
                        return prefix + "Icon_Black_Drugs_Powder";
                    case ID.Ecstasy:
                        return prefix + "Icon_Black_Drugs_Pill_Plural";
                    case ID.Heroin:
                        return prefix + "Icon_Black_Drugs_Baggie";
                    case ID.LSD:
                        return prefix + "Icon_Black_Drugs_Blister_Pack";
                    case ID.Mushrooms:
                        return prefix + "Icon_Black_Drugs_Mushrooms";
                    default:
                        return prefix + "Icon_Black_Drugs_Narcotic";
                }
            }
        }
    }
}
