using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public enum AnimEvent_Audio
    {
        Footstep_Indoor, None
    }

    [SerializeField] private AudioClip[] AnimEvent_Footstep_Indoor;

    private AudioSource AudioSource_main;

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
        AudioSource_main = GetComponentInChildren<AudioSource>();
    }

    public void PlayAnimEventAudio(AnimEvent_Audio animEvent)
    {
        switch (animEvent)
        {
            case AnimEvent_Audio.Footstep_Indoor:
                PlayRandomClip(AnimEvent_Footstep_Indoor);
                break;
            case AnimEvent_Audio.None:
                break;
        }
    }

    private void PlayRandomClip(AudioClip[] audioClips)
    {
        int random = Random.Range(0, audioClips.Length - 1);

        PlayClip(audioClips[random]);
    }

    private void PlayClip(AudioClip clip)
    {
        if (AudioSource_main != null)
        {
            AudioSource_main.PlayOneShot(clip);
        }
    }
}
