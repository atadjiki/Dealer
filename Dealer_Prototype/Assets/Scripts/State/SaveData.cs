using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    public int Day;

    public int Money;

    public int Drugs;

    public override string ToString()
    {
        return 
            "Day: " + Day.ToString() + ", " +
            "Money: " + Money.ToString() + ", " +
            "Drugs: " + Drugs.ToString();
    }
}
