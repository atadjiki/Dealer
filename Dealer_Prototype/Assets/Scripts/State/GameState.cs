using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class Data
{
    public virtual void Load()
    {
        Debug.Log("Loading " + this.GetType().Name);
    }

    public virtual void Save()
    {
        Debug.Log("Saving " + this.GetType().Name);
    }
}

public class PlayerData : Data
{
    public string Name;
    public int Money;
    public int Drugs;

    public override void Load()
    {
        base.Load();

        if(ES3.KeyExists(SaveKeys.Player_Name))
        {
            Name = ES3.Load<string>(SaveKeys.Player_Name);
        }
        else
        {
            Name = "Default_Player_Name";
        }
        
        Money = ES3.Load<int>(SaveKeys.Player_Money, 0);
        Drugs = ES3.Load<int>(SaveKeys.Player_Drugs, 0);
    }

    public override void Save()
    {
        base.Save();

        ES3.Save(SaveKeys.Player_Name, Name);
        ES3.Save(SaveKeys.Player_Money, Money);
        ES3.Save(SaveKeys.Player_Drugs, Drugs);
    }
}

public class GameData : Data
{
    public int Day;

    public override void Load()
    {
        base.Load();

        Day = ES3.Load<int>(SaveKeys.Game_Day, 0);
    }

    public override void Save()
    {
        base.Save();

        ES3.Save(SaveKeys.Game_Day, Day);
    }
}

public class GameState 
{
    public Enumerations.GameMode gameMode; 
    public Enumerations.GamePlayState gameplayState; 

    public PlayerData playerData;
    public GameData gameData;

    public GameState()
    {
        playerData = new PlayerData();
        gameData = new GameData();
    }

    public void SaveState()
    {
        playerData.Save();
        gameData.Save();
    }

    public void LoadState()
    {
        playerData.Load();
        gameData.Load();
    }
}
