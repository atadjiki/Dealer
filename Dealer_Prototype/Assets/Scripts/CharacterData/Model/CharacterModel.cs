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

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Setup(CharacterInfo characterInfo)
    {
        _info = characterInfo;

        SetAnimationController();
        SetupCharacterCanvas();
    }

    private void SetAnimationController()
    {
        _animator.runtimeAnimatorController = AnimationManager.Instance.GetControllerByWeaponID(_info.WeaponID);
    }

    private void SetupCharacterCanvas()
    {
        GameObject characterCanvasPrefab = Instantiate(
    PrefabManager.Instance.GetUIElement(Enumerations.PrefabID.CharacterCanvas), this.transform);

        characterCanvasPrefab.GetComponent<RectTransform>().localEulerAngles = this.transform.eulerAngles * -1;

        CharacterCanvas characterCanvas = characterCanvasPrefab.GetComponent<CharacterCanvas>();
        characterCanvas.SetName(_info.CharacterName);
    }

    private void OnMouseDown()
    {
        Debug.Log("mouse clicked " + _info.CharacterName);
    }
}
