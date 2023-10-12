using System;
using System.Collections;
using System.Collections.Generic;
using EPOOutline;
using UnityEngine;
using static Constants;

public class CharacterHandgun : CharacterWeapon
{
    private WeaponMuzzleAnchor _muzzleAnchor;
    private AudioSource _audioSource;

    private List<GameObject> VFX_Muzzle_Prefabs;
    private List<AudioClip> SFX_Fire;
    private List<AudioClip> SFX_Reload;

    private WeaponAttackDefinition _attackDef;

    private void Awake()
    {
        _muzzleAnchor = GetComponentInChildren<WeaponMuzzleAnchor>();
        _audioSource = GetComponentInChildren<AudioSource>();
    }

    public override void Setup(CharacterID characterID, WeaponID ID)
    {
        base.Setup(characterID, ID);

        _attackDef = WeaponAttackDefinition.Get(ID);

        StartCoroutine(Coroutine_LoadVFX());
    }

    private IEnumerator Coroutine_LoadVFX()
    {
        WeaponDefinition weaponDef = WeaponDefinition.Get(_ID);

        VFX_Muzzle_Prefabs = new List<GameObject>();

        foreach (PrefabID prefabID in weaponDef.MuzzleVFX)
        {
            ResourceRequest request = GetWeaponFX(prefabID);

            yield return new WaitUntil( () => request.isDone);

            VFX_Muzzle_Prefabs.Add((GameObject) request.asset);
        }

        SFX_Fire = new List<AudioClip>();

        foreach(AudioID audioID in weaponDef.AttackSFX)
        {
            ResourceRequest request = GetAudioClip(audioID);

            yield return new WaitUntil(() => request.isDone);

            SFX_Fire.Add((AudioClip)request.asset);
        }

        SFX_Reload = new List<AudioClip>();

        foreach (AudioID audioID in weaponDef.ReloadSFX)
        {
            ResourceRequest request = GetAudioClip(audioID);

            yield return new WaitUntil(() => request.isDone);

            SFX_Reload.Add((AudioClip)request.asset);
        }


    }

    public override void OnAbility(AbilityID ability)
    {
        base.OnAbility(ability);

        switch(ability)
        {
            case AbilityID.Attack:
                {
                    HandleAbility_Attack();
                    break;
                }
            case AbilityID.Reload:
                {
                    _ammo = GetMaxAmmo();
                    _audioSource.PlayOneShot(GetRandom(SFX_Reload));
                    break;
                }
            default:
                break;
        }
    }

    private void HandleAbility_Attack()
    {
        if(_ammo > 0)
        {
            _ammo--;
            StartCoroutine(Coroutine_HandleAbility_Attack());

        }
        else
        {
            Debug.Log("Out of ammo!");
        }

    }

    private IEnumerator Coroutine_HandleAbility_Attack()
    {
        int shotCount = _attackDef.CalculateShotCount();
        for (int i = 0; i < shotCount; i++)
        {
            _audioSource.PlayOneShot(GetRandom(SFX_Fire));
            PlayMuzzleFX();
            yield return new WaitForSecondsRealtime(_attackDef.TimeBetweenShots);
        }

    }

    private void PlayMuzzleFX()
    {
        foreach(GameObject muzzlePrefab in VFX_Muzzle_Prefabs)
        {
            Instantiate<GameObject>(muzzlePrefab, _muzzleAnchor.transform);
        }
    }

    private AudioClip GetRandom(List<AudioClip> soundbank)
    {
        return soundbank[UnityEngine.Random.Range(0, soundbank.Count)];
    }
}
