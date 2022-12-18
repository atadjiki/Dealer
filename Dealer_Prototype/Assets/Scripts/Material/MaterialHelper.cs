using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MaterialHelper
{
    public static void SetCharacterOutline(CharacterModel model, Color color)
    {
        OutlineMaterialComponent[] outlineMaterials = model.GetComponentsInChildren<OutlineMaterialComponent>();

        foreach (OutlineMaterialComponent outlineMaterial in outlineMaterials)
        {
            SkinnedMeshRenderer renderer = outlineMaterial.gameObject.GetComponent<SkinnedMeshRenderer>();

            foreach (Material material in renderer.materials)
            {
                material.SetColor("OutlineColor", color);
            }
        }
    }

    public static void SetPlayerOutline(CharacterModel model)
    {
        SetCharacterOutline(model, Color.green);
    }

    public static void SetNeutralOutline(CharacterModel model)
    {
        SetCharacterOutline(model, Color.white);
    }

    public static void ResetCharacterOutline(CharacterModel model)
    {
        SetCharacterOutline(model, Color.black);
    }
}
