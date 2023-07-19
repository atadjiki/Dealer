using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CombatEncounterCamera : MonoBehaviour
{
    private CinemachineVirtualCamera _camera;

    private Transform _initialTarget;

    private void Awake()
    {
        _camera = GetComponent<CinemachineVirtualCamera>();
        _initialTarget = _camera.Follow;
    }

    public void SetTarget(Transform newTarget)
    {
        _camera.Follow = newTarget;
        //_camera.transform.rotation = Quaternion.Euler(new Vector3(20.0f, -82f, 0f));
        //AdjustTransposer(new Vector3(1.5f, 1.5f, 1.5f), 5);
    }

    public void Reset()
    {
        _camera.Follow = _initialTarget;
        //_camera.transform.rotation = Quaternion.Euler(new Vector3(45.0f, -125f, 0f));
        //AdjustTransposer(new Vector3(0,0,0), 7);
    }

    private void AdjustTransposer(Vector3 offset, float distance)
    {
        CinemachineFramingTransposer transposer = _camera.GetCinemachineComponent<CinemachineFramingTransposer>();
        if(transposer != null)
        {
            transposer.m_TrackedObjectOffset = offset;
            transposer.m_CameraDistance = distance;

        }
    }
}
