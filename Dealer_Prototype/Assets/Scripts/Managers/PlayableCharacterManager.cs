using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableCharacterManager : MonoBehaviour
{
    private static PlayableCharacterManager _instance;

    public static PlayableCharacterManager Instance { get { return _instance; } }

    private List<PlayableCharacterComponent> Characters;

    private PlayableCharacterComponent selectedCharacter;


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

    private void Build()
    {
        Characters = new List<PlayableCharacterComponent>();
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
        if (DebugManager.Instance.LogNPCManager) Debug.Log("Selected " + Character.GetID());
        selectedCharacter = Character;
        selectedCharacter.PerformSelect();

        GameplayCanvas.Instance.OnCharacterSelected(Character);
        CameraFollowTarget.Instance.AttachTo(Character);
    }

    private void UnpossessCharacter()
    {
        if (DebugManager.Instance.LogNPCManager) Debug.Log("Unselected " + selectedCharacter.GetID());
        selectedCharacter.PerformUnselect();
        selectedCharacter = null;

        GameplayCanvas.Instance.OnCharacterDeselected();
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

    public void AttemptInteractWithPossesedCharacter(Interactable interactable)
    {
        if (selectedCharacter != null)
        {
            bool success;
            CharacterBehaviorScript behaviorScript;

            if (BehaviorHelper.IsInteractionAllowed(selectedCharacter, interactable))
            {
                behaviorScript = BehaviorHelper.InteractWithBehavior(selectedCharacter, interactable, out success);
            }
            else
            {
                behaviorScript = BehaviorHelper.ApproachBehavior(selectedCharacter, interactable, out success);
            }

            selectedCharacter.AddNewBehavior(behaviorScript);


        }
    }
}
