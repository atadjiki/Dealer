using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

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
    [SerializeField] private WeaponSoundBank Soundbank_Fire;
    [SerializeField] private WeaponSoundBank Soundbank_Reload;

    private WeaponMuzzleAnchor _muzzleAnchor;
    private AudioSource _audioSource;

    private void Awake()
    {
        _muzzleAnchor = GetComponentInChildren<WeaponMuzzleAnchor>();
        _audioSource = GetComponentInChildren<AudioSource>();
    }

    public override void OnAbility(AbilityID ability)
    {
        base.OnAbility(ability);

        switch(ability)
        {
            case AbilityID.Attack:
                {
                    _ammo--;
                    _audioSource.PlayOneShot(Soundbank_Fire.GetRandom());
                    PlayMuzzleFX();
                    break;
                }
            case AbilityID.Reload:
                {
                    _ammo = GetMaxAmmo();
                    _audioSource.PlayOneShot(Soundbank_Reload.GetRandom());
                    break;
                }
            default:
                break;
        }
    }

    private void PlayMuzzleFX()
    {
        foreach(GameObject muzzlePrefab in Prefabs_Muzzle_FX)
        {
            Instantiate<GameObject>(muzzlePrefab, _muzzleAnchor.transform);
        }
    }
}
