using System;
using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class UIManager : Manager
{
    [SerializeField] private Panel_Loading Panel_Loading;
    [SerializeField] private Panel_InGame_Main Panel_InGame_Main;

    private List<UIPanel> panels;

    private static UIManager _instance;

    private int buildCount = 0;

    public static UIManager Instance { get { return _instance; } }

    public override void Build()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        panels = new List<UIPanel>() { Panel_Loading, Panel_InGame_Main };

        foreach (UIPanel panel in panels)
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

    public override int AssignDelegates()
    {
        InteractableManager.Instance.onInteractableRegistered += RegisterInteractable;
        InteractableManager.Instance.onInteractableUnRegistered += UnRegisterInteractable;

        CharacterManager.Instance.onCharacterRegistered += RegisterCharacter;
        CharacterManager.Instance.onCharacterUnRegistered += UnRegisterCharacter;
        CharacterManager.Instance.onCharacterManagerUpdate += OnCharacterManagerUpdate;

        GameStateManager.Instance.onGameStateChanged += OnGameStateChanged;
        GameStateManager.Instance.onGameModeChanged += OnGameModeChanged;
        GameStateManager.Instance.onGamePlayModeChanged += OnGamePlayModeChanged;

        return 11;
    }

    private void RegisterCharacter(CharacterComponent character)
    {
        foreach(UIPanel panel in panels)
        {
            panel.RegisterCharacter(character);
        }
    }

    private void UnRegisterCharacter(CharacterComponent character)
    {
        foreach (UIPanel panel in panels)
        {
            panel.UnRegisterCharacter(character);
        }
    }

    private void OnCharacterManagerUpdate()
    {
        foreach (UIPanel panel in panels)
        {
            panel.OnCharacterManagerUpdate();
        }
    }

    private void RegisterInteractable(Interactable interactable)
    {
        foreach (UIPanel panel in panels)
        {
            panel.RegisterInteractable(interactable);
        }
    }

    private void UnRegisterInteractable(Interactable interactable)
    {
        foreach (UIPanel panel in panels)
        {
            panel.UnRegisterInteractable(interactable);
        }
    }

    private void OnGameStateChanged(GameState GameState)
    {
        foreach (UIPanel panel in panels)
        {
            panel.OnGameStateChanged(GameState);
        }
    }

    private void OnLoadStart(GameConstants.GameMode levelName)
    {
        foreach (UIPanel panel in panels)
        {
            panel.OnLoadStart(levelName);
        }
    }

    private void OnLoadProgress(GameConstants.GameMode levelName, float progress)
    {
        foreach (UIPanel panel in panels)
        {
            panel.OnLoadProgress(levelName, progress);
        }
    }

    private void OnLoadEnd(GameConstants.GameMode levelName)
    {
        foreach (UIPanel panel in panels)
        {
            panel.OnLoadEnd(levelName);
        }
    }

    private void OnGamePlayModeChanged(State.GamePlayMode GamePlayMode)
    {
        foreach(UIPanel panel in panels)
        {
            panel.OnGamePlayModeChanged(GamePlayMode);
        }
    }

    private void OnGameModeChanged(State.GameMode GameMode)
    {
        foreach(UIPanel panel in panels)
        {
            panel.OnGameModeChanged(GameMode);
        }
    }

    public override bool PerformUpdate(float tick)
    {
        foreach (UIPanel panel in panels)
        {
            panel.UpdatePanel();
        }

        return base.PerformUpdate(tick);
    }
}
