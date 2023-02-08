using System.Collections.Generic;
using GameDelegates;
using Constants;
using UnityEngine;
using static LevelUtility;
using System;

[System.Serializable]
public class GameData
{
    public int Day;

    public Dictionary<Enumerations.InventoryID, int> PlayerInventory;
    public Dictionary<Enumerations.InventoryID, int> SafehouseInventory;

    public GameData()
    {
        Day = 1;

        PlayerInventory = DefaultPlayerInventory();
        SafehouseInventory = DefaultSafehouseInventory();
    }

    private static Dictionary<Enumerations.InventoryID, int> DefaultPlayerInventory()
    {
        Dictionary<Enumerations.InventoryID, int> inventory = MakeInventory();

        inventory[Enumerations.InventoryID.MONEY] = 0;
        inventory[Enumerations.InventoryID.DRUGS] = 0;

        return inventory;
    }

    private static Dictionary<Enumerations.InventoryID, int> DefaultSafehouseInventory()
    {
        Dictionary<Enumerations.InventoryID, int> inventory = MakeInventory();

        inventory[Enumerations.InventoryID.MONEY] = 1000;
        inventory[Enumerations.InventoryID.DRUGS] = 100;

        return inventory;
    }

    private static Dictionary<Enumerations.InventoryID, int> MakeInventory()
    {
        Dictionary<Enumerations.InventoryID, int> inventory = new Dictionary<Enumerations.InventoryID, int>();

        foreach (Enumerations.InventoryID ID in Enum.GetValues(typeof(Enumerations.InventoryID)))
        {
            inventory.Add(ID, 0);
        }

        return inventory;
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
        return _data.PlayerInventory[Enumerations.InventoryID.MONEY];
    }

    public static int GetPlayerDrugs()
    {
        return _data.PlayerInventory[Enumerations.InventoryID.DRUGS];
    }

    public static int GetSafehouseMoney()
    {
        return _data.SafehouseInventory[Enumerations.InventoryID.MONEY];
    }

    public static int GetSafehouseDrugs()
    {
        return _data.SafehouseInventory[Enumerations.InventoryID.DRUGS];
    }

    private static void AdjustPlayerDrugs(int amount)
    {
        _data.PlayerInventory[Enumerations.InventoryID.DRUGS] += amount;
    }

    private static void AdjustPlayerMoney(int amount)
    {
        _data.PlayerInventory[Enumerations.InventoryID.MONEY] += amount;
    }

    private static void AdjustSafehouseDrugs(int amount)
    {
        _data.SafehouseInventory[Enumerations.InventoryID.DRUGS] += amount;
    }

    private static void AdjustSafehouseMoney(int amount)
    {
        _data.SafehouseInventory[Enumerations.InventoryID.MONEY] += amount;
    }
}