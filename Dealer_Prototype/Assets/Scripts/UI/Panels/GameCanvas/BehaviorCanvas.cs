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
}
