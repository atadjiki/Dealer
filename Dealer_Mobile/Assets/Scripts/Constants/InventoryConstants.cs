namespace Constants
{
    public class Inventory
    {
        public enum ID
        {
            None,

            //Drugs
            KETRACEL_WHITE,
            NEO_OPIUM,
            ULTRA_STIM
        }

        public static string Format(ID id)
        {
            switch(id)
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

        public class Drugs
        {
            public enum Quality { None, Poor, Average, Good };
        }

    }
}
