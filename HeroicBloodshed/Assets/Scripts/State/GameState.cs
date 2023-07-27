using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class Inventory
{
    //who does this belong to?
    private OwnerID _OwnerID;

    public OwnerID GetOwner() { return _OwnerID; }

    //the collection itself (maps inventory IDs to quantities
    private Dictionary<InventoryItemID, int> _ItemMap;

    public bool HasItem(InventoryItemID Item)
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

    public int GetQuantity(InventoryItemID Item)
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

    public int AddQuantity(InventoryItemID Item, int Quantity)
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
    public int RemoveQuantity(InventoryItemID Item, int Quantity)
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

    public Inventory(OwnerID Owner)
    {
        _OwnerID = Owner;

        _ItemMap = new Dictionary<InventoryItemID, int>();
    }

    public static Inventory GenerateDrugInventory(OwnerID Owner)
    {
        Inventory drugInventory = new Inventory(Owner);

        foreach(InventoryItemID drugID in GetDrugIDs())
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

    [SerializeField] private Inventory PlayerStash = Inventory.GenerateDrugInventory(OwnerID.Player_Stash);

    [SerializeField] private Inventory PlayerBag = Inventory.GenerateDrugInventory(OwnerID.Player_Bag);

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
        PlayerStash.AddQuantity(InventoryItemID.Cocaine, 100);
    }

    public int GetDay()
    {
        return Day;
    }

    public int GetMoney()
    {
        return Money;
    }

    public Inventory GetInventory(OwnerID ID)
    {
        if(ID == OwnerID.Player_Stash)
        {
            return PlayerStash;
        }
        else if (ID == OwnerID.Player_Bag)
        {
            return PlayerBag;
        }
        else
        {
            return null;
        }
    }

    public void TransferItemQuantity(OwnerID SenderID, OwnerID RecepientID, InventoryItemID Item, int Quantity)
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

    public void AddToBag(InventoryItemID ID, int Quantity)
    {
        TransferItemQuantity(OwnerID.Player_Stash, OwnerID.Player_Bag, ID, Quantity);
    }

    public void RemoveFromBag(InventoryItemID ID, int Quantity)
    {
        TransferItemQuantity(OwnerID.Player_Bag, OwnerID.Player_Stash, ID, Quantity);
    }
}
