using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterModel : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer Mesh_Main;

    [SerializeField] private SkinnedMeshRenderer Mesh_Outline;

    [SerializeField] private SkinnedMeshRenderer Mesh_Highlight;

    private CapsuleCollider _collider;

    public void Setup(CharacterDefinition definition)
    {
        Constants.TeamID team = Constants.GetTeamByID(definition.ID);

        Color teamColor = Constants.GetColorByTeam(team);

        SetupOutline(teamColor);

        SetupHighlight(teamColor);

        ToggleOutline(true);

        ToggleHighlight(false);
    }

    private void SetupOutline(Color color)
    {
        if (Mesh_Outline != null)
        {
            Mesh_Outline.material.SetColor("OutlineColor", Color.black);
        }
    }

    private void SetupHighlight(Color color)
    {
        color.a = 0.25f;

        if (Mesh_Highlight!= null)
        {
            Mesh_Highlight.material.SetColor("_BaseColor", color);
        }
    }

    public void ToggleModel(bool flag)
    {
        Mesh_Main.gameObject.SetActive(flag);
    }

    public void ToggleOutline(bool flag)
    {
        Mesh_Outline.gameObject.SetActive(flag);
    }

    public void ToggleHighlight(bool flag)
    {
        Mesh_Highlight.gameObject.SetActive(flag);
    }
}
