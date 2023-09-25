using System;
using System.Collections;
using System.Collections.Generic;
using EPOOutline;
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
    [Header("SFX")]
    [SerializeField] private WeaponSoundBank Soundbank_Fire;
    [SerializeField] private WeaponSoundBank Soundbank_Reload;

    private WeaponMuzzleAnchor _muzzleAnchor;
    private AudioSource _audioSource;

    private List<GameObject> VFX_Muzzle_Prefabs;

    private void Awake()
    {
        _muzzleAnchor = GetComponentInChildren<WeaponMuzzleAnchor>();
        _audioSource = GetComponentInChildren<AudioSource>();
        _outline = GetComponent<Outlinable>();
    }

    public override void Setup(CharacterID characterID, WeaponID ID)
    {
        base.Setup(characterID, ID);

        StartCoroutine(Coroutine_LoadVFX());
    }

    private IEnumerator Coroutine_LoadVFX()
    {
        WeaponDefinition weaponDef = WeaponDefinition.Get(_ID);

        VFX_Muzzle_Prefabs = new List<GameObject>();

        foreach (PrefabID prefabID in weaponDef.MuzzleVFX)
        {
            ResourceRequest request = GetPrefab(prefabID);

            yield return new WaitUntil( () => request.isDone);

            VFX_Muzzle_Prefabs.Add((GameObject) request.asset);
        }
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
        foreach(GameObject muzzlePrefab in VFX_Muzzle_Prefabs)
        {
            Instantiate<GameObject>(muzzlePrefab, _muzzleAnchor.transform);
        }
    }
}
