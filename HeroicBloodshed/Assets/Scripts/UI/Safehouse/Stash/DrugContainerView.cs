using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class DrugContainerView : MonoBehaviour
{
    [SerializeField] private GameObject Prefab_DrugItem;

    [SerializeField] private GameObject Panel_Content;

    public void Add(InventoryItemID itemID)
    {
        GameObject itemObject = Instantiate<GameObject>(Prefab_DrugItem, Panel_Content.transform);

        DrugContainerViewItem itemComponent = itemObject.GetComponent<DrugContainerViewItem>();

        if(itemComponent != null)
        {
            itemComponent.Setup(itemID);
        }
    }
}
