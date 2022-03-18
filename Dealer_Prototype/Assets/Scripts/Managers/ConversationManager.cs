using UnityEngine;
using TMPro;
using System.Collections;

public class ConversationManager : MonoBehaviour
{
    private static ConversationManager _instance;

    public static ConversationManager Instance { get { return _instance; } }

    [SerializeField] GameObject Canvas_Dialogue;

    [SerializeField] TextMeshProUGUI Text_Speaker;
    [SerializeField] TextMeshProUGUI Text_Dialogue;

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

        ToggleDialogueUI(false);
    }

    public void StartConversation()
    {
        GameState.Instance.ToState(GameState.State.Conversation);
    }

    public void EndConversation()
    {
        GameState.Instance.ToState(GameState.State.GamePlay);
    }

    public void CharacterLine_Begin(CharacterComponent character, string text, float time)
    {
        ToggleDialogueUI(true);

        CharacterPortraitCamera.Instance.SetCharacter(character);

        Text_Speaker.text = character.GetID();

        Text_Dialogue.text = text;

        StartCoroutine(CharacterLine_Wait(time));

    }

    public void CharacterLine_End()
    {
        CharacterPortraitCamera.Instance.Reset();
        ToggleDialogueUI(false);
    }

    private IEnumerator CharacterLine_Wait(float time)
    {
        yield return new WaitForSecondsRealtime(time);

        CharacterLine_End();

    }

    protected void ToggleDialogueUI(bool flag)
    {
        Canvas_Dialogue.SetActive(flag);
    }
}
