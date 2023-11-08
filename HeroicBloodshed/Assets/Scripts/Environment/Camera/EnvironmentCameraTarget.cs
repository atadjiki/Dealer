using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class EnvironmentCameraTarget : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("on trigger enter");
    }
}
