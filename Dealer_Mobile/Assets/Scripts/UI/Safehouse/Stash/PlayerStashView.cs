using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Text;
using static Constants.Inventory;

public class PlayerStashView : OverlayMenu
{
    [Header("Views")]
    [SerializeField] private DrugContainerView stashContainerView;

    private void Awake()
    {
        Populate();
    }

    private void Populate()
    {
        //get player data
        Inventory inventory = FindObjectOfType<Inventory>(true);

        if(inventory != null)
        {
            foreach (KeyValuePair<Drugs.ID,int> pair in inventory.GetDrugStash())
            {
                stashContainerView.Add(pair);
            }
        }
    }
}
