using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    [Header("Orthographic Camera")]
    [SerializeField] private Vector3 RotateAround;
    [SerializeField] private float Degrees;

    void LateUpdate()
    {
        if(Camera.main != null)
        {
            if(Camera.main.orthographic)
            {
                Quaternion cameraRotation = Camera.main.transform.rotation;

                this.transform.rotation = cameraRotation;
                this.transform.Rotate(RotateAround, Degrees);
            }
        }    
    }
}

