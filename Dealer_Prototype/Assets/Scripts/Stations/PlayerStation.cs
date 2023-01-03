using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PlayerStation : MonoBehaviour
{
    [SerializeField] private Transform _entryTransform;

    public Transform GetEntryTransform()
    {
        return _entryTransform;
    }

    private void OnMouseDown()
    {
        Debug.Log("Clicked on " + this.name);
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Handles.Label(GetEntryTransform().position, this.gameObject.name);
    }
#endif
}
