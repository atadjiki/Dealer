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
                POOR,
                AVERAGE,
                GOOD,
                PURE
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

            public static string Format(Grade grade)
            {
                switch (grade)
                {
                    case Grade.POOR:
                        return "Poor";
                    case Grade.AVERAGE:
                        return "Average";
                    case Grade.GOOD:
                        return "Good";
                    case Grade.PURE:
                        return "Pure";
                    default:
                        return grade.ToString();
                };
            }
        }
    }
}
