using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private CameraFollowTarget _followTarget;

    private void Start()
    {
        _followTarget = GetComponentInChildren<CameraFollowTarget>();
    }


}
