using System;
using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    public static void SetObjectToColor(GameObject _object, Color color)
    {
        if (_object is null)
        {
            throw new ArgumentNullException(nameof(_object));
        }

        //get all mesh renderers
        HashSet<Renderer> renderers = new HashSet<Renderer>();

        foreach (Renderer child_renderer in _object.GetComponentsInChildren<Renderer>())
        {
            renderers.Add(child_renderer);
        }

        foreach (Renderer renderer in renderers)
        {
            foreach(Material material in renderer.materials)
            {
                material.color = color;
                material.SetColor("Color_42c7f5bfb6334aa5bbf8cc1a11a49afe", color);
                material.SetColor("InTint", color);
                material.SetFloat("InAlpha", color.a/3);
                material.SetFloat("InBlendAmount", color.a/2);
            }
        }
    }
}
