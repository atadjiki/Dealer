using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class EnvironmentWall : MonoBehaviour
{
    private List<MeshRenderer> _renderers;

    private void Awake()
    {
        _renderers = new List<MeshRenderer>(GetComponentsInChildren<MeshRenderer>());
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject collidedObject = other.gameObject;

        if (collidedObject.GetComponent<EnvironmentCameraTarget>())
        {
            ToggleVisiblity(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject collidedObject = other.gameObject;

        if (collidedObject.GetComponent<EnvironmentCameraTarget>())
        {
            ToggleVisiblity(false);
        }
    }

    private void ToggleVisiblity(bool flag)
    {
        foreach (MeshRenderer renderer in _renderers)
        {
            renderer.enabled = flag;
        }
    } 
}
