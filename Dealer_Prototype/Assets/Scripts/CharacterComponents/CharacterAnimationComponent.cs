using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationComponent : MonoBehaviour
{
    private void Awake()
    {
        
    }

    public void AnimEvent_Footstep()
    {
        AudioManager.Instance.PlayAnimEventAudio(AudioManager.AnimEvent_Audio.Footstep_Indoor);
    }
}
