using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextPanel : MonoBehaviour
{
    [SerializeField] protected GameObject ListItemPrefab;

    [SerializeField] protected Transform ContentTransform;

    protected virtual ListItem GenerateListItem(string itemText)
    {
        GameObject listItemObject = Instantiate(ListItemPrefab, ContentTransform);
        return listItemObject.GetComponent<ListItem>();
    }
}
