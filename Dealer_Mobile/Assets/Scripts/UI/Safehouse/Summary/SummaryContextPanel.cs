using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SummaryContextPanel : ContextPanel
{
    private void Awake()
    {
        int randomNumber = Random.Range(1, 10);

        for(int i = 0; i < randomNumber; i++)
        {
            GenerateListItem("item " + i + " out of " + randomNumber, i);
        }
    }

    protected override ListItem GenerateListItem(string itemText, int index)
    {
        GameObject listItemObject = Instantiate(ListItemPrefab, ContentTransform);
        listItemObject.name = "ListItem_" + index;
        SummaryListItem listItem = (SummaryListItem) listItemObject.GetComponent<ListItem>();

        if(listItem != null)
        {
            listItem.Setup(itemText);
        }

        return listItem;
    }
}
