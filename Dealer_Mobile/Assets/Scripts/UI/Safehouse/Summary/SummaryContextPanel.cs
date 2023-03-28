using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SummaryContextPanel : MonoBehaviour
{
    [SerializeField] private GameObject ListItemPrefab;

    [SerializeField] private Transform ContentTransform;

    private void Awake()
    {
        int randomNumber = Random.Range(1, 10);

        for(int i = 0; i < randomNumber; i++)
        {
            GenerateListItem("item " + i + " out of " + randomNumber, i);
        }
    }

    private SummaryListItem GenerateListItem(string itemText, int index)
    {
        GameObject listItemObject = Instantiate(ListItemPrefab, ContentTransform);
        listItemObject.name = "SummaryListItem_" + index;
        SummaryListItem listItem = listItemObject.GetComponent<SummaryListItem>();
        listItem.SetText(itemText);

        return listItem;
    }
}
