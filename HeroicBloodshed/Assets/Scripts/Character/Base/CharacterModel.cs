using System.Collections;
using System.Collections.Generic;
using EPOOutline;
using UnityEngine;
using static Constants;

public class CharacterModel : MonoBehaviour, ICharacterEventReceiver
{
    [SerializeField] private GameObject MeshGroup_Main;

    [SerializeField] private Outlinable Outline;

    [SerializeField] private Outlinable Highlight;

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

        Outline.OutlineParameters.Color = color; //setup outliner color
    }

    private void SetDeadOutline()
    {
        Color color = Outline.OutlineParameters.Color;

        color.a = 0.25f;

        Outline.OutlineParameters.Color = color;
    }

    private void SetupHighlight(Color color)
    {
        color.a = 0.5f;
        Highlight.OutlineParameters.FillPass.SetColor("_PublicColor", color);

        color.a = 1f;
        Highlight.OutlineParameters.FillPass.SetColor("_PublicGapColor", color);
    }

    public void ToggleModel(bool flag)
    {
        MeshGroup_Main.SetActive(flag);
    }

    public void ToggleOutline(bool flag)
    {
        Outline.gameObject.SetActive(flag);
    }

    public void ToggleHighlight(bool flag)
    {
        Highlight.gameObject.SetActive(flag);
    }

    public void HandleEvent(object eventData, CharacterEvent characterEvent)
    {
        switch (characterEvent)
        {
            case CharacterEvent.KILLED:
                ToggleHighlight(false);
                SetDeadOutline();
                break;
            default:
                break;
        }
    }
}
