using GameDelegates;
using UnityEngine;
using System;
using static LevelUtility;
using static Constants.Enumerations;

[Serializable]
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
        _data.PlayerInventory[InventoryID.DRUGS_TYPE_A] -= Quantity;
        _data.PlayerInventory[InventoryID.MONEY] += Quantity * 100;
    }

    public static int GetDay()
    {
        return _data.Day;
    }

    public static int GetPlayerItem(InventoryID ID)
    {
        return _data.PlayerInventory[ID];
    }

    public static int GetSafehouseItem(InventoryID ID)
    {
        return _data.SafehouseInventory[ID];
    }

    public static Inventory GetInventory(InventoryType Type)
    {
        if(Type == InventoryType.Player)
        {
            return _data.PlayerInventory;
        }
        else if(Type == InventoryType.Safehouse)
        {
            return _data.SafehouseInventory;
        }

        return null;
    }
}