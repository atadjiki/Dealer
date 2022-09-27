using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Animator))]
public class CharacterModel : MonoBehaviour
{
    private Animator _animator;
    private CharacterInfo _info;
    private WeaponSocket _weaponSocket;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _weaponSocket = GetComponentInChildren<WeaponSocket>();
    }

    public void Setup(CharacterInfo characterInfo)
    {
        _info = characterInfo;

        SetupWeapon();
        SetupCharacterCanvas();
    }

    private void SetupWeapon()
    {
        Enumerations.WeaponID weaponID = _info.WeaponID;

        if(weaponID != Enumerations.WeaponID.None && _weaponSocket != null)
        {
            GameObject weaponPrefab = Instantiate(PrefabManager.Instance.GetWeaponModel(weaponID), _weaponSocket.transform);
            weaponPrefab.transform.localScale = Vector3.one;

            _animator.runtimeAnimatorController = AnimationManager.Instance.GetControllerByWeaponID(weaponID);
        }
    }

    private void SetupCharacterCanvas()
    {
        GameObject characterCanvasPrefab = Instantiate(
    PrefabManager.Instance.GetUIElement(Enumerations.UIID.CharacterCanvas), this.transform);

        characterCanvasPrefab.GetComponent<RectTransform>().localEulerAngles = this.transform.eulerAngles * -1;

        CharacterCanvas characterCanvas = characterCanvasPrefab.GetComponent<CharacterCanvas>();
        characterCanvas.SetName(_info.CharacterName);
    }

    private void OnMouseDown()
    {
        Debug.Log("mouse clicked " + _info.CharacterName);
    }
}
