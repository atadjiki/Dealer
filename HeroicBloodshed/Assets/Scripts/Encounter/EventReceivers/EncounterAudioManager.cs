using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class EncounterAudioManager : EncounterEventReceiver
{
    private enum EncounterMusicTrack { Gameplay, Win, Loss}

    [Header("Soundtrack")]
    [SerializeField] private AudioClip Track_Gameplay;
    [SerializeField] private AudioClip Track_Win;
    [SerializeField] private AudioClip Track_Loss;

    [Header("Sources")]
    [SerializeField] private AudioSource Source_Music;
    [SerializeField] private AudioSource Source_SFX;
    [SerializeField] private AudioSource Source_Voice;

    public override IEnumerator Coroutine_Init(EncounterModel model)
    {
        yield return null;
    }

    public override IEnumerator Coroutine_StateUpdate(Constants.EncounterState stateID, EncounterModel model)
    {
        switch(stateID)
        {
            case Constants.EncounterState.SETUP_COMPLETE:
               yield return PlayTrack(EncounterMusicTrack.Gameplay);
                break;
            case Constants.EncounterState.DONE:
                if(model.DidPlayerWin())
                {
                    yield return PlayTrack(EncounterMusicTrack.Win);
                }
                else
                {
                    yield return PlayTrack(EncounterMusicTrack.Loss);
                }

                break;
            default:
                break;
        }

        yield return null;
    }

    private IEnumerator PlayTrack(EncounterMusicTrack track)
    {
        yield return Coroutine_ToggleMusic(false);

        switch(track)
        {
            case EncounterMusicTrack.Gameplay:
                Source_Music.loop = true;
                Source_Music.clip = Track_Gameplay;
                break;
            case EncounterMusicTrack.Win:
                Source_Music.loop = false;
                Source_Music.clip = Track_Win;
                break;
            case EncounterMusicTrack.Loss:
                Source_Music.loop = false;
                Source_Music.clip = Track_Loss;
                break;
        }

        Source_Music.Play();
        yield return Coroutine_ToggleMusic(true);
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
        float duration = 0.5f;

        while(time < duration)
        {
            time += Time.deltaTime;
            Source_Music.volume = Mathf.Lerp(currentVolume, targetVolume, time / duration);
            yield return null;
        }
    }
}
