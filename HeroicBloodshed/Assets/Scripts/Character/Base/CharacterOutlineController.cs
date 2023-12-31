using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPOOutline;
using static Constants;

public class CharacterOutlineController : MonoBehaviour, ICharacterEventReceiver
{
    [SerializeField] private Outlinable Outline;
    [SerializeField] private Outlinable Highlight;

    private bool _canReceive = true;

    public void Setup(CharacterDefinition definition, GameObject parentObject)
    {
        TeamID team = GetTeamByID(definition.ID);

        Color teamColor = GetColor(team, 1.0f);

        SetupOutline(teamColor);
        SetupHighlight(teamColor);

        ToggleOutline(true);
        ToggleHighlight(false);

        foreach (SkinnedMeshRenderer renderer in parentObject.GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            OutlineTarget target = new OutlineTarget(renderer);
            Outline.TryAddTarget(target);
            Highlight.TryAddTarget(target);
        }

        foreach (MeshRenderer renderer in parentObject.GetComponentsInChildren<MeshRenderer>())
        {
            OutlineTarget target = new OutlineTarget(renderer);
            Outline.TryAddTarget(target);
            Highlight.TryAddTarget(target);
        }
    }

    private void SetupOutline(Color color)
    {
        color.a = 1.0f;

        Outline.OutlineParameters.Color = color; //setup outliner color
    }

    private void SetupHighlight(Color color)
    {
        color.a = 0.5f;
        Highlight.OutlineParameters.FillPass.SetColor("_PublicColor", color);

        color.a = 1f;
        Highlight.OutlineParameters.FillPass.SetColor("_PublicGapColor", color);
    }

    public void ToggleOutline(bool flag)
    {
        Outline.gameObject.SetActive(flag);
    }

    public void ToggleHighlight(bool flag)
    {
        Highlight.gameObject.SetActive(flag);
    }

    private void SetDeadOutline()
    {
        Color color = Outline.OutlineParameters.Color;

        color.a = 0.25f;

        Outline.OutlineParameters.Color = color;
    }

    public void HandleEvent(CharacterEvent characterEvent, object eventData)
    {
        if (!_canReceive) { return; }

        switch (characterEvent)
        {
            case CharacterEvent.DEATH:
                ToggleHighlight(false);
                SetDeadOutline();
                break;
            default:
                break;
        }
    }

    public bool CanReceiveCharacterEvents()
    {
        return _canReceive;
    }
}
