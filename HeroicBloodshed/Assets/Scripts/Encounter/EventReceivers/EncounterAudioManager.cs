using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class EncounterAudioManager : EncounterEventReceiver
{
    [Header("Soundtrack")]
    [SerializeField] private AudioClip Track_Music;

    [Header("Sources")]
    [SerializeField] private AudioSource Source_Music;
    [SerializeField] private AudioSource Source_SFX;

    public override IEnumerator Coroutine_Init(EncounterModel model)
    {
        Source_Music.volume = 0;
        Source_Music.loop = true;
        Source_Music.clip = Track_Music;
        yield return null;
    }

    public override IEnumerator Coroutine_StateUpdate(Constants.EncounterState stateID, EncounterModel model)
    {
        switch(stateID)
        {
            case Constants.EncounterState.SETUP_COMPLETE:
                Source_Music.Play();
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
        StopAllCoroutines();

        if(flag)
        {
            StartCoroutine(Coroutine_ToggleMusic(true));
        }
        else
        {
            StartCoroutine(Coroutine_ToggleMusic(false));
        }
    }

    private IEnumerator Coroutine_ToggleMusic(bool flag)
    {

        float currentVolume;
        float targetVolume;

        if(flag)
        {
            currentVolume = 0;
            targetVolume = 1; 
        }
        else
        {
            currentVolume = 1;
            targetVolume = 0;
        }

        Source_Music.volume = currentVolume;

        float time = 0;
        float duration = 3.0f;

        while(time < duration)
        {
            time += Time.deltaTime;
            Source_Music.volume = Mathf.Lerp(currentVolume, targetVolume, time / duration);
            yield return null;
        }
    }
}
