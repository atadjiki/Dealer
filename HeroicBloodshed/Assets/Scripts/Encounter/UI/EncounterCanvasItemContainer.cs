using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterCanvasItemContainer : EncounterCanvasElement
{
    [SerializeField] protected GameObject Prefab_Item;

    [SerializeField] protected Transform Container;

    public override void Clear()
    {
        DestroyTransformChildren();
    }

    private void DestroyTransformChildren()
    {
        for (int i = 0; i < Container.childCount; i++)
        {
            DestroyImmediate(Container.GetChild(i).gameObject);
        }
    }
}
