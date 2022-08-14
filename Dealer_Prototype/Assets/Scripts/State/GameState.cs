using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class Data
{
    public virtual void Load()
    {
        if (GameStateManager.Instance.debug) Debug.Log("Loading " + this.GetType().Name);
    }

    public virtual void Save()
    {
        if (GameStateManager.Instance.debug) Debug.Log("Saving " + this.GetType().Name);
    }
}

public class PartyData : Data
{
    public Enumerations.CharacterID Leader;
    public Enumerations.CharacterID[] Muscle;
}

public class PlayerPartyData : PartyData
{
    public override void Load()
    {
        base.Load();

        if (ES3.KeyExists(SaveKeys.Player_Party_Leader))
        {
            Leader = ES3.Load<Enumerations.CharacterID>(SaveKeys.Player_Party_Leader);
        }
        else
        {
            Leader = Enumerations.CharacterID.Player;
        }

        if(ES3.KeyExists(SaveKeys.Player_Party_Muscle))
        {
            Muscle = ES3.Load(SaveKeys.Player_Party_Muscle, new Enumerations.CharacterID[3]);
        }
        else
        {
            Muscle = new Enumerations.CharacterID[3];
        }
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

        if (ES3.KeyExists(SaveKeys.Player_Name))
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
    private Stack<Enumerations.GameMode> modeQueue;

    private Enumerations.Environment environment;

    private PlayerPartyData playerPartyData;
    private PlayerData playerData;
    private GameData gameData;

    public GameState()
    {
        modeQueue = new Stack<Enumerations.GameMode>();

        environment = Enumerations.Environment.None;

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

    public bool EnqueueGameMode(Enumerations.GameMode mode)
    {
        if (modeQueue.Contains(mode)) return false;

        modeQueue.Push(mode);
        PrintModeQueue();
        return true;
    }

    public bool DequeueGameMode()
    {
        if (modeQueue.Count == 0) { return false; }

        modeQueue.Pop();
        PrintModeQueue();
        return true;
    }

    public Enumerations.GameMode GetActiveMode()
    {
        if (modeQueue.Count == 0) { return Enumerations.GameMode.Root; }

        return modeQueue.Peek();
    }

    public PlayerData GetPlayerData() { return playerData; }
    public GameData GetGameData() { return gameData; }

    public PlayerPartyData GetPlayerPartyData() { return playerPartyData; }

    public void SetEnvironment(Enumerations.Environment _environment)
    {
        environment = _environment;
    }

    public Enumerations.Environment GetEnvironment()
    {
        return environment;
    }

    private void PrintModeQueue()
    {
        if (GameStateManager.Instance.debug)
        {
            string output = "Mode Queue: ";

            foreach (Enumerations.GameMode mode in modeQueue)
            {
                output += mode.ToString();
                output += ", ";
            }

            Debug.Log(output);
        }
    }
}
