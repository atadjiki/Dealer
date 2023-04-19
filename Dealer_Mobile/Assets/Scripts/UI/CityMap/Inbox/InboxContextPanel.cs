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
            GenerateListItem("item " + i + " out of " + randomNumber);
        }
    }

    protected override ListItem GenerateListItem(string itemText)
    {
        InboxListItem listItem = (InboxListItem)base.GenerateListItem(itemText);

        if (listItem != null)
        {
            listItem.Setup(null, "sender", itemText);
        }

        return listItem;
    }
}
