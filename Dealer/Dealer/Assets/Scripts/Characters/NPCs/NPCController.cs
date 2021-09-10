using Pathfinding.RVO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : Character
{
    private Vector3 InteractionLocation;
    internal float InteractionDistance = 2.0f;
    internal float SpeechTime = 3.5f;
    internal float CameraTime = 3.5f;

    public bool Unavailable = false;
    public bool Generic = false;

    private void Awake()
    {
        Initialize();

        InteractionLocation = this.transform.position + this.transform.forward * InteractionDistance;
    }

    public Vector3 GetConversationLocation()
    {
        return InteractionLocation;
    }

    public virtual void StartConversation()
    {
        StartCoroutine(DoConversation());
    }

    public virtual IEnumerator DoConversation()
    {
        yield return null;

        if(Generic)
        {
            UIManager.Instance.DisplaySpeech("...", 1.0f);
            PlayerController.Instance.ToIdle();
        }
    }
}
