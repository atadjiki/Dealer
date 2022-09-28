using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterModel : MonoBehaviour
{
    private Animator _animator;
    private WeaponSocket _weaponSocket;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _weaponSocket = GetComponentInChildren<WeaponSocket>();
    }

    public void SetupWeapon(Enumerations.WeaponID weaponID)
    {
        if(weaponID != Enumerations.WeaponID.None && _weaponSocket != null)
        {
            GameObject weaponPrefab = Instantiate(PrefabManager.Instance.GetWeaponModel(weaponID), _weaponSocket.transform);
            weaponPrefab.transform.localScale = Vector3.one;

            _animator.runtimeAnimatorController = AnimationManager.Instance.GetControllerByWeaponID(weaponID);
        }
    }
}
