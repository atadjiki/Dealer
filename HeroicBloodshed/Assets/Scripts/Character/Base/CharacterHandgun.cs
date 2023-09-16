using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct WeaponSoundBank
{
    public List<AudioClip> SoundBank;

    public AudioClip GetRandom()
    {
        return SoundBank[UnityEngine.Random.Range(0, SoundBank.Count)];
    }
}

public class CharacterHandgun : CharacterWeapon
{
    [Header("VFX")]
    [SerializeField] private GameObject[] Prefabs_Muzzle_FX;

    [Header("SFX")]
    [SerializeField] private WeaponSoundBank Soundbank;

    private WeaponMuzzleAnchor _muzzleAnchor;
    private AudioSource _audioSource;

    private void Awake()
    {
        _muzzleAnchor = GetComponentInChildren<WeaponMuzzleAnchor>();
        _audioSource = GetComponentInChildren<AudioSource>();
    }

    public override void OnAttack()
    {
        base.OnAttack();

        _ammo--;

        PlayVFX();
        PlaySFX();
    }

    private void PlayVFX()
    {
        if(Prefabs_Muzzle_FX.Length > 0)
        {
            Instantiate<GameObject>(Prefabs_Muzzle_FX[UnityEngine.Random.Range(0,Prefabs_Muzzle_FX.Length -1) ], _muzzleAnchor.transform);
        }
    }


    private void PlaySFX()
    {
       _audioSource.PlayOneShot(Soundbank.GetRandom());
    }
}
