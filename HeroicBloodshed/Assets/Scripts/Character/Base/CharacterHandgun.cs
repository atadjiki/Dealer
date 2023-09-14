using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHandgun : CharacterWeapon
{
    [SerializeField] private GameObject[] Prefabs_Muzzle_FX;

    private WeaponMuzzleAnchor _muzzleAnchor;

    private void Awake()
    {
        _muzzleAnchor = GetComponentInChildren<WeaponMuzzleAnchor>();
    }

    public override void OnAttack()
    {
        base.OnAttack();

        PlayVFX();
    }

    private void PlayVFX()
    {
        if(Prefabs_Muzzle_FX.Length > 0)
        {
            Instantiate<GameObject>(Prefabs_Muzzle_FX[Random.Range(0,Prefabs_Muzzle_FX.Length -1) ], _muzzleAnchor.transform);
        }
    }
}
