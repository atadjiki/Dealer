using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InboxContextPanel : ContextPanel
{

    private void Awake()
    {
        int randomNumber = Random.Range(1, 10);

        for (int i = 0; i < randomNumber; i++)
        {
            GenerateListItem("item " + i + " out of " + randomNumber, i);
        }
    }

    protected override ListItem GenerateListItem(string itemText, int index)
    {
        GameObject listItemObject = Instantiate(ListItemPrefab, ContentTransform);
        listItemObject.name = "ListItem_" + index;
        InboxListItem listItem = (InboxListItem)listItemObject.GetComponent<ListItem>();

        if (listItem != null)
        {
            listItem.Setup(null, "sender", itemText);
        }

        return listItem;
    }
}
