using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableCharacterManager : CharacterManager
{
    private static PlayableCharacterManager _instance;

    public static PlayableCharacterManager Instance { get { return _instance; } }

    private PlayableCharacterComponent selectedCharacter;

    private bool playerLock = false;

    public bool IsPlayerLocked() { return playerLock; }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        Build();
    }

    public override bool Register(CharacterComponent Character)
    {
        PlayableCharacterComponent playableCharacter = Character.GetComponent<PlayableCharacterComponent>();

        if(playableCharacter != null)
        {
            if (Characters.Count == _popCap)
            {
                DebugManager.Instance.Print(DebugManager.Log.LogPlayableManager, "Could not register Playable Character, reached pop cap");
                return false;
            }

            Characters.Add(playableCharacter);

            if (playableCharacter != null) DebugManager.Instance.Print(DebugManager.Log.LogPlayableManager, "Registered Playable Character " + playableCharacter.GetID());
            return true;
        }

        return false;
    }

    public override void UnRegister(CharacterComponent Character)
    {
        PlayableCharacterComponent playableCharacter = Character.GetComponent<PlayableCharacterComponent>();

        if (playableCharacter != null)
        {
            if (playableCharacter != null) DebugManager.Instance.Print(DebugManager.Log.LogPlayableManager,"Unregistered Playable Character " + playableCharacter.GetID());
            Characters.Remove(Character);
        }
            
    }

    public void HandleCharacterSelection(PlayableCharacterComponent Character)
    {
        //if nobody is selected, register
        if (selectedCharacter == null)
        {
            PossessCharacter(Character);
        }
        else if (selectedCharacter != Character)
        {
            UnpossessCharacter();
            PossessCharacter(Character);
        }
        else
        {
            UnpossessCharacter();
        }
    }

    private void PossessCharacter(PlayableCharacterComponent Character)
    {
        DebugManager.Instance.Print(DebugManager.Log.LogPlayableManager, "Selected " + Character.GetID());
        selectedCharacter = Character;
        selectedCharacter.PerformSelect();

        if(UIManager.Instance) UIManager.Instance.HandleEvent(Constants.UI.Events.CharacterSelected, Character);
        CameraFollowTarget.Instance.AttachTo(Character);
    }

    private void UnpossessCharacter()
    {
        DebugManager.Instance.Print(DebugManager.Log.LogPlayableManager, "Unselected " + selectedCharacter.GetID());
        selectedCharacter.PerformUnselect();
        selectedCharacter = null;

        UIManager.Instance.HandleEvent(Constants.UI.Events.CharacterDeselected);
        CameraFollowTarget.Instance.Release();
    }

    public bool IsCharacterCurrentlySelected()
    {
        return (selectedCharacter != null);
    }

    public PlayableCharacterComponent GetSelectedCharacter()
    {
        return selectedCharacter;
    }

    public void AttemptMoveOnPossesedCharacter(Vector3 Location)
    {
        if (selectedCharacter != null)
        {
            bool success;
            CharacterBehaviorScript behaviorScript = BehaviorHelper.MoveToBehavior(selectedCharacter, Location, out success);

            selectedCharacter.AddNewBehavior(behaviorScript);
        }
    }

    public void AttemptInteractWithPossesedCharacter(CharacterComponent character)
    {
        if (selectedCharacter != null)
        {
            bool success;
            CharacterBehaviorScript behaviorScript;

            behaviorScript = BehaviorHelper.ApproachCharacterBehavior(selectedCharacter, character, out success);

            selectedCharacter.AddNewBehavior(behaviorScript);
        }
    }

    public void AttemptInteractWithPossesedCharacter(Interactable interactable)
    {
        if (selectedCharacter != null)
        {
            bool success;
            CharacterBehaviorScript behaviorScript;

            behaviorScript = BehaviorHelper.ApproachInteractableBehavior(selectedCharacter, interactable, out success);
           
            selectedCharacter.AddNewBehavior(behaviorScript);
        }
    }

    public void AttemptBehaviorAbortWithPossesedCharacter()
    {
        if(selectedCharacter != null)
        {
            selectedCharacter.AbortBehaviors();
        }
    }

    public void LockToCharacter(PlayableCharacterComponent character)
    {
        HandleCharacterSelection(character);

        playerLock = true;

    }

    public void UnlockFromCharacter()
    {
        playerLock = false;
    }
}
