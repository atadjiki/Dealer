namespace Constants
{
    public struct DistrictInfo
    {
        public Enumerations.DistrictName District;
        public Enumerations.Level Danger;
        public Enumerations.Level Demand;
        public string Blurb;
    }

    public class CityConstants
    {
        private static DistrictInfo GetDowntownInfo()
        {
            return new DistrictInfo()
            {
                District = Enumerations.DistrictName.Downtown,
                Danger = Enumerations.Level.Low,
                Demand = Enumerations.Level.Low,
                Blurb = 
                "this historic district was once the beating heart of a great city, " +
                "but has since fallen into disrepair. " +
                "life has little value here, but drugs are in high demand.."
            };
        }

        public static DistrictInfo GetInfo(Enumerations.DistrictName district)
        {
            switch (district)
            {
                case Enumerations.DistrictName.Downtown:
                    return GetDowntownInfo();
                default:
                    return new DistrictInfo();
            }
        }
    }
}