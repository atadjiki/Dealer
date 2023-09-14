using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHandgun : CharacterWeapon
{
    [SerializeField] private GameObject[] Prefabs_Muzzle_FX;

    private WeaponMuzzleAnchor _muzzleAnchor;

    private int Ammo = 0;

    private void Awake()
    {
        _muzzleAnchor = GetComponentInChildren<WeaponMuzzleAnchor>();
        Reload();
    }

    public override void OnAttack()
    {
        base.OnAttack();

        Ammo--;

        PlayVFX();
    }

    private void PlayVFX()
    {
        if(Prefabs_Muzzle_FX.Length > 0)
        {
            Instantiate<GameObject>(Prefabs_Muzzle_FX[Random.Range(0,Prefabs_Muzzle_FX.Length -1) ], _muzzleAnchor.transform);
        }
    }

    public int GetAmmoRemaining()
    {
        return Ammo;
    }

    public bool HasAmmo()
    {
        return Ammo > 0;
    }

    public void Reload()
    {
        WeaponDefinition weaponDefinition = WeaponDefinition.Get(GetID());

        Ammo = weaponDefinition.Ammo;
    }
}
