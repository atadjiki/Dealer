using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowTarget : MonoBehaviour
{
    private Transform initialParent;

    private bool _attached = false;

    private static float _maxDistanceFromOrigin = 5.0f; //how far we can wander from the origin 
    private static float _wanderSpeed = 0.5f; //wandering away from the origin
    private static float _returnSpeed = 0.025f; //when the camera slingshots back 

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

    private void FixedUpdate()
    {
        if(_attached)
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 direction = new Vector3(x, 0, z);

            if (direction.magnitude > 0)
            {
                if (CanWander())
                {
                    this.transform.position = this.transform.position + direction * _wanderSpeed;
                }
            }
            else if (this.transform.localPosition != Vector3.zero)
            {
               this.transform.localPosition = Vector3.Slerp(Vector3.zero, this.transform.localPosition, Time.fixedDeltaTime * _returnSpeed);
            }
        }
    }

    private float GetDistanceFromOrigin()
    {
        return Mathf.Abs(Vector3.Distance(this.transform.localPosition, Vector3.zero));
    }

    private bool CanWander()
    {
        return GetDistanceFromOrigin() <= _maxDistanceFromOrigin;
    }
}
