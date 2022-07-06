using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanel : MonoBehaviour
{
    public delegate void OnBuildComplete(UIPanel panel);
    public OnBuildComplete onBuildComplete;

    protected bool allowUpdate = false;

    public virtual void Build() 
    {
        onBuildComplete(this);
    }
    public virtual void UpdatePanel() { }
    public virtual void ShowPanel() { }
    public virtual void HidePanel() { }

    public virtual void RegisterCharacter(CharacterComponent character) { }

    public virtual void UnRegisterCharacter(CharacterComponent character) { }

    public virtual void OnCharacterManagerUpdate() { }

    public virtual void RegisterInteractable(Interactable interactable) { }

    public virtual void UnRegisterInteractable(Interactable interactable) { }

    public virtual void OnLoadStart(Constants.GameConstants.GameMode levelName) { }

    public virtual void OnLoadProgress(Constants.GameConstants.GameMode levelName, float progress) { }

    public virtual void OnLoadEnd(Constants.GameConstants.GameMode levelName) { }

    public virtual void OnGameStateChanged(GameState gameState) { }

    public virtual void OnGamePlayModeChanged(Constants.State.GamePlayMode GamePlayMode) { }

    public virtual void OnGameModeChanged(Constants.State.GameMode GameMode) { }
}
