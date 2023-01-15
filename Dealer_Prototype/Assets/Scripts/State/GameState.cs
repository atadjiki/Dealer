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

        if(ES3.KeyExists(Key_SaveData))
        {
            _saveData = ES3.Load<SaveData>(Key_SaveData);
            Debug.Log(_saveData.ToString());
            return _saveData;
        }
        else
        {
            return null;
        }

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
        _saveData.Money += Quantity * 100; //TODO: in the future prices will fluctuate

        Save(_saveData);
    }

    //what phone calls correspond to today
    //what NPC visits correspond to today
    //what customers appear today
    //what is the danger/demand for each district today
}
