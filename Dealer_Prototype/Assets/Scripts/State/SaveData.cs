using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData 
{
    public int Day;

    public int Money;

    public int Drugs;

    public static SaveData GetDefault()
    {
        SaveData defaultData = new SaveData();

        defaultData.Day = 1;
        defaultData.Money = 0;
        defaultData.Drugs = 0;

        return defaultData;

    }

    public override string ToString()
    {
        return 
            "Day: " + Day.ToString() + ", " +
            "Money: " + Money.ToString() + ", " +
            "Drugs: " + Drugs.ToString();
    }
}
