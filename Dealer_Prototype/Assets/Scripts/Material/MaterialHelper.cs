using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MaterialHelper
{
    private static Color playerColor = Color.green;

    private static Color neutralColor = new Color(0.9f, 0.9f, 0.9f);

    private static Color enemyColor = new Color(0.52f, 0, 0);

    private static void SetCharacterOutline(CharacterModel model, Color color)
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
        SetCharacterOutline(model, playerColor);
    }

    public static void SetNeutralOutline(CharacterModel model)
    {
        SetCharacterOutline(model, neutralColor);
    }

    public static void SetEnemyOutline(CharacterModel model)
    {
        SetCharacterOutline(model, enemyColor);
    }

    public static void ResetCharacterOutline(CharacterModel model)
    {
        SetCharacterOutline(model, Color.black);
    }
}
