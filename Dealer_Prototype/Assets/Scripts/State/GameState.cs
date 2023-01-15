using System;
using System.Collections;
using System.Collections.Generic;
using GameDelegates;
using UnityEngine;
using static LevelUtility;

public class GameState : MonoBehaviour
{
    private static string Key_SaveData = "SaveData";
    public static PlayerLocation location = PlayerLocation.Safehouse; 

    public static void Save(SaveData _saveData)
    {
        ES3.Save<SaveData>(Key_SaveData, _saveData);

        if(Global.OnGameStateChanged != null)
        {
            Global.OnGameStateChanged.Invoke(_saveData);
        }
    }

    public static SaveData Load()
    {
        SaveData _saveData;
        _saveData = ES3.Load<SaveData>(Key_SaveData, SaveData.GetDefault(), ES3Settings.defaultSettings);
        Debug.Log(_saveData.ToString());
        return _saveData;
    }

    public static void Reset()
    {
        SaveData _data = new SaveData();

        Save(_data);
    }

    public static int GetDay()
    {
        return Load().Day;
    }

    public static int GetMoney()
    {
        return Load().Money;
    }

    public static int GetDrugs()
    {
        return Load().Drugs;
    }

    public static int IncrementDay()
    {
        SaveData _saveData = Load();

        _saveData.Day += 1;

        Save(_saveData);

        return _saveData.Day;
    }

    public static void HandleTransaction(int Quantity)
    {
        SaveData _saveData = Load();

        _saveData.Drugs -= Quantity;
        _saveData.Drugs = Mathf.Clamp(_saveData.Drugs, 0, _saveData.Drugs);

        _saveData.Money += Quantity * 100; //TODO: in the future prices will fluctuate

        Save(_saveData);
    }
}
