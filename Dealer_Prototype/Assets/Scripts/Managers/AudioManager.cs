using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public enum AnimEvent_Audio
    {
        Footstep_Indoor, None
    }

    private static AudioManager _instance;

    public static AudioManager Instance { get { return _instance; } }

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
    }

    public void PlayAnimEventAudio(AnimEvent_Audio animEvent, GameObject socket)
    {
        switch (animEvent)
        {
            case AnimEvent_Audio.Footstep_Indoor:
                PlayOneShot("event:/Character/Footsteps/A/Footstep_Indoor_Male", socket);
                break;
            case AnimEvent_Audio.None:
                break;
        }
    }

    private void PlayOneShot(string eventPath, GameObject socket)
    {
        if(socket != null)
        {
            FMODUnity.RuntimeManager.PlayOneShotAttached(eventPath, socket);
        }
        else
        {
            FMODUnity.RuntimeManager.PlayOneShot(eventPath);
        }
        
    }
}
