using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

//this class manages camera in the arena
public class ArenaCameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera MainCamera;

    [SerializeField] private CameraFollowTarget FollowTarget;

    private Transform defaultFollowTargetParent;

    private void Awake()
    {
        defaultFollowTargetParent = FollowTarget.transform.parent;
    }

    public void SetFollowTarget(Transform newParent)
    {
        FollowTarget.transform.parent = newParent;
        FollowTarget.transform.localPosition = Vector3.zero;
    }

    public void ResetFollowTarget()
    {
        SetFollowTarget(defaultFollowTargetParent);
    }
}
