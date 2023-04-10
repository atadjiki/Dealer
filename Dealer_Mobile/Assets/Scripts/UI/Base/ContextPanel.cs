using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextPanel : MonoBehaviour
{
    [SerializeField] protected GameObject ListItemPrefab;

    [SerializeField] protected Transform ContentTransform;

    protected virtual ListItem GenerateListItem(string itemText, int index)
    {
        return null;
    }
}
