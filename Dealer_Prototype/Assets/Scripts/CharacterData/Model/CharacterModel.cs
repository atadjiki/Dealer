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

    public void ApplyCharacterInfo(CharacterInfo characterInfo)
    {
        SetupWeapon(characterInfo);
        SetupMaterials(characterInfo);
    }

    private void SetupMaterials(CharacterInfo characterInfo)
    {
        foreach (CharacterMaterialSetter materialSetter in GetComponentsInChildren<CharacterMaterialSetter>())
        {
            materialSetter.ApplyCharacterInfo(characterInfo);
        }

        foreach (ObjectMaterialSetter materialSetter in GetComponentsInChildren<ObjectMaterialSetter>())
        {
            materialSetter.ApplyCharacterInfo(characterInfo);
        }
    }

    private void SetupWeapon(CharacterInfo characterInfo)
    {
        Enumerations.WeaponID weaponID = characterInfo.WeaponID;

        if(weaponID != Enumerations.WeaponID.None && _weaponSocket != null)
        {
            GameObject weaponPrefab = Instantiate(PrefabManager.Instance.GetWeaponModel(weaponID), _weaponSocket.transform);
            weaponPrefab.transform.localScale = Vector3.one;

            _animator.runtimeAnimatorController = AnimationManager.Instance.GetControllerByWeaponID(weaponID);
        }
    }
}
