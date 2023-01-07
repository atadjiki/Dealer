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

    [SerializeField] private SaveData _saveData;

    private void Awake()
    {
        if(_saveData == null)
        {
            Load();
        }
        else
        {
            Save();
        }
    }

    public void Save()
    {
        ES3.Save<SaveData>(Key_SaveData, _saveData);

        if(Global.OnGameStateChanged != null)
        {
            Global.OnGameStateChanged.Invoke(_saveData);
        }
    }

    public void Load()
    {
        if(ES3.KeyExists(Key_SaveData))
        {
            _saveData = ES3.Load<SaveData>(Key_SaveData);
            Debug.Log(_saveData.ToString());
        }
        else
        {
            _saveData = this.gameObject.AddComponent<SaveData>();
        }

    }

    public int GetDay()
    {
        return _saveData.Day;
    }

    public int GetMoney()
    {
        return _saveData.Money;
    }

    public int GetDrugs()
    {
        return _saveData.Drugs;
    }

    public int IncrementDay()
    {
        _saveData.Day += 1;

        Save();

        return _saveData.Day;
    }

    public int AddMoney(int amount)
    {
        _saveData.Money += amount;

        Save();

        return _saveData.Money;
    }

    public int RemoveMoney(int amount)
    {
        _saveData.Money -= amount;

        Save();

        return _saveData.Money;
    }

    public int AddDrugs(int amount)
    {
        _saveData.Drugs += amount;

        Save();

        return _saveData.Drugs;
    }

    public int RemoveDrugs(int amount)
    {
        _saveData.Drugs -= amount;

        Save();

        return _saveData.Drugs;
    }

    //what phone calls correspond to today
    //what NPC visits correspond to today
    //what customers appear today
    //what is the danger/demand for each district today
}
