using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using Constants;

public class BehaviorCanvas : GameCanvas
{

    [SerializeField] private TextMeshProUGUI Text_Interaction_Tip;

    [SerializeField] private GameObject Panel_SelectedCharacter;
    [SerializeField] private TextMeshProUGUI Text_SelectedCharacter;

    [SerializeField] private GameObject Panel_CharacterState;
    [SerializeField] private TextMeshProUGUI Text_CurrentAnimation;
    [SerializeField] private TextMeshProUGUI Text_CurrentBehavior;

    [SerializeField] private GameObject Panel_BehaviorQueue;
    [SerializeField] private TextMeshProUGUI Text_BehaviorQueue;
    [SerializeField] private GameObject Image_BehaviorQueue_CurrentBehavior;

    public override void Clear()
    {
        base.Clear();

        ClearInteractionTipText();
    }

    public override void Reset()
    {
        Toggle(true);

        ToggleCharacterPanels(DebugManager.Instance.State_Character != DebugManager.State.None);
    }

    public override void Toggle(bool flag)
    {
        gameObject.SetActive(flag);
    }

    public override void HandleEvent_InteractionContext(InteractableConstants.InteractionContext context)
    {
        base.HandleEvent_InteractionContext(context);

        SetInteractionTipTextContext(context);
    }

    public override void HandleEvent_SetBehaviorText(AIConstants.BehaviorType behaviorType)
    {
        SetBehaviorText(behaviorType);
    }

    public override void HandleEvent_SetAnimText(AnimationConstants.Anim anim)
    {
        base.HandleEvent_SetAnimText(anim);

        SetAnimationText(anim.ToString());
    }

    public override void HandleEvent_CharacterSelected(CharacterComponent character)
    {
        base.HandleEvent_CharacterSelected(character);

        OnCharacterSelected(character);
    }

    private void OnCharacterSelected(CharacterComponent character)
    {
        if (DebugManager.Instance.State_Character != DebugManager.State.None)
        {
            ToggleCharacterPanels(true);

            Text_SelectedCharacter.text = character.GetID();
            Text_CurrentBehavior.text = character.GetCurrentBehavior().ToString();
            Text_CurrentAnimation.text = character.GetCurrentAnimation().ToString();
        }
    }

    public override void HandleEvent_CharacterDeselected()
    {
        base.HandleEvent_CharacterDeselected();

        OnCharacterDeselected();
    }

    private void OnCharacterDeselected()
    {
        if (DebugManager.Instance.State_Character != DebugManager.State.None)
        {
            ToggleCharacterPanels(false);

            Text_SelectedCharacter.text = "";
            Text_CurrentBehavior.text = "";
            Text_CurrentAnimation.text = "";
        }
    }

    private void ToggleCharacterPanels(bool flag)
    {
        Panel_SelectedCharacter.SetActive(flag);
        Panel_CharacterState.SetActive(flag);
        Panel_BehaviorQueue.SetActive(flag);
    }

    private void SetBehaviorText(AIConstants.BehaviorType behaviorType)
    {
        Text_CurrentBehavior.text = behaviorType.ToString();
    }

    private void SetAnimationText(string anim)
    {
        Text_CurrentAnimation.text = anim;
    }

    private void SetInteractionTipTextContext(InteractableConstants.InteractionContext context)
    {
        Text_Interaction_Tip.text = InteractableConstants.GetInteractionTipTextContext(context);
    }

    private void ClearInteractionTipText()
    {
        Text_Interaction_Tip.text = "";
    }

    public override void HandleEvent_UpdateBehaviorQueue(Queue<CharacterBehaviorScript> queue)
    {
        base.HandleEvent_UpdateBehaviorQueue(queue);

        UpdateBehaviorQueue(queue);
    }

    private void UpdateBehaviorQueue(Queue<CharacterBehaviorScript> behaviorQueue)
    {
        if (DebugManager.Instance.State_Character != DebugManager.State.None)
        {
            string text = "";

            foreach (CharacterBehaviorScript behaviorScript in behaviorQueue)
            {
                text += "* ";
                text += behaviorScript.name;
                text += "\n\n";
            }

            Text_BehaviorQueue.text = text;

            Panel_BehaviorQueue.SetActive(behaviorQueue.Count != 0);
        }
    }
}
