using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Constants;
using System;

[Serializable]
public struct CameraInfo
{
    public CameraConstants.CameraID ID;
    public CinemachineVirtualCamera VirtualCamera;
}

//this class manages camera in the arena
public class CameraManager : MonoBehaviour
{
    [SerializeField] private List<CameraInfo> Cameras;

    private int _priority_off = 0;
    private int _priority_on = 10;



    private void Awake()
    {

    }

    public void GoTo(CameraConstants.CameraID ID)
    {
        CinemachineVirtualCamera camera = GetCamera(ID);

        if (camera != null)
        {
            ToggleAll(false);
            camera.Priority = GetPriority(true);
        }
    }

    public CinemachineVirtualCamera GetCamera(CameraConstants.CameraID ID)
    {
        foreach (CameraInfo info in Cameras)
        {
            if (info.ID == ID)
            {
                return info.VirtualCamera;
            }
        }

        return null;
    }

    private void ToggleAll(bool flag)
    {
        foreach (CameraInfo info in Cameras)
        {
            info.VirtualCamera.Priority = GetPriority(flag);
        }
    }

    private int GetPriority(bool flag)
    {
        if (flag)
        {
            return _priority_on;
        }
        else
        {
            return _priority_off;
        }
    }
}
