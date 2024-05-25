using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowTarget : MonoBehaviour
{
    private Transform _defaultParent;

    private void Awake()
    {
        _defaultParent = this.transform.parent;
    }

    public void AttachToCharacter(CharacterComponent character)
    {
        Debug.Log("Attaching camera to character " + character.GetID());

        this.transform.parent = character.GetModel().transform;
    }

    public void Release()
    {
        this.transform.parent = _defaultParent;
    }

    private void Update()
    {
        this.transform.localPosition = Vector3.zero;
        this.transform.localRotation = Quaternion.identity;
        this.transform.localScale = Vector3.one;
    }
}
