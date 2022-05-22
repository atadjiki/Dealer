using System.Collections;
using UnityEngine;
using TMPro;
using Constants;
using UnityEngine.UI;

public class ConversationCanvas : GameCanvas
{
    [SerializeField] RawImage SpeakerImage;
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

        CharacterLine_Begin(dialogue);
    }

    public override void HandleEvent_CharacterLineEnd()
    {
        base.HandleEvent_CharacterLineEnd();

        CharacterLine_End();
    }

    private void CharacterLine_Begin(Dialogue dialogue)
    {
        ToggleDialogueUI(true);

        Text_Speaker.text = dialogue.ID.ToString();

        Text_Dialogue.text = dialogue.Text;

        SpeakerImage.texture = dialogue.Sprite;

        StartCoroutine(CharacterLine_Wait(dialogue.ID, dialogue.Emote, 5.0f));

    }

    private IEnumerator CharacterLine_Wait(CharacterConstants.CharacterID ID, Constants.AnimationConstants.Anim Emote, float duration)
    {
        //Character.FadeToAnimation(Emote, duration, false);

        yield return new WaitForSecondsRealtime(duration);

        CharacterLine_End();

    }

    private void CharacterLine_End()
    {
        ToggleDialogueUI(false);
    }

    private void ToggleDialogueUI(bool flag)
    {
        Canvas_Dialogue.SetActive(flag);
    }
}
