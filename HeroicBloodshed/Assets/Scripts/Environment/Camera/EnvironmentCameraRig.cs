using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class EnvironmentCameraRig : MonoBehaviour
{
    private List<CinemachineVirtualCamera> _cameras;
    private EnvironmentCameraTarget _followTarget;

    private BoxCollider _collider;
    private float _targetSpeed = 10f;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();

        _followTarget = GetComponentInChildren<EnvironmentCameraTarget>();

        _cameras = new List<CinemachineVirtualCamera>();

        foreach(CinemachineVirtualCamera virtualCamera in GetComponentsInChildren<CinemachineVirtualCamera>())
        {
            virtualCamera.Priority = 0;
            _cameras.Add(virtualCamera);
        }

        if(_cameras.Count > 0)
        {
            _cameras[0].Priority = 10;
        }
    }

    private void Update()
    {
        UpdateFollowTarget();
        UpdateCameraCycle();
    }

    private void CycleCameras(bool forward)
    {
        int activeIndex = 0;

        for(int i = 0; i < _cameras.Count; i++)
        {
            if(_cameras[i].Priority > 0)
            {
                activeIndex = i;
                break;
            }
        }

        int modifier = 1;

        if (!forward) { modifier = -1; }

        activeIndex += modifier;

        if(activeIndex < 0)
        {
            activeIndex = _cameras.Count - 1;
        }
        else if(activeIndex >= _cameras.Count)
        {
            activeIndex = 0;
        }

        for (int i = 0; i < _cameras.Count; i++)
        {
            if(i == activeIndex)
            {
                _cameras[i].Priority = 10;
            }
            else
            {
                _cameras[i].Priority = 0;
            }
        }
    }

    private void UpdateCameraCycle()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            CycleCameras(false);
        }
        else if(Input.GetKeyDown(KeyCode.E))
        {
            CycleCameras(true);
        }
    }

    private void UpdateFollowTarget()
    {
        Vector3 direction = Vector3.zero;

        if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            direction.x = Input.GetAxis("Horizontal");
            direction.z = Input.GetAxis("Vertical");
            direction *= _targetSpeed;

            //camera forward and right vectors:
            Vector3 forward = Camera.main.transform.forward;
            forward.y = 0;

            Vector3 right = Camera.main.transform.right;

            direction = forward * direction.z + right * direction.x;

            Vector3 origin = _followTarget.transform.position;
            Vector3 destination = _followTarget.transform.position + direction;
            Vector3 lerpedPosition = Vector3.Lerp(origin, destination, Time.smoothDeltaTime);

            if (_collider.bounds.Contains(lerpedPosition))
            {
                _followTarget.transform.position = lerpedPosition;
            }
        }
    }
}
