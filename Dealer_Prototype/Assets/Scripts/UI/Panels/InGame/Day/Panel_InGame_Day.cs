using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel_InGame_Day : UIPanel
{
    [SerializeField] private Panel_InGame_Day_Character panel_character;
    [SerializeField] private Panel_InGame_Day_Party panel_party;
    [SerializeField] private Panel_InGame_Day_Interactables panel_interactables;
    [SerializeField] private Panel_InGame_Day_Money panel_money;
    [SerializeField] private Panel_InGame_Day_Time panel_time;

    private List<UIPanel> panels;

    private int buildCount = 0;

    public override void Build()
    {
        panels = new List<UIPanel> { panel_character, panel_party, panel_interactables, panel_money, panel_time };

        foreach(UIPanel panel in panels)
        {
            panel.onBuildComplete += OnChildPanelBuilt;
            panel.Build();
        }
    }

    public void OnChildPanelBuilt(UIPanel panel)
    {
        buildCount += 1;

        if (buildCount == panels.Count)
        {
            onBuildComplete(this);
        }
    }

    public override void UpdatePanel()
    {
        foreach(UIPanel panel in panels)
        {
            panel.UpdatePanel();
        }
    }

    public override void ShowPanel()
    {
        foreach(UIPanel panel in panels)
        {
            panel.ShowPanel();
        }
    }

    public override void HidePanel()
    {
        foreach (UIPanel panel in panels)
        {
            panel.HidePanel();
        }    
    }

    public override void OnGameStateChanged(GameState gameState)
    {
        foreach(UIPanel panel in panels)
        {
            panel.OnGameStateChanged(gameState);
        }
    }

    public override void OnGamePlayModeChanged(Constants.State.GamePlayMode gamePlayMode)
    {
        foreach (UIPanel panel in panels)
        {
            panel.OnGamePlayModeChanged(gamePlayMode);
        }
    }

    public override void OnGameModeChanged(Constants.State.GameMode gameMode)
    {
        foreach (UIPanel panel in panels)
        {
            panel.OnGameModeChanged(gameMode);
        }
    }

    public override void RegisterCharacter(CharacterComponent character)
    {
        foreach(UIPanel panel in panels)
        {
            panel.RegisterCharacter(character);
        }
    }

    public override void UnRegisterCharacter(CharacterComponent character)
    {
        foreach (UIPanel panel in panels)
        {
            panel.UnRegisterCharacter(character);
        }
    }

    public override void OnCharacterManagerUpdate()
    {
        foreach (UIPanel panel in panels)
        {
            panel.OnCharacterManagerUpdate();
        } 
    }

    public override void RegisterInteractable(Interactable interactable)
    {
        foreach (UIPanel panel in panels)
        {
            panel.RegisterInteractable(interactable);
        }
    }

    public override void UnRegisterInteractable(Interactable interactable)
    {
        foreach (UIPanel panel in panels)
        {
            panel.UnRegisterInteractable(interactable);
        }
    }
}
