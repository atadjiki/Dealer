using UnityEngine;
using System.Collections;

public class ConversationManager : MonoBehaviour
{
    private static ConversationManager _instance;

    public static ConversationManager Instance { get { return _instance; } }

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


    }

    public void StartConversation()
    {
        GameState.Instance.ToState(GameState.State.Conversation);
    }

    public void EndConversation()
    {
        GameState.Instance.ToState(GameState.State.GamePlay);
    }
}
