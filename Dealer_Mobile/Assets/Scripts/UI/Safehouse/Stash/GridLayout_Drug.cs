using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants.Inventory;

public class GridLayout_Drug : MonoBehaviour
{
    [SerializeField] private GameObject Prefab_DrugItem;

    [SerializeField] private GameObject Panel_Content;

    public void Add(KeyValuePair<Drugs.ID, int> pair)
    {
        GameObject itemObject = Instantiate<GameObject>(Prefab_DrugItem, Panel_Content.transform);

        ListItem_Drug itemComponent = itemObject.GetComponent<ListItem_Drug>();

        if(itemComponent != null)
        {
            itemComponent.Setup(pair);
        }
    }
}
