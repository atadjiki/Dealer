using System.Collections;
using System.Collections.Generic;
using Constants;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class InventoryPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Text_Title;
    [SerializeField] private TextMeshProUGUI Text_Status;

    [SerializeField] private GameObject ListPanel;

    [SerializeField] private Enumerations.InventoryType Type;

    private void Start()
    {
        SetupPanel();
    }

    private void SetupPanel()
    {
        Text_Title.text = Type.ToString();

        Inventory inventory = GameState.GetInventory(Type);

        Text_Status.text = inventory.GetCount() + "/" + inventory.GetCapacity();

        foreach(Enumerations.InventoryID ID in inventory.Keys)
        {
            if(ID != Enumerations.InventoryID.None && ID != Enumerations.InventoryID.MONEY)
            {
                GameObject listItemObject = Instantiate<GameObject>(PrefabLibrary.GetInventoryListItem(), ListPanel.transform);

                TextMeshProUGUI itemText = listItemObject.GetComponentInChildren<TextMeshProUGUI>();

                itemText.text = ID.ToString();
            }
        }

    }
}
