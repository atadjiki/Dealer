using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    //who does this belong to?
    private Constants.Inventory.OwnerID _OwnerID;

    public Constants.Inventory.OwnerID GetOwner() { return _OwnerID; }

    //the collection itself (maps inventory IDs to quantities
    private Dictionary<Constants.Inventory.ItemID, int> _ItemMap;

    public bool HasItem(Constants.Inventory.ItemID Item)
    {
        return _ItemMap.ContainsKey(Item);
    }

    public int GetSize()
    {
        int total = 0;

        foreach (int quantity in _ItemMap.Values)
        {
            total += quantity;
        }

        return total;
    }

    public int GetQuantity(Constants.Inventory.ItemID Item)
    {
        if(_ItemMap.ContainsKey(Item))
        {
            return _ItemMap[Item];
        }
        else
        {
            Debug.Log("Item " + Item.ToString() + " not found in " + _OwnerID.ToString());
            return 0;
        }
    }

    public int AddQuantity(Constants.Inventory.ItemID Item, int Quantity)
    {
        //add check for capacity later 

        if(_ItemMap.ContainsKey(Item))
        {
            _ItemMap[Item] += Mathf.Abs(Quantity);

            return _ItemMap[Item];
        }

        Debug.Log("Item " + Item.ToString() + " not found in " + _OwnerID.ToString());
        return 0;
    }
    public int RemoveQuantity(Constants.Inventory.ItemID Item, int Quantity)
    {
        if (_ItemMap.ContainsKey(Item))
        {
            _ItemMap[Item] -= Mathf.Abs(Quantity);

            _ItemMap[Item] = Mathf.Clamp(_ItemMap[Item], 0, _ItemMap[Item]);

            return _ItemMap[Item];
        }

        Debug.Log("Item " + Item.ToString() + " not found in " + _OwnerID.ToString());
        return 0;
    }

    public Inventory(Constants.Inventory.OwnerID Owner)
    {
        _OwnerID = Owner;

        _ItemMap = new Dictionary<Constants.Inventory.ItemID, int>();
    }

    public static Inventory GenerateDrugInventory(Constants.Inventory.OwnerID Owner)
    {
        Inventory drugInventory = new Inventory(Owner);

        foreach(Constants.Inventory.ItemID drugID in Constants.Inventory.GetDrugIDs())
        {
            drugInventory._ItemMap.Add(drugID, 0);
        }

        return drugInventory;
    }
}

public class GameState : MonoBehaviour
{
    private static GameState _instance;

    public static GameState Instance { get { return _instance; } }

    [SerializeField] private int Day = 1;

    [SerializeField] private int Money = 100;

    [SerializeField] private Inventory PlayerStash = Inventory.GenerateDrugInventory(Constants.Inventory.OwnerID.Player_Stash);

    [SerializeField] private Inventory PlayerBag = Inventory.GenerateDrugInventory(Constants.Inventory.OwnerID.Player_Bag);

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        Build();
    }

    private void Build()
    {
        PlayerStash.AddQuantity(Constants.Inventory.ItemID.Cocaine, 100);
    }

    public int GetDay()
    {
        return Day;
    }

    public int GetMoney()
    {
        return Money;
    }

    public Inventory GetInventory(Constants.Inventory.OwnerID ID)
    {
        if(ID == Constants.Inventory.OwnerID.Player_Stash)
        {
            return PlayerStash;
        }
        else if (ID == Constants.Inventory.OwnerID.Player_Bag)
        {
            return PlayerBag;
        }
        else
        {
            return null;
        }
    }

    public void TransferItemQuantity(Constants.Inventory.OwnerID SenderID, Constants.Inventory.OwnerID RecepientID, Constants.Inventory.ItemID Item, int Quantity)
    {
        if(SenderID == RecepientID)
        {
            Debug.Log("Cannot transfer from sender to itself!");
            return;
        }

        Inventory SenderInventory = GetInventory(SenderID);
        Inventory RecepientInventory = GetInventory(RecepientID);

        if(SenderInventory.HasItem(Item))
        {
            if(SenderInventory.GetQuantity(Item) >= Quantity)
            {
                SenderInventory.RemoveQuantity(Item, Quantity);
                RecepientInventory.AddQuantity(Item, Quantity);
            }
            else
            {
                Debug.Log("Sender " + SenderID.ToString() + " does not have enough item " + Item.ToString() + " to complete transer");
            }

        }
        else
        {
            Debug.Log("Sender " + SenderID.ToString() + " does not have item " + Item.ToString());
        }
    }

    public void AddToBag(Constants.Inventory.ItemID ID, int Quantity)
    {
        TransferItemQuantity(Constants.Inventory.OwnerID.Player_Stash, Constants.Inventory.OwnerID.Player_Bag, ID, Quantity);
    }

    public void RemoveFromBag(Constants.Inventory.ItemID ID, int Quantity)
    {
        TransferItemQuantity(Constants.Inventory.OwnerID.Player_Bag, Constants.Inventory.OwnerID.Player_Stash, ID, Quantity);
    }
}
