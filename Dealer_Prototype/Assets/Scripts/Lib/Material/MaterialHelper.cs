using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MaterialHelper
{
    private static Color playerColor = Color.green;

    private static Color neutralColor = new Color(0.9f, 0.9f, 0.9f);

    private static Color enemyColor = new Color(0.52f, 0, 0);

    private static float groundDecalOpacity = 0.15f;

    private static void SetGroundDecal(CharacterGroundDecal decal, Color color)
    {
        MeshRenderer renderer = decal.gameObject.GetComponent<MeshRenderer>();
        renderer.enabled = true;

        color.a = groundDecalOpacity;

        renderer.material.color = color;
    }

    public static void HideGroundDecal(CharacterGroundDecal decal)
    {
        MeshRenderer renderer = decal.gameObject.GetComponent<MeshRenderer>();
        renderer.enabled = false;
    }

    public static void SetPlayerGroundDecal(CharacterGroundDecal decal)
    {
        SetGroundDecal(decal, playerColor);
    }

    public static void SetNeutralGroundDecal(CharacterGroundDecal decal)
    {
        SetGroundDecal(decal, neutralColor);
    }

    public static void SetEnemyGroundDecal(CharacterGroundDecal decal)
    {
        SetGroundDecal(decal, enemyColor);
    }

    public static void SetNeutralOutline(CharacterGroundDecal decal)
    {
        SetGroundDecal(decal, neutralColor);
    }

    public static void SetEnemyOutline(CharacterGroundDecal decal)
    {
        SetGroundDecal(decal, enemyColor);
    }

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
