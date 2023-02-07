using System.Collections.Generic;
using GameDelegates;
using Constants;
using UnityEngine;
using static LevelUtility;

public struct Inventory
{
    public int Money;
    public int Drugs;

    public static Inventory DefaultPlayerInventory()
    {
        Inventory inventory = new Inventory();

        inventory.Money = 0;
        inventory.Drugs = 0;

        return inventory;
    }

    public static Inventory DefaultSafehouseInventory()
    {
        Inventory inventory = new Inventory();

        inventory.Money = 1000;
        inventory.Drugs = 100;

        return inventory;
    }
}

[System.Serializable]
public class GameData
{
    public int Day;

    public Inventory PlayerInventory;
    public Inventory SafehouseInventory;

    public GameData()
    {
        Day = 1;

        PlayerInventory = Inventory.DefaultPlayerInventory();
        SafehouseInventory = Inventory.DefaultSafehouseInventory();
    }
}

public class GameState : MonoBehaviour
{
    private static GameData _data;
    public static PlayerLocation location = PlayerLocation.Safehouse;

    private void Awake()
    {
        _data = new GameData();
    }

    public void StateChange()
    {
        if (Global.OnGameStateChanged != null)
        {
            Global.OnGameStateChanged.Invoke();
        }
    }

    public static int IncrementDay()
    {
        _data.Day += 1;

        return _data.Day;
    }

    public static void HandleTransaction(int Quantity)
    {
        AdjustPlayerDrugs(-1* Quantity);

        AdjustPlayerMoney(Quantity * 100);
    }

    public static int GetDay()
    {
        return _data.Day;
    }

    public static int GetPlayerMoney()
    {
        return _data.PlayerInventory.Money;
    }

    public static int GetPlayerDrugs()
    {
        return _data.PlayerInventory.Drugs;
    }

    public static int GetSafehouseMoney()
    {
        return _data.SafehouseInventory.Money;
    }

    public static int GetSafehouseDrugs()
    {
        return _data.SafehouseInventory.Drugs;
    }

    private static void AdjustPlayerDrugs(int amount)
    {
        _data.PlayerInventory.Drugs += amount;
    }

    private static void AdjustPlayerMoney(int amount)
    {
        _data.PlayerInventory.Money += amount;
    }

    private static void AdjustSafehouseDrugs(int amount)
    {
        _data.SafehouseInventory.Drugs += amount;
    }

    private static void AdjustSafehouseMoney(int amount)
    {
        _data.SafehouseInventory.Money += amount;
    }
}