using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowTarget : MonoBehaviour
{
    private Rigidbody _rigidBody;
    private SphereCollider _collider;

    private Transform _defaultParent;

    private void Awake()
    {
        _rigidBody = this.gameObject.AddComponent<Rigidbody>();
        _rigidBody.useGravity = false;
        _rigidBody.isKinematic = false;

         _collider = this.gameObject.AddComponent<SphereCollider>();
        _collider.isTrigger = true;
        _collider.radius = 5f;

        _defaultParent = this.transform.parent;
    }

    public void AttachToCharacter(CharacterComponent character)
    {
        Debug.Log("Attaching camera to character " + character.GetID());

        this.transform.parent = character.GetNavigator().transform;

        this.transform.localPosition = Vector3.zero;
        this.transform.localRotation = Quaternion.identity;
        this.transform.localScale = Vector3.one;
    }

    public void Release()
    {
        this.transform.parent = _defaultParent;
    }
}
