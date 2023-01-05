using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PlayerStation : MonoBehaviour
{
    [SerializeField] private Transform _entryTransform;
    [SerializeField] private Enumerations.MouseContext mouseContext = Enumerations.MouseContext.None;
    [SerializeField] private Enumerations.SafehouseStation stationID = Enumerations.SafehouseStation.None;

    public Transform GetEntryTransform()
    {
        return _entryTransform;
    }

    public Enumerations.MouseContext GetMouseContext()
    {
        return mouseContext;
    }

    private void OnMouseDown()
    {
        Debug.Log("Clicked on " + this.name);
    }

    public Enumerations.SafehouseStation GetStationID()
    {
        return stationID;
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
