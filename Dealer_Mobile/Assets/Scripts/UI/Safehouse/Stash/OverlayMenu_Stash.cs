using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Text;
using static Constants.Inventory;

public class OverlayMenu_Stash : OverlayMenu
{

    [Header("Grid Layouts")]
    [SerializeField] private GridLayout_Drug Grid_Stash;

    private void Awake()
    {
        PopulateGrids();
    }

    private void PopulateGrids()
    {
        //get player data
        Inventory inventory = FindObjectOfType<Inventory>(true);

        if(inventory != null)
        {
            foreach (KeyValuePair<Drugs.ID,int> pair in inventory.GetDrugStash())
            {
                Grid_Stash.Add(pair);
            }
        }
    }
}

