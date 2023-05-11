using UnityEngine;

namespace Constants
{
    public class ColorHelper
    {
        public static Color FromGrade(Inventory.Drugs.Grade Grade)
        {
            float alpha = 0.5f;

            switch (Grade)
            {
                case Inventory.Drugs.Grade.A:
                    return new Color(0.29804f,  0.90196f,  0.00000f, alpha);
                case Inventory.Drugs.Grade.B:
                    return new Color(0.20000f,  0.60392f,  0.00000f, alpha);
                case Inventory.Drugs.Grade.C:
                    return new Color(1.00000f,  1.00000f,  0.74902f, alpha);
                case Inventory.Drugs.Grade.D:
                    return new Color(0.99216f,  0.68235f,  0.38039f, alpha);
                case Inventory.Drugs.Grade.F:
                    return new Color(0.84314f,  0.09804f,  0.10980f, alpha);

                default:
                    return Color.clear;
            }
        }
    }
}