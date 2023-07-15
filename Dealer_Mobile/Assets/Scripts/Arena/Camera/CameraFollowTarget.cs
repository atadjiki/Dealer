using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowTarget : MonoBehaviour
{
    [SerializeField] private GameObject debugMesh;

    private void Awake()
    {
        if(debugMesh != null)
        {
            debugMesh.SetActive(false);
        }
    }
}
