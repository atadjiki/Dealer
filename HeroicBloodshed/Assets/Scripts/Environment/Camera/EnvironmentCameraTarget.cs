using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentCameraTarget : MonoBehaviour
{
    private Rigidbody _rigidBody;
    private SphereCollider _collider;

    private void Awake()
    {
        _rigidBody = this.gameObject.AddComponent<Rigidbody>();
        _rigidBody.useGravity = false;
        _rigidBody.isKinematic = false;

         _collider = this.gameObject.AddComponent<SphereCollider>();
        _collider.isTrigger = true;
        _collider.radius = 1.5f;
    }
}
