using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowTarget : MonoBehaviour
{
    private Transform initialParent;

    private void Start()
    {
        initialParent = transform.parent;

        PlayerComponent.OnPlayerSpawnedDelegate += AttachFollowTarget;
    }

    public void AttachFollowTarget(Transform newParent)
    {
        transform.parent = newParent;
        transform.localPosition = Vector3.zero;
    }

    public void ResetFollowTarget()
    {
        transform.parent = initialParent;
    }
}
