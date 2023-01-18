using GameDelegates;
using UnityEngine;
using static LevelUtility;

[System.Serializable]
public class GameData
{
    public int Day;
    public int Player_Drugs;
    public int Player_Money;

    public GameData()
    {
        Day = 1;
    }
}

public class GameState : MonoBehaviour
{
    private static GameData _data;
    public static PlayerLocation location = PlayerLocation.Safehouse;

    public void StateChange()
    {
        if (Global.OnGameStateChanged != null)
        {
            Global.OnGameStateChanged.Invoke();
        }
    }

    public static void Refresh()
    {
        if(_data == null)
        {
            Load();
        }
    }

    public static int IncrementDay()
    {
        Refresh();

        _data.Day += 1;

        return _data.Day;
    }

    public static void HandleTransaction(int Quantity)
    {
        AdjustDrugs(-1* Quantity);

        AdjustMoney(Quantity * 100);
    }

    public static int GetDay()
    {
        Refresh();

        return _data.Day;
    }

    public static int GetMoney()
    {
        Refresh();

        return _data.Player_Money;
    }

    public static int GetDrugs()
    {
        Refresh();

        return _data.Player_Drugs;
    }

    private static void AdjustDrugs(int amount)
    {
        Refresh();

        _data.Player_Drugs += amount;
    }

    private static void AdjustMoney(int amount)
    {
        Refresh();

        _data.Player_Money += amount;
    }

    private static void Load()
    {
        _data = ES3.Load<GameData>("GameData", new GameData());
    }

    private static void Save()
    {
        ES3.Save<GameData>("GameData", _data);
    }
}

