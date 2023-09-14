using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterAudioManager : EncounterEventReceiver
{
    [Header("Soundtrack")]
    [SerializeField] private AudioClip Track_Music;

    //[Header("SFX")]
    //[SerializeField] private AudioClip SFX_Select;
    //[SerializeField] private AudioClip SFX_Highlight;

    [Header("Sources")]
    [SerializeField] private AudioSource Source_Music;

    public override IEnumerator Coroutine_Init(EncounterModel model)
    {
        Source_Music.loop = true;
        Source_Music.clip = Track_Music;
        yield return null;
    }

    public override IEnumerator Coroutine_StateUpdate(Constants.EncounterState stateID, EncounterModel model)
    {
        switch(stateID)
        {
            case Constants.EncounterState.SETUP_COMPLETE:
                ToggleMusic(true);
                break;
            case Constants.EncounterState.DONE:
                ToggleMusic(false);
                break;
            default:
                break;
        }

        yield return null;
    }

    public void ToggleMusic(bool flag)
    {
        if(flag)
        {
            Source_Music.Play();
        }
        else
        {
            Source_Music.Stop();
        }
    }
}
