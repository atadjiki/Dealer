using System.Collections;
using UnityEngine;
using TMPro;

public class ConversationCanvas : GameCanvas
{

    [SerializeField] GameObject Canvas_Dialogue;

    [SerializeField] TextMeshProUGUI Text_Speaker;
    [SerializeField] TextMeshProUGUI Text_Dialogue;

    public override void Reset()
    {
        Toggle(false);
    }

    public override void Toggle(bool flag)
    {
        Canvas_Dialogue.SetActive(flag);
        ToggleDialogueUI(flag);
    }

    public override void HandleEvent_CharacterLineBegin(Dialogue dialogue)
    {
        base.HandleEvent_CharacterLineBegin(dialogue);

        CharacterLine_Begin(dialogue.Speaker, dialogue.Text, dialogue.Emote, dialogue.Duration);
    }

    public override void HandleEvent_CharacterLineEnd()
    {
        base.HandleEvent_CharacterLineEnd();

        CharacterLine_End();
    }

    private void CharacterLine_Begin(CharacterComponent character, string text, Constants.AnimationConstants.Anim emote, float duration)
    {
        ToggleDialogueUI(true);

        CharacterPortraitCamera.Instance.SetCharacter(character);

        Text_Speaker.text = character.GetID();

        Text_Dialogue.text = text;

        StartCoroutine(CharacterLine_Wait(character, emote, duration));

    }

    private IEnumerator CharacterLine_Wait(CharacterComponent Character, Constants.AnimationConstants.Anim Emote, float duration)
    {
        Character.FadeToAnimation(Emote, duration, false);

        yield return new WaitForSecondsRealtime(duration);

        CharacterLine_End();

    }

    private void CharacterLine_End()
    {
        CharacterPortraitCamera.Instance.Reset();
        ToggleDialogueUI(false);
    }

    private void ToggleDialogueUI(bool flag)
    {
        Canvas_Dialogue.SetActive(flag);
    }
}
