using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationComponent : MonoBehaviour
{
    private GameObject socket = null;

    public void SetSocket(GameObject inObject) { socket = inObject; }

    public void AnimEvent_Footstep()
    {
        AudioManager.PlayAnimEventAudio(AudioManager.AnimEvent_Audio.Footstep_Indoor, socket);
    }
}
