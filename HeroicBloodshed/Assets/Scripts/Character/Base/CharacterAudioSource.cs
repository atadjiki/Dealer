using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

[Serializable]
public struct CharacterSoundBank
{
    public CharacterAudioType Type;

    public List<AudioClip> SoundBank;

    public AudioClip GetRandom()
    {
        return SoundBank[UnityEngine.Random.Range(0, SoundBank.Count)];
    }
}

public class CharacterAudioSource : MonoBehaviour
{
    [SerializeField] private List<CharacterSoundBank> SoundbankMap;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private AudioClip GetAudioClip(CharacterAudioType audioType)
    {
        foreach(CharacterSoundBank soundbank in SoundbankMap)
        {
            if(soundbank.Type == audioType && soundbank.SoundBank.Count > 0)
            {
                return soundbank.GetRandom();
            }
        }

        return null;
    }

    public void Play(CharacterAudioType audioType)
    {
        AudioClip audioClip = GetAudioClip(audioType);

        if(audioClip != null)
        {
            _audioSource.PlayOneShot(audioClip);
        }
    }
}
