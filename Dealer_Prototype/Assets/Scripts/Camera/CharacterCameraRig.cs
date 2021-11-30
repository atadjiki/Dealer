using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CharacterCameraRig : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera CM_POV;
    [SerializeField] private CinemachineVirtualCamera CM_Facing;

    private void Awake()
    {
        CM_Facing.Follow = this.transform.parent.parent;
    }
}
