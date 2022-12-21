using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using GameDelegates;
using UnityEngine;

public class CameraFollowTarget : MonoBehaviour
{
    private Transform initialParent;

    private bool _attached = false;

    private void Start()
    {
        initialParent = transform.parent;

        Global.OnNewCameraTarget += AttachFollowTarget;
    }

    public void AttachFollowTarget(Transform newParent)
    {
        transform.parent = newParent;
        transform.localPosition = Vector3.zero;
        _attached = true;
    }

    public void ResetFollowTarget()
    {
        transform.parent = initialParent;
        _attached = false;
    }
}
