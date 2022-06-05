using UnityEngine;
using System.Collections;
using Constants;

public class ConversationManager : Manager
{
    private static ConversationManager _instance;

    public static ConversationManager Instance { get { return _instance; } }

    public override void Build()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        base.Build();
    }

    public void StartConversation()
    {
        GameStateManager.Instance.ToGamePlayMode(State.GamePlayMode.Conversation);
    }

    public void EndConversation()
    {
        GameStateManager.Instance.ToGamePlayMode(State.GamePlayMode.Day);
    }
}
