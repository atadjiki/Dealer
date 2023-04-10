using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextPanel : MonoBehaviour
{
    [SerializeField] protected GameObject ListItemPrefab;

    [SerializeField] protected Transform ContentTransform;

    protected ListItem GenerateListItem(string itemText, int index)
    {
        GameObject listItemObject = Instantiate(ListItemPrefab, ContentTransform);
        listItemObject.name = "ListItem_" + index;
        ListItem listItem = listItemObject.GetComponent<ListItem>();
        listItem.SetText(itemText);

        return listItem;
    }
}
