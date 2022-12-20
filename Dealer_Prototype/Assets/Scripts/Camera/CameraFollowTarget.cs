using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class CameraFollowTarget : MonoBehaviour
{
    private Transform initialParent;

    private bool _attached = false;

    //private static float _wanderSpeed = 0.15f; //wandering away from the origin
    //private static float _returnSpeed = 0.015f; //when the camera slingshots back

    //private static float _radius = 5;

    private void Start()
    {
        initialParent = transform.parent;

        PlayerComponent.OnPlayerSpawnedDelegate += AttachFollowTarget;
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

    //private void FixedUpdate()
    //{
    //    if(_attached)
    //    {
    //        float x = Input.GetAxis("Horizontal");
    //        float z = Input.GetAxis("Vertical");

    //        Vector3 direction = new Vector3(x, 0, z);

    //        Vector3 target = this.transform.position + direction * _wanderSpeed;

    //        if (direction.magnitude > 0)
    //        {
    //            if (target.magnitude < _radius)
    //            {
    //                this.transform.position = target;
    //            }

    //        }
    //        else if (this.transform.localPosition != Vector3.zero)
    //        {
    //            this.transform.localPosition = Vector3.Slerp(Vector3.zero, this.transform.localPosition, Time.fixedDeltaTime * _returnSpeed);
    //        }
    //    }
    //}
}
