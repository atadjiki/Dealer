using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEditor;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(BoxCollider))]
public class PlayerStation : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _camera;
    [SerializeField] private Transform _entryTransform;
    [SerializeField] private Enumerations.SafehouseStation stationID = Enumerations.SafehouseStation.None;

    private StationCanvas _stationCanvas;

    public Transform GetEntryTransform()
    {
        return _entryTransform;
    }

    public Enumerations.SafehouseStation GetStationID()
    {
        return stationID;
    }

    private void Awake()
    {
        GameObject stationCanvasObject = Instantiate<GameObject>(PrefabLibrary.GetStationCanvas(), _entryTransform);
        _stationCanvas = stationCanvasObject.GetComponent<StationCanvas>();
        _stationCanvas.Setup(stationID);
    }

    public void ToggleUI(bool flag)
    {
        if(_stationCanvas != null)
        {
            _stationCanvas.gameObject.SetActive(flag);
        }
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
