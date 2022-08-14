using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraRig : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera CM_Main;
    [SerializeField] private CinemachineVirtualCamera CM_Character;

    public GameObject DefaultTarget;

    public void ResetCamera()
    {
        CM_Main.Priority = 10;
        CM_Character.Priority = 0;
        CM_Character.Follow = null;
    }

    public void SetFollowTarget(Transform target)
    {
        CM_Character.Follow = target;
        CM_Character.Priority = 10;

        CM_Main.Priority = 0;

    }
}

