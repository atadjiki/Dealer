using System;
using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class UIManager : Manager
{
    public enum UIState { None, Gameplay, Conversation, Loading };

    private UIState currentState = UIState.None;

    [SerializeField] private Panel_Loading Panel_Loading;
    [SerializeField] private Panel_InGame Panel_InGame;

    private List<UIPanel> panels;

    private static UIManager _instance;

    private int buildCount = 0;

    public static UIManager Instance { get { return _instance; } }

    private void HideAll()
    {
        foreach(UIPanel panel in panels)
        {
            panel.HidePanel();
        }
    }

    public void SetState(UIState newState)
    {
        if(newState == currentState) { return; }

        currentState = newState;

        HideAll();

        switch (currentState)
        {
            case UIState.Gameplay:
                Panel_InGame.ShowPanel();
                break;
            case UIState.Loading:
                Panel_Loading.ShowPanel();
                break;
            default:
                break;
        }

        Debug.Log("UI State = " + currentState);
    }

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

        panels = new List<UIPanel>() { Panel_InGame, Panel_Loading };

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
            Debug.Log(this.name + " | Panels Built = " + buildCount + "/" + panels.Count);

            onBuildComplete(this);
        }
    }

    public override int AssignDelegates()
    {
        LevelManager.Instance.onLoadStart += OnLoadStart;
        LevelManager.Instance.onLoadProgress += OnLoadProgress;
        LevelManager.Instance.onLoadEnd += OnLoadEnd;

        InteractableManager.Instance.onInteractableRegistered += RegisterInteractable;
        InteractableManager.Instance.onInteractableUnRegistered += UnRegisterInteractable;

        CharacterManager.Instance.onCharacterRegistered += RegisterCharacter;
        CharacterManager.Instance.onCharacterUnRegistered += UnRegisterCharacter;
        CharacterManager.Instance.onCharacterManagerUpdate += OnCharacterManagerUpdate;

        GameStateManager.Instance.onStateChanged += OnStateChanged;
        GameStateManager.Instance.onModeChanged += OnModeChanged;


        return 10;
    }

    private void OnStateChanged(GameState state)
    {
        Panel_InGame.GetMoneyPanel().OnStateChanged(state);
    }

    private void RegisterCharacter(CharacterComponent character)
    {
        Panel_InGame.GetCharacterPanel().RegisterCharacter(character);
    }

    private void UnRegisterCharacter(CharacterComponent character)
    {
        Panel_InGame.GetCharacterPanel().UnRegisterCharacter(character);
    }

    private void OnCharacterManagerUpdate()
    {
        Panel_InGame.GetPartyPanel().OnCharacterManagerUpdate();
    }

    private void RegisterInteractable(Interactable interactable)
    {
        Panel_InGame.GetInteractablesPanel().RegisterInteractable(interactable);
    }

    private void UnRegisterInteractable(Interactable interactable)
    {
        Panel_InGame.GetInteractablesPanel().UnRegisterInteractable(interactable);
    }

    private void OnLoadStart(LevelConstants.LevelName levelName)
    {
        //hide in game UI
        SetState(UIState.Loading);
    }

    private void OnLoadProgress(LevelConstants.LevelName levelName, float progress)
    {

    }

    private void OnLoadEnd(LevelConstants.LevelName levelName)
    {
        if (levelName == LevelConstants.LevelName.Apartment)
        {
            SetState(UIState.Gameplay);
        }
        else
        {
            SetState(UIState.None);
        }
    }


    public override void Activate()
    {
        base.Activate();
        SetState(UIState.Loading);
    }

    private void OnModeChanged(GameStateManager.Mode newMode)
    {
        if (newMode == GameStateManager.Mode.GamePlay)
        {
            SetState(UIState.Gameplay);
        }
        else if (newMode == GameStateManager.Mode.Conversation)
        {
            SetState(UIState.Conversation);
        }
        else
        {
            SetState(UIState.None);
        }
    }

    public override bool PerformUpdate(float tick)
    {
        if (currentState == UIState.Gameplay)
        {
            Panel_InGame.UpdatePanel();
        }

        return base.PerformUpdate(tick);
    }
}
