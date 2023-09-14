using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

[Serializable]
public struct WeaponSoundBank
{
    public WeaponID ID;
    public List<AudioClip> SoundBank;

    public AudioClip GetRandom()
    {
        return SoundBank[UnityEngine.Random.Range(0, SoundBank.Count - 1)];
    }
}
public class EncounterAudioManager : EncounterEventReceiver
{
    [Header("Soundtrack")]
    [SerializeField] private AudioClip Track_Music;

    [Header("SFX")]
    [Header("Handgun")]
    [SerializeField] private WeaponSoundBank[] WeaponSoundBanks;

    [Header("Sources")]
    [SerializeField] private AudioSource Source_Music;
    [SerializeField] private AudioSource Source_SFX;

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

    public void PlayWeaponSFX(WeaponID weaponID)
    {
        foreach (WeaponSoundBank soundBank in WeaponSoundBanks)
        {
            if (soundBank.ID == weaponID)
            {
                Source_SFX.PlayOneShot(soundBank.GetRandom());
            }
        }
    }
}
