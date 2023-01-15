using System;
using System.Collections;
using System.Collections.Generic;
using GameDelegates;
using UnityEngine;
using static LevelUtility;

public class GameState : MonoBehaviour
{
    private static string Key_Day = "Day";
    private static string Key_Money = "Money";
    private static string Key_Drugs = "Drugs";

    public static PlayerLocation location = PlayerLocation.Safehouse;

    public void StateChange()
    {
        if (Global.OnGameStateChanged != null)
        {
            Global.OnGameStateChanged.Invoke();
        }
    }


    public static int IncrementDay()
    {
        int value = GetDay();

        value++;

        SetValue(Key_Day, value);

        return value;
    }

    public static void HandleTransaction(int Quantity)
    {
        AdjustDrugs(-1* Quantity);

        AdjustMoney(Quantity * 100);
    }

    public static int GetDay()
    {
        return GetValue(Key_Day);
    }

    public static int GetMoney()
    {
        return GetValue(Key_Money);
    }

    public static int GetDrugs()
    {
        return GetValue(Key_Drugs);
    }

    private static void AdjustDrugs(int amount)
    {
        AdjustValue(Key_Drugs, amount);
    }

    private static void AdjustMoney(int amount)
    {
        AdjustValue(Key_Money, amount);
    }

    private static void AdjustValue(string key, int amount)
    {
        int value = GetValue(key);
        value += amount;
        value = Mathf.Clamp(value, 0, value);
        SetValue(key, value);
    }

    private static int GetValue(string key)
    {
        return ES3.Load<int>(key, 0);
    }

    private static void SetValue(string key, int value)
    {
        ES3.Save<int>(key, value);
    }
}
