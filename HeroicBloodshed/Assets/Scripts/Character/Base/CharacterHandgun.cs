using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHandgun : CharacterWeapon
{
    [SerializeField] private GameObject[] Prefabs_Muzzle_FX;

    [SerializeField] private List<AudioClip> SoundBank_Gunshot;

    private WeaponMuzzleAnchor _muzzleAnchor;

    private void Awake()
    {
        _muzzleAnchor = GetComponentInChildren<WeaponMuzzleAnchor>();
    }

    public override void OnAttack()
    {
        base.OnAttack();

        PlayVFX();
        PlaySFX();
    }

    private void PlayVFX()
    {
        foreach(GameObject prefab in Prefabs_Muzzle_FX)
        {
            Instantiate<GameObject>(prefab, _muzzleAnchor.transform);
        }
    }

    private void PlaySFX()
    {
        if(SoundBank_Gunshot.Count > 0)
        {
            AudioSource.PlayClipAtPoint(SoundBank_Gunshot[Random.Range(0, SoundBank_Gunshot.Count - 1)], this.transform.position);
        }
    }
}
