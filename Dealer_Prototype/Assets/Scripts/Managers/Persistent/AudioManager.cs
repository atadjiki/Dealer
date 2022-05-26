using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Manager
{
    public override void Build()
    {
        base.Build();
    }

    public enum AnimEvent_Audio
    {
        Footstep_Indoor, None
    }

    public static void PlayAnimEventAudio(AnimEvent_Audio animEvent, GameObject socket)
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

    private static void PlayOneShot(string eventPath, GameObject socket)
    {
        if(socket != null)
        {
        }
        else
        {
        }
    }
}
