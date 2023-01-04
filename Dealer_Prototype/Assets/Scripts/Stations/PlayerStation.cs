using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(InteractionComponent))]
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
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(GetEntryTransform().position, 0.1f);
        Gizmos.DrawRay(new Ray(GetEntryTransform().position, GetEntryTransform().forward));


        Handles.Label(GetEntryTransform().position + new Vector3(-0.5f,0,-0.25f), this.gameObject.name);
    }
#endif
}
