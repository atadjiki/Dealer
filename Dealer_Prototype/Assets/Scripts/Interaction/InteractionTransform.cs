using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionTransform : MonoBehaviour
{
    private void Awake()
    {
        if(LevelManager.IsManagerLoaded())
        {
            ColorManager.Instance.SetObjectToColor(this.gameObject, ColorManager.Instance.GetInteractionTransformColor());
        }
        else
        {
            this.enabled = false;
        }
    }
}
