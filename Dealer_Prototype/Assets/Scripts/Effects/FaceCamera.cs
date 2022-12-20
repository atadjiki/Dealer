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
            else
            {
                Vector3 v = Camera.main.transform.position - transform.position;
                v.x = v.z = 0.0f;
                transform.LookAt(Camera.main.transform.position - v);
               // transform.rotation = (Camera.main.transform.rotation); // Take care about camera rotation
            }
        }    
    }
}