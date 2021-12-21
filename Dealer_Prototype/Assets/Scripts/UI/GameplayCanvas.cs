using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using Constants;

public class GameplayCanvas : MonoBehaviour
{
    private static GameplayCanvas _instance;

    public static GameplayCanvas Instance { get { return _instance; } }

    [SerializeField] private TextMeshProUGUI Text_Interaction_Tip;

    [SerializeField] private GameObject Panel_SelectedCharacter;
    [SerializeField] private TextMeshProUGUI Text_SelectedCharacter;

    [SerializeField] private GameObject Panel_CharacterState;
    [SerializeField] private TextMeshProUGUI Text_CurrentAnimation;
    [SerializeField] private TextMeshProUGUI Text_CurrentBehavior;

    [SerializeField] private GameObject Panel_BehaviorQueue;
    [SerializeField] private TextMeshProUGUI Text_BehaviorQueue;
    [SerializeField] private GameObject Image_BehaviorQueue_CurrentBehavior;

    public enum InteractionContext { Select, Deselect, Move, Approach, Interact, None };

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
        Toggle(true);
    }

    public void Toggle(bool flag)
    {
        this.gameObject.SetActive(flag);
    }

    public void OnCharacterSelected(CharacterComponent character)
    {
        ToggleCharacterPanels(true);

        Text_SelectedCharacter.text = character.GetID();
        Text_CurrentBehavior.text = character.GetCurrentBehavior().ToString();
        Text_CurrentAnimation.text = character.GetCurrentAnimation().ToString();
    }

    public void OnCharacterDeselected()
    {
        ToggleCharacterPanels(false);

        Text_SelectedCharacter.text = "";
        Text_CurrentBehavior.text = "";
        Text_CurrentAnimation.text = "";
    }

    private void ToggleCharacterPanels(bool flag)
    {
        Panel_SelectedCharacter.SetActive(flag);
        Panel_CharacterState.SetActive(flag);
        Panel_BehaviorQueue.SetActive(flag);
    }

    public void SetBehaviorText(CharacterConstants.BehaviorType behaviorType)
    {
        Text_CurrentBehavior.text = behaviorType.ToString();
    }

    public void SetAnimationText(AnimationConstants.Animations anim)
    {
        Text_CurrentAnimation.text = anim.ToString();
    }

    public void SetInteractionTipTextContext(InteractionContext context)
    {
        string text = "";

        switch(context)
        {
            case InteractionContext.Select:
                text = "Select";
                break;
            case InteractionContext.Deselect:
                text = "Deselect";
                break;
            case InteractionContext.Move:
                text = "Move";
                break;
            case InteractionContext.Interact:
                text = "Interact";
                break;
            case InteractionContext.Approach:
                text = "Approach";
                break;
        }

        Text_Interaction_Tip.text = text;
    }

    public void ClearInteractionTipText()
    {
        Text_Interaction_Tip.text = "";
    }

    public void UpdateBehaviorQueue(Queue<CharacterBehaviorScript> behaviorQueue)
    {
        string text = "";

        foreach(CharacterBehaviorScript behaviorScript in behaviorQueue)
        {
            text += "* ";
            text += behaviorScript.name;
            text += "\n\n";
        }

        Text_BehaviorQueue.text = text;

        Panel_BehaviorQueue.SetActive(behaviorQueue.Count != 0);

    }
}
