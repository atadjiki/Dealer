using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionTransform : MonoBehaviour
{
    private void Awake()
    {
        ColorManager.Instance.SetObjectToColor(this.gameObject, ColorManager.Instance.GetInteractionTransformColor());
    }
}
