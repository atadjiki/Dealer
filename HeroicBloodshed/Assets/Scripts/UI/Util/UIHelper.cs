using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHelper : MonoBehaviour
{
    public static void ClearTransformChildren(Transform container)
    {
        for (int i = 0; i < container.childCount; i++)
        {
            DestroyImmediate(container.GetChild(i).gameObject);
        }
    }
}
