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

    private void Awake()
    {
        _muzzleAnchor = GetComponentInChildren<WeaponMuzzleAnchor>();
        _audioSource = GetComponentInChildren<AudioSource>();
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

    protected override void HandleEvent_Fire()
    {
        base.HandleEvent_Fire();

        StartCoroutine(Coroutine_Fire());
    }

    protected override void HandleEvent_Reload()
    {
        _ammo = GetMaxAmmo();
        _audioSource.PlayOneShot(GetRandom(SFX_Reload));
    }

    private IEnumerator Coroutine_Fire()
    {
        if(_ammo > 0)
        {
            _ammo--;

            _audioSource.PlayOneShot(GetRandom(SFX_Fire));
            PlayMuzzleFX();
            yield return null;
        }
        else
        {
            Debug.Log("Out of ammo!");
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
