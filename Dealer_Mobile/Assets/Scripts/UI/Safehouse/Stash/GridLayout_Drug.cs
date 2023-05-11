using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridLayout_Drug : MonoBehaviour
{
    [SerializeField] private GameObject Prefab_DrugGridItem;

    [SerializeField] private GameObject Panel_Content;

    public void Add(DrugItem item)
    {
        GameObject itemObject = Instantiate<GameObject>(Prefab_DrugGridItem, Panel_Content.transform);

        GridItem_Drug itemComponent = itemObject.GetComponent<GridItem_Drug>();

        if(itemComponent != null)
        {
            itemComponent.Setup(item);
        }
    }
}
