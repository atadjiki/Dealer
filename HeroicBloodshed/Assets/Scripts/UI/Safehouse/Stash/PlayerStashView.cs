using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Text;
using static Constants;

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
        Global.OnSafehouseMenuComplete.Invoke(SafehouseMenuID.Inventory);

        base.OnCancelButtonClicked();
    }

    private void Populate()
    {
        if(GameState.Instance != null)
        {
            foreach (InventoryItemID ID in GetDrugIDs())
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
