namespace Constants
{
    public class Inventory
    {
        public class Drugs
        {
            public enum ID
            {
                None,
                KETRACEL_WHITE,
                NEO_OPIUM,
                ULTRA_STIM

            }
            public enum Grade 
            { 
                None, 
                F,
                D,
                C,
                B,
                A,
            };

            public static string Format(ID id)
            {
                switch (id)
                {
                    case ID.KETRACEL_WHITE:
                        return "Ketracel White";
                    case ID.NEO_OPIUM:
                        return "Neo Opium";
                    case ID.ULTRA_STIM:
                        return "Ultra Stim";
                    default:
                        return id.ToString();
                };
            }
        }
    }
}
