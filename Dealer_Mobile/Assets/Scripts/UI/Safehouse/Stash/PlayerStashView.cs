using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Text;
using static Constants.Inventory;
using GameDelegates;

public class PlayerStashView : OverlayMenu
{
    [Header("Views")]
    [SerializeField] private DrugContainerView stashContainerView;

    private void Awake()
    {
        Populate();
    }

    protected override void OnCancelButtonClicked()
    {
        GameDelegates.Global.OnSafehouseMenuComplete.Invoke(Constants.Safehouse.SafehouseMenu.Inventory);

        base.OnCancelButtonClicked();
    }

    private void Populate()
    {
        if(GameState.Instance != null)
        {
            foreach (ItemID ID in Constants.Inventory.GetDrugIDs())
            {
                stashContainerView.Add(ID);
            }
        }
        else
        {
            Debug.Log("Cant populate stash view, GameState is null!");
        }
    }
}
