using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    private static GameState _instance;

    public static GameState Instance { get { return _instance; } }


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
    }

    [SerializeField] private int Day = 1;

    [SerializeField] private int Money = 100;

    [SerializeField] private List<DrugInventoryData> Inventory;

    public int GetDay()
    {
        return Day;
    }

    public int GetMoney()
    {
        return Money;
    }

    public List<DrugInventoryData> GetInventory()
    {
        return Inventory;
    }

    public void AddToBag(Constants.Inventory.Drugs.ID ID, int amount)
    {
        for (int i = 0; i < Inventory.Count; i++)
        {
            if(Inventory[i].ID == ID)
            {
                DrugInventoryData data = Inventory[i];

                if(data.Quantity_Stash - amount >= 0)
                {
                    data.Quantity_Bag += amount;
                    data.Quantity_Stash -= amount;

                    Inventory[i] = data;
                }
                else
                {
                    Debug.Log("Cannot add to bag, no more in stash!");
                }
            }
        }
    }

    public void RemoveFromBag(Constants.Inventory.Drugs.ID ID, int amount)
    {
        for (int i = 0; i < Inventory.Count; i++)
        {
            if (Inventory[i].ID == ID)
            {
                DrugInventoryData data = Inventory[i];

                if(data.Quantity_Bag - amount >= 0)
                {
                    data.Quantity_Stash += amount;
                    data.Quantity_Bag -= amount;

                    Inventory[i] = data;
                }
                else
                {
                    Debug.Log("Cannot remove from bag, no more in bag!");
                }
                
            }
        }
    }
}
