namespace Constants
{
    public class DisplayText
    {
        public static string Get(Enumerations.MouseContext context)
        {
            switch(context)
            {
                case Enumerations.MouseContext.Approach:
                    return "Approach";
                case Enumerations.MouseContext.Door:
                    return "Exit";
                case Enumerations.MouseContext.Interact:
                    return "Interact";
                case Enumerations.MouseContext.Move:
                    return "Move";
                case Enumerations.MouseContext.Phone:
                    return "Phone";
                case Enumerations.MouseContext.Pickup:
                    return "Pickup";
                case Enumerations.MouseContext.Save:
                    return "Save";
                case Enumerations.MouseContext.Stash:
                    return "Stash";

                default:
                    return string.Empty;
            }
        }

        public static string Get(Enumerations.SafehouseStation station)
        {
            switch (station)
            {
                case Enumerations.SafehouseStation.Door:
                    return "Exit";
                case Enumerations.SafehouseStation.Phone:
                    return "Phone";
                case Enumerations.SafehouseStation.Save:
                    return "Save";
                case Enumerations.SafehouseStation.Stash:
                    return "Stash";

                default:
                    return string.Empty;
            }
        }
    }
}