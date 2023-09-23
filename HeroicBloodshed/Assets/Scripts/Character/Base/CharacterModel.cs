using System.Collections;
using System.Collections.Generic;
using EPOOutline;
using UnityEngine;

public class CharacterModel : MonoBehaviour
{
    [SerializeField] private GameObject MeshGroup_Main;

    [SerializeField] private GameObject MeshGroup_Outline;

    [SerializeField] private GameObject MeshGroup_Highlight;

    private CapsuleCollider _collider;

    public void Setup(CharacterDefinition definition)
    {
        Constants.TeamID team = Constants.GetTeamByID(definition.ID);

        Color teamColor = Constants.GetColorByTeam(team, 1.0f);

        SetupOutline(teamColor);

        SetupHighlight(teamColor);

        ToggleOutline(true);

        ToggleHighlight(false);
    }

    private void SetupOutline(Color color)
    {
        color.a = 1.0f;

        foreach(Outlinable outlinable in MeshGroup_Outline.GetComponentsInChildren<Outlinable>())
        {
            outlinable.OutlineParameters.Color = color;
        }
    }

    private void SetupHighlight(Color color)
    {
        color.a = 0.5f;

        foreach (SkinnedMeshRenderer renderer in MeshGroup_Highlight.GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            renderer.material.SetColor("_BaseColor", color);
        }
    }

    public void ToggleModel(bool flag)
    {
        MeshGroup_Main.SetActive(flag);
    }

    public void ToggleOutline(bool flag)
    {
        MeshGroup_Outline.SetActive(flag);
    }

    public void ToggleHighlight(bool flag)
    {
        MeshGroup_Highlight.SetActive(flag);
    }
}
