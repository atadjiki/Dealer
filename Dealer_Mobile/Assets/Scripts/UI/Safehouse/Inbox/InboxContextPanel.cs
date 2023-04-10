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
}
